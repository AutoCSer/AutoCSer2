﻿using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// 红黑树哈希表客户端示例
    /// </summary>
    internal static class SortedSetNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNodeLocalClientNode<int>> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateSortedSetNode<int>(nameof(SortedSetNode)));
        /// <summary>
        /// 红黑树哈希表客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNodeLocalClientNode<int> node = nodeResult.Value.notNull();
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
            var valueResult = await node.GetMin();
            if (!valueResult.IsSuccess || valueResult.Value.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.GetMax();
            if (!valueResult.IsSuccess || valueResult.Value.Value != 4)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}