using AutoCSer.Extensions;
using AutoCSer.IO;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库服务端数据加载
    /// </summary>
    internal unsafe sealed class ServiceLoader : StreamPersistenceLoader
    {
        /// <summary>
        /// 文件版本号
        /// </summary>
        internal const uint FileVersion = 1;
        /// <summary>
        /// 文件头部前 4 个字节，头部版本号为 1
        /// </summary>
        internal const uint FieHead = 'a' + ('m' << 8) + ('d' << 16) + (FileVersion << 24);
        /// <summary>
        /// 文件头部字节大小 [版本号]+[持久化流重建起始位置]+[快照结束位置]
        /// </summary>
        internal const int FileHeadSize = sizeof(uint) + sizeof(ulong) + sizeof(long);
        /// <summary>
        /// 持久化回调异常位置文件版本号
        /// </summary>
        internal const uint ExceptionPositionFileVersion = 0;
        /// <summary>
        /// 持久化回调异常位置文件头部前 4 个字节，头部版本号为 0
        /// </summary>
        internal const uint PersistenceCallbackExceptionPositionFileHead = 'c' + ('e' << 8) + ('p' << 16) + (ExceptionPositionFileVersion << 24);
        /// <summary>
        /// 持久化回调异常位置文件头部字节大小 [版本号]+[持久化流重建起始位置]
        /// </summary>
        internal const int ExceptionPositionFileHeadSize = sizeof(uint) + sizeof(ulong);

        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 持久化异常位置文件读取位置
        /// </summary>
        private long persistenceCallbackExceptionFilePosition;
        /// <summary>
        /// 反序列化
        /// </summary>
#if NetStandard21
        private BinaryDeserializer? deserializer;
#else
        private BinaryDeserializer deserializer;
#endif
        /// <summary>
        /// 持久化回调异常位置集合
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private HashSet<long> persistenceCallbackExceptionPositions;
        /// <summary>
        /// 日志流持久化文件名称
        /// </summary>
        protected override string persistenceFileName { get { return service.PersistenceFileInfo.FullName; } }
        /// <summary>
        /// 读取文件缓冲区大小，最小为 4KB
        /// </summary>
        protected override int readBufferSize { get { return service.Config.BufferMaxSize; } }
        /// <summary>
        /// 持久化回调异常位置文件名称
        /// </summary>
        private string persistenceCallbackExceptionPositionFileName { get { return service.PersistenceCallbackExceptionPositionFileInfo.FullName; } }
        /// <summary>
        /// 日志流持久化内存数据库服务端数据加载
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        internal ServiceLoader(StreamPersistenceMemoryDatabaseService service) : base(FileHeadSize, sizeof(NodeIndex) + sizeof(int) * 3)
        {
            this.service = service;
        }
        /// <summary>
        /// 开始加载数据
        /// </summary>
        /// <returns>加载请求数量</returns>
        public override long Load()
        {
            base.Load();
            service.SetPersistencePosition(position, persistenceCallbackExceptionFilePosition);
            return loadCount;
        }
        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="compressData">压缩后的数据</param>
        /// <param name="destinationData">等待写入的原始数据缓冲区</param>
        /// <returns>是否解压成功</returns>
        internal override bool Decompress(ref SubArray<byte> compressData, ref SubArray<byte> destinationData)
        {
            return service.Config.Decompress(ref compressData, ref destinationData);
        }
        /// <summary>
        /// 加载文件头部数据
        /// </summary>
        /// <param name="data">读取文件数据</param>
        /// <returns>文件头部大小</returns>
        protected override unsafe int loadHead(SubArray<byte> data)
        {
            fixed (byte* dataFixed = data.GetFixedBuffer())
            {
                byte* start = dataFixed + data.Start;
                if ((*(uint*)start & 0xffffff) == (FieHead & 0xffffff))
                {
                    switch (*(start + 3))
                    {
                        case 0:
                            service.SetPersistenceFileHeadVersion(*(uint*)start, *(ulong*)(start + sizeof(uint)), 0);
                            loadPersistenceCallbackExceptionPositionVersion0();
                            return sizeof(uint) + sizeof(ulong);
                        case 1:
                            service.SetPersistenceFileHeadVersion(*(uint*)start, *(ulong*)(start + sizeof(uint)), *(long*)(start + (sizeof(uint) + sizeof(ulong))));
                            loadPersistenceCallbackExceptionPositionVersion0();
                            return sizeof(uint) + sizeof(ulong) + sizeof(long);
                        default: throw new Exception(Culture.Configuration.Default.GetServiceLoaderFileVersionNotSupported(persistenceFileName, *(start + 3)));
                    }
                }
            }
            throw new Exception(Culture.Configuration.Default.GetServiceLoaderFileHeaderNotMatch(persistenceFileName));
        }
        /// <summary>
        /// 加载持久化回调异常位置集合
        /// </summary>
        private void loadPersistenceCallbackExceptionPositionVersion0()
        {
            int fileHeadSize = sizeof(uint) + sizeof(ulong);
            long unreadSize = service.PersistenceCallbackExceptionPositionFileInfo.Length;
            if (unreadSize < fileHeadSize) throw new InvalidCastException(Culture.Configuration.Default.GetServiceLoaderExceptionPositionFileHeaderSizeNotMatch(persistenceCallbackExceptionPositionFileName, (int)unreadSize, fileHeadSize));
            persistenceCallbackExceptionPositions = HashSetCreator<long>.Create();
            ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(Math.Max(readBufferSize, 4 << 10));
            try
            {
                int bufferSize = buffer.Buffer.notNull().BufferSize;
                using (FileStream readStream = new FileStream(persistenceCallbackExceptionPositionFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None, bufferSize, FileOptions.SequentialScan))
                {
                    persistenceCallbackExceptionFilePosition = unreadSize = readStream.Length;
                    if (((unreadSize - fileHeadSize) & (sizeof(long) - 1)) == 0)
                    {
                        int endIndex = 0;
                        fixed (byte* dataFixed = buffer.GetFixedBuffer())
                        {
                            byte* start = dataFixed + buffer.StartIndex;
                            do
                            {
                                int readSize = readStream.Read(buffer.Buffer.notNull().Buffer, buffer.StartIndex + endIndex, bufferSize - endIndex);
                                endIndex += readSize;
                                unreadSize -= readSize;
                                if (fileHeadSize != 0)
                                {
                                    if (*(uint*)start != (PersistenceCallbackExceptionPositionFileHead & 0xffffff))
                                    {
                                        throw new Exception(Culture.Configuration.Default.GetServiceLoaderExceptionPositionFileHeaderNotMatch(persistenceCallbackExceptionPositionFileName));
                                    }
                                    if (service.RebuildPosition != *(ulong*)(start + sizeof(uint)))
                                    {
                                        throw new Exception(Culture.Configuration.Default.GetServiceLoaderExceptionPositionRebuildPositionNotMatch(persistenceCallbackExceptionPositionFileName, *(ulong*)(start + sizeof(uint)), service.RebuildPosition));
                                    }
                                    service.PersistenceCallbackExceptionPositionFileHeadVersion = *(uint*)start;
                                    if ((endIndex -= fileHeadSize) == 0) return;
                                    AutoCSer.Common.CopyTo(start + fileHeadSize, start, endIndex);
                                    fileHeadSize = 0;
                                }
                                readSize = endIndex & (int.MaxValue - (sizeof(long) - 1));
                                byte* end = start + readSize;
                                for (byte* read = start; read != end; read += sizeof(long)) persistenceCallbackExceptionPositions.Add(*(long*)read);
                                if ((endIndex -= readSize) == 0)
                                {
                                    if (unreadSize == 0) return;
                                }
                                else *(long*)start = *(long*)end;
                            }
                            while (true);
                        }
                    }
                    throw new Exception(Culture.Configuration.Default.GetServiceLoaderExceptionPositionFileSizeUnrecognized(persistenceCallbackExceptionPositionFileName, unreadSize));
                }
            }
            finally { buffer.Free(); }
        }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        protected override void loadBuffer()
        {
            try
            {
                deserializer = AutoCSer.BinaryDeserializer.YieldPool.Default.Pop() ?? new AutoCSer.BinaryDeserializer();
                deserializer.SetContextNoCheckRemoteType(CommandServerSocket.CommandServerSocketContext, AutoCSer.BinaryDeserializer.DefaultConfig);
                base.loadBuffer();
            }
            finally
            {
                deserializer?.FreeContextCheckRemoteType();
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data">当前加载数据缓冲区</param>
        /// <param name="position">当前数据所在持久化流中的位置</param>
        protected override void load(SubArray<byte> data, long position)
        {
            service.LoadRepairNodeMethod(position);
            fixed (byte* dataFixed = data.GetFixedBuffer())
            {
                long dataPosition = position;
                int bufferIndex = data.Start, bufferSize = data.Length;
                do
                {
                    if (bufferSize < sizeof(NodeIndex) + sizeof(int) * 2)
                    {
                        throw new InvalidCastException(Culture.Configuration.Default.GetServiceLoaderFailed(persistenceFileName, position, bufferIndex - data.Start));
                    }
                    int parameterSize = *(int*)(dataFixed + (bufferIndex + (sizeof(NodeIndex) + sizeof(int)))), dataSize = parameterSize + (sizeof(NodeIndex) + sizeof(int) * 2);
                    if (parameterSize < 0 || bufferSize < dataSize)
                    {
                        throw new InvalidCastException(Culture.Configuration.Default.GetServiceLoaderFailed(persistenceFileName, position, bufferIndex - data.Start));
                    }
                    if (!persistenceCallbackExceptionPositions.Contains(dataPosition))
                    {
                        NodeIndex index = *(NodeIndex*)(dataFixed + bufferIndex);
                        int methodIndex = *(int*)(dataFixed + (bufferIndex + sizeof(NodeIndex)));
                        ++loadCount;
                        CallStateEnum state = service.Load(index, methodIndex, deserializer.notNull(), new SubArray<byte>(bufferIndex + (sizeof(NodeIndex) + sizeof(int) * 2), parameterSize, data.Array));
                        switch (state)
                        {
                            case CallStateEnum.Success:
                            case CallStateEnum.PersistenceCallbackException:
                                break;
                            default: throw new InvalidCastException(Culture.Configuration.Default.GetServiceLoaderFailed(state, persistenceFileName, position, bufferIndex - data.Start));
                        }
                    }
                    bufferSize -= dataSize;
                    bufferIndex += dataSize;
                    ++dataPosition;
                }
                while (bufferSize != 0);
            }
        }
        /// <summary>
        /// 获取持久化文件头部版本信息
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="rebuildPosition"></param>
        /// <returns></returns>
        internal static uint GetPersistenceFileHeadVersion(byte[] buffer, out ulong rebuildPosition)
        {
            fixed(byte* bufferFixed = buffer)
            {
                rebuildPosition = *(ulong*)(bufferFixed + sizeof(uint));
                return *(uint*)bufferFixed;
            }
        }
    }
}
