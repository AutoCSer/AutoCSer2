using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 uint:int
    /// </summary>
    public abstract class UIntKeyIntValueCache : UIntKeyCache<int, IntIndex<uint>>
    {
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> node;
        /// <summary>
        /// 索引数据磁盘块索引缓存 uint:int
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量，少量数据为 8B 元素，大量数据为 16B 元素</param>
        /// <param name="capacity">容器初始化大小</param>
        protected UIntKeyIntValueCache(StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> node, long maxCount, int capacity = 1 << 16) : base(maxCount, capacity)
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
                ResponseResult<IRemoveMarkHashKeyIndexNodeClientNode<int>> nodeResult = await node.GetSynchronousNode();
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
            foreach (IntIndex<uint> node in cache.Values) node.Free();
            cache.Clear();
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override IntIndex<uint> createNode(uint key)
        {
            return new IntIndex<uint>(this, key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<uint, int> node)
        {
            ResponseResult<IRemoveMarkHashKeyIndexNodeClientNode<int>> nodeResult = await this.node.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            do
            {
                int changeKeyVersion = node.ChangeKeyVersion, getChangeKeyVersion = this.GetChangeKeyVersion;
                ResponseResult<BlockIndexData<int>> data = await nodeResult.Value.notNull().GetIntBlockIndexData((int)node.Key);
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
