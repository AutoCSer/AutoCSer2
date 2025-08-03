using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Example of 64-bit auto-increment identity generation client node
    /// 64 位自增 ID 生成客户端节点示例
    /// </summary>
    internal static class IdentityGeneratorNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateIdentityGeneratorNode(nameof(IdentityGeneratorNode)));
        /// <summary>
        /// Example of 64-bit auto-increment identity generation client node
        /// 64 位自增 ID 生成客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeClientNode node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var valueResult = await node.Next();
            if (!valueResult.IsSuccess && valueResult.Value > 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var fragmentResult = await node.NextFragment(16);
            if (!fragmentResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
