using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode.Client
{
    /// <summary>
    /// The API client sample for initialize and load the persistent data
    /// 初始化加载持久化数据 API 客户端示例
    /// </summary>
    internal static class LoadPersistenceNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<ILoadPersistenceNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<ILoadPersistenceNodeClientNode>(nameof(LoadPersistenceNode), client.ClientNode.CreateLoadPersistenceNode));
        /// <summary>
        /// The API client sample for initialize and load the persistent data
        /// 初始化加载持久化数据 API 客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            ILoadPersistenceNodeClientNode node = nodeResult.Value.notNull();
            var valueResult = await node.GetCount();
            if (!valueResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var result = await node.Increment();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var nextResult = await node.GetCount();
            if (!nextResult.IsSuccess || nextResult.Value != valueResult.Value + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
