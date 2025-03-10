using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 索引条件接口
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public interface IIndexCondition<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 预估数据数量
        /// </summary>
        int EstimatedCount { get; }
        /// <summary>
        /// 是否已经加载数据
        /// </summary>
        bool IsLoaded { get; }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<ArrayBuffer<T>> Get(QueryCondition<T> condition);
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<ArrayBuffer<T>> GetFilter(QueryCondition<T> condition);
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult> Load();
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(T value);
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="hashSet"></param>
        void GetLoaded(QueryCondition<T> condition, BufferHashSet<T> hashSet);
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> GetLoaded(QueryCondition<T> condition);
    }
}
