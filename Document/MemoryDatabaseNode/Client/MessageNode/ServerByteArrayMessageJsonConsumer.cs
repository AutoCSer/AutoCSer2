﻿using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.Document.MemoryDatabaseNode.Client.MessageNode
{
    /// <summary>
    /// 客户端 JSON 消息客户端消费者示例
    /// </summary>
    internal sealed class ServerByteArrayMessageJsonConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageJsonConsumer<Data.TestClass>
    {
        /// <summary>
        /// 客户端 JSON 消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">服务端字节数组消息客户端节点</param>
        internal ServerByteArrayMessageJsonConsumer(AutoCSer.Net.CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node) : base(commandClient, node) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected override Task<bool> onMessage(Data.TestClass? message)
        {
            lock (messageLock) messages.Remove(message.notNull().Int);
            return AutoCSer.Common.GetCompletedTask(true);
        }

        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateServerByteArrayMessageNode(nameof(ServerByteArrayMessageJsonConsumer)));
        /// <summary>
        /// 未完成消息集合
        /// </summary>
        private static Dictionary<int, Data.TestClass> messages = new Dictionary<int, Data.TestClass>();
        /// <summary>
        /// 消费消息测试访问锁
        /// </summary>
        private static readonly object messageLock = new object();
        /// <summary>
        /// 客户端 JSON 消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
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
                Data.TestClass messageData = new Data.TestClass { Int = message, String = message.ToString() };
                messages.Add(message, messageData);

                result = await node.AppendMessage(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage.JsonSerialize(messageData));//生产消息测试
                if (!result.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (ServerByteArrayMessageJsonConsumer consumer = new ServerByteArrayMessageJsonConsumer(CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.Client, node))
            {
                consumer.Start(1 << 10).NotWait(); //启动客户端消费测试

                #region 等待测试消息完成
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