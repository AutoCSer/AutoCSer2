using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    /// <typeparam name="NT">索引数据磁盘块索引缓存节点类型</typeparam>
    public abstract class GenericKeyCache<KT, VT, NT> : BlockIndexDataCache<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
        where NT : BlockIndexDataCacheNode<VT>
    {
        /// <summary>
        /// 索引缓存
        /// </summary>
        protected readonly ReusableDictionary<KT, NT> cache;
        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="capacity">容器初始化大小</param>
        protected GenericKeyCache(long maxCount, int capacity) : base(maxCount)
        {
            cache = new ReusableDictionary<KT, NT>(capacity);
        }
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        protected void getChangeKeys(ResponseResult<KT> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
#pragma warning disable CS8600
                KT key = result.Value;
#pragma warning restore CS8600
                var node = default(NT);
                Monitor.Enter(CacheLock);
                try
                {
#pragma warning disable CS8604
                    if (cache.TryGetValue(key, out node)) ++node.ChangeKeyVersion;
#pragma warning restore CS8604
                }
                finally { Monitor.Exit(CacheLock); }
                return;
            }
            command.Dispose();
            if (!isDispose) getChangeKeys().NotWait();
        }
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="result"></param>
        protected void getChangeKeys(LocalResult<KT> result)
        {
            if (result.IsSuccess)
            {
#pragma warning disable CS8600
                KT key = result.Value;
#pragma warning restore CS8600
                var node = default(NT);
                Monitor.Enter(CacheLock);
                try
                {
#pragma warning disable CS8604
                    if (cache.TryGetValue(key, out node)) ++node.ChangeKeyVersion;
#pragma warning restore CS8604
                }
                finally { Monitor.Exit(CacheLock); }
                return;
            }
            if (!isDispose) getChangeKeys().NotWait();
        }
        /// <summary>
        /// 淘汰缓存数据
        /// </summary>
        protected override void remove()
        {
            var node = default(NT);
            do
            {
                if (cache.RemoveRoll(out node)) maxCount += node.GetRemoveCacheValueCount() + 1;
                else return;
            }
            while (maxCount < 0);
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract NT createNode(KT key);
        /// <summary>
        /// 获取索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal NT GetNode(KT key)
        {
            var node = default(NT);
            Monitor.Enter(CacheLock);
            try
            {
                if (!cache.TryGetValue(key, out node, true))
                {
                    cache.Set(key, node = createNode(key));
                    --maxCount;
                }
            }
            finally { Monitor.Exit(CacheLock); }
            return node;
        }
        /// <summary>
        /// 获取索引数据磁盘块索引缓存节点集合
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        internal IndexNode<VT>[] GetNode(KT[] keys)
        {
            var node = default(NT);
            IndexNode<VT>[] nodes = new IndexNode<VT>[keys.Length];
            int index = 0;
            Monitor.Enter(CacheLock);
            try
            {
                foreach (KT key in keys)
                {
                    if (cache.TryGetValue(key, out node, true)) nodes[index++].Set(node);
                    else
                    {
                        cache.Set(key, node = createNode(key));
                        nodes[index++].Node = node;
                        --maxCount;
                    }
                }
            }
            finally { Monitor.Exit(CacheLock); }
            return nodes;
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="indexMergeType">索引合并操作类型</param>
        /// <returns></returns>
        public Task<ResponseResult<IIndexCondition<VT>>> GetCondition(KT[] keys, IndexMergeTypeEnum indexMergeType = IndexMergeTypeEnum.Intersection)
        {
            switch (keys.Length)
            {
                case 0: return BlockIndexDataCacheNode<VT>.NullIndexCondition;
                case 1:
                    NT node = GetNode(keys[0]);
                    if (node.Type != IndexDataTypeEnum.None) return Task.FromResult(new ResponseResult<IIndexCondition<VT>>(new BlockIndexCondition<VT>(node)));
                    return loadCondition(node);
                default:
                    IndexNode<VT>[] nodes = GetNode(keys);
                    foreach (IndexNode<VT> nextNode in nodes)
                    {
                        if (nextNode.Node.Type == IndexDataTypeEnum.None) return loadCondition(nodes, indexMergeType);
                    }
                    return Task.FromResult(new ResponseResult<IIndexCondition<VT>>(new BlockIndexArrayCondition<VT>(nodes, indexMergeType)));
            }
        }
    }
    /// <summary>
    /// 索引数据磁盘块索引缓存 KT:VT
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public abstract class GenericKeyCache<KT, VT> : GenericKeyCache<KT, VT, GenericIndex<KT, VT>>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashIndexNodeClientNode<KT, VT>> node;
        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="capacity">容器初始化大小</param>
        protected GenericKeyCache(StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashIndexNodeClientNode<KT, VT>> node, long maxCount, int capacity = 1 << 16) : base(maxCount, capacity)
        {
            this.node = node;
            getChangeKeys().NotWait();
        }
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <returns></returns>
        protected override async Task getChangeKeys()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                ResponseResult<IRemoveMarkHashIndexNodeClientNode<KT, VT>> nodeResult = await node.GetSynchronousNode();
                if (nodeResult.IsSuccess && setGetChangeKeyKeepCallback(await nodeResult.Value.notNull().GetChangeKeys(getChangeKeys))) return;
                await Task.Delay(1000);
            }
            while (!isDispose);
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点 KT:VT
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override GenericIndex<KT, VT> createNode(KT key)
        {
            return new GenericIndex<KT, VT>(this, key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<KT, VT> node)
        {
            ResponseResult<IRemoveMarkHashIndexNodeClientNode<KT, VT>> nodeResult = await this.node.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            do
            {
                int changeKeyVersion = node.ChangeKeyVersion, getChangeKeyVersion = this.GetChangeKeyVersion;
                ResponseResult<BlockIndexData<VT>> data = await nodeResult.Value.notNull().GetBlockIndexData(node.Key);
                if (data.IsSuccess)
                {
                    if (getChangeKeyVersion == this.GetChangeKeyVersion && node.Set(changeKeyVersion, getChangeKeyVersion, data.Value)) return CallStateEnum.Success;
                }
                else return data;
            }
            while (true);
        }
    }
}
