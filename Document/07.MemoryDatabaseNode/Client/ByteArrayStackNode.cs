using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Example of byte[] stack client node
    /// byte[] 栈客户端节点示例
    /// </summary>
    internal static class ByteArrayStackNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayStackNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateByteArrayStackNode(nameof(ByteArrayStackNode)));
        /// <summary>
        /// Example of byte[] stack client node
        /// byte[] 栈客户端节点示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayStackNodeClientNode node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            result = await node.Push(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.JsonSerialize(new Data.TestClass { Int = 2 }));
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var valueResult = await node.TryPopJsonDeserialize<Data.TestClass>();
            if (!valueResult.IsSuccess || valueResult.Value?.Int != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
