using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// Keyword sorting array client node example
    /// 关键字排序数组客户端节点示例
    /// </summary>
    internal static class SortedListNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNodeClientNode<int, string>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateSortedListNode<int, string>(nameof(SortedListNode)));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNodeClientNode<int, string>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateSortedListNode<int, string>(nameof(SortedListNode)));
        /// <summary>
        /// Keyword sorting array client node example
        /// 关键字排序数组客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// Keyword sorting array client node example
        /// 关键字排序数组客户端节点示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNodeClientNode<int, string>> client)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNodeClientNode<int, string> node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
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
