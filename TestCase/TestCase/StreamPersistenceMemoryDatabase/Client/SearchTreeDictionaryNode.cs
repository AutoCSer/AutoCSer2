using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// Example of balanced tree client node
    /// 平衡树客户端节点示例
    /// </summary>
    internal static class SearchTreeDictionaryNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNodeClientNode<int, Data.TestClass>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateSearchTreeDictionaryNode<int, Data.TestClass>(nameof(SearchTreeDictionaryNode)));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNodeClientNode<int, Data.TestClass>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateSearchTreeDictionaryNode<int, Data.TestClass>(nameof(SearchTreeDictionaryNode)));
        /// <summary>
        /// Example of balanced tree client node
        /// 平衡树客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// Example of balanced tree client node
        /// 平衡树客户端节点示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNodeClientNode<int, Data.TestClass>> client)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNodeClientNode<int, Data.TestClass> node = nodeResult.Value.AutoCSerExtensions().NotNull();
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
#if NetStandard21
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
#endif
            return true;
        }
    }
}
