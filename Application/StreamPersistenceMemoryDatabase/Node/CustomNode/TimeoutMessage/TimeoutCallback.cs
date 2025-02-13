using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage
{
    /// <summary>
    /// 超时检查队列回调
    /// </summary>
    /// <typeparam name="T">任务消息数据类型</typeparam>
    internal sealed class TimeoutCallback<T> : QueueTaskNode
    {
        /// <summary>
        /// 超时任务消息节点
        /// </summary>
        private readonly TimeoutMessageNode<T> messageNode;
        /// <summary>
        /// 超时检查队列回调
        /// </summary>
        /// <param name="messageNode"></param>
        internal TimeoutCallback(TimeoutMessageNode<T> messageNode)
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
