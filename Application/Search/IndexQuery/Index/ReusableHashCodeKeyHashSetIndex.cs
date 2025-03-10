using System;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 哈希表索引
    /// </summary>
    internal sealed class ReusableHashCodeKeyHashSetIndex : IIndex<uint>
    {
        /// <summary>
        /// 索引数据
        /// </summary>
        internal ReusableHashCodeKeyHashSet HashSet;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get { return HashSet.Count; } }
        /// <summary>
        /// 哈希表索引
        /// </summary>
        /// <param name="hashSet">索引数据</param>
        internal ReusableHashCodeKeyHashSetIndex(ReusableHashCodeKeyHashSet hashSet)
        {
            HashSet = hashSet;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(uint value)
        {
            return HashSet.Contains(value);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="bufferHashSet"></param>
        public void Get(BufferHashSet<uint> bufferHashSet)
        {
            int count = HashSet.Count;
            if (count != 0)
            {
                foreach (ReusableHashNode node in HashSet.Nodes)
                {
                    bufferHashSet.Add(node.HashCode);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<uint> Get(QueryCondition<uint> condition)
        {
            int count = HashSet.Count;
            if (count != 0)
            {
                ArrayBuffer<uint> buffer = condition.GetBuffer(count);
                foreach (ReusableHashNode node in HashSet.Nodes)
                {
                    buffer.UnsafeAdd(node.HashCode);
                    if (--count == 0) break;
                }
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
}
