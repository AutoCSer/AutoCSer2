using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端执行队列任务
    /// </summary>
    public abstract class CommandServerCallQueueCustomNode : QueueTaskNode
    {
        /// <summary>
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
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
