﻿using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.IO;
using System.Threading;

namespace AutoCSer.IO
{
    /// <summary>
    /// 日志流持久化初始化加载数据
    /// </summary>
    public unsafe abstract class StreamPersistenceLoader
    {
        /// <summary>
        /// 数据加载缓冲区
        /// </summary>
        private StreamPersistenceLoaderBuffer buffer;
        /// <summary>
        /// 数据加载缓冲区
        /// </summary>
        private StreamPersistenceLoaderBuffer buffer2;
        /// <summary>
        /// 加载数据等待锁
        /// </summary>
        private AutoResetEvent loadBufferWait;
        /// <summary>
        /// 持久化数据读取文件流
        /// </summary>
        private FileStream readStream;
        /// <summary>
        /// 日志流持久化文件名称
        /// </summary>
        protected abstract string persistenceFileName { get; }
        /// <summary>
        /// 读取文件缓冲区大小，最小为 4KB
        /// </summary>
        protected abstract int readBufferSize { get; }
        /// <summary>
        /// 已经正常读取文件位置
        /// </summary>
        protected long position;
        /// <summary>
        /// 未读取数据大小
        /// </summary>
        private long unreadSize;
        /// <summary>
        /// 加载请求数量
        /// </summary>
        protected long loadCount;
        /// <summary>
        /// 文件头部字节大小
        /// </summary>
        private int fileHeadSize;
        /// <summary>
        /// 读取数据块头部字节大小，最小为 8 字节
        /// </summary>
        protected readonly int blockHeadSize;
        /// <summary>
        /// 读取数据缓冲区相对起始位置
        /// </summary>
        private int readIndex;
        /// <summary>
        /// 读取数据缓冲区相对结束位置
        /// </summary>
        private int endIndex;
        /// <summary>
        /// 读取数据缓冲区大小
        /// </summary>
        private int bufferSize;
        /// <summary>
        /// 读取文件数据是否异常
        /// </summary>
        private bool isReadException;
        /// <summary>
        /// 加载数据异常
        /// </summary>
        private Exception loadBufferException;
        /// <summary>
        /// 日志流持久化初始化加载数据
        /// </summary>
        /// <param name="fileHeadSize">文件头部字节大小</param>
        /// <param name="blockHeadSize">读取数据块头部字节大小，最小为 8 字节</param>
        protected StreamPersistenceLoader(int fileHeadSize = 0, int blockHeadSize = sizeof(int) * 2)
        {
            this.fileHeadSize = Math.Max(fileHeadSize, 0);
            this.blockHeadSize = Math.Max(blockHeadSize, sizeof(int) * 2);
            buffer.SetWait();
            buffer2.SetWait();
        }
        /// <summary>
        /// 开始加载数据
        /// </summary>
        /// <returns>加载请求数量</returns>
        public virtual long Load()
        {
            bool isLoadBuffer = false, isReadCompleted = false;
            try
            {
                ByteArrayPool.GetBuffer(ref buffer.ReadBuffer, Math.Max(readBufferSize, 4 << 10));
                ByteArrayPool.GetBuffer(ref buffer2.ReadBuffer, bufferSize = buffer.ReadBuffer.Buffer.BufferSize);
                using (readStream = new FileStream(persistenceFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None, bufferSize, FileOptions.SequentialScan))
                {
                    unreadSize = readStream.Length;
                    if (unreadSize < fileHeadSize) throw new InvalidCastException($"文件 {persistenceFileName} 头部数据不足 {unreadSize.toString()} < {fileHeadSize.toString()}");
                    loadBufferWait = new AutoResetEvent(false);
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(loadBuffer);
                    isLoadBuffer = true;
                    fixed (byte* dataFixed = buffer.ReadBuffer.GetFixedBuffer(), dataFixed2 = buffer2.ReadBuffer.GetFixedBuffer())
                    {
                        buffer.ReadBufferStart = dataFixed + buffer.ReadBuffer.StartIndex;
                        buffer2.ReadBufferStart = dataFixed2 + buffer2.ReadBuffer.StartIndex;
                        do
                        {
                            buffer.ReadFileWait.WaitOne();
                            if (loadBufferException != null || readFile(ref buffer, ref buffer2)) break;
                            buffer.LoadDataWait.Set();

                            buffer2.ReadFileWait.WaitOne();
                            if (loadBufferException != null || readFile(ref buffer2, ref buffer)) break;
                            buffer2.LoadDataWait.Set();
                        }
                        while (true);
                    }
                }
                isReadCompleted = true;
            }
            finally
            {
                if (!isReadCompleted) isReadException = true;
                buffer.LoadDataWait.Set();
                buffer2.LoadDataWait.Set();
                if (isLoadBuffer) loadBufferWait.WaitOne();
                buffer.Free();
                buffer2.Free();
            }
            if (loadBufferException != null) throw loadBufferException;
            return loadCount;
        }
        /// <summary>
        /// 加载文件头部数据
        /// </summary>
        /// <param name="data">读取文件数据</param>
        /// <returns>文件头部大小</returns>
        protected virtual int loadHead(SubArray<byte> data) { return fileHeadSize; }
        /// <summary>
        /// 读取文件数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="buffer2"></param>
        /// <returns>是否结束</returns>
        private bool readFile(ref StreamPersistenceLoaderBuffer buffer, ref StreamPersistenceLoaderBuffer buffer2)
        {
            if (fileHeadSize == 0)
            {
                if ((endIndex -= readIndex) == 0) endIndex = 0;
                else AutoCSer.Common.Config.CopyTo(buffer2.ReadBufferStart + readIndex, buffer.ReadBufferStart, endIndex);
                readIndex = 0;
            }
            else
            {
                int readSize = readStream.Read(buffer.ReadBuffer.Buffer.Buffer, buffer.ReadBuffer.StartIndex, bufferSize);
                unreadSize -= readSize;
                endIndex = readSize;
                position = readIndex = loadHead(buffer.ReadBuffer.GetSubArray(readSize));
                fileHeadSize = 0;

                if ((endIndex -= readIndex) == 0) endIndex = 0;
                else AutoCSer.Common.Config.CopyTo(buffer.ReadBufferStart + readIndex, buffer.ReadBufferStart, endIndex);
                readIndex = 0;
            }
            if (readFile(ref buffer, blockHeadSize)) return true;
            int dataSize = *(int*)buffer.ReadBufferStart;
            if (dataSize < 0)
            {
                int compressionDataSize = -dataSize;
                dataSize = *(int*)(buffer.ReadBufferStart + sizeof(int));
                if (dataSize < 0 || compressionDataSize == 0)
                {
                    throw new InvalidCastException($"文件 {persistenceFileName} 位置 {position} 处数据长度错误");
                }
                if (readFile(ref buffer, compressionDataSize + sizeof(int) * 2)) return true;
            }
            else if (readFile(ref buffer, dataSize + sizeof(int))) return true;
            buffer.LoadBufferPosition = position;
            position = readStream.Position - (endIndex - readIndex);
            return false;
        }
        /// <summary>
        /// 读取指定字节数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns>是否结束</returns>
        private bool readFile(ref StreamPersistenceLoaderBuffer buffer, int size)
        {
            if (endIndex >= size)
            {
                getReadBuffer(ref buffer);
                return false;
            }
            if (endIndex + unreadSize < size)
            {
                int errorSize = endIndex + (int)unreadSize;
                if (errorSize != 0)
                {
                    buffer.CopyBuffer.Free();
                    ByteArrayPool.GetBuffer(ref buffer.CopyBuffer, errorSize);
                    AutoCSer.Common.Config.CopyTo(buffer.ReadBufferStart, buffer.CopyBuffer.Buffer.Buffer, buffer.CopyBuffer.StartIndex, endIndex);
                    if (unreadSize != 0) readStream.Read(buffer.CopyBuffer.Buffer.Buffer, buffer.CopyBuffer.StartIndex + endIndex, (int)unreadSize);
                    string errorFileName = persistenceFileName + AutoCSer.Threading.SecondTimer.Now.ToString(".yyyyMMddHHmmss.") + ((ulong)position).toHex();
                    using (FileStream errorStream = new FileStream(errorFileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, errorSize, FileOptions.None))
                    {
                        errorStream.Write(buffer.CopyBuffer.Buffer.Buffer, buffer.CopyBuffer.StartIndex, errorSize);
                    }
                    readStream.Seek(position, SeekOrigin.Begin);
                    readStream.SetLength(position);
                    AutoCSer.LogHelper.ErrorIgnoreException($"文件 {persistenceFileName} 数据记载错误，于 {position} 处截断 {errorSize} 字节保存备份为文件 {errorFileName}", LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                }
                return true;
            }
            if (size <= bufferSize)
            {
                int readSize = readStream.Read(buffer.ReadBuffer.Buffer.Buffer, buffer.ReadBuffer.StartIndex + endIndex, bufferSize - endIndex);
                unreadSize -= readSize;
                endIndex += readSize;
                getReadBuffer(ref buffer);
                return false;
            }
            buffer.CopyBuffer.Free();
            ByteArrayPool.GetBuffer(ref buffer.CopyBuffer, size);
            AutoCSer.Common.Config.CopyTo(buffer.ReadBufferStart, buffer.CopyBuffer.Buffer.Buffer, buffer.CopyBuffer.StartIndex, endIndex);
            unreadSize -= readStream.Read(buffer.CopyBuffer.Buffer.Buffer, buffer.CopyBuffer.StartIndex + endIndex, size - endIndex);
            endIndex = 0;
            buffer.SetCopyBuffer(size);
            return false;
        }
        /// <summary>
        /// 获取读取数据缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        private void getReadBuffer(ref StreamPersistenceLoaderBuffer buffer)
        {
            readIndex = 0;
            do
            {
                int bufferSize = endIndex - readIndex;
                if (bufferSize < sizeof(int)) break;
                int dataSize = *(int*)(buffer.ReadBufferStart + readIndex);
                if (dataSize < 0)
                {
                    if (bufferSize < sizeof(int) * 2) break;
                    int compressionDataSize = -dataSize;
                    dataSize = *(int*)(buffer.ReadBufferStart + (readIndex + sizeof(int)));
                    if (dataSize < 0 || compressionDataSize == 0)
                    {
                        throw new InvalidCastException($"文件 {persistenceFileName} 位置 {position + readIndex} 处数据长度错误");
                    }
                    if (bufferSize < compressionDataSize + sizeof(int) * 2) break;
                    readIndex += compressionDataSize + sizeof(int) * 2;
                }
                else
                {
                    if (bufferSize < dataSize + sizeof(int)) break;
                    readIndex += dataSize + sizeof(int);
                }
            }
            while (true);
            buffer.SetReadBuffer(readIndex);
        }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        protected virtual void loadBuffer()
        {
            try
            {
                do
                {
                    buffer.LoadDataWait.WaitOne();
                    if (isReadException || buffer.LoadBuffer.Length == 0) return;
                    loadBuffer(ref buffer.LoadBuffer, buffer.LoadBufferPosition);
                    buffer.LoadBuffer.SetEmpty();
                    buffer.ReadFileWait.Set();

                    buffer2.LoadDataWait.WaitOne();
                    if (isReadException || buffer2.LoadBuffer.Length == 0) return;
                    loadBuffer(ref buffer2.LoadBuffer, buffer2.LoadBufferPosition);
                    buffer2.LoadBuffer.SetEmpty();
                    buffer2.ReadFileWait.Set();
                }
                while (true);
            }
            catch (Exception exception)
            {
                loadBufferException = exception;
            }
            finally
            {
                buffer.ReadFileWait.Set();
                buffer2.ReadFileWait.Set();
                loadBufferWait.Set();
            }
        }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="position"></param>
        private void loadBuffer(ref SubArray<byte> data, long position)
        {
            fixed (byte* dataFixed = data.GetFixedBuffer())
            {
                byte* start = dataFixed + data.Start, dataStart = start + sizeof(int), compressionDataStart = start + sizeof(int) * 2, end = start + data.Length;
                while (start != end)
                {
                    int dataSize = *(int*)start;
                    start += sizeof(int);
                    if (dataSize < 0)
                    {
                        int compressionDataSize = -dataSize;
                        dataSize = *(int*)start;
                        start += sizeof(int);
                        SubArray<byte> compressionData = new SubArray<byte>((int)(start - dataFixed), compressionDataSize, data.Array);
                        ByteArrayBuffer compressBuffer = ByteArrayPool.GetBuffer(dataSize);
                        try
                        {
                            SubArray<byte> nextData = compressBuffer.GetSubArray(dataSize);
                            if (!AutoCSer.Common.Config.Decompress(compressionData, ref nextData))
                            {
                                throw new InvalidCastException($"文件 {persistenceFileName} 位置 {position + (start - compressionDataStart)} 处数据解压缩失败");
                            }
                            load(nextData, position + (start - compressionDataStart));
                        }
                        finally { compressBuffer.Free(); }
                        start += compressionDataSize;
                    }
                    else
                    {
                        load(new SubArray<byte>((int)(start - dataFixed), dataSize, data.Array), position + (start - dataStart));
                        start += dataSize;
                    }
                }
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data">当前加载数据缓冲区</param>
        /// <param name="position">当前数据所在持久化流中的位置</param>
        protected abstract void load(SubArray<byte> data, long position);
    }
}