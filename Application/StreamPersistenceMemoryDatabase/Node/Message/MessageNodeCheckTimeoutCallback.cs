using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
#if AOT
    /// <summary>
    /// 超时检查队列回调
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    /// <typeparam name="IT">消息处理节点接口类型</typeparam>
    internal sealed class MessageNodeCheckTimeoutCallback<T, IT> : ReadWriteQueueNode
        where IT : class, IMessageNode<T>
#else
    /// <summary>
    /// 超时检查队列回调
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    internal sealed class MessageNodeCheckTimeoutCallback<T> : ReadWriteQueueNode
#endif
        where T : Message<T>
    {
        /// <summary>
        /// 消息处理节点
        /// </summary>
#if AOT
        private readonly MessageNode<T, IT> messageNode;
#else
        private readonly MessageNode<T> messageNode;
#endif
        /// <summary>
        /// 超时检查队列回调
        /// </summary>
        /// <param name="messageNode"></param>
#if AOT
        internal MessageNodeCheckTimeoutCallback(MessageNode<T, IT> messageNode)
#else
        internal MessageNodeCheckTimeoutCallback(MessageNode<T> messageNode)
#endif
        {
            this.messageNode = messageNode;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            messageNode.CheckTimeoutCallback();
        }
    }
}
