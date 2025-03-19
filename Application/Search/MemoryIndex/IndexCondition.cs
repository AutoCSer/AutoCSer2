using AutoCSer.CommandService.Search.IndexQuery;
using System;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 索引条件
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public sealed class IndexCondition<T> : IIndexCondition<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引
        /// </summary>
        private readonly IIndex<T> index;
        /// <summary>
        /// 预估数据数量
        /// </summary>
        public int EstimatedCount { get { return index.Count; } }
        /// <summary>
        /// 索引条件
        /// </summary>
        /// <param name="index">索引</param>
        internal IndexCondition(IIndex<T> index)
        {
            this.index = index;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IIndexCondition<T>.Contains(T value)
        {
            return index.Contains(value);
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="hashSet"></param>
        void IIndexCondition<T>.Get(QueryCondition<T> condition, BufferHashSet<T> hashSet) { index.Get(hashSet); }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> Get(QueryCondition<T> condition)
        {
            return index.Get(condition);
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ArrayBuffer<T> GetFilter(QueryCondition<T> condition)
        {
            return index.GetFilter(condition);
        }
    }
}
