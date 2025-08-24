using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// Example of a red-black tree dictionary client
    /// 红黑树字典客户端示例
    /// </summary>
    internal static class SortedDictionaryNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNodeLocalClientNode<int, Data.TestClass>> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateSortedDictionaryNode<int, Data.TestClass>(nameof(SortedDictionaryNode)));
        /// <summary>
        /// Red-black tree dictionary client node example
        /// 红黑树字典客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNodeLocalClientNode<int, Data.TestClass> node = nodeResult.Value.AutoCSerExtensions().NotNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.TryAdd(1, new Data.TestClass { Int = 1 });
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.ContainsKey(1);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryGetValue(1);
            if (!valueResult.IsSuccess || valueResult.Value.Value?.Int != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
