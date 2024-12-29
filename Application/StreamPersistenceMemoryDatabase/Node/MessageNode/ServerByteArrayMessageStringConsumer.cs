using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端字节数组消息 字符串消息消费者
    /// </summary>
    public abstract class ServerByteArrayMessageStringConsumer : MessageConsumer<ServerByteArrayMessage>
    {
        /// <summary>
        /// 服务端字节数组消息 字符串消息消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">服务端字节数组消息客户端节点</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected ServerByteArrayMessageStringConsumer(ICommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected override Task<bool> onMessage(ServerByteArrayMessage message)
        {
            var value = default(string);
            if (message.GetString(ref value)) return onMessage(value);
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
#if NetStandard21
        protected abstract Task<bool> onMessage(string? message);
#else
        protected abstract Task<bool> onMessage(string message);
#endif
    }
}
