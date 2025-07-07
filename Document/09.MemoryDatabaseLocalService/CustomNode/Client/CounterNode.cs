using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode.Client
{
    /// <summary>
    /// Counter node client example
    /// 计数器节点客户端示例
    /// </summary>
    internal static class CounterNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<ICounterNodeLocalClientNode> nodeCache = ServiceConfig.Client.CreateNode(client => client.GetOrCreateNode<ICounterNodeLocalClientNode>(nameof(CounterNode), client.ClientNode.CreateCounterNode));
        /// <summary>
        /// Counter node client example
        /// 计数器节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            ICounterNodeLocalClientNode node = nodeResult.Value.notNull();
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
