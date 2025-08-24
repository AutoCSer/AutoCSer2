using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Example of a generic dictionary client node
    /// 泛型字典客户端节点示例
    /// </summary>
    internal static class DictionaryNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNodeClientNode<string, string>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateDictionaryNode<string, string>(nameof(DictionaryNode)));
        /// <summary>
        /// Example of a generic dictionary client node
        /// 泛型字典客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNodeClientNode<string, string> node = nodeResult.Value.AutoCSerExtensions().NotNull();
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
        /// <summary>
        /// The client node singleton of the API that directly returns the value
        /// 直接返回值的 API 的客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNodeReturnValueNode<string, string> returnValueNode = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNodeReturnValueNode<string, string>(nodeCache);
        /// <summary>
        /// Example of a generic dictionary client node
        /// 泛型字典客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> ReturnValueTest()
        {
            await returnValueNode.Clear();

            bool boolResult = await returnValueNode.Set("3A", "AAA");
            if (!boolResult)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await returnValueNode.TryGetValue("3A");
            if (valueResult.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            valueResult = await returnValueNode.TryGetValue("3B");
            if (valueResult.IsValue)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            boolResult = await returnValueNode.Remove("3A");
            if (!boolResult)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
