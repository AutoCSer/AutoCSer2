﻿using AutoCSer.CommandService.DiskBlock;
using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 单磁盘块索引信息 带移除标记的可重用哈希索引节点
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">Data keyword type
    /// 数据关键字类型</typeparam>
    public sealed class SingleDiskBlockRemoveMarkHashIndexNode<KT, VT> : RemoveMarkHashIndexNode<KT, VT>
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
        public SingleDiskBlockRemoveMarkHashIndexNode(IDiskBlockClientSocketEvent diskBlockClient) : base()
        {
            this.diskBlockClient = diskBlockClient;
        }
        /// <summary>
        /// 根据索引关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(KT key)
        {
            return diskBlockClient.DiskBlockClient;
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
