using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 委托任务节点
    /// </summary>
    public sealed class ActionQueueTaskNode : QueueTaskNode
    {
        /// <summary>
        /// 任务委托
        /// </summary>
        private readonly Action action;
        /// <summary>
        /// 委托任务节点
        /// </summary>
        /// <param name="action">任务委托</param>
        public ActionQueueTaskNode(Action action)
        {
            this.action = action;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            action();
        }

        /// <summary>
        /// 默认空队列任务节点
        /// </summary>
        internal static readonly ActionQueueTaskNode Empty = new ActionQueueTaskNode(AutoCSer.Common.EmptyAction);
    }
}
