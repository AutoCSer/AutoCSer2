using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// 平衡树客户端示例
    /// </summary>
    internal static class SearchTreeDictionaryNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNodeClientNode<int, Data.TestClass>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateSearchTreeDictionaryNode<int, Data.TestClass>(nameof(SearchTreeDictionaryNode)));
        /// <summary>
        /// 平衡树客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNodeClientNode<int, Data.TestClass> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.TryAdd(2, new Data.TestClass { Int = 2 });
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.ContainsKey(2);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Set(1, new Data.TestClass { Int = 1 });
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Set(4, new Data.TestClass { Int = 4 });
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Set(3, new Data.TestClass { Int = 3 });
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryGetValue(3);
            if (!valueResult.IsSuccess || valueResult.Value.Value?.Int != 3)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.TryGetValueByIndex(1);
            if (!valueResult.IsSuccess || valueResult.Value.Value?.Int != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.TryGetFirstValue();
            if (!valueResult.IsSuccess || valueResult.Value.Value?.Int != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.TryGetLastValue();
            if (!valueResult.IsSuccess || valueResult.Value.Value?.Int != 4)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var intResult = await node.IndexOf(2);
            if (!intResult.IsSuccess || intResult.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var keyResult = await node.TryGetFirstKey();
            if (!keyResult.IsSuccess || keyResult.Value.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            keyResult = await node.TryGetLastKey();
            if (!keyResult.IsSuccess || keyResult.Value.Value != 4)
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
            var valuesResult = await node.GetValues(1, 2);
            if (!valuesResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            int checkKey = 2;
            await foreach (var value in valuesResult.GetAsyncEnumerable())
            {
                if (!value.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (checkKey == 4)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (value.Value.Value?.Int != checkKey)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++checkKey;
            }
            return true;
        }
    }
}
