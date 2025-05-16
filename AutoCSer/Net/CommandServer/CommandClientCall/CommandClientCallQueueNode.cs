using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端执行队列任务
    /// </summary>
    public abstract class CommandClientCallQueueNode : AutoCSer.Threading.Link<CommandClientCallQueueNode>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public abstract void RunTask(CommandClientCallQueue queue);
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="next"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void RunTask(CommandClientCallQueue queue, ref CommandClientCallQueueNode? next)
#else
        internal void RunTask(CommandClientCallQueue queue, ref CommandClientCallQueueNode next)
#endif
        {
            next = LinkNext;
            LinkNext = null;
            RunTask(queue);
        }
    }
}
