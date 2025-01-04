using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Client
{
    /// <summary>
    /// 服务端泛型消息客户端消费者示例
    /// </summary>
    internal sealed class MessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceBinaryMessageConsumer<Data.TestClass>
    {
        /// <summary>
        /// 服务端泛型消息客户端消费者示例
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="node">二进制序列化消息消息客户端节点</param>
        internal MessageConsumer(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeLocalClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>> node) : base(client, node, 1 << 10) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected override Task<bool> onMessage(Data.TestClass message)
        {
            lock (messageLock) messages.Remove(message.Int);
            return AutoCSer.Common.GetCompletedTask(true);
        }

        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeLocalClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>>> nodeCache = Server.ServiceConfig.Client.CreateNode(client => client.GetOrCreateBinaryMessageNode<Data.TestClass>(nameof(MessageConsumer)));
        /// <summary>
        /// 未完成消息集合
        /// </summary>
        private static Dictionary<int, Data.TestClass> messages = new Dictionary<int, Data.TestClass>();
        /// <summary>
        /// 消费消息测试访问锁
        /// </summary>
        private static readonly object messageLock = new object();
        /// <summary>
        /// 服务端泛型消息客户端消费者示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeLocalClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.BinaryMessage<Data.TestClass>> node = nodeResult.Value.notNull();
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

                result = await node.AppendMessage(messageData);//生产消息测试
                if (!result.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (MessageConsumer consumer = new MessageConsumer(Server.ServiceConfig.Client, node))
            {
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
