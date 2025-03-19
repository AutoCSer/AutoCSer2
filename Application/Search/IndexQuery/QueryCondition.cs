using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public abstract class QueryCondition<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public abstract Task<ArrayBuffer<T>> Filter(ArrayBuffer<T> keys);
        /// <summary>
        /// 获取空数组缓冲区
        /// </summary>
        /// <returns></returns>
        public abstract Task<ArrayBuffer<T>> GetNullBuffer();
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="size">数组最小容量</param>
        /// <returns></returns>
        public abstract ArrayBuffer<T> GetBuffer(int size);
        /// <summary>
        /// 获取可重用哈希表
        /// </summary>
        /// <returns></returns>
        public abstract HashSetPool<T>[] GetHashSetPool();
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <typeparam name="KT">查询数据关键字类型</typeparam>
    /// <typeparam name="VT">查询条件数据</typeparam>
    public sealed class QueryCondition<KT, VT> : QueryCondition<KT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 查询上下文
        /// </summary>
        private readonly IQueryContext<KT, VT> context;
        /// <summary>
        /// 索引条件集合
        /// </summary>
        private readonly IIndexCondition<KT> indexCondition;
        /// <summary>
        /// 非索引条件
        /// </summary>
#if NetStandard21
        private readonly Func<VT, bool>? condition;
#else
        private readonly Func<VT, bool> condition;
#endif
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <param name="indexCondition">索引条件</param>
        /// <param name="condition">非索引条件</param>
#if NetStandard21
        public QueryCondition(IQueryContext<KT, VT> context, IIndexCondition<KT> indexCondition, Func<VT, bool>? condition)
#else
        public QueryCondition(IQueryContext<KT, VT> context, IIndexCondition<KT> indexCondition, Func<VT, bool> condition)
#endif
        {
            this.context = context;
            this.indexCondition = indexCondition;
            this.condition = condition;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <param name="indexs">索引条件集合</param>
        /// <param name="condition">非索引条件</param>
#if NetStandard21
        public QueryCondition(IQueryContext<KT, VT> context, LeftArray<IIndexCondition<KT>> indexs, Func<VT, bool>? condition)
#else
        public QueryCondition(IQueryContext<KT, VT> context, LeftArray<IIndexCondition<KT>> indexs, Func<VT, bool> condition)
#endif
        {
            switch (indexs.Length)
            {
                case 0: throw new ArgumentNullException();
                case 1: indexCondition = indexs.Array[0]; break;
                default: indexCondition = new IndexArrayCondition<KT>(indexs); break;
            }
            this.context = context;
            this.condition = condition;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <param name="indexs">索引条件集合</param>
        /// <param name="condition">非索引条件</param>
#if NetStandard21
        public QueryCondition(IQueryContext<KT, VT> context, IIndexCondition<KT>[] indexs, Func<VT, bool>? condition) 
#else
        public QueryCondition(IQueryContext<KT, VT> context, IIndexCondition<KT>[] indexs, Func<VT, bool> condition)
#endif
            : this(context, new LeftArray<IIndexCondition<KT>>(indexs), condition) { }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ArrayBuffer<KT>> Query(PageParameter queryParameter)
        {
            return condition != null ? indexCondition.GetFilter(this) : indexCondition.Get(this);
        }
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public override Task<ArrayBuffer<KT>> Filter(ArrayBuffer<KT> keys)
        {
            return context.Filter(keys, condition.notNull());
        }
        /// <summary>
        /// 获取空数组缓冲区
        /// </summary>
        /// <returns></returns>
        public override Task<ArrayBuffer<KT>> GetNullBuffer()
        {
            return context.NullBufferTask;
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="size">数组最小容量</param>
        /// <returns></returns>
        public override ArrayBuffer<KT> GetBuffer(int size)
        {
            return context.GetBuffer(size);
        }
        /// <summary>
        /// 获取可重用哈希表
        /// </summary>
        /// <returns></returns>
        public override HashSetPool<KT>[] GetHashSetPool()
        {
            return context.HashSetPool;
        }
    }
}
