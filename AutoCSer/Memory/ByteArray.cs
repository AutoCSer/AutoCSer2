using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 字节数组缓冲区
    /// </summary>
    internal class ByteArray
    {
        /// <summary>
        /// 字节数组缓冲区
        /// </summary>
        internal readonly byte[] Buffer;
        /// <summary>
        /// 字节数组缓冲区池
        /// </summary>
#if NetStandard21
        internal readonly ByteArrayPool? Pool;
#else
        internal readonly ByteArrayPool Pool;
#endif
        /// <summary>
        /// 缓存区字节大小
        /// </summary>
        internal int BufferSize
        {
            get { return Pool != null ? Pool.Size : Buffer.Length; }
        }
        /// <summary>
        /// 缓存区指针
        /// </summary>
        internal Pointer Indexs;
        /// <summary>
        /// 是否已经从缓存区池中移除
        /// </summary>
        internal bool IsRemove;
        /// <summary>
        /// 字节数组缓冲区池
        /// </summary>
        /// <param name="size"></param>
        internal ByteArray(int size)
        {
            Buffer = AutoCSer.Common.GetUninitializedArray<byte>(size);
        }
        /// <summary>
        /// 字节数组缓冲区池
        /// </summary>
        /// <param name="pool"></param>
        internal ByteArray(ByteArrayPool pool)
        {
            this.Pool = pool;
            int size = pool.Size;
            if (size < ByteArrayPool.FixedBufferSize)
            {
                Buffer = AutoCSer.Common.GetUninitializedArray<byte>(ByteArrayPool.FixedBufferSize);
                Indexs = Unmanaged.GetPointer((ByteArrayPool.FixedBufferSize / size) * sizeof(int), false);
                Indexs.Write(0);
                for (int index = 0; Indexs.IsFreeSize != 0; Indexs.Write(index += size)) ;
            }
            else Buffer = AutoCSer.Common.GetUninitializedArray<byte>(size);
        }
        /// <summary>
        /// 释放缓存区指针
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeIndex()
        {
            Unmanaged.FreeOnly(ref Indexs);
            Indexs.SetNull();
        }
        /// <summary>
        /// 尝试释放字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TryFree(ref ByteArrayBuffer buffer)
        {
            if (Pool != null) Pool.Free(ref buffer);
            else buffer.Buffer = null;
        }
        /// <summary>
        /// 释放字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>是否需要重新添加到 空闲缓冲区集合</returns>
        internal bool Free(ref ByteArrayBuffer buffer)
        {
            if (Indexs.CurrentIndex == 0 && !IsRemove)
            {
                Indexs.Write(buffer.StartIndex);
                buffer.Buffer = null;
                return true;
            }
            Indexs.Write(buffer.StartIndex);
            buffer.Buffer = null;
            if (IsRemove && Indexs.IsFreeSize == 0) FreeIndex();
            return false;
        }
        /// <summary>
        /// 移除需要清除的字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void Remove(ref ByteArrayBuffer buffer)
        {
            Indexs.Write(buffer.StartIndex);
            buffer.Buffer = null;
            if (Indexs.IsFreeSize == 0) FreeIndex();
        }
        /// <summary>
        /// 尝试移除需要清除的字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TryRemoveGet(ref ByteArrayBuffer buffer)
        {
            if (IsRemove) Pool.notNull().TryRemoveGet(ref buffer);
        }

        /// <summary>
        /// 未使用缓存区数量排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int sortComparer(ByteArray left, ByteArray right)
        {
            return left.Indexs.CurrentIndex - right.Indexs.CurrentIndex;
        }
        /// <summary>
        /// 未使用缓存区数量排序
        /// </summary>
        internal static readonly Func<ByteArray, ByteArray, int> SortComparer = sortComparer;
    }
}
