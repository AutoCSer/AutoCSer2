using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage
{
    /// <summary>
    /// 执行任务消息数据回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class TimeoutMessageCallback<T> : QueueTaskNode
    {
        /// <summary>
        /// 超时任务消息节点
        /// </summary>
        private readonly TimeoutMessageNode<T> messageNode;
        /// <summary>
        /// 执行任务消息数据
        /// </summary>
        private readonly T message;
        /// <summary>
        /// 执行任务消息数据回调
        /// </summary>
        /// <param name="messageNode"></param>
        /// <param name="message"></param>
        internal TimeoutMessageCallback(TimeoutMessageNode<T> messageNode, T message)
        {
            this.messageNode = messageNode;
            this.message = message;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            messageNode.Callback(message);
        }
    }
}
