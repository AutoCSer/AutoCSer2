using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    public abstract class BlockIndexDataCache<T> : IDisposable
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 缓存访问锁
        /// </summary>
        internal readonly object CacheLock;
        /// <summary>
        /// 获取更新关键字集合保持回调
        /// </summary>
#if NetStandard21
        protected IDisposable? getChangeKeyKeepCallback;
#else
        protected IDisposable getChangeKeyKeepCallback;
#endif
        /// <summary>
        /// 最大缓存数据数量
        /// </summary>
        protected long maxCount;
        /// <summary>
        /// 获取更新关键字集合保持回调版本
        /// </summary>
        internal int GetChangeKeyVersion;
        /// <summary>
        /// 是否释放资源
        /// </summary>
        protected bool isDispose;
        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="maxCount">最大缓存数据数量</param>
        protected BlockIndexDataCache(long maxCount)
        {
            this.maxCount = Math.Max(maxCount, 1);
            CacheLock = new object();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            isDispose = true;
            getChangeKeyKeepCallback?.Dispose();
        }
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <returns></returns>
        protected abstract Task getChangeKeys();
        /// <summary>
        /// 设置获取更新关键字集合保持回调
        /// </summary>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
#if NetStandard21
        protected bool setGetChangeKeyKeepCallback(IDisposable? keepCallback)
#else
        protected bool setGetChangeKeyKeepCallback(IDisposable keepCallback)
#endif
        {
            if (keepCallback != null)
            {
                getChangeKeyKeepCallback?.Dispose();
                ++GetChangeKeyVersion;
                getChangeKeyKeepCallback = keepCallback;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected async Task<ResponseResult<IIndexCondition<T>>> loadCondition(BlockIndexDataCacheNode<T> node)
        {
            ResponseResult result = await node.GetBlockIndexData();
            if (result.IsSuccess) return new BlockIndexCondition<T>(node);
            return result;
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="indexMergeType"></param>
        /// <returns></returns>
        protected async Task<ResponseResult<IIndexCondition<T>>> loadCondition(IndexNode<T>[] nodes, IndexMergeTypeEnum indexMergeType)
        {
            foreach (IndexNode<T> node in nodes)
            {
                if (node.Node.Type == IndexDataTypeEnum.None)
                {
                    ResponseResult result = await node.Node.GetBlockIndexData();
                    if (!result.IsSuccess) return result;
                }
            }
            return new BlockIndexArrayCondition<T>(nodes, indexMergeType);
        }
        /// <summary>
        /// 添加缓存数据数量
        /// </summary>
        /// <param name="node"></param>
        /// <param name="count"></param>
        internal void AddValueCount(BlockIndexDataCacheNode<T> node, int count)
        {
            Monitor.Enter(CacheLock);
            if (!node.IsRemoved && (maxCount -= count) < 0)
            {
                try
                {
                    remove();
                }
                finally { Monitor.Exit(CacheLock); }
                return;
            }
            Monitor.Exit(CacheLock);
        }
        /// <summary>
        /// 移除缓存数据数量
        /// </summary>
        /// <param name="node"></param>
        /// <param name="count"></param>
        internal void RemoveValueCount(BlockIndexDataCacheNode<T> node, int count)
        {
            Monitor.Enter(CacheLock);
            if (!node.IsRemoved) maxCount -= count;
            Monitor.Exit(CacheLock);
        }
        /// <summary>
        /// 淘汰缓存数据
        /// </summary>
        protected abstract void remove();
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        public abstract IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex);
    }
    /// <summary>
    /// 索引数据磁盘块索引缓存
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public abstract class BlockIndexDataCache<KT, VT> : BlockIndexDataCache<VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 节点缓存加载访问锁集合
        /// </summary>
        private readonly Dictionary<KT, SemaphoreSlimCache> loadLocks;

        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="maxCount">最大缓存数据数量</param>
        protected BlockIndexDataCache(long maxCount) : base(maxCount)
        {
            loadLocks = DictionaryCreator<KT>.Create<SemaphoreSlimCache>();
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<KT, VT> node);
        /// <summary>
        /// 获取节点缓存加载访问锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal SemaphoreSlimCache GetSemaphoreSlimCache(KT key)
        {
            var semaphoreSlim = default(SemaphoreSlimCache);
            Monitor.Enter(loadLocks);
            try
            {
                if (!loadLocks.TryGetValue(key, out semaphoreSlim)) loadLocks.Add(key, semaphoreSlim = SemaphoreSlimCache.Get());
                ++semaphoreSlim.Count;
            }
            finally { Monitor.Exit(loadLocks); }
            return semaphoreSlim;
        }
        /// <summary>
        /// 释放获取节点缓存加载访问锁
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        /// <param name="key"></param>
        internal void Release(SemaphoreSlimCache semaphoreSlim, KT key)
        {
            Monitor.Enter(loadLocks);
            if (--semaphoreSlim.Count == 0)
            {
                try
                {
                    loadLocks.Remove(key);
                }
                finally { Monitor.Exit(loadLocks); }
                SemaphoreSlimCache.Free(semaphoreSlim);
            }
            else Monitor.Exit(loadLocks);
        }
    }
}
