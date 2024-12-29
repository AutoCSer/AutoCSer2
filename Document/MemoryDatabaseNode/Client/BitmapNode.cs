using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 二进制位图客户端示例
    /// </summary>
    internal static class BitmapNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateBitmapNode(nameof(BitmapNode), 8));
        /// <summary>
        /// 二进制位图客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNodeClientNode node = nodeResult.Value.notNull();
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
