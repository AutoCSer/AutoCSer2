using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二进制序列化消息节点消费者
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public abstract class LocalServiceBinaryMessageConsumer<T> : LocalServiceMessageConsumer<BinaryMessage<T>>
    {
        /// <summary>
        /// 二进制序列化消息消费者
        /// </summary>
        /// <param name="client">日志流持久化内存数据库本地服务客户端</param>
        /// <param name="node">二进制序列化消息消息客户端节点</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected LocalServiceBinaryMessageConsumer(LocalClient client, IMessageNodeLocalClientNode<BinaryMessage<T>> node, int delayMilliseconds = MessageConsumer.DefaultDelayMilliseconds) : base(client, node, delayMilliseconds) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected override Task<bool> onMessage(BinaryMessage<T> message)
        {
            return onMessage((T)message);
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected abstract Task<bool> onMessage(T message);
    }
}
