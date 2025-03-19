using AutoCSer.CommandService.Search.DiskBlockIndex;
using System;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 哈希表索引
    /// </summary>
    internal sealed class RemoveMarkHashSetIndex : IIndex<uint>
    {
        /// <summary>
        /// 索引数据
        /// </summary>
        private RemoveMarkHashSet hashSet;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get { return hashSet.Count; } }
        /// <summary>
        /// 哈希表索引
        /// </summary>
        /// <param name="hashSet">索引数据</param>
        internal RemoveMarkHashSetIndex(RemoveMarkHashSet hashSet)
        {
            this.hashSet = hashSet;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(uint value)
        {
            return hashSet.Contains(value);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="bufferHashSet"></param>
        public void Get(BufferHashSet<uint> bufferHashSet)
        {
            hashSet.Get(bufferHashSet);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public unsafe ArrayBuffer<uint> Get(QueryCondition<uint> condition)
        {
            int count = hashSet.Count;
            if (count != 0)
            {
                ArrayBuffer<uint> buffer = condition.GetBuffer(count);
                fixed (uint* bufferFixed = buffer.Array) hashSet.GetBuffer(bufferFixed);
                buffer.SetCount(count);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
    /// <summary>
    /// 哈希表索引
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    internal sealed class RemoveMarkHashSetIndex<T> : IIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引数据
        /// </summary>
        private readonly RemoveMarkHashSet<T> hashSet;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get { return hashSet.Count; } }
        /// <summary>
        /// 哈希表索引
        /// </summary>
        /// <param name="hashSet">索引数据</param>
        internal RemoveMarkHashSetIndex(RemoveMarkHashSet<T> hashSet)
        {
            this.hashSet = hashSet;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return hashSet.Contains(value);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="bufferHashSet"></param>
        public void Get(BufferHashSet<T> bufferHashSet)
        {
            this.hashSet.Get(bufferHashSet);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> Get(QueryCondition<T> condition)
        {
            int count = hashSet.Count;
            if (count != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(count);
                foreach (RemoveMarkHashNode<T> node in hashSet.Nodes)
                {
                    buffer.UnsafeAdd(node.Value);
                    if (--count == 0) break;
                }
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
}
