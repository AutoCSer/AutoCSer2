using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Dictionary node client example
    /// 字典节点客户端示例
    /// </summary>
    internal static class HashBytesDictionaryNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateHashBytesDictionaryNode(nameof(HashBytesDictionaryNode)));
        /// <summary>
        /// Dictionary node client example
        /// 字典节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNodeClientNode node = nodeResult.Value.AutoCSerExtensions().NotNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            #region Keyword JSON serialization
            Data.TestClass keyData = new Data.TestClass { Int = 1, String = "3A" };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray jsonKey = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.JsonSerialize(keyData);
            var boolResult = await node.Set(jsonKey, "AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var stringResult = await node.TryGetString(jsonKey);
            if (!stringResult.IsSuccess || stringResult.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            #region Keyword binary serialization
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray binarySerializeKey = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray.BinarySerialize(keyData);
            boolResult = await node.Set(binarySerializeKey, "AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            stringResult = await node.TryGetString(binarySerializeKey);
            if (!stringResult.IsSuccess || stringResult.Value != "AAA")
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            #endregion

            return true;
        }
    }
}
