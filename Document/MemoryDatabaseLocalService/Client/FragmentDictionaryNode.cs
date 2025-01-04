using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// 泛型字典客户端示例
    /// </summary>
    internal static class FragmentDictionaryNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNodeLocalClientNode<string, string>> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateFragmentDictionaryNode<string, string>(nameof(FragmentDictionaryNode)));
        /// <summary>
        /// 泛型字典客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNodeLocalClientNode<string, string> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await node.Set("3A", "AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryGetValue("3A");
            if (!valueResult.IsSuccess || valueResult.Value.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await node.TryGetValue("3B");
            if (!valueResult.IsSuccess || valueResult.Value.IsValue)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await node.Remove("3A");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
