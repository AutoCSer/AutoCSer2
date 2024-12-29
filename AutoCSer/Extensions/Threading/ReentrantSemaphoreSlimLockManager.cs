using AutoCSer.Extensions.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 可重入异步锁管理（非线程安全，不支持多线程并发操作同一个异步上下文）
    /// </summary>
    public class ReentrantSemaphoreSlimLockManager
    {
        /// <summary>
        /// 可重入异步锁管理
        /// </summary>
        internal static readonly AsyncLocal<ReentrantSemaphoreSlimLockManager> Manager = new AsyncLocal<ReentrantSemaphoreSlimLockManager>();
        /// <summary>
        /// 获取可重入异步锁管理，第一次（最外层）调用该方法的调用点为当前异步可重入锁的异步上下文，对于上层异步调用无效
        /// </summary>
        /// <returns></returns>
        public static ReentrantSemaphoreSlimLockManager Get()
        {
            var lockManager = Manager.Value;
            if (lockManager == null) Manager.Value = lockManager = new ReentrantSemaphoreSlimLockManager();
            return lockManager;
        }

        /// <summary>
        /// 异步锁重入计数
        /// </summary>
        private LeftArray<ReentrantSemaphoreSlimLockCount> locks = new LeftArray<ReentrantSemaphoreSlimLockCount>(0);
        /// <summary>
        /// 当前异步上下文锁数量
        /// </summary>
        public int Count
        {
            get { return locks.Count; }
        }
        /// <summary>
        /// 申请锁计数
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        private void enter(SemaphoreSlimLock semaphoreSlimLock)
        {
            SemaphoreSlim semaphoreSlim = semaphoreSlimLock.LockObject;
            int index = locks.Length;
            if (index != 0)
            {
                ReentrantSemaphoreSlimLockCount[] lockArray = locks.Array;
                do
                {
                    if (lockArray[--index].Enter(semaphoreSlim)) return;
                }
                while (index != 0);
            }
            locks.Add(new ReentrantSemaphoreSlimLockCount(semaphoreSlim));
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Enter(SemaphoreSlimLock semaphoreSlimLock
#if DEBUG
#if NetStandard21
             , [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             , [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            enter(semaphoreSlimLock);
#if DEBUG
            semaphoreSlimLock.Enter(callerMemberName, callerFilePath, callerLineNumber);
#else
            semaphoreSlimLock.Enter();
#endif
        }
#if DEBUG
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
#else
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        /// <returns></returns>
#endif
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task EnterAsync(SemaphoreSlimLock semaphoreSlimLock
#if DEBUG
#if NetStandard21
             , [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             , [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            enter(semaphoreSlimLock);
#if DEBUG
            return semaphoreSlimLock.EnterAsync(callerMemberName, callerFilePath, callerLineNumber);
#else
            return semaphoreSlimLock.EnterAsync();
#endif
        }
        /// <summary>
        /// 释放锁计数
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        /// <returns>失败表示没有找到需要释放的锁，意味着应用层出现锁申请与释放不匹配的情况</returns>
        public bool Exit(SemaphoreSlimLock semaphoreSlimLock)
        {
            SemaphoreSlim semaphoreSlim = semaphoreSlimLock.LockObject;
            int index = locks.Length;
            if (index != 0)
            {
                ReentrantSemaphoreSlimLockCount[] lockArray = locks.Array;
                do
                {
                    int count = lockArray[--index].Exit(semaphoreSlim);
                    if (count >= 0)
                    {
                        if (count == 0) locks.RemoveAt(index);
                        return true;
                    }
                }
                while (index != 0);
            }
            return false;
        }
    }
}
