using AutoCSer.Memory;
using System;
using System.Threading;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 数组缓冲区池
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal sealed class ArrayBufferPool<T>
    {
        /// <summary>
        /// 数组缓冲区集合
        /// </summary>
        private LeftArray<T[]> buffers;
        /// <summary>
        /// 数组大小
        /// </summary>
        private readonly int size;
        /// <summary>
        /// 是否申请了新的缓冲区
        /// </summary>
        private bool isGetNewBuffer;
        /// <summary>
        /// 数组缓冲区池
        /// </summary>
        /// <param name="bit">数组大小二进制位数</param>
        internal ArrayBufferPool(int bit)
        {
            size = 1 << bit;
            buffers = new LeftArray<T[]>(0);
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="poolIndex"></param>
        /// <returns></returns>
        internal ArrayBuffer<T> GetBuffer(ArrayBufferPoolArray<T> pool, int poolIndex)
        {
            if (buffers.Length != 0)
            {
                var buffer = default(T[]);
                Monitor.Enter(this);
                if (buffers.TryPop(out buffer))
                {
                    Monitor.Exit(this);
                    return new ArrayBuffer<T>(pool, buffer, poolIndex);
                }
                Monitor.Exit(this);
            }
            isGetNewBuffer = true;
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
                try
                {
                    buffers.Add(pushBuffer);
                }
                finally { Monitor.Exit(this); }
            }
        }
        /// <summary>
        /// 释放部分缓冲区
        /// </summary>
        internal void FreeCache()
        {
            if (isGetNewBuffer)
            {
                Monitor.Enter(this);
                buffers.ClearCache();
                Monitor.Exit(this);
                isGetNewBuffer = false;
            }
        }
    }
}
