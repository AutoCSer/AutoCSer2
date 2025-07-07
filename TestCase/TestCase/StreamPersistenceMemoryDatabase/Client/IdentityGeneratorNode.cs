using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
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
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeClientNode> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateIdentityGeneratorNode(nameof(IdentityGeneratorNode)));
        /// <summary>
        /// Example of 64-bit auto-increment identity generation client node
        /// 64 位自增 ID 生成客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// Example of 64-bit auto-increment identity generation client node
        /// 64 位自增 ID 生成客户端节点示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNodeClientNode> client)
        {
            var nodeResult = await client.GetNode();
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
