using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.Document.MemoryDatabaseNode.Client.Message
{
    /// <summary>
    /// Client binary serialized message client consumer example
    /// 客户端二进制序列化消息客户端消费者示例
    /// </summary>
    internal sealed class ServerByteArrayBinaryMessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageConsumer<Data.TestClass>
    {
        /// <summary>
        /// Client binary serialized message client consumer example
        /// 客户端二进制序列化消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Server-side byte array messages client nodes
        /// 服务端字节数组消息客户端节点</param>
        internal ServerByteArrayBinaryMessageConsumer(AutoCSer.Net.CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node) : base(commandClient, node, 1 << 10) { }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(Data.TestClass? message)
        {
            lock (messageLock) messages.Remove(message.AutoCSerClassGenericTypeExtensions().NotNull().Int);
            return AutoCSer.Common.GetCompletedTask(true);
        }

        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateServerByteArrayMessageNode(nameof(ServerByteArrayBinaryMessageConsumer)));
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
        /// Client binary serialized message client consumer example
        /// 客户端二进制序列化消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
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

                result = await node.AppendMessage(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage.BinarySerialize(messageData));
                if (!result.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (ServerByteArrayBinaryMessageConsumer consumer = new ServerByteArrayBinaryMessageConsumer(CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.Client, node))
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
