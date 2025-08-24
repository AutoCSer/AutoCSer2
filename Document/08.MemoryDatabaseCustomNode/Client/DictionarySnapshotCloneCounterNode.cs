using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode.Client
{
    /// <summary>
    /// A client example of a dictionary counter node that supports snapshot cloning
    /// 支持快照克隆的字典计数器节点客户端示例
    /// </summary>
    internal static class DictionarySnapshotCloneCounterNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IDictionarySnapshotCloneCounterNodeClientNode<int>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IDictionarySnapshotCloneCounterNodeClientNode<int>>(nameof(DictionarySnapshotCloneCounterNode), (index, key, nodeInfo) => client.ClientNode.CreateDictionarySnapshotCloneCounterNode(index, key, nodeInfo, typeof(int), 0)));
        /// <summary>
        /// A client example of a dictionary counter node that supports snapshot cloning
        /// 支持快照克隆的字典计数器节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            IDictionarySnapshotCloneCounterNodeClientNode<int> node = nodeResult.Value.AutoCSerExtensions().NotNull();

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
