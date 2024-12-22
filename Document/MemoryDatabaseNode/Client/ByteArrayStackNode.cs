using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// byte[] 栈客户端示例
    /// </summary>
    internal static class ByteArrayStackNode
    {
        /// <summary>
        /// byte[] 栈客户端示例
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateByteArrayStackNode(nameof(ByteArrayStackNode));
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayStackNodeClientNode node = nodeResult.Value.notNull();
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
