using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 uint:uint
    /// </summary>
    public abstract class UIntKeyUIntValueLocalCache : UIntKeyCache<uint, UIntIndex<uint>>
    {
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<uint>> node;
        /// <summary>
        /// 索引数据磁盘块索引缓存 uint:uint
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量，少量数据为 8B 元素，大量数据为 16B 元素</param>
        /// <param name="capacity">容器初始化大小</param>
        protected UIntKeyUIntValueLocalCache(StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<uint>> node, long maxCount, int capacity = 1 << 16) : base(maxCount, capacity)
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
                LocalResult<IRemoveMarkHashKeyIndexNodeLocalClientNode<uint>> nodeResult = await node.GetSynchronousNode();
                if (nodeResult.IsSuccess && setGetChangeKeyKeepCallback(await nodeResult.Value.notNull().GetChangeKeys(getChangeKeys))) return;
                await Task.Delay(1000);
            }
            while (!isDispose);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            foreach (UIntIndex<uint> node in cache.Values) node.Free();
            cache.Clear();
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override UIntIndex<uint> createNode(uint key)
        {
            return new UIntIndex<uint>(this, key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<uint, uint> node)
        {
            LocalResult<IRemoveMarkHashKeyIndexNodeLocalClientNode<uint>> nodeResult = await this.node.GetNode();
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
