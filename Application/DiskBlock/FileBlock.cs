﻿using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
#if DotNet45 || NetStandard2
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 文件块
    /// </summary>
    public sealed class FileBlock : Block
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        private readonly FileInfo file;
        /// <summary>
        /// 写文件流
        /// </summary>
        private readonly FileStream writeStream;
        /// <summary>
        /// 读取数据缓存
        /// </summary>
        private readonly byte[] buffer;
        /// <summary>
        /// 写入文件流缓存字节数
        /// </summary>
        private readonly int writeBufferSize;
        /// <summary>
        /// 文件块
        /// </summary>
        /// <param name="service">磁盘块服务</param>
        /// <param name="file">文件信息</param>
        /// <param name="startIndex">文件信息</param>
        /// <param name="position">磁盘块当前写入位置</param>
        /// <param name="readBufferSize">文件流缓存字节数</param>
        internal FileBlock(DiskBlockService service, FileInfo file, long startIndex, long position, int readBufferSize) : base(service, startIndex)
        {
            this.file = file;
            buffer = AutoCSer.Common.Config.GetUninitializedArray<byte>(readBufferSize);
            Position = position;
        }
        /// <summary>
        /// 文件块
        /// </summary>
        /// <param name="service">磁盘块服务</param>
        /// <param name="file">磁盘块文件信息</param>
        /// <param name="startIndex">文件信息</param>
        /// <param name="position">磁盘块当前写入位置</param>
        /// <param name="readBufferSize">文件流缓存字节数</param>
        /// <param name="writeBufferSize">写入文件流缓存字节数</param>
        /// <param name="writeStream">写文件流</param>
        internal FileBlock(DiskBlockService service, FileInfo file, long startIndex, long position, int readBufferSize, int writeBufferSize, ref FileStream writeStream)
            : this(service, file, startIndex, position, readBufferSize)
        {
            this.writeStream = writeStream;
            this.writeBufferSize = writeBufferSize;
            writeStream = null;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            writeStream?.Dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public override async ValueTask DisposeAsync()
        {
            if (writeStream != null) await writeStream.DisposeAsync();
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns>磁盘块当前写入位置</returns>
        internal override async Task<long> Write(WriteRequest request)
        {
            switch (request.OperationType)
            {
                case WriteOperationTypeEnum.Append:
                    int size = request.Buffer.CurrentIndex;
                    request.Index.Set(writeStream.Position + StartIndex, size - sizeof(int));
                    await writeStream.WriteAsync(request.Buffer.Buffer.Buffer, request.Buffer.StartIndex, size);
                    return writeStream.Position + StartIndex;
                case WriteOperationTypeEnum.SwitchBlock:
                    if(writeStream.Position >= service.MinSwitchSize)
                    {
                        await Flush();

                        Block block = null;
                        FileStream writeStream = null;
                        FileInfo file = new FileInfo(Path.Combine(this.file.Directory.FullName, service.Identity.toHex() + ((ulong)Position).toHex() + FileBlockServiceConfig.ExtensionName));
                        try
                        {
                            writeStream = await AutoCSer.Common.Config.CreateFileStream(file.FullName, FileMode.Create, FileAccess.Write, FileShare.Read, writeBufferSize);
                            block = new FileBlock(service, file, Position, Position, writeBufferSize, buffer.Length, ref writeStream);
                            service.SetSwitch(ref block);
                            await this.writeStream.DisposeAsync();
                        }
                        finally
                        {
                            if (writeStream != null) await writeStream.DisposeAsync();
                            if (block != null) await block.DisposeAsync();
                        }
                        return Position;
                    }
                    return writeStream.Position + StartIndex;
            }
            return long.MinValue;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns>磁盘块当前写入位置</returns>
        public override async Task<long> Flush()
        {
            await writeStream.FlushAsync();
            return Position = writeStream.Position + StartIndex;
        }
        /// <summary>
        /// 获取读取数据上下文
        /// </summary>
        /// <returns></returns>
        protected override async Task<object> getReadContext()
        {
            return await AutoCSer.Common.Config.CreateFileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, buffer.Length, FileOptions.RandomAccess);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context">获取读取数据上下文</param>
        /// <returns></returns>
        protected override async Task read(ReadRequest request, object context)
        {
            FileStream fileStream = (FileStream)context;
            await AutoCSer.Common.Config.Seek(fileStream, request.Index - StartIndex, SeekOrigin.Begin);
            int maxSize = request.GetMaxSize();
            int readSize = await fileStream.ReadAsync(this.buffer, 0, Math.Min(this.buffer.Length, maxSize + sizeof(int)));
            if (readSize < sizeof(int))
            {
                request.Buffer.State = ReadBufferStateEnum.ReadSize;
                return;
            }
            int size = BitConverter.ToInt32(this.buffer, 0);
            if ((size != maxSize && !request.CheckSize(size)) || size <= BlockIndex.MaxIndexSize)
            {
                request.Buffer.State = ReadBufferStateEnum.Size;
                return;
            }
            if ((readSize -= sizeof(int)) >= size)
            {
                request.Buffer.Set(new SubArray<byte>(sizeof(int), size, this.buffer).GetArray());
                return;
            }
            byte[] buffer = AutoCSer.Common.Config.GetUninitializedArray<byte>(size);
            if (await fileStream.ReadAsync(buffer, readSize, size - readSize) + readSize == size)
            {
                AutoCSer.Common.Config.CopyTo(this.buffer, sizeof(int), buffer, 0, readSize);
                request.Buffer.Set(buffer);
            }
            else request.Buffer.State = ReadBufferStateEnum.ReadSize;
        }
        /// <summary>
        /// 释放读取数据上下文
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task freeReadContext(object context) 
        {
            await ((FileStream)context).DisposeAsync();
        }
        /// <summary>
        /// 删除磁盘块
        /// </summary>
        /// <returns>是否删除成功</returns>
        internal override async Task<bool> Delete()
        {
            await AutoCSer.Common.Config.DeleteFile(file);
            return true;
        }
    }
}
