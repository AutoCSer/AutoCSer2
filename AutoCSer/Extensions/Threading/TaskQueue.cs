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
        private Link<QueueTaskNode>.YieldQueue queue;
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                waitHandle.Wait();
                if (isDisposed) return;
                var value = queue.GetClear();
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
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(QueueTaskNode node)
        {
            if (queue.IsPushHead(node)) waitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void Add(CommandServerCallQueueCustomNode node)
        {
            if (node.CheckQueue()) AddOnly(node);
            else throw new Exception("node.isQueue is true");
        }
    }
}
