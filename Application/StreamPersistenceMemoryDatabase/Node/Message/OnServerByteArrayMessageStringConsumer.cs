using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The string message consumer of the server-side byte array message
    /// 服务端字节数组消息 字符串消息消费者
    /// </summary>
    public sealed class OnServerByteArrayMessageStringConsumer : ServerByteArrayMessageStringConsumer
    {
        /// <summary>
        /// Message processing: An exception or a return of false indicates that the message execution has failed
        /// 消息处理，异常或者返回 false 则表示消息执行失败
        /// </summary>
#if NetStandard21
        private readonly Func<string?, Task<bool>> getMessageTask;
#else
        private readonly Func<string, Task<bool>> getMessageTask;
#endif
        /// <summary>
        /// The string message consumer of the server-side byte array message
        /// 服务端字节数组消息 字符串消息消费者
        /// </summary>
        /// <param name="commandClient">Command client</param>
        /// <param name="node">Server-side byte array messages client nodes
        /// 服务端字节数组消息客户端节点</param>
        /// <param name="onMessage">Message processing: An exception or a return of false indicates that the message execution has failed
        /// 消息处理，异常或者返回 false 则表示消息执行失败</param>
        /// <param name="delayMilliseconds">The retry interval is in milliseconds, with a default of 1000 and a minimum value of 1
        /// 重试间隔毫秒数，默认为 1000，最小值为 1</param>
#if NetStandard21
        public OnServerByteArrayMessageStringConsumer(ICommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node, Func<string?, Task<bool>> onMessage, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds)
#else
        public OnServerByteArrayMessageStringConsumer(CommandClient commandClient, IMessageNodeClientNode<ServerByteArrayMessage> node, Func<string, Task<bool>> onMessage, int delayMilliseconds = DefaultDelayMilliseconds) : base(commandClient, node, delayMilliseconds)
#endif
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
#if NetStandard21
        protected override Task<bool> onMessage(string? message)
#else
        protected override Task<bool> onMessage(string message)
#endif
        {
            return getMessageTask(message);
        }
    }
}
