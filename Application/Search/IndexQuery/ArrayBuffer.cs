using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 数组缓冲区
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ArrayBuffer<T>
    {
        /// <summary>
        /// 数据数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// 缓冲区池所在索引位置
        /// </summary>
        internal int PoolIndex;
        /// <summary>
        /// 数组缓冲区池数组
        /// </summary>
        private readonly ArrayBufferPoolArray<T> pool;
        /// <summary>
        /// 空数组缓冲区
        /// </summary>
        /// <param name="pool"></param>
        internal ArrayBuffer(ArrayBufferPoolArray<T> pool)
        {
            this.pool = pool;
            Array = EmptyArray<T>.Array;
            count = 0;
            PoolIndex = -1;
        }
        /// <summary>
        /// 数组缓冲区
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="array"></param>
        /// <param name="poolIndex"></param>
        internal ArrayBuffer(ArrayBufferPoolArray<T> pool, T[] array, int poolIndex)
        {
            this.pool = pool;
            Array = array;
            count = 0;
            PoolIndex = poolIndex;
        }
        /// <summary>
        /// 数组缓冲区
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="size">数组大小</param>
        internal ArrayBuffer(ArrayBufferPoolArray<T> pool, int size)
        {
            this.pool = pool;
            Array = new T[size];
            count = 0;
            PoolIndex = -1;
        }
        /// <summary>
        /// 获取数组子串并清除有效数据数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LeftArray<T> GetLeftArray()
        {
            int count = this.count;
            this.count = 0;
            return new LeftArray<T>(count, Array);
        }
        /// <summary>
        /// 获取数组子串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public IEnumerable<LeftArray<T>> GetLeftArray(int length)
        {
            for (int index = 0, count = this.count - index; count > 0; count = this.count - index)
            {
                if (count > length) count = length;
                if (index != 0) System.Array.Copy(Array, index, Array, 0, count);
                yield return new LeftArray<T>(count, Array);
                index += count;
            }
            this.count = 0;
        }
        /// <summary>
        /// 释放数组缓冲区
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Free()
        {
            if (PoolIndex >= 0)
            {
                pool.Free(ref this);
                PoolIndex = -1;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeAdd(T value)
        {
            Array[count++] = value;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            if (count != Array.Length)
            {
                Array[count++] = value;
                return;
            }
            if (PoolIndex >= 0)
            {
                ArrayBuffer<T> newBuffer;
                if (pool.GetBuffer(PoolIndex + 1, out newBuffer))
                {
                    AutoCSer.Common.CopyTo(Array, newBuffer.Array);
                    Array = newBuffer.Array;
                    ++PoolIndex;
                    Array[count++] = value;
                    return;
                }
                PoolIndex = -1;
            }
            Array = AutoCSer.Common.GetCopyArray(Array, count << 1);
            Array[count++] = value;
        }
        /// <summary>
        /// 设置有效数据数量
        /// </summary>
        /// <param name="count"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCount(int count)
        {
            this.count = count;
        }
        /// <summary>
        /// 获取有效数据数量并设置为 0
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetCount()
        {
            int count = this.count;
            this.count = 0;
            return count;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CopyFrom(T[] array)
        {
            AutoCSer.Common.CopyTo(array, Array);
            count = array.Length;
        }
    }
}
