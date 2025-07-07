using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using System;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 KT:int
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="NT">索引数据磁盘块索引缓存节点类型</typeparam>
    public sealed class SingleDiskBlockGenericKeyIntValueLocalCache<KT, NT> : GenericKeyIntValueLocalCache<KT, NT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 数据关键字磁盘块索引信息客户端
        /// </summary>
        private readonly IDiskBlockClientSocketEvent diskBlockClient;
        /// <summary>
        /// 索引数据磁盘块索引缓存 KT:int
        /// </summary>
        /// <param name="diskBlockClient">数据关键字磁盘块索引信息客户端</param>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量，少量数据为 8B 元素，大量数据为 16B 元素</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public SingleDiskBlockGenericKeyIntValueLocalCache(IDiskBlockClientSocketEvent diskBlockClient, StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<KT>> node, long maxCount, int capacity = 1 << 16) : base(node, maxCount, capacity)
        {
            this.diskBlockClient = diskBlockClient;
        }
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex)
        {
            return diskBlockClient.DiskBlockClient;
        }
    }
}
