using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端字节数组消息 JSON 序列化消息消费者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class OnServerByteArrayMessageJsonConsumer<T> : ServerByteArrayMessageJsonConsumer<T>
    {
        /// <summary>
        /// 消息处理，异常或者返回 false 则表示消息执行失败
        /// </summary>
#if NetStandard21
        private readonly Func<T?, Task<bool>> getMessageTask;
#else
        private readonly Func<T, Task<bool>> getMessageTask;
#endif
        /// <summary>
        /// 服务端字节数组消息 JSON 序列化消息消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">服务端字节数组消息客户端节点</param>
        /// <param name="onMessage">消息处理，异常或者返回 false 则表示消息执行失败</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
#if NetStandard21
        public OnServerByteArrayMessageJsonConsumer(ICommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node, Func<T?, Task<bool>> onMessage, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds)
#else
        public OnServerByteArrayMessageJsonConsumer(CommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node, Func<T, Task<bool>> onMessage, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds)
#endif
        {
            getMessageTask = onMessage;
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
#if NetStandard21
        protected override Task<bool> onMessage(T? message)
#else
        protected override Task<bool> onMessage(T message)
#endif
        {
            return getMessageTask(message);
        }
    }
}
