using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// 哈希表客户端示例
    /// </summary>
    internal static class FragmentHashSetNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNodeLocalClientNode<int>> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateFragmentHashSetNode<int>(nameof(FragmentHashSetNode)));
        /// <summary>
        /// 哈希表客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNodeLocalClientNode<int> node = nodeResult.Value.notNull();
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
            boolResult = await node.Remove(2);
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
