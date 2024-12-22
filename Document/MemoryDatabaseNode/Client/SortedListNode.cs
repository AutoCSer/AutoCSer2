using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 关键字排序数组客户端示例
    /// </summary>
    internal static class SortedListNode
    {
        /// <summary>
        /// 关键字排序数组客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateSortedListNode<int, string>(nameof(SortedListNode));
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNodeClientNode<int, string> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.TryAdd(2, "AA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.ContainsKey(2);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.ContainsValue("AA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryGetValue(2);
            if (!valueResult.IsSuccess || valueResult.Value.Value != "AA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.TryAdd(3, "AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.TryAdd(1, "A");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var indexResult = await node.IndexOfKey(2);
            if (!indexResult.IsSuccess || indexResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfValue("AAA");
            if (!indexResult.IsSuccess || indexResult.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.RemoveAt(1);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Remove(1);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetRemove(3);
            if (!valueResult.IsSuccess || valueResult.Value.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.Count();
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
