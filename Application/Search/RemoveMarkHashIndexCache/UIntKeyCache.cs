using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Html;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存
    /// </summary>
    /// <typeparam name="VT">索引数据类型</typeparam>
    /// <typeparam name="NT">索引数据磁盘块索引缓存节点类型</typeparam>
    public abstract class UIntKeyCache<VT, NT> : BlockIndexDataCache<uint, VT>
#if NetStandard21
        where VT : notnull, IEquatable<VT>
#else
        where VT : IEquatable<VT>
#endif
        where NT : BlockIndexDataCacheNode<VT>
    {
        /// <summary>
        /// 索引缓存
        /// </summary>
        protected readonly ReusableHashCodeKeyDictionary<NT> cache;
        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        protected UIntKeyCache(long maxCount, int capacity) : base(maxCount)
        {
            cache = new ReusableHashCodeKeyDictionary<NT>(capacity, ReusableDictionaryGroupTypeEnum.Roll);
        }
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="key"></param>
        private void getChangeKeys(uint key)
        {
            var node = default(NT);
            Monitor.Enter(CacheLock);
            try
            {
                if (cache.TryGetValue(key, out node)) ++node.ChangeKeyVersion;
            }
            finally { Monitor.Exit(CacheLock); }
        }
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        protected void getChangeKeys(ResponseResult<uint> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                getChangeKeys(result.Value);
                return;
            }
            command.Dispose();
            if (!isDispose) getChangeKeys().Catch();
        }
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        protected void getChangeKeys(ResponseResult<int> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                getChangeKeys((uint)result.Value);
                return;
            }
            command.Dispose();
            if (!isDispose) getChangeKeys().Catch();
        }
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="result"></param>
        protected void getChangeKeys(LocalResult<uint> result)
        {
            if (result.IsSuccess)
            {
                getChangeKeys(result.Value);
                return;
            }
            if (!isDispose) getChangeKeys().Catch();
        }
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="result"></param>
        protected void getChangeKeys(LocalResult<int> result)
        {
            if (result.IsSuccess)
            {
                getChangeKeys((uint)result.Value);
                return;
            }
            if (!isDispose) getChangeKeys().Catch();
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
        protected abstract NT createNode(uint key);
        /// <summary>
        /// 获取索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal NT GetNode(uint key)
        {
            var node = default(NT);
            Monitor.Enter(CacheLock);
            try
            {
                if (!cache.TryGetValue(key, out node, true)) cache.Set(key, node = createNode(key));
            }
            finally { Monitor.Exit(CacheLock); }
            return node;
        }
        /// <summary>
        /// 获取索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal NT GetNode(int key)
        {
            return GetNode((uint)key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引缓存节点集合
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        internal IndexNode<VT>[] GetNode(uint[] keys)
        {
            var node = default(NT);
            IndexNode<VT>[] nodes = new IndexNode<VT>[keys.Length];
            int index = 0;
            Monitor.Enter(CacheLock);
            try
            {
                foreach (uint key in keys)
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
        /// 获取索引数据磁盘块索引缓存节点集合
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        internal IndexNode<VT>[] GetNode(int[] keys)
        {
            var node = default(NT);
            IndexNode<VT>[] nodes = new IndexNode<VT>[keys.Length];
            int index = 0;
            Monitor.Enter(CacheLock);
            try
            {
                foreach (int key in keys)
                {
                    if (cache.TryGetValue(key, out node, true)) nodes[index++].Set(node);
                    else
                    {
                        cache.Set(key, node = createNode((uint)key));
                        nodes[index++].Node = node;
                        --maxCount;
                    }
                }
            }
            finally { Monitor.Exit(CacheLock); }
            return nodes;
        }

        /// <summary>
        /// 获取文本搜索索引条件
        /// </summary>
        /// <param name="trieGraphNodeCache">字符串 Trie 图客户端节点接口</param>
        /// <param name="text">搜索文本</param>
        /// <param name="indexMergeType">索引合并操作类型</param>
        /// <returns>索引条件</returns>
        public async Task<ResponseResult<IIndexCondition<VT>>> GetCondition(StreamPersistenceMemoryDatabaseClientNodeCache<IStaticTrieGraphNodeClientNode> trieGraphNodeCache, string text, IndexMergeTypeEnum indexMergeType = IndexMergeTypeEnum.IntersectionNotEmpty)
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> trieGraphNode = await trieGraphNodeCache.GetNode();
            if (!trieGraphNode.IsSuccess) return trieGraphNode.Cast<IIndexCondition<VT>>();
            ResponseResult<int[]> wordIdentitysResult = await trieGraphNode.Value.notNull().GetWordSegmentIdentity(text);
            if (!wordIdentitysResult.IsSuccess) return wordIdentitysResult.Cast<IIndexCondition<VT>>();
            return await GetCondition(wordIdentitysResult.Value.notNull(), indexMergeType);
        }
        /// <summary>
        /// 获取文本搜索索引条件
        /// </summary>
        /// <param name="trieGraphNodeCache">字符串 Trie 图客户端节点接口</param>
        /// <param name="text">搜索文本</param>
        /// <param name="indexMergeType">索引合并操作类型</param>
        /// <returns>索引条件</returns>
        public async Task<ResponseResult<IIndexCondition<VT>>> GetCondition(StreamPersistenceMemoryDatabaseLocalClientNodeCache<IStaticTrieGraphNodeLocalClientNode> trieGraphNodeCache, string text, IndexMergeTypeEnum indexMergeType = IndexMergeTypeEnum.IntersectionNotEmpty)
        {
            LocalResult<IStaticTrieGraphNodeLocalClientNode> trieGraphNode = await trieGraphNodeCache.GetNode();
            if (!trieGraphNode.IsSuccess) return trieGraphNode.CallState;
            LocalResult<int[]> wordIdentitysResult = await trieGraphNode.Value.notNull().GetWordSegmentIdentity(text);
            if (!wordIdentitysResult.IsSuccess) return wordIdentitysResult.CallState;
            return await GetCondition(wordIdentitysResult.Value.notNull(), indexMergeType);
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="indexMergeType">索引合并操作类型</param>
        /// <returns></returns>
        public Task<ResponseResult<IIndexCondition<VT>>> GetCondition(uint[] keys, IndexMergeTypeEnum indexMergeType = IndexMergeTypeEnum.Intersection)
        {
            switch (keys.Length)
            {
                case 0: return BlockIndexDataCacheNode<VT>.NullIndexCondition;
                case 1: return getCondition(GetNode(keys[0]));
                default: return getCondition(GetNode(keys), indexMergeType);
            }
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="indexMergeType">索引合并操作类型</param>
        /// <returns></returns>
        public Task<ResponseResult<IIndexCondition<VT>>> GetCondition(int[] keys, IndexMergeTypeEnum indexMergeType = IndexMergeTypeEnum.Intersection)
        {
            switch (keys.Length)
            {
                case 0: return BlockIndexDataCacheNode<VT>.NullIndexCondition;
                case 1: return getCondition(GetNode(keys[0]));
                default: return getCondition(GetNode(keys), indexMergeType);
            }
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Task<ResponseResult<IIndexCondition<VT>>> getCondition(NT node)
        {
            if (node.Type != IndexDataTypeEnum.None) return Task.FromResult(new ResponseResult<IIndexCondition<VT>>(new BlockIndexCondition<VT>(node)));
            return loadCondition(node);
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="indexMergeType"></param>
        /// <returns></returns>
        private Task<ResponseResult<IIndexCondition<VT>>> getCondition(IndexNode<VT>[] nodes, IndexMergeTypeEnum indexMergeType)
        {
            foreach (IndexNode<VT> nextNode in nodes)
            {
                if (nextNode.Node.Type == IndexDataTypeEnum.None) return loadCondition(nodes, indexMergeType);
            }
            return Task.FromResult(new ResponseResult<IIndexCondition<VT>>(new BlockIndexArrayCondition<VT>(nodes, indexMergeType)));
        }
    }
    /// <summary>
    /// 索引数据磁盘块索引缓存 uint:VT
    /// </summary>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public abstract class UIntKeyCache<VT> : UIntKeyCache<VT, GenericIndex<uint, VT>>
#if NetStandard21
        where VT : notnull, IEquatable<VT>
#else
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// Node interface with reusable hash indexes with removal tags
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashIndexNodeClientNode<uint, VT>> node;
        /// <summary>
        /// 索引数据磁盘块索引缓存 uint:VT
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        protected UIntKeyCache(StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashIndexNodeClientNode<uint, VT>> node, long maxCount, int capacity = 1 << 16) : base(maxCount, capacity)
        {
            this.node = node;
            getChangeKeys().Catch();
        }
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <returns></returns>
        protected override async Task getChangeKeys()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                ResponseResult<IRemoveMarkHashIndexNodeClientNode<uint, VT>> nodeResult = await node.GetSynchronousNode();
                if (nodeResult.IsSuccess && setGetChangeKeyKeepCallback(await nodeResult.Value.notNull().GetChangeKeys(getChangeKeys))) return;
                await Task.Delay(1000);
            }
            while (!isDispose);
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override GenericIndex<uint, VT> createNode(uint key)
        {
            return new GenericIndex<uint, VT>(this, key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<uint, VT> node)
        {
            ResponseResult<IRemoveMarkHashIndexNodeClientNode<uint, VT>> nodeResult = await this.node.GetNode();
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
