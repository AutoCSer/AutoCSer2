using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二进制序列化消息节点消费者
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public sealed class OnBinaryMessageConsumer<T> : BinaryMessageConsumer<T>
    {
        /// <summary>
        /// 消息处理，异常或者返回 false 则表示消息执行失败
        /// </summary>
        private readonly Func<T, Task<bool>> getMessageTask;
        /// <summary>
        /// 二进制序列化消息消费者
        /// </summary>
        /// <param name="commandClient">客户端</param>
        /// <param name="node">二进制序列化消息消息客户端节点</param>
        /// <param name="onMessage">消息处理，异常或者返回 false 则表示消息执行失败</param>
        /// <param name="delayMilliseconds">重试间隔毫秒数，默认为 1000，最小值为 1</param>
        public OnBinaryMessageConsumer(CommandClient commandClient, IMessageNodeClientNode<BinaryMessage<T>> node, Func<T, Task<bool>> onMessage, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds)
        {
            getMessageTask = onMessage;
        }
        /// <summary>
        /// 消息处理，异常则表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>消息是否执行成功</returns>
        protected override Task<bool> onMessage(T message)
        {
            return getMessageTask(message);
        }
    }
}
