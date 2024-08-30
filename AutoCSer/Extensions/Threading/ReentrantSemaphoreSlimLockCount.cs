using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Extensions.Threading
{
    /// <summary>
    /// 异步锁重入计数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ReentrantSemaphoreSlimLockCount
    {
        /// <summary>
        /// 异步锁
        /// </summary>
        private SemaphoreSlim semaphoreSlim;
        /// <summary>
        /// 重入计数
        /// </summary>
        private int count;
        /// <summary>
        /// 异步锁重入计数
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        internal ReentrantSemaphoreSlimLockCount(SemaphoreSlim semaphoreSlim)
        {
            this.semaphoreSlim = semaphoreSlim;
            count = 1;
        }
        /// <summary>
        /// 申请锁计数
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Enter(SemaphoreSlim semaphoreSlim)
        {
            if (object.ReferenceEquals(this.semaphoreSlim, semaphoreSlim))
            {
                ++count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放锁计数
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        /// <returns>-1 表示不匹配</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Exit(SemaphoreSlim semaphoreSlim)
        {
            if (object.ReferenceEquals(this.semaphoreSlim, semaphoreSlim)) return --count;
            return -1;
        }
    }
}
