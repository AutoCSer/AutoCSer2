using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode.Client
{
    /// <summary>
    /// Sample node client example for persistent pre-check
    /// 持久化前置检查示例节点客户端示例
    /// </summary>
    internal static class BeforePersistenceNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IBeforePersistenceNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IBeforePersistenceNodeClientNode>(nameof(BeforePersistenceNode), (index, key, nodeInfo) => client.ClientNode.CreateBeforePersistenceNode(index, key, nodeInfo, 0)));
        /// <summary>
        /// Sample node client example for persistent pre-check
        /// 持久化前置检查示例节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            IBeforePersistenceNodeClientNode node = nodeResult.Value.notNull();
            var identityResult = await node.AppendEntity(new IdentityEntity());
            long identity = identityResult.Value;
            if (!identityResult.IsSuccess || identity == 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.GetCount(identity);
            if (!valueResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var result = await node.Increment(identity);
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var nextResult = await node.GetCount(identity);
            if (!nextResult.IsSuccess || nextResult.Value != valueResult.Value + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Remove(identity);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
