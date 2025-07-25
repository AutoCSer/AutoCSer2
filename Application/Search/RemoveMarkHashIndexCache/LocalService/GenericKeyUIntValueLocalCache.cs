﻿using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 KT:int/uint
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    /// <typeparam name="NT">索引数据磁盘块索引缓存节点类型</typeparam>
    public abstract class GenericKeyUIntValueLocalCache<KT, VT, NT> : GenericKeyCache<KT, VT, NT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
        where NT : UIntIndex<KT, VT>
    {
        /// <summary>
        /// Node interface with reusable hash indexes with removal tags
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<KT>> node;
        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量，少量数据为 8B 元素，大量数据为 16B 元素</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        protected GenericKeyUIntValueLocalCache(StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<KT>> node, long maxCount, int capacity) : base(maxCount, capacity)
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
                LocalResult<IRemoveMarkHashKeyIndexNodeLocalClientNode<KT>> nodeResult = await node.GetSynchronousNode();
                if (nodeResult.IsSuccess && setGetChangeKeyKeepCallback(await nodeResult.Value.notNull().GetChangeKeys(getChangeKeys))) return;
                await Task.Delay(1000);
            }
            while (!isDispose);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            foreach (UIntIndex<KT, VT> node in cache.Values) node.Free();
            cache.ClearCount();
        }
    }
    /// <summary>
    /// 索引数据磁盘块索引缓存 KT:uint
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    public abstract class GenericKeyUIntValueLocalCache<KT> : GenericKeyUIntValueLocalCache<KT, uint, UIntIndex<KT>>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 索引数据磁盘块索引缓存 KT:uint
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量，少量数据为 8B 元素，大量数据为 16B 元素</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        protected GenericKeyUIntValueLocalCache(StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<KT>> node, long maxCount, int capacity = 1 << 16) : base(node, maxCount, capacity)
        {
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override UIntIndex<KT> createNode(KT key)
        {
            return new UIntIndex<KT>(this, key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<KT, uint> node)
        {
            LocalResult<IRemoveMarkHashKeyIndexNodeLocalClientNode<KT>> nodeResult = await this.node.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.CallState;
            do
            {
                int changeKeyVersion = node.ChangeKeyVersion, getChangeKeyVersion = this.GetChangeKeyVersion;
                LocalResult<BlockIndexData<uint>> data = await nodeResult.Value.notNull().GetBlockIndexData(node.Key);
                if (data.IsSuccess)
                {
                    if (getChangeKeyVersion == this.GetChangeKeyVersion && node.Set(changeKeyVersion, getChangeKeyVersion, data.Value)) return CallStateEnum.Success;
                }
                else return data.CallState;
            }
            while (true);
        }
    }
}
