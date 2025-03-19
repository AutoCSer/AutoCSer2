using AutoCSer.Algorithm;
using System;
using System.Collections.Generic;
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
        private LeftArray<ReusableHashNode<T>[]> buffers;
        /// <summary>
        /// 可重用哈希表集合
        /// </summary>
        private LeftArray<BufferHashSet<T>> hashSets;
        /// <summary>
        /// 哈希取余
        /// </summary>
        internal IntegerDivision CapacityDivision;
        /// <summary>
        /// 是否申请了新的缓冲区
        /// </summary>
        private bool isGetNewBuffer;
        /// <summary>
        /// 是否申请了新的缓冲区
        /// </summary>
        private bool isGetNewHashSet;
        /// <summary>
        /// 可重用哈希表缓冲区池
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        private HashSetPool(int capacity)
        {
            CapacityDivision.Set(capacity);
            buffers = new LeftArray<ReusableHashNode<T>[]>(0);
            hashSets = new LeftArray<BufferHashSet<T>>(0);
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <returns></returns>
        internal ReusableHashNode<T>[] GetBuffer()
        {
            if (buffers.Length != 0)
            {
                var buffer = default(ReusableHashNode<T>[]);
                Monitor.Enter(this);
                if (buffers.TryPop(out buffer))
                {
                    Monitor.Exit(this);
                    return buffer;
                }
                Monitor.Exit(this);
            }
            isGetNewBuffer = true;
            return new ReusableHashNode<T>[CapacityDivision.Divisor];
        }
        /// <summary>
        /// 释放数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void Free(ReusableHashNode<T>[] buffer)
        {
            Monitor.Enter(this);
            try
            {
                buffers.Add(buffer);
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
            var hashSet = default(BufferHashSet<T>);
            var buffer = default(ReusableHashNode<T>[]);
            Monitor.Enter(this);
            if (hashSets.TryPop(out hashSet))
            {
                Monitor.Exit(this);
                hashSet.Clear();
                return hashSet;
            }
            if (buffers.TryPop(out buffer))
            {
                Monitor.Exit(this);
                isGetNewHashSet = true;
                return new BufferHashSet<T>(pools, this, buffer);
            }
            Monitor.Exit(this);
            isGetNewHashSet = true;
            return new BufferHashSet<T>(pools, this, new ReusableHashNode<T>[CapacityDivision.Divisor]);
        }
        /// <summary>
        /// 释放可重用哈希表
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Free(BufferHashSet<T> hashSet)
        {
            Monitor.Enter(this);
            try
            {
                hashSets.Add(hashSet);
            }
            finally { Monitor.Exit(this); }
        }
        /// <summary>
        /// 释放部分缓冲区
        /// </summary>
        public void FreeCache()
        {
            if (isGetNewBuffer)
            {
                Monitor.Enter(this);
                buffers.ClearCache();
                Monitor.Exit(this);
                isGetNewBuffer = false;
            }
            if (isGetNewHashSet)
            {
                Monitor.Enter(this);
                hashSets.ClearCache();
                Monitor.Exit(this);
                isGetNewHashSet = false;
            }
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
        /// <summary>
        /// 释放部分缓冲区
        /// </summary>
        /// <param name="pools"></param>
        public static void FreeCache(HashSetPool<T>[] pools)
        {
            foreach (HashSetPool<T> pool in pools) pool.FreeCache();
        }
    }
}
