using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 可重用哈希表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BufferHashSet<T> : ReusableHashSet<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 可重用哈希表缓冲区池数组
        /// </summary>
        private HashSetPool<T>[] pools;
        /// <summary>
        /// 当前可重用哈希表缓冲区池
        /// </summary>
#if NetStandard21
        private HashSetPool<T>? pool;
#else
        private HashSetPool<T> pool;
#endif
        /// <summary>
        /// 可重用哈希表
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        internal BufferHashSet(int capacity = 0) : base(capacity)
        {
            pools = EmptyArray<HashSetPool<T>>.Array;
        }
        /// <summary>
        /// 可重用哈希表
        /// </summary>
        /// <param name="pools"></param>
        /// <param name="pool"></param>
        /// <param name="nodes"></param>
        internal BufferHashSet(HashSetPool<T>[] pools, HashSetPool<T> pool, ReusableHashNode<T>[] nodes) : base(int.MinValue)
        {
            this.pools = pools;
            this.pool = pool;
            CapacityDivision = pool.CapacityDivision;
            Nodes = nodes;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        protected override void resize()
        {
            var historyPool = this.pool;
            ReusableHashNode<T>[] nodes = Nodes;
            var pool = HashSetPool<T>.Get(pools, (int)CapacityDivision.Divisor + 1);
            int rollIndex = this.rollIndex;
            if (pool != null)
            {
                Nodes = pool.GetBuffer();
                CapacityDivision = pool.CapacityDivision;
                this.pool = pool;
                resize(nodes, rollIndex);
            }
            else
            {
                int capacity = GetResizeCapacity((int)CapacityDivision.Divisor);
                Nodes = new ReusableHashNode<T>[capacity];
                CapacityDivision.Set(capacity);
                this.pool = null;
                resize(nodes, rollIndex);
                pools = EmptyArray<HashSetPool<T>>.Array;
            }
            if (historyPool != null) historyPool.Free(nodes);
        }
        /// <summary>
        /// 释放可重用哈希表
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            pool?.Free(this);
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private void copyTo(ref ArrayBuffer<T> buffer)
        {
            int count = Count;
            foreach (ReusableHashNode<T> node in Nodes)
            {
                buffer.UnsafeAdd(node.Value);
                if (--count == 0) return;
            }
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        internal ArrayBuffer<T> GetArrayBuffer(QueryCondition<T> condition)
        {
            if (Count != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(Count);
                copyTo(ref buffer);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        internal ArrayBuffer<T> GetArrayBuffer(MemoryIndex.QueryCondition<T> condition)
        {
            if (Count != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(Count);
                copyTo(ref buffer);
                return buffer;
            }
            return condition.GetNullBuffer();
        }
    }
}
