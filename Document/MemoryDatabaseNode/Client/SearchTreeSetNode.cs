using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 平衡树客户端示例
    /// </summary>
    internal static class SearchTreeSetNode
    {
        /// <summary>
        /// 平衡树客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateSearchTreeSetNode<int>(nameof(SearchTreeSetNode));
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNodeClientNode<int> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Add(2);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Contains(2);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Add(1);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Add(4);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Add(3);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.GetByIndex(1);
            if (!valueResult.IsSuccess || valueResult.Value.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetFrist();
            if (!valueResult.IsSuccess || valueResult.Value.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetLast();
            if (!valueResult.IsSuccess || valueResult.Value.Value != 4)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var intResult = await node.IndexOf(2);
            if (!intResult.IsSuccess || intResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            intResult = await node.CountLess(3);
            if (!intResult.IsSuccess || intResult.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            intResult = await node.CountThan(3);
            if (!intResult.IsSuccess || intResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
