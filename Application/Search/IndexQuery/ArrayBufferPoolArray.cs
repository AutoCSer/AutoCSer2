using AutoCSer.Extensions;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 数组缓冲区池数组
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ArrayBufferPoolArray<T>
    {
        /// <summary>
        /// 数组缓冲区池集合
        /// </summary>
        private readonly ArrayBufferPool<T>[] pools;
        /// <summary>
        /// 数组大小起始二进制位数
        /// </summary>
        private readonly int startBit;
        /// <summary>
        /// 数组缓冲区池数组
        /// </summary>
        /// <param name="startBit">数组大小起始二进制位数，建议为 10</param>
        /// <param name="endBit">数组大小结束二进制位数，默认为最大值 30</param>
        public ArrayBufferPoolArray(int startBit, int endBit = 30)
        {
            this.startBit = Math.Min(Math.Max(startBit, 4), 30);
            pools = new ArrayBufferPool<T>[Math.Max((Math.Min(endBit, 30) - this.startBit), 0) + 1];
            for (int index = 0; index != pools.Length; ++index) pools[index] = new ArrayBufferPool<T>(index + this.startBit);
        }
        /// <summary>
        /// 释放数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free(ref ArrayBuffer<T> buffer)
        {
            pools[buffer.PoolIndex].Free(ref buffer.Array);
        }
        ///// <summary>
        ///// 获取空缓冲区
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public ArrayBuffer<T> GetNull()
        //{
        //    return new ArrayBuffer<T>(this);
        //}
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="size">数组最小容量</param>
        /// <returns></returns>
        public ArrayBuffer<T> GetBuffer(int size)
        {
            int bit = ((uint)size).bits() - 1;
            if ((1 << bit) != size) ++bit;
            int index = Math.Max(bit - startBit, 0);
            if (index < pools.Length) return pools[index].GetBuffer(this, index);
            return new ArrayBuffer<T>(this, size);
        }
        /// <summary>
        /// 根据缓冲区池索引位置获取数组缓冲区
        /// </summary>
        /// <param name="poolIndex"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal bool GetBuffer(int poolIndex, out ArrayBuffer<T> buffer)
        {
            if (poolIndex < pools.Length)
            {
                buffer = pools[poolIndex].GetBuffer(this, poolIndex);
                return true;
            }
            buffer = default(ArrayBuffer<T>);
            return false;
        }
        /// <summary>
        /// 获取空数组缓冲区
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ArrayBuffer<T> GetNull()
        {
            return new ArrayBuffer<T>(this);
        }
    }
}
