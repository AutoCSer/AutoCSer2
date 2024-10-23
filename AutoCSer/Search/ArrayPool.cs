using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 数组池参数
    /// </summary>
    internal static class ArrayPool
    {
        /// <summary>
        /// 节点池数组二进制位长度
        /// </summary>
        internal const int ArraySizeBit = 16;
        /// <summary>
        /// 节点池数组长度
        /// </summary>
        internal const int ArraySize = 1 << 16;
        /// <summary>
        /// 节点池数组最大索引位置
        /// </summary>
        internal const int ArraySizeAnd = ArraySize - 1;
    }
    /// <summary>
    /// 数组池
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ArrayPool<T> where T : struct
    {
        /// <summary>
        /// 数组池
        /// </summary>
        internal T[][] Pool;
        /// <summary>
        /// 当前分配数组
        /// </summary>
        private T[] currentArray;
        /// <summary>
        /// 空闲索引集合
        /// </summary>
        private int[] freeIndexs;
        /// <summary>
        /// 当前分配数组位置
        /// </summary>
        private int currentArrayIndex;
        /// <summary>
        /// 当前分配数组起始位置
        /// </summary>
        private int currentArrayBaseIndex;
        /// <summary>
        /// 池数组位置
        /// </summary>
        private int poolIndex;
        /// <summary>
        /// 当前空闲索引
        /// </summary>
        private int freeIndex;
        /// <summary>
        /// 数组池访问锁
        /// </summary>
        internal readonly object Lock;
        /// <summary>
        /// 数组池
        /// </summary>
        /// <param name="freeCount">空闲索引集合初始化数量</param>
        /// <param name="currentArrayIndex">当前分配数组位置</param>
        internal ArrayPool(int freeCount, int currentArrayIndex = 0)
        {
            Lock = new object();
            Pool = new T[][] { currentArray = new T[ArrayPool.ArraySize] };
            freeIndexs = new int[freeCount];
            poolIndex = 1;
            this.currentArrayIndex = currentArrayIndex;
            currentArrayBaseIndex = freeIndex = 0;
        }
        /// <summary>
        /// 获取可用索引
        /// </summary>
        /// <param name="array">当前分配数组</param>
        /// <returns>索引</returns>
        internal int GetNoLock(out T[] array)
        {
            if (currentArrayIndex != ArrayPool.ArraySize)
            {
                array = currentArray;
                return currentArrayIndex++ + currentArrayBaseIndex;
            }
            if (freeIndex != 0)
            {
                int index = freeIndexs[--freeIndex];
                array = Pool[index >> ArrayPool.ArraySizeBit];
                return index;
            }
            create();
            array = currentArray;
            return currentArrayBaseIndex;
        }
        /// <summary>
        /// 创建当前分配数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void create()
        {
            if (poolIndex == Pool.Length) Pool = AutoCSer.Common.Config.GetCopyArray(Pool, poolIndex << 1);
            Pool[poolIndex++] = currentArray = new T[ArrayPool.ArraySize];
            currentArrayIndex = 1;
            currentArrayBaseIndex += ArrayPool.ArraySize;
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="indexs"></param>
        internal unsafe void FreeNoLock<keyType>(KeyValue<keyType, int>[] indexs)
        {
            int count = freeIndex + indexs.Length;
            if (count > freeIndexs.Length) freeIndexs = AutoCSer.Common.Config.GetUninitializedArray(freeIndexs, (int)((uint)count).upToPower2(), freeIndex);
            fixed (int* indexFixed = freeIndexs)
            {
                int* write = indexFixed + freeIndex;
                foreach (KeyValue<keyType, int> index in indexs) *write++ = index.Value;
            }
            freeIndex = count;
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void FreeNoLock(int index)
        {
            if (freeIndex == freeIndexs.Length) freeIndexs = AutoCSer.Common.Config.GetUninitializedArray(freeIndexs, freeIndex << 1); 
            freeIndexs[freeIndex++] = index;
        }
        /// <summary>
        /// 释放索引
        /// </summary>
        /// <param name="indexs"></param>
        internal void FreeNoLock(ICollection<int> indexs)
        {
            int count = indexs.Count;
            if (count != 0)
            {
                if ((count += freeIndex) > freeIndexs.Length) freeIndexs = AutoCSer.Common.Config.GetUninitializedArray(freeIndexs, (int)((uint)count).upToPower2(), freeIndex);
                foreach (int index in indexs) freeIndexs[freeIndex++] = index;
            }
        }
    }
}
