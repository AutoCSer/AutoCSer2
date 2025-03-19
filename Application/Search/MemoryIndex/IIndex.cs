using AutoCSer.CommandService.Search.IndexQuery;
using System;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 索引接口
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    internal interface IIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 数据数量
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(T value);
        /// <summary>
        /// 判断是否全部包含于另一个集合
        /// </summary>
        /// <param name="index">另一个集合</param>
        /// <returns></returns>
        bool AllIn(IIndex<T> index);
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> Get(QueryCondition<T> condition);
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> GetFilter(QueryCondition<T> condition);
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        void Get(BufferHashSet<T> hashSet);
    }
}
