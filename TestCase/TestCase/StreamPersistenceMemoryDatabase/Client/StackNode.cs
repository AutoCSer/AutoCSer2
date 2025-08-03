using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// Generic stack client node example
    /// 泛型栈客户端节点示例
    /// </summary>
    internal static class StackNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNodeClientNode<int>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateStackNode<int>(nameof(StackNode)));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNodeClientNode<int>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateStackNode<int>(nameof(StackNode)));
        /// <summary>
        /// Generic stack client node example
        /// 泛型栈客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// Generic stack client node example
        /// 泛型栈客户端节点示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNodeClientNode<int>> client)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNodeClientNode<int> node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.Push(2);
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Contains(2);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryPop();
            if (!valueResult.IsSuccess || valueResult.Value.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
