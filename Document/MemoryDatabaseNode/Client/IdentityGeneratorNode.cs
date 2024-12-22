using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 64 位自增 ID 生成客户端示例
    /// </summary>
    internal static class IdentityGeneratorNode
    {
        /// <summary>
        /// 64 位自增 ID 生成客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateIdentityGeneratorNode(nameof(IdentityGeneratorNode));
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeClientNode node = nodeResult.Value.notNull();
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
