using AutoCSer.CommandService.Search.DiskBlockIndex;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.IndexQuery
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
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        void Get(BufferHashSet<T> hashSet);
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> Get(QueryCondition<T> condition);
    }
}
