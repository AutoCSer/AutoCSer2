﻿using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct PersistenceBuffer
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 输出数据缓冲区
        /// </summary>
        internal ByteArrayBuffer OutputBuffer;
        /// <summary>
        /// 临时复制数据缓冲区
        /// </summary>
        private ByteArrayBuffer outputCopyBuffer;
        /// <summary>
        /// 压缩数据缓冲区
        /// </summary>
        private ByteArrayBuffer outputCompressBuffer;
        /// <summary>
        /// 数据缓冲区最大字节数
        /// </summary>
        internal readonly int SendBufferMaxSize;
        /// <summary>
        /// 启用压缩最低字节数量
        /// </summary>
        private readonly int minCompressSize;
        /// <summary>
        /// 输出数据流
        /// </summary>
        internal UnmanagedStream OutputStream;
        /// <summary>
        /// 输出数据缓冲区起始位置
        /// </summary>
        private byte* start;
        /// <summary>
        /// 输出数据缓冲区大小
        /// </summary>
        private int bufferLength;
        /// <summary>
        /// 当前已经创建输出数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// 当前缓冲区已确认输出位置
        /// </summary>
        private int currentIndex;
        /// <summary>
        /// 是否创建了新的缓冲区
        /// </summary>
        private bool isNewBuffer;
        /// <summary>
        /// 日志流持久化缓冲区
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        internal PersistenceBuffer(StreamPersistenceMemoryDatabaseService service)
        {
            this.service = service;
            OutputBuffer = default(ByteArrayBuffer);
            outputCopyBuffer = default(ByteArrayBuffer);
            outputCompressBuffer = default(ByteArrayBuffer);
            SendBufferMaxSize = Math.Max(service.Config.BufferMaxSize, 4 << 10);
            minCompressSize = service.Config.MinCompressSize;
            if (minCompressSize <= 0) minCompressSize = int.MaxValue;
            bufferLength = service.PersistenceBufferPool.Size;
            Count = currentIndex = 0;
            OutputStream = null;
            start = null;
            isNewBuffer = false;
        }
        /// <summary>
        /// 获取输出数据缓冲区大小
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void GetBufferLength()
        {
            service.PersistenceBufferPool.Get(ref OutputBuffer);
            bufferLength = OutputBuffer.Buffer.BufferSize;
        }
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            OutputBuffer.Free();
            outputCopyBuffer.Free();
            outputCompressBuffer.Free();
        }
        /// <summary>
        /// 设置输出数据缓冲区起始位置
        /// </summary>
        /// <param name="dataFixed"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetStart(byte* dataFixed)
        {
            start = dataFixed + OutputBuffer.StartIndex;
        }
        /// <summary>
        /// 重置状态数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Reset()
        {
            if (OutputStream.Data.Pointer.Byte != start) OutputStream.Reset(start, OutputBuffer.Buffer.BufferSize);
            isNewBuffer = false;
            Count = 0;
            OutputStream.Data.Pointer.CurrentIndex = currentIndex = MethodParameter.PersistenceStartIndex;
            OutputStream.SetCanResize(true);
        }
        /// <summary>
        /// 还原当前缓冲区已确认输出位置
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RestoreCurrentIndex()
        {
            OutputStream.Data.Pointer.CurrentIndex = currentIndex;
        }
        /// <summary>
        /// 输出数量不为 0 时设置当前缓冲区已确认输出位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool TrySetCurrentIndex()
        {
            if (Count != 0)
            {
                currentIndex = OutputStream.Data.Pointer.CurrentIndex;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查输出流是否已经超限
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckResizeError()
        {
            if (OutputStream.IsResizeError)
            {
                OutputStream.Data.Pointer.CurrentIndex = currentIndex;
                return true;
            }
            ++Count;
            return false;
        }
        /// <summary>
        /// 输出数据缓冲区未更新时设置输出计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckDataStart()
        {
            if (OutputStream.Data.Pointer.Byte != start) return true;
            Count = 1;
            OutputStream.CanResize = false;
            return false;
        }
        /// <summary>
        /// 检查数据缓冲区是否被新建
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckNewBuffer()
        {
            outputCompressBuffer.Free();
            if (!isNewBuffer)
            {
                outputCopyBuffer.Free();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取输出数据
        /// </summary>
        /// <returns></returns>
        internal unsafe SubArray<byte> GetData()
        {
            int streamDataSize = OutputStream.Data.Pointer.CurrentIndex, dataSize = streamDataSize - MethodParameter.PersistenceStartIndex;
            if (streamDataSize <= bufferLength)
            {
                byte* dataStart = OutputStream.Data.Pointer.Byte;
                if (dataStart != start) AutoCSer.Common.Config.CopyTo(dataStart + MethodParameter.PersistenceStartIndex, start + MethodParameter.PersistenceStartIndex, dataSize);
                SubArray<byte> outputData = new SubArray<byte>(OutputBuffer.StartIndex + MethodParameter.PersistenceStartIndex, dataSize, OutputBuffer.Buffer.Buffer);
                if (dataSize >= minCompressSize && compress(ref outputData)) return outputData;
                outputData.MoveStart(-MethodParameter.PersistenceStartIndex);
                *(int*)start = dataSize;
                return outputData;
            }
            else
            {
                OutputStream.Data.Pointer.GetBuffer(ref outputCopyBuffer, MethodParameter.PersistenceStartIndex);
                SubArray<byte> outputData = new SubArray<byte>(outputCopyBuffer.StartIndex + MethodParameter.PersistenceStartIndex, dataSize, outputCopyBuffer.Buffer.Buffer);
                if (outputCopyBuffer.Buffer.BufferSize <= SendBufferMaxSize)
                {
                    OutputBuffer.CopyFromFree(ref outputCopyBuffer);
                    isNewBuffer = true;
                }
                if (dataSize >= minCompressSize && compress(ref outputData)) return outputData;
                outputData.MoveStart(-MethodParameter.PersistenceStartIndex);
                fixed (byte* sendDataFixed = outputData.GetFixedBuffer()) *(int*)(sendDataFixed + outputData.Start) = dataSize;
                return outputData;
            }
        }
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="outputData"></param>
        /// <returns>是否压缩成功</returns>
        private bool compress(ref SubArray<byte> outputData)
        {
            int dataSize = outputData.Length;
            if (AutoCSer.Common.Config.Compress(outputData.Array, outputData.Start, dataSize, ref outputCompressBuffer, ref outputData, MethodParameter.CompressPersistenceStartIndex, sizeof(int) * 2, service.Config.CompressionLevel))
            {
                int compressionDataSize = outputData.Length;
                outputData.MoveStart(-MethodParameter.CompressPersistenceStartIndex);
                fixed (byte* sendDataFixed = outputData.GetFixedBuffer())
                {
                    byte* dataStart = sendDataFixed + outputData.Start;
                    *(int*)dataStart = -compressionDataSize;
                    *(int*)(dataStart + sizeof(int)) = dataSize;
                }
                return true;
            }
            return false;
        }
    }
}