﻿using AutoCSer.Extensions;
using AutoCSer.IO;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
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
        internal const uint FileVersion = 0;
        /// <summary>
        /// 文件头部前 4 个字节，头部版本号为 0
        /// </summary>
        internal const uint FieHead = 'a' + ('m' << 8) + ('d' << 16) + (FileVersion << 24);
        /// <summary>
        /// 持久化回调异常位置文件头部前 4 个字节，头部版本号为 0
        /// </summary>
        internal const uint PersistenceCallbackExceptionPositionFileHead = 'c' + ('e' << 8) + ('p' << 16) + (FileVersion << 24);
        /// <summary>
        /// 文件头部字节大小
        /// </summary>
        internal const int FileHeadSize = sizeof(uint) + sizeof(ulong);

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
        private BinaryDeserializer deserializer;
        /// <summary>
        /// 持久化回调异常位置集合
        /// </summary>
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
                            service.SetPersistenceFileHeadVersion(*(uint*)start, *(ulong*)(start + sizeof(uint)));
                            loadPersistenceCallbackExceptionPositionVersion0();
                            return sizeof(uint) + sizeof(ulong);
                        default: throw new Exception($"文件 {persistenceFileName} 头部版本号不被支持 {(*(start + 3)).toString()}");
                    }
                }
            }
            throw new Exception($"文件 {persistenceFileName} 头部识别失败");
        }
        /// <summary>
        /// 加载持久化回调异常位置集合
        /// </summary>
        private void loadPersistenceCallbackExceptionPositionVersion0()
        {
            int fileHeadSize = sizeof(uint) + sizeof(ulong);
            long unreadSize = service.PersistenceCallbackExceptionPositionFileInfo.Length;
            if (unreadSize < fileHeadSize) throw new InvalidCastException($"持久化回调异常位置文件 {persistenceCallbackExceptionPositionFileName} 头部数据不足 {unreadSize.toString()} < {fileHeadSize.toString()}");
            persistenceCallbackExceptionPositions = HashSetCreator<long>.Create();
            ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(Math.Max(readBufferSize, 4 << 10));
            try
            {
                int bufferSize = buffer.Buffer.BufferSize;
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
                                int readSize = readStream.Read(buffer.Buffer.Buffer, buffer.StartIndex + endIndex, bufferSize - endIndex);
                                endIndex += readSize;
                                unreadSize -= readSize;
                                if (fileHeadSize != 0)
                                {
                                    if (*(uint*)start != (PersistenceCallbackExceptionPositionFileHead & 0xffffff))
                                    {
                                        throw new Exception($"持久化回调异常位置文件 {persistenceCallbackExceptionPositionFileName} 头部识别失败");
                                    }
                                    if (service.RebuildPosition != *(ulong*)(start + sizeof(uint)))
                                    {
                                        throw new Exception($"持久化回调异常位置文件 {persistenceCallbackExceptionPositionFileName} 重建索引位置 {*(ulong*)(start + sizeof(uint))} 与数据库文件位置 {service.RebuildPosition} 不匹配");
                                    }
                                    service.PersistenceCallbackExceptionPositionFileHeadVersion = *(uint*)start;
                                    if ((endIndex -= fileHeadSize) == 0) return;
                                    AutoCSer.Common.Config.CopyTo(start + fileHeadSize, start, endIndex);
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
                    throw new Exception($"持久化回调异常位置文件 {persistenceCallbackExceptionPositionFileName} 长度 {unreadSize} 不可识别");
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
                deserializer.SetContext(CommandServerSocket.CommandServerSocketContext, AutoCSer.BinaryDeserializer.DefaultConfig);
                base.loadBuffer();
            }
            finally
            {
                deserializer?.FreeContext();
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
                        throw new InvalidCastException($"文件 {persistenceFileName} 位置 {position}+{bufferIndex - data.Start} 处数据错误");
                    }
                    int parameterSize = *(int*)(dataFixed + (bufferIndex + (sizeof(NodeIndex) + sizeof(int)))), dataSize = parameterSize + (sizeof(NodeIndex) + sizeof(int) * 2);
                    if (parameterSize < 0 || bufferSize < dataSize)
                    {
                        throw new InvalidCastException($"文件 {persistenceFileName} 位置 {position}+{bufferIndex - data.Start} 处数据错误");
                    }
                    if (!persistenceCallbackExceptionPositions.Contains(dataPosition))
                    {
                        NodeIndex index = *(NodeIndex*)(dataFixed + bufferIndex);
                        int methodIndex = *(int*)(dataFixed + (bufferIndex + sizeof(NodeIndex)));
                        ++loadCount;
                        CallStateEnum state = service.Load(index, methodIndex, deserializer, new SubArray<byte>(bufferIndex + (sizeof(NodeIndex) + sizeof(int) * 2), parameterSize, data.Array));
                        switch (state)
                        {
                            case CallStateEnum.Success:
                            case CallStateEnum.PersistenceCallbackException:
                                break;
                            default: throw new InvalidCastException($"文件 {persistenceFileName} 位置 {position}+{bufferIndex - data.Start} 处数据错误 {state}");
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