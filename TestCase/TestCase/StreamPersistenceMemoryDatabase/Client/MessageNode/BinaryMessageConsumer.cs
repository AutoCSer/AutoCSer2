using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client.MessageNode
{
    /// <summary>
    /// Server-side generic message client-side consumer example
    /// 服务端泛型消息客户端消费者示例
    /// </summary>
    internal sealed class BinaryMessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessageConsumer<Data.TestClass>
    {
        /// <summary>
        /// Server-side generic message client-side consumer example
        /// 服务端泛型消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Binary serialized message client node
        /// 二进制序列化消息消息客户端节点</param>
        internal BinaryMessageConsumer(AutoCSer.Net.ICommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>> node) : base(commandClient, node, 1 << 10) { }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(Data.TestClass message)
        {
            lock (messageLock) messages.Remove(message.Int);
            return AutoCSer.Common.GetCompletedTask(true);
        }

        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateBinaryMessageNode<Data.TestClass>(nameof(BinaryMessageConsumer)));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateBinaryMessageNode<Data.TestClass>(nameof(BinaryMessageConsumer)));
        /// <summary>
        /// Message collection not completed
        /// 未完成消息集合
        /// </summary>
        private static Dictionary<int, Data.TestClass> messages = new Dictionary<int, Data.TestClass>();
        /// <summary>
        /// Access lock for consumption message testing
        /// 消费消息测试访问锁
        /// </summary>
        private static readonly object messageLock = new object();
        /// <summary>
        /// Server-side generic message client-side consumer example
        /// 服务端泛型消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache, CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.Client)) return false;
            if (!await test(readWriteQueueNodeCache, CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.Client.Client)) return false;
            return true;
        }
        /// <summary>
        /// Server-side generic message client-side consumer example
        /// 服务端泛型消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>>> client, AutoCSer.Net.ICommandClient commandClient)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            messages.Clear();
            for (char message = 'A'; message <= 'Z'; ++message)
            {
                Data.TestClass messageData = new Data.TestClass { Int = message, String = message.ToString() };
                messages.Add(message, messageData);

                result = await node.AppendMessage(messageData);
                if (!result.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (BinaryMessageConsumer consumer = new BinaryMessageConsumer(commandClient, node))
            {
                #region Wait for the test message to complete
                long timeout = Stopwatch.GetTimestamp() + AutoCSer.Date.GetTimestampBySeconds(10);
                while (messages.Count != 0)
                {
                    if (timeout < Stopwatch.GetTimestamp())
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                    await Task.Delay(1);
                }
                #endregion
            }

            return true;
        }
    }
}
