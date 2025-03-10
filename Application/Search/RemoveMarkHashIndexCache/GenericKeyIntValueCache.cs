using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 KT:int
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="NT">索引数据磁盘块索引缓存节点类型</typeparam>
    public abstract class GenericKeyIntValueCache<KT, NT> : GenericKeyUIntValueCache<KT, int, IntIndex<KT>>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 索引数据磁盘块索引缓存 KT:int
        /// </summary>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量，少量数据为 8B 元素，大量数据为 16B 元素</param>
        /// <param name="capacity">容器初始化大小</param>
        protected GenericKeyIntValueCache(StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<KT>> node, long maxCount, int capacity = 1 << 16) : base(node, maxCount, capacity)
        {
        }
        /// <summary>
        /// 创建索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override IntIndex<KT> createNode(KT key)
        {
            return new IntIndex<KT>(this, key);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal override async Task<ResponseResult> GetBlockIndexData(BlockIndexDataCacheNode<KT, int> node)
        {
            ResponseResult<IRemoveMarkHashKeyIndexNodeClientNode<KT>> nodeResult = await this.node.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            do
            {
                int changeKeyVersion = node.ChangeKeyVersion, getChangeKeyVersion = this.GetChangeKeyVersion;
                ResponseResult<BlockIndexData<int>> data = await nodeResult.Value.notNull().GetIntBlockIndexData(node.Key);
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
