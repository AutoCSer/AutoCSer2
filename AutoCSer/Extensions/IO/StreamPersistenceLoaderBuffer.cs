using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.IO
{
    /// <summary>
    /// 日志流持久化初始化加载数据缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct StreamPersistenceLoaderBuffer
    {
        /// <summary>
        /// 读取文件缓冲区等待锁
        /// </summary>
        internal AutoResetEvent ReadFileWait;
        /// <summary>
        /// 加载数据等待锁
        /// </summary>
        internal AutoResetEvent LoadDataWait;
        /// <summary>
        /// 读取数据缓冲区
        /// </summary>
        internal ByteArrayBuffer ReadBuffer;
        /// <summary>
        /// 复制数据缓冲区
        /// </summary>
        internal ByteArrayBuffer CopyBuffer;
        /// <summary>
        /// 读取数据缓冲区相对开始位置指针
        /// </summary>
        internal byte* ReadBufferStart;
        /// <summary>
        /// 读取文件结果
        /// </summary>
        internal SubArray<byte> LoadBuffer;
        /// <summary>
        /// 读取文件结果的文件流起始位置
        /// </summary>
        internal long LoadBufferPosition;
        /// <summary>
        /// 等待锁初始化
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetWait()
        {
            ReadFileWait = new AutoResetEvent(true);
            LoadDataWait = new AutoResetEvent(false);
        }
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            ReadBuffer.Free();
            CopyBuffer.Free();
        }
        /// <summary>
        /// 设置读取文件结果
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetReadBuffer(int size)
        {
            LoadBuffer.Set(ReadBuffer.Buffer.notNull().Buffer, ReadBuffer.StartIndex, size);
        }
        /// <summary>
        /// 设置读取文件结果
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCopyBuffer(int size)
        {
            LoadBuffer.Set(CopyBuffer.Buffer.notNull().Buffer, CopyBuffer.StartIndex, size);
        }
    }
}
