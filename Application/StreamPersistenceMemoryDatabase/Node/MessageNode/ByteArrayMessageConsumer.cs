using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字节数组消息节点消费者
    /// </summary>
    public abstract class ByteArrayMessageConsumer : MessageConsumer<ByteArrayMessage>
    {
        /// <summary>
        /// 字节数组消息消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">字节数组消息客户端节点</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        protected ByteArrayMessageConsumer(CommandClient commandClient, IMessageNodeClientNode<ByteArrayMessage> node, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds) { }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override async Task onMessage(ByteArrayMessage message)
        {
            await onMessage((byte[])message);
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task onMessage(byte[] message);
    }
}
