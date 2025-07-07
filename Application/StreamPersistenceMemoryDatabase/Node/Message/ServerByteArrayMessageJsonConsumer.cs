using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The JSON mixed binary serialization message consumer of the server-side byte array message
    /// 服务端字节数组消息 JSON 混杂二进制序列化消息消费者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ServerByteArrayMessageJsonConsumer<T> : MessageConsumer<ServerByteArrayMessage>
    {
        /// <summary>
        /// The JSON mixed binary serialization message consumer of the server-side byte array message
        /// 服务端字节数组消息 JSON 混杂二进制序列化消息消费者
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Server-side byte array messages client nodes
        /// 服务端字节数组消息客户端节点</param>
        /// <param name="maxMessageCount">The maximum number of single callback messages on the server side
        /// 服务端单次最大回调消息数量</param>
        /// <param name="delayMilliseconds">The retry interval is in milliseconds, with a default of 1000 and a minimum value of 1
        /// 重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected ServerByteArrayMessageJsonConsumer(ICommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node, int maxMessageCount, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, maxMessageCount, delayMilliseconds) { }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(ServerByteArrayMessage message)
        {
            var value = default(T);
            if (message.JsonDeserialize(ref value)) return onMessage(value);
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
#if NetStandard21
        protected abstract Task<bool> onMessage(T? message);
#else
        protected abstract Task<bool> onMessage(T message);
#endif
    }
}
