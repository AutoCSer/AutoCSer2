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
        /// 二进制位图客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateBitmapNode(nameof(BitmapNode), 8);
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
