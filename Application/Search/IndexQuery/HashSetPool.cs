using AutoCSer.Algorithm;
using System;
using System.Threading;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 可重用哈希表缓冲区池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HashSetPool<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 可重用哈希表缓冲区数组
        /// </summary>
        private ReusableHashNode<T>[][] buffers;
        /// <summary>
        /// 可重用哈希表集合
        /// </summary>
        private BufferHashSet<T>[] hashSets;
        /// <summary>
        /// 哈希取余
        /// </summary>
        internal IntegerDivision CapacityDivision;
        /// <summary>
        /// 空闲数组缓冲区数量
        /// </summary>
        private int count;
        /// <summary>
        /// 空闲数组缓冲区数量
        /// </summary>
        private int hashSetCount;
        /// <summary>
        /// 可重用哈希表缓冲区池
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        private HashSetPool(int capacity)
        {
            CapacityDivision.Set(capacity);
            buffers = EmptyArray<ReusableHashNode<T>[]>.Array;
            hashSets = EmptyArray<BufferHashSet<T>>.Array;
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <returns></returns>
        internal ReusableHashNode<T>[] GetBuffer()
        {
            Monitor.Enter(this);
            if (count != 0)
            {
                ReusableHashNode<T>[] buffer = buffers[--count];
                Monitor.Exit(this);
                return buffer;
            }
            Monitor.Exit(this);
            return new ReusableHashNode<T>[CapacityDivision.Divisor];
        }
        /// <summary>
        /// 释放数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void Free(ReusableHashNode<T>[] buffer)
        {
            Monitor.Enter(this);
            if (count != buffers.Length)
            {
                buffers[count++] = buffer;
                Monitor.Exit(this);
                return;
            }
            try
            {
                if (count != 0) buffers = AutoCSer.Common.GetCopyArray(buffers, count << 1);
                else buffers = new ReusableHashNode<T>[sizeof(int)][];
                buffers[count++] = buffer;
            }
            finally { Monitor.Exit(this); }
        }
        /// <summary>
        /// 获取可重用哈希表
        /// </summary>
        /// <param name="pools"></param>
        /// <returns></returns>
        private BufferHashSet<T> GetHashSet(HashSetPool<T>[] pools)
        {
            Monitor.Enter(this);
            if (hashSetCount != 0)
            {
                BufferHashSet<T> hashSet = hashSets[--hashSetCount];
                Monitor.Exit(this);
                hashSet.Clear();
                return hashSet;
            }
            if (count != 0)
            {
                ReusableHashNode<T>[] buffer = buffers[--count];
                Monitor.Exit(this);
                return new BufferHashSet<T>(pools, this, buffer);
            }
            Monitor.Exit(this);
            return new BufferHashSet<T>(pools, this, new ReusableHashNode<T>[CapacityDivision.Divisor]);
        }
        /// <summary>
        /// 释放可重用哈希表
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Free(BufferHashSet<T> hashSet)
        {
            Monitor.Enter(this);
            if (hashSetCount != hashSets.Length)
            {
                hashSets[hashSetCount++] = hashSet;
                Monitor.Exit(this);
                return;
            }
            try
            {
                if (hashSetCount != 0) hashSets = AutoCSer.Common.GetCopyArray(hashSets, hashSetCount << 1);
                else hashSets = new BufferHashSet<T>[sizeof(int)];
                hashSets[hashSetCount++] = hashSet;
            }
            finally { Monitor.Exit(this); }
        }

        /// <summary>
        /// 获取可重用哈希表缓冲区池数组
        /// </summary>
        /// <param name="minCapacity">最小容器数组大小，建议为 1024</param>
        /// <param name="maxCapacity">最大容器数组大小，默认为最大值 0x7fffffc3</param>
        /// <returns></returns>
        public static HashSetPool<T>[] GetArray(int minCapacity, int maxCapacity = ReusableDictionary.MaxPrime)
        {
            LeftArray<HashSetPool<T>> array = new LeftArray<HashSetPool<T>>(0);
            int capacity = 3;
            do
            {
                if (capacity >= minCapacity && capacity <= maxCapacity) array.Add(new HashSetPool<T>(capacity));
                if (capacity == ReusableDictionary.MaxPrime) break;
                capacity = ReusableDictionary.GetResizeCapacity(capacity);
            }
            while (capacity <= maxCapacity);
            return array.ToArray();
        }
        /// <summary>
        /// 根据指定容器大小获取可重用哈希表缓冲区池
        /// </summary>
        /// <param name="pools"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
#if NetStandard21
        internal static HashSetPool<T>? Get(HashSetPool<T>[] pools, int capacity)
#else
        internal static HashSetPool<T> Get(HashSetPool<T>[] pools, int capacity)
#endif
        {
            foreach (HashSetPool<T> pool in pools)
            {
                if (pool.CapacityDivision.Divisor >= capacity) return pool;
            }
            return null;
        }
        /// <summary>
        /// 获取可重用哈希表
        /// </summary>
        /// <param name="pools"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        internal static BufferHashSet<T> GetHashSet(HashSetPool<T>[] pools, int capacity)
        {
            foreach (HashSetPool<T> pool in pools)
            {
                if (pool.CapacityDivision.Divisor >= capacity) return pool.GetHashSet(pools);
            }
            return new BufferHashSet<T>(capacity);
        }
    }
}
