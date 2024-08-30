using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 异步锁扩展
    /// </summary>
    public static class SemaphoreSlimLock
    {
#if DEBUG
        /// <summary>
        /// 异步锁重入计数，第一次（最外层）调用该方法的调用点为当前异步可重入锁的异步上下文，对于上层异步调用无效（非线程安全，不支持多线程并发操作同一个异步上下文）
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
#else
        /// <summary>
        /// 异步锁重入计数，第一次（最外层）调用该方法的调用点为当前异步可重入锁的异步上下文，对于上层异步调用无效（非线程安全，不支持多线程并发操作同一个异步上下文）
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
#endif
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Reentrant(this AutoCSer.Threading.SemaphoreSlimLock semaphoreSlimLock
#if DEBUG
             , [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
            )
        {
#if DEBUG
            AutoCSer.Threading.ReentrantSemaphoreSlimLockManager.Get().Enter(semaphoreSlimLock, callerMemberName, callerFilePath, callerLineNumber);
#else
            AutoCSer.Threading.ReentrantSemaphoreSlimLockManager.Get().Enter(semaphoreSlimLock);
#endif
        }
        /// <summary>
        /// 释放异步锁重入计数（非线程安全，不支持多线程并发操作同一个异步上下文）
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool ReentrantExit(this AutoCSer.Threading.SemaphoreSlimLock semaphoreSlimLock)
        {
            AutoCSer.Threading.ReentrantSemaphoreSlimLockManager manager = AutoCSer.Threading.ReentrantSemaphoreSlimLockManager.Manager.Value;
            return manager != null && manager.Exit(semaphoreSlimLock);
        }
    }
}
