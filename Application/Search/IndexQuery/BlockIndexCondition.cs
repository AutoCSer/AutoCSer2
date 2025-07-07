using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 索引条件
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    public sealed class BlockIndexCondition<T> : IIndexCondition<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 关键字数据磁盘块索引信息节点
        /// </summary>
        private IndexNode<T> node;
        /// <summary>
        /// 预估数据数量
        /// </summary>
        public int EstimatedCount { get { return node.EstimatedCount; } }
        /// <summary>
        /// 是否已经加载数据
        /// </summary>
        bool IIndexCondition<T>.IsLoaded { get { return node.IsLoaded; } }
        /// <summary>
        /// 索引条件
        /// </summary>
        /// <param name="node">关键字数据磁盘块索引信息节点集合</param>
        public BlockIndexCondition(IndexNode<T> node)
        {
            this.node = node;
        }
        /// <summary>
        /// 索引条件
        /// </summary>
        /// <param name="node">关键字数据磁盘块索引信息节点集合</param>
        public BlockIndexCondition(BlockIndexDataCacheNode<T> node)
        {
            this.node.Set(node);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public Task<ArrayBuffer<T>> Get(QueryCondition<T> condition)
        {
            return node.IsLoaded ? Task.FromResult(node.Index.Get(condition)) : get(condition);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        private async Task<ArrayBuffer<T>> get(QueryCondition<T> condition)
        {
            ResponseResult<IIndex<T>> result = await node.Node.Load();
            if (result.IsSuccess) return result.Value.notNull().Get(condition);
            return condition.GetNullBuffer().Result;
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        async Task<ArrayBuffer<T>> IIndexCondition<T>.GetFilter(QueryCondition<T> condition)
        {
            return await condition.Filter(await Get(condition));
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <returns></returns>
        async Task<ResponseResult> IIndexCondition<T>.Load()
        {
            ResponseResult<IIndex<T>> result = await node.Node.Load();
            if (result.IsSuccess)
            {
                node.Index = result.Value.notNull();
                return CallStateEnum.Success;
            }
            return result;
        }
        /// <summary>
        /// 判断是否包含数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IIndexCondition<T>.Contains(T value) { return node.Index.Contains(value); }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="hashSet"></param>
        void IIndexCondition<T>.GetLoaded(QueryCondition<T> condition, BufferHashSet<T> hashSet) { node.Index.Get(hashSet); }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> IIndexCondition<T>.GetLoaded(QueryCondition<T> condition) { return node.Index.Get(condition); }
    }
}
