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
    public sealed class OnBinaryMessageConsumer<T> : BinaryMessageConsumer<T>
    {
        /// <summary>
        /// Message processing: An exception or a return of false indicates that the message execution has failed
        /// 消息处理，异常或者返回 false 则表示消息执行失败
        /// </summary>
        private readonly Func<T, Task<bool>> getMessageTask;
        /// <summary>
        /// Binary serialized message consumer
        /// 二进制序列化消息消费者
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Binary serialized message client node
        /// 二进制序列化消息消息客户端节点</param>
        /// <param name="onMessage">Message processing: An exception or a return of false indicates that the message execution has failed
        /// 消息处理，异常或者返回 false 则表示消息执行失败</param>
        /// <param name="delayMilliseconds">The retry interval is in milliseconds, with a default of 1000 and a minimum value of 1
        /// 重试间隔毫秒数，默认为 1000，最小值为 1</param>
        public OnBinaryMessageConsumer(ICommandClient commandClient, IMessageNodeClientNode<BinaryMessage<T>> node, Func<T, Task<bool>> onMessage, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds)
        {
            getMessageTask = onMessage;
        }
        /// <summary>
        /// Message processing. An exception also indicates that the message execution failed
        /// 消息处理，异常也表示消息执行失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Whether the message was executed successfully
        /// 消息是否执行成功</returns>
        protected override Task<bool> onMessage(T message)
        {
            return getMessageTask(message);
        }
    }
}
