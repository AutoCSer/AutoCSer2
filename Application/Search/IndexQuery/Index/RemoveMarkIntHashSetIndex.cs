using AutoCSer.CommandService.Search.DiskBlockIndex;
using System;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 哈希表索引
    /// </summary>
    internal sealed class RemoveMarkIntHashSetIndex : IIndex<int>
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
        internal RemoveMarkIntHashSetIndex(RemoveMarkHashSet hashSet)
        {
            this.hashSet = hashSet;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(int value)
        {
            return hashSet.Contains(value);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="bufferHashSet"></param>
        public void Get(BufferHashSet<int> bufferHashSet)
        {
            this.hashSet.Get(bufferHashSet);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public unsafe ArrayBuffer<int> Get(QueryCondition<int> condition)
        {
            int count = hashSet.Count;
            if (count != 0)
            {
                ArrayBuffer<int> buffer = condition.GetBuffer(count);
                fixed (int* bufferFixed = buffer.Array) hashSet.GetBuffer((uint*)bufferFixed);
                buffer.SetCount(count);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
}
