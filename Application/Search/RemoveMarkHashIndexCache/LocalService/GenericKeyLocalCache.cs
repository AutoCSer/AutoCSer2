using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 KT:VT
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public abstract class GenericKeyLocalCache<KT, VT> : GenericKeyCache<KT, VT, GenericIndex<KT, VT>>
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
        private readonly StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashIndexNodeLocalClientNode<KT, VT>> node;
        /// <summary>
        /// 索引数据磁盘块索引缓存
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="capacity">容器初始化大小</param>
        protected GenericKeyLocalCache(StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashIndexNodeLocalClientNode<KT, VT>> node, long maxCount, int capacity = 1 << 16) : base(maxCount, capacity)
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
                LocalResult<IRemoveMarkHashIndexNodeLocalClientNode<KT, VT>> nodeResult = await node.GetSynchronousNode();
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
            LocalResult<IRemoveMarkHashIndexNodeLocalClientNode<KT, VT>> nodeResult = await this.node.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.CallState;
            do
            {
                int changeKeyVersion = node.ChangeKeyVersion, getChangeKeyVersion = this.GetChangeKeyVersion;
                LocalResult<BlockIndexData<VT>> data = await nodeResult.Value.notNull().GetBlockIndexData(node.Key);
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
