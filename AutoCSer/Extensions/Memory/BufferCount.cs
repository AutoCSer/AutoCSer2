using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 数据缓冲区计数
    /// </summary>
    internal sealed class BufferCount
    {
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal ByteArrayBuffer Buffer;
        /// <summary>
        /// 当前计数
        /// </summary>
        internal int Count;
        /// <summary>
        /// 默认空数据缓冲区计数
        /// </summary>
        private BufferCount() { }
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        /// <param name="pool"></param>
        internal BufferCount(ByteArrayPool pool)
        {
            pool.Get(ref Buffer);
            Count = 1;
        }
        /// <summary>
        /// 释放计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            if (Interlocked.Decrement(ref Count) == 0) Buffer.Free();
        }

        /// <summary>
        /// 默认空数据缓冲区计数
        /// </summary>
        internal static readonly BufferCount Null = new BufferCount();
    }
}
