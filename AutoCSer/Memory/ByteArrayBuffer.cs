using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 字节数组缓冲区
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ByteArrayBuffer
    {
        /// <summary>
        /// 字节数组缓冲区
        /// </summary>
#if NetStandard21
        internal ByteArray? Buffer;
#else
        internal ByteArray Buffer;
#endif
        /// <summary>
        /// 缓冲区起始位置
        /// </summary>
        internal int StartIndex;
        /// <summary>
        /// 当前相对位置，保留字段
        /// </summary>
        internal int CurrentIndex;
        /// <summary>
        /// 当前绝对位置
        /// </summary>
        internal int BufferCurrentIndex { get { return StartIndex + CurrentIndex; } }
        ///// <summary>
        ///// 获取空闲字节数
        ///// </summary>
        //internal int FreeSize { get { return Buffer.BufferSize - CurrentIndex; } }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="buffer"></param>
        internal unsafe ByteArrayBuffer(ref SubArray<byte> buffer)
        {
            CurrentIndex = buffer.Length;
            StartIndex = 0;
            Buffer = new ByteArray(CurrentIndex);
            fixed (byte* dataFixed = GetFixedBuffer()) AutoCSer.Common.CopyTo(buffer.Array, buffer.Start, dataFixed, CurrentIndex);
        }
        /// <summary>
        /// 清除数据并返回字节数组缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ByteArray? GetClearBuffer()
#else
        internal ByteArray GetClearBuffer()
#endif
        {
            var buffer = Buffer;
            Buffer = null;
            return buffer;
        }
        /// <summary>
        /// 设置字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ByteArray buffer)
        {
            StartIndex = buffer.Indexs.PopInt();
            Buffer = buffer;
        }
        /// <summary>
        /// 设置字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ByteArray buffer, int startIndex)
        {
            Buffer = buffer;
            StartIndex = startIndex;
        }
        /// <summary>
        /// 尝试移除需要清除的字节数组缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TryRemoveGet()
        {
            Buffer.notNull().TryRemoveGet(ref this);
            CurrentIndex = 0;
        }
        /// <summary>
        /// 释放字节数组缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Free()
        {
            if (Buffer != null) Buffer.TryFree(ref this);
        }
        /// <summary>
        /// 释放复制缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeCopy(ref ByteArrayBuffer buffer)
        {
            if (StartIndex != buffer.StartIndex || !object.ReferenceEquals(Buffer, buffer.Buffer)) Free();
            buffer.Free();
        }
        /// <summary>
        /// 当字节大小不满足时，重新获取字节数组缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <param name="currentIndex"></param>
        internal void ReSize(int size, int currentIndex)
        {
            CurrentIndex = currentIndex;
            if (Buffer == null) ByteArrayPool.GetBuffer(ref this, size);
            else if (Buffer.BufferSize < size)
            {
                Buffer.TryFree(ref this);
                ByteArrayPool.GetBuffer(ref this, size);
            }
        }
        /// <summary>
        /// 设置套接字缓存区
        /// </summary>
        /// <param name="receiveAsyncEventArgs"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBuffer(SocketAsyncEventArgs receiveAsyncEventArgs)
        {
            receiveAsyncEventArgs.SetBuffer(Buffer.notNull().Buffer, StartIndex, Buffer.notNull().BufferSize);
        }
        /// <summary>
        /// 设置套接字缓存区
        /// </summary>
        /// <param name="receiveAsyncEventArgs"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCurrent(SocketAsyncEventArgs receiveAsyncEventArgs)
        {
            receiveAsyncEventArgs.SetBuffer(StartIndex + CurrentIndex, Buffer.notNull().BufferSize - CurrentIndex);
        }
        /// <summary>
        /// 释放当前缓存区并复制字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void CopyFromFree(ref ByteArrayBuffer buffer)
        {
            Free();
            StartIndex = buffer.StartIndex;
            Buffer = buffer.GetClearBuffer();
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="data"></param>
        internal unsafe void CopyFromSetSize(ref SubArray<byte> data)
        {
#if DEBUG
            if (data.Length + sizeof(int) > Buffer.notNull().BufferSize) throw new Exception(data.Length.toString() + " + 4 > " + Buffer.notNull().BufferSize.toString());
#endif
            CurrentIndex = data.Length + sizeof(int);
            fixed (byte* dataFixed = GetFixedBuffer())
            {
                byte* write = dataFixed + StartIndex;
                *(int*)write = data.Length;
                AutoCSer.Common.CopyTo(data.Array, data.Start, write + sizeof(int), data.Length);
            }
        }
        /// <summary>
        /// 获取数组字串
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubArray<byte> GetSubArray(int startIndex, int length)
        {
            return new SubArray<byte>(StartIndex + startIndex, length, Buffer.notNull().Buffer);
        }
        /// <summary>
        /// 获取数组字串
        /// </summary>
        /// <param name="seek"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubArray<byte> GetSeekSubArray(int seek = 0)
        {
            return new SubArray<byte>(StartIndex + seek, CurrentIndex - seek, Buffer.notNull().Buffer);
        }
        /// <summary>
        /// 设置数组字串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        internal void Set(ref SubArray<byte> data, int startIndex, int length)
        {
            data.Set(Buffer.notNull().Buffer, StartIndex + startIndex, length);
#if DEBUG
            data.DebugCheckFixed();
#endif
        }
        /// <summary>
        /// 获取数组字串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SubArray<byte> GetSubArray(int length)
        {
            return new SubArray<byte>(StartIndex, length, Buffer.notNull().Buffer);
        }
        /// <summary>
        /// 获取 fixed 缓冲区，DEBUG 模式对数据范围进行检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal byte[] GetFixedBuffer()
        {
#if DEBUG
            if (StartIndex < 0) throw new Exception(StartIndex.toString() + " < 0");
            int bufferSize = Buffer.notNull().BufferSize;
            if (bufferSize < 0) throw new Exception(bufferSize.toString() + " < 0");
            if (StartIndex + bufferSize > Buffer.notNull().Buffer.Length) throw new Exception(StartIndex.toString() + " + " + bufferSize.toString() + " > " + Buffer.notNull().Buffer.Length.toString());
#endif
            return Buffer.notNull().Buffer;
        }
    }
}
