using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client.MessageNode
{
    /// <summary>
    /// Client string message client consumer example
    /// 客户端 string 二进制序列化消息客户端消费者示例
    /// </summary>
    internal sealed class ServerByteArrayMessageStringConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageStringConsumer
    {
        /// <summary>
        /// Client string message client consumer example
        /// 客户端 string 二进制序列化消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Server-side byte array messages client nodes
        /// 服务端字节数组消息客户端节点</param>
        internal ServerByteArrayMessageStringConsumer(AutoCSer.Net.CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node) : base(commandClient, node, 1 << 10) { }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(string message)
        {
            lock (messageLock) messages.Remove(message.notNull());
            return AutoCSer.Common.GetCompletedTask(true);
        }

        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateServerByteArrayMessageNode(nameof(ServerByteArrayMessageStringConsumer)));
        /// <summary>
        /// Client node singleton (supporting concurrent read operations)
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>> readWriteQueueNodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateServerByteArrayMessageNode(nameof(ServerByteArrayMessageStringConsumer)));
        /// <summary>
        /// Message collection not completed
        /// 未完成消息集合
        /// </summary>
        private static HashSet<string> messages = new HashSet<string>();
        /// <summary>
        /// Access lock for consumption message testing
        /// 消费消息测试访问锁
        /// </summary>
        private static readonly object messageLock = new object();
        /// <summary>
        /// Client string message client consumer example
        /// 客户端 string 二进制序列化消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(nodeCache, CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.Client)) return false;
            if (!await test(readWriteQueueNodeCache, CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.Client.Client)) return false;
            return true;
        }
        /// <summary>
        /// Client string message client consumer example
        /// 客户端 string 二进制序列化消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>> client, AutoCSer.Net.CommandClient commandClient)
        {
            var nodeResult = await client.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node = nodeResult.Value.notNull();
            var result = await node.Clear();
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            messages.Clear();
            for (char message = 'A'; message <= 'Z'; ++message)
            {
                string messageData = message.ToString();
                messages.Add(messageData);

                result = await node.AppendMessage(messageData);
                if (!result.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (ServerByteArrayMessageStringConsumer consumer = new ServerByteArrayMessageStringConsumer(commandClient, node))
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
