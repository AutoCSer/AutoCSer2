using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 超时检查队列回调
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    internal sealed class MessageNodeCheckTimeoutCallback<T> : QueueTaskNode
        where T : Message<T>
    {
        /// <summary>
        /// 消息处理节点
        /// </summary>
        private readonly MessageNode<T> messageNode;
        /// <summary>
        /// 超时检查队列回调
        /// </summary>
        /// <param name="messageNode"></param>
        internal MessageNodeCheckTimeoutCallback(MessageNode<T> messageNode)
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
