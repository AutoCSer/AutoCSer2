using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 数组客户端示例
    /// </summary>
    internal static class ArrayNode
    {
        /// <summary>
        /// 数组客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            int length = 4;
            var nodeResult = await client.GetOrCreateArrayNode<int>(nameof(ArrayNode), length);
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNodeClientNode<int> node = nodeResult.Value.notNull();
            var result = await node.ClearArray();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.FillArray(2);//[2,2,2,2]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var indexResult = await node.IndexOfArray(2);
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.LastIndexOfArray(2);
            if (!indexResult.IsSuccess || indexResult.Value != length - 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Fill(3, 1, length - 2);//[2,3,3,2]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(3);
            if (!indexResult.IsSuccess || indexResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.LastIndexOfArray(3);
            if (!indexResult.IsSuccess || indexResult.Value != length - 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.SetValue(2, 4);//[2,3,4,2]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.GetValue(2);
            if (!valueResult.IsSuccess || valueResult.Value.Value != 4)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.SortArray();//[2,2,3,4]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(3);
            if (!indexResult.IsSuccess || indexResult.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.ReverseArray();//[4,3,2,2]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(3);
            if (!indexResult.IsSuccess || indexResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetValueSet(2, 1);//[4,3,1,2]
            if (!valueResult.IsSuccess || valueResult.Value.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
