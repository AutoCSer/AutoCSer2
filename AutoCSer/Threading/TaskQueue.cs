using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public sealed class TaskQueue : TaskQueueBase
    {
        /// <summary>
        /// 任务队列
        /// </summary>
        private LinkStack<QueueTaskNode> queue;
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.WaitOne();
                if (isDisposed) return;
                AutoCSer.Threading.ThreadYield.YieldOnly();
                var value = queue.GetQueue();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            value.RunTask(ref value);
                            if (isDisposed) return;
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (!isDisposed);
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(QueueTaskNode node)
        {
            if (queue.IsPushHead(node)) WaitHandle.Set();
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(CommandServerCallQueueCustomNode node)
        {
            if (node.CheckQueue()) AddOnly(node);
            else throw new Exception("node.isQueue is true");
        }

        /// <summary>
        /// 默认任务队列，用于系统可延时任务
        /// </summary>
        internal static readonly TaskQueue Default = new TaskQueue();
        /// <summary>
        /// 添加默认任务
        /// </summary>
        /// <param name="action"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AddDefault(Action action)
        {
            Default.AddOnly(new ActionQueueTaskNode(action));
        }
    }
}
