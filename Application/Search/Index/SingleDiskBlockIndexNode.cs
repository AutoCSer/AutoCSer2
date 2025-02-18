using AutoCSer.CommandService.DiskBlock;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 单磁盘块索引信息关键字索引节点
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    public sealed class SingleDiskBlockIndexNode<KT, VT> : IndexNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 数据关键字磁盘块索引信息客户端
        /// </summary>
        private readonly IDiskBlockClientSocketEvent diskBlockClient;
        /// <summary>
        /// 关键字索引节点
        /// </summary>
        /// <param name="diskBlockClient">数据关键字磁盘块索引信息客户端</param>
        /// <param name="writeCount">触发重置写入磁盘块索引信息的更新数据量，最小值为 16</param>
        public SingleDiskBlockIndexNode(IDiskBlockClientSocketEvent diskBlockClient, int writeCount = 1 << 10) : base(writeCount)
        {
            this.diskBlockClient = diskBlockClient;
        }
        /// <summary>
        /// 根据索引关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(KT key)
        {
            return diskBlockClient.DiskBlockClient;
        }
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex)
        {
            return diskBlockClient.DiskBlockClient;
        }
    }
}
