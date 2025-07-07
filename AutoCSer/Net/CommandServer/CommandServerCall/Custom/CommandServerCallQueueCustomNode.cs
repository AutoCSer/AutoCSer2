using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Server-side custom queue task nodes
    /// 服务端自定义队列任务节点
    /// </summary>
    public abstract class CommandServerCallQueueCustomNode : QueueTaskNode
    {
        /// <summary>
        /// Has it been added to the queue
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// Check whether it has been added to the queue
        /// 检查是否已经添加到队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckQueue()
        {
            return System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0;
        }
    }
}
