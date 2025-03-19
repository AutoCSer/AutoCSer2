using AutoCSer.CommandService.Search.IndexQuery;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 查询上下文
    /// </summary>
    /// <typeparam name="KT">查询数据关键字类型</typeparam>
    /// <typeparam name="VT">查询条件数据</typeparam>
    public interface IQueryContext<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys">关键字集合</param>
        /// <param name="isValue">条件委托</param>
        /// <param name="buffer">关键字集合</param>
        /// <returns></returns>
        ArrayBuffer<KT> Filter(IEnumerable<KT> keys, Func<VT, bool> isValue, ArrayBuffer<KT> buffer);
        /// <summary>
        /// 空数组缓冲区
        /// </summary>
        ArrayBuffer<KT> NullBuffer { get; }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="size">数组最小容量</param>
        /// <returns></returns>
        ArrayBuffer<KT> GetBuffer(int size);
        /// <summary>
        /// 关键字可重用哈希表缓冲区池
        /// </summary>
        HashSetPool<KT>[] HashSetPool { get; }
    }
}
