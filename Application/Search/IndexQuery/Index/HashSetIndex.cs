using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 哈希表索引
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    internal sealed class HashSetIndex<T> : IIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引数据
        /// </summary>
        internal readonly HashSet<T> HashSet;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get { return HashSet.Count; } }
        /// <summary>
        /// 哈希表索引
        /// </summary>
        /// <param name="hashSet">索引数据</param>
        internal HashSetIndex(HashSet<T> hashSet)
        {
            HashSet = hashSet;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return HashSet.Contains(value);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="bufferHashSet"></param>
        public void Get(BufferHashSet<T> bufferHashSet)
        {
            foreach (T value in HashSet) bufferHashSet.Add(value);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> Get(QueryCondition<T> condition)
        {
            if (HashSet.Count != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(HashSet.Count);
                foreach (T value in HashSet) buffer.UnsafeAdd(value);
                return buffer;
            }
            return condition.GetNullBuffer().Result;
        }
    }
}
