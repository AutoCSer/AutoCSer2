﻿using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
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
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNodeClientNode> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateHashBytesDictionaryNode(nameof(HashBytesDictionaryNode)));
        /// <summary>
        /// Dictionary node client example
        /// 字典节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache)) return false;
            if (!await test(readWriteQueueNodeCache)) return false;
            return true;
        }
        /// <summary>
        /// Dictionary node client example
        /// 字典节点客户端示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNodeClientNode> client)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNodeClientNode node = nodeResult.Value.notNull();
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
