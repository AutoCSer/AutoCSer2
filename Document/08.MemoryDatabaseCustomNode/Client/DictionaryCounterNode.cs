using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode.Client
{
    /// <summary>
    /// Dictionary counter node client example
    /// 字典计数器节点客户端示例
    /// </summary>
    internal static class DictionaryCounterNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IDictionaryCounterNodeClientNode<int>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IDictionaryCounterNodeClientNode<int>>(nameof(DictionaryCounterNode), (index, key, nodeInfo) => client.ClientNode.CreateDictionaryCounterNode(index, key, nodeInfo, typeof(int), 0)));
        /// <summary>
        /// Dictionary counter node client example
        /// 字典计数器节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            IDictionaryCounterNodeClientNode<int> node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            //Test the counter with the user identity of 1
            //测试用户ID 为 1 的计数器
            int key = 1;
            var valueResult = await node.GetCount(key);
            if (!valueResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var result = await node.Increment(key);
            if (!result.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var nextResult = await node.GetCount(key);
            if (!nextResult.IsSuccess || nextResult.Value != valueResult.Value + 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
