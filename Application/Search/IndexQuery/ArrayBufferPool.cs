using System;
using System.Threading;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 数组缓冲区池
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    internal sealed class ArrayBufferPool<T>
    {
        /// <summary>
        /// 数组缓冲区集合
        /// </summary>
        private T[][] buffers;
        /// <summary>
        /// 空闲数组缓冲区数量
        /// </summary>
        private int count;
        /// <summary>
        /// 数组大小
        /// </summary>
        private readonly int size;
        /// <summary>
        /// 数组缓冲区池
        /// </summary>
        /// <param name="bit">数组大小二进制位数</param>
        internal ArrayBufferPool(int bit)
        {
            size = 1 << bit;
            buffers = EmptyArray<T[]>.Array;
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="poolIndex"></param>
        /// <returns></returns>
        internal ArrayBuffer<T> GetBuffer(ArrayBufferPoolArray<T> pool, int poolIndex)
        {
            if (count != 0)
            {
                Monitor.Enter(this);
                if (count != 0)
                {
                    T[] buffer = buffers[--count];
                    Monitor.Exit(this);
                    return new ArrayBuffer<T>(pool, buffer, poolIndex);
                }
                Monitor.Exit(this);
            }
            return new ArrayBuffer<T>(pool, new T[size], poolIndex);
        }
        /// <summary>
        /// 释放数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void Free(ref T[] buffer)
        {
            T[] pushBuffer = buffer;
            if (pushBuffer?.Length == size)
            {
                buffer = EmptyArray<T>.Array;
                Monitor.Enter(this);
                if (count != buffers.Length)
                {
                    buffers[count++] = pushBuffer;
                    Monitor.Exit(this);
                    return;
                }
                try
                {
                    if (count != 0) buffers = AutoCSer.Common.GetCopyArray(buffers, count << 1);
                    else buffers = new T[sizeof(int)][];
                    buffers[count++] = pushBuffer;
                }
                finally { Monitor.Exit(this); }
            }
        }
    }
}
