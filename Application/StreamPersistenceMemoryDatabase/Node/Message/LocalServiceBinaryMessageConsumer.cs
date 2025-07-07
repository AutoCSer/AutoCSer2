using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Binary serialized message consumer
    /// 二进制序列化消息消费者
    /// </summary>
    /// <typeparam name="T">Message data object type
    /// 消息数据对象类型</typeparam>
    public abstract class LocalServiceBinaryMessageConsumer<T> : LocalServiceMessageConsumer<BinaryMessage<T>>
    {
        /// <summary>
        /// Binary serialized message consumer
        /// 二进制序列化消息消费者
        /// </summary>
        /// <param name="client">Log stream persistence in-memory database local service client
        /// 日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">Binary serialized message client node
        /// 二进制序列化消息消息客户端节点</param>
        /// <param name="delayMilliseconds">The retry interval is in milliseconds, with a default of 1000 and a minimum value of 1
        /// 重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceBinaryMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<BinaryMessage<T>> node, int delayMilliseconds = MessageConsumer.DefaultDelayMilliseconds) : base(client, node, delayMilliseconds) { }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(BinaryMessage<T> message)
        {
            return onMessage((T)message);
        }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
    }
}
