using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using System;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存 uint:VT
    /// </summary>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public sealed class SingleDiskBlockUIntKeyCache<VT> : UIntKeyCache<VT>
#if NetStandard21
        where VT : notnull, IEquatable<VT>
#else
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 数据关键字磁盘块索引信息客户端
        /// </summary>
        private readonly IDiskBlockClientSocketEvent diskBlockClient;
        /// <summary>
        /// 索引数据磁盘块索引缓存 uint:VT
        /// </summary>
        /// <param name="diskBlockClient">数据关键字磁盘块索引信息客户端</param>
        /// <param name="node">带移除标记的可重用哈希索引节点接口</param>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public SingleDiskBlockUIntKeyCache(IDiskBlockClientSocketEvent diskBlockClient, StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashIndexNodeClientNode<uint, VT>> node, long maxCount, int capacity = 1 << 16) : base(node, maxCount, capacity)
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
