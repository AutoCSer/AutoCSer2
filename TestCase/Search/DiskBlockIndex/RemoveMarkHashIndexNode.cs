using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService;
using System;

namespace AutoCSer.TestCase.SearchDiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希索引节点
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    internal sealed class RemoveMarkHashIndexNode<KT, VT> : AutoCSer.CommandService.Search.DiskBlockIndex.RemoveMarkHashIndexNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 磁盘块命令客户端套接字事件
        /// </summary>
        private readonly DiskBlockCommandClientSocketEvent client;
        /// <summary>
        /// 带移除标记的可重用哈希索引节点
        /// </summary>
        /// <param name="capacity">初始化容器尺寸</param>
        internal RemoveMarkHashIndexNode(int capacity) : base(capacity)
        {
            client = DiskBlockCommandClientSocketEvent.CommandClient.SocketEvent;
        }
        /// <summary>
        /// 根据索引关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(KT key)
        {
            return client.DiskBlockClient;
        }
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex)
        {
            return client.DiskBlockClient;
        }
    }
}
