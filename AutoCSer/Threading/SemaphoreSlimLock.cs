using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 异步锁（不支持重入，重入则死锁）
    /// </summary>
#if DEBUG
    public sealed class SemaphoreSlimLock
#else
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SemaphoreSlimLock
#endif
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        internal readonly System.Threading.SemaphoreSlim LockObject;
#if DEBUG
        /// <summary>
        /// 锁调用信息
        /// </summary>
        private LockCaller caller;
#endif
        /// <summary>
        /// 异步锁（不支持重入，重入则死锁）
        /// </summary>
        /// <param name="initialCount">初始允许并发数量，默认应该传 1</param>
        /// <param name="maxCount">最大并发数量</param>
        public SemaphoreSlimLock(int initialCount, int maxCount = 1)
        {
            LockObject = new System.Threading.SemaphoreSlim(initialCount, maxCount);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Enter(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
#if DEBUG
            caller.Enter(LockObject, callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#else
            LockObject.Wait();
#endif
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task EnterAsync(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
#if DEBUG
            return caller.EnterAsync(LockObject, callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#else
            return LockObject.WaitAsync();
#endif
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Exit()
        {
#if DEBUG
            caller.Exit();
#endif
            LockObject.Release();
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="semaphoreSlimLock"></param>
        internal static void TryExit(SemaphoreSlimLock semaphoreSlimLock)
        {
#if DEBUG
            if (semaphoreSlimLock != null)
            {
                try
                {
                    semaphoreSlimLock.Exit();
                }
                catch { }
            }
#else
            if (semaphoreSlimLock.LockObject != null)
            {
                try
                {
                    semaphoreSlimLock.Exit();
                }
                catch { }
            }
#endif
        }
    }
}
