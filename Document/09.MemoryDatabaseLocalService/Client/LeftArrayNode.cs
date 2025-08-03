using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// Example of an array node client
    /// 数组节点客户端示例
    /// </summary>
    internal static class LeftArrayNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNodeLocalClientNode<int>> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateLeftArrayNode<int>(nameof(LeftArrayNode)));
        /// <summary>
        /// Example of an array node client
        /// 数组节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNodeLocalClientNode<int> node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var result = await node.SetEmpty();//[]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.Add(2);//[2]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.Add(4);//[2,4]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Insert(1, 1);//[2,1,4]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var indexResult = await node.IndexOfArray(4);
            if (!indexResult.IsSuccess || indexResult.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.LastIndexOfArray(2);
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.Add(1);//[2,1,4,1]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Fill(3, 1, 2);//[2,3,3,1]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.SetValue(1, 4);//[2,4,3,1]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.GetValue(1);
            if (!valueResult.IsSuccess || valueResult.Value.Value != 4)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(3);
            if (!indexResult.IsSuccess || indexResult.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.LastIndexOfArray(4);
            if (!indexResult.IsSuccess || indexResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.SortArray();//[1,2,3,4]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(4);
            if (!indexResult.IsSuccess || indexResult.Value != 3)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.ReverseArray();//[4,3,2,1]
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(4);
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetValueSet(2, 1);//[4,3,1,1]
            if (!valueResult.IsSuccess || valueResult.Value.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.RemoveToEnd(0);//[1,3,1]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(1);
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Remove(1);//[3,1]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(1);
            if (!indexResult.IsSuccess || indexResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.TryPop();//[3]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.IndexOfArray(3);
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.RemoveAt(0);//[]
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            indexResult = await node.GetLength();
            if (!indexResult.IsSuccess || indexResult.Value != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
