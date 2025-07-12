using System;

namespace AutoCSer.Document.ServerRegistry.MessageNodeClusterClient
{
    /// <summary>
    /// Client JSONN mixed binary  message client consumer example
    /// 客户端 JSON 混杂二进制消息客户端消费者示例
    /// </summary>
    internal sealed class MessageConsumer : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessageJsonConsumer<Data.TestClass>
    {
        /// <summary>
        /// Client JSONN mixed binary  message client consumer example
        /// 客户端 JSON 混杂二进制消息客户端消费者示例
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Server-side byte array messages client nodes
        /// 服务端字节数组消息客户端节点</param>
        internal MessageConsumer(AutoCSer.Net.CommandClient commandClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage> node) : base(commandClient, node, 1 << 8) { }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(Data.TestClass? message)
        {
            Console.Write('-');
            return AutoCSer.Common.GetCompletedTask(true);
        }
    }
}
