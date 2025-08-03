using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// Bitmap node client example
    /// 位图节点客户端示例
    /// </summary>
    internal static class BitmapNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateBitmapNode(nameof(BitmapNode), 8));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNodeClientNode> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateBitmapNode(nameof(BitmapNode), 8));
        /// <summary>
        /// Bitmap node client example
        /// 位图节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// Bitmap node client example
        /// 位图节点客户端示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNodeClientNode> client)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNodeClientNode node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var result = await node.ClearMap();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.SetBit(3);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.GetBit(3);
            if (!valueResult.IsSuccess || valueResult.Value.Value == 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.InvertBit(4);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetBitInvertBit(3);
            if (!valueResult.IsSuccess || valueResult.Value.Value == 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetBit(3);
            if (!valueResult.IsSuccess || valueResult.Value.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetBitClearBit(4);
            if (!valueResult.IsSuccess || valueResult.Value.Value == 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetBitSetBit(4);
            if (!valueResult.IsSuccess || valueResult.Value.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.ClearBit(4);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetBit(4);
            if (!valueResult.IsSuccess || valueResult.Value.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
