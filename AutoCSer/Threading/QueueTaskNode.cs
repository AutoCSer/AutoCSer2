using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Queue task node
    /// 队列任务节点
    /// </summary>
    public abstract class QueueTaskNode : AutoCSer.Threading.Link<QueueTaskNode>
    {
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        public abstract void RunTask();
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClearLinkRunTask()
        {
            LinkNext = null;
            RunTask();
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void RunTask(ref QueueTaskNode? next)
#else
        internal void RunTask(ref QueueTaskNode next)
#endif
        {
            next = LinkNext;
            LinkNext = null;
            RunTask();
            //CheckRunTask();
        }
        /// <summary>
        /// Queue task execution exception
        /// 队列任务执行异常
        /// </summary>
        /// <param name="exception"></param>
        internal virtual void OnException(Exception exception)
        {
            AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// Server-side queue timeout notification
        /// 服务端队列超时通知
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        internal virtual Task OnTimeout(AutoCSer.Net.CommandServerCallQueue queue, long seconds)
        {
            if (queue.Controller == null) AutoCSer.LogHelper.DebugIgnoreException($"服务队列 [{queue.Index}] 任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            else AutoCSer.LogHelper.DebugIgnoreException($"控制器 {queue.Controller.ControllerName} 队列任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            return AutoCSer.Common.CompletedTask;
        }
    }
}
