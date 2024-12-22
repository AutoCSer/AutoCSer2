using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.Document.MemoryDatabaseNode.Client.MessageNode
{
    /// <summary>
    /// byte[] 消息客户端消费者示例
    /// </summary>
    internal sealed class ServerByteArrayMessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageConsumer
    {
        /// <summary>
        /// byte[] 消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">服务端字节数组消息客户端节点</param>
        internal ServerByteArrayMessageConsumer(AutoCSer.Net.CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node) : base(commandClient, node) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected override Task<bool> onMessage(byte[]? message)
        {
            lock (messageLock) messages.Remove(message.notNull()[0]);
            return AutoCSer.Common.GetCompletedTask(true);
        }

        /// <summary>
        /// 未完成消息集合
        /// </summary>
        private static Dictionary<int, byte[]> messages = new Dictionary<int, byte[]>();
        /// <summary>
        /// 消费消息测试访问锁
        /// </summary>
        private static readonly object messageLock = new object();
        /// <summary>
        /// byte[] 消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> Test(AutoCSer.Net.CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> client)
        {
            var nodeResult = await client.GetOrCreateServerByteArrayMessageNode(nameof(ServerByteArrayMessageConsumer));
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
                byte[] messageData = new byte[] { (byte)message };
                messages.Add(message, messageData);

                result = await node.AppendMessage(messageData);//生产消息测试
                if (!result.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (ServerByteArrayMessageConsumer consumer = new ServerByteArrayMessageConsumer(commandClient, node))
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
