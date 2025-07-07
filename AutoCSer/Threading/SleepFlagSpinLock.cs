using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 休眠标志自旋锁
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SleepFlagSpinLock
    {
        /// <summary>
        /// 锁数据
        /// </summary>
        private int lockValue;
        /// <summary>
        /// 休眠标志
        /// </summary>
        public volatile int SleepFlag;
#if DEBUG
        /// <summary>
        /// 锁调用信息
        /// </summary>
        private LockCaller caller;
#endif
        /// <summary>
        /// Apply for a lock
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryEnter(
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
            if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
            {
                caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
                return true;
            }
            return false;
#else
            return System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0;
#endif
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
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
            if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
            {
#if DEBUG
                caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#endif
                return;
            }
            enter(
#if DEBUG
                callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber
#endif
                );
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        private void enter(
#if DEBUG
             string callerMemberName, string callerFilePath, int callerLineNumber
#endif
            )
        {
            ThreadYield.YieldOnly();
            do
            {
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                if (SleepFlag == 0)
                {
                    ThreadYield.YieldOnly();
                    if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                    {
#if DEBUG
                        caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                        return;
                    }
                    if (SleepFlag == 0)
                    {
                        ThreadYield.YieldOnly();
                        if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                        {
#if DEBUG
                            caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                            return;
                        }
                        if (SleepFlag == 0)
                        {
                            ThreadYield.YieldOnly();
                            if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                            {
#if DEBUG
                                caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                                return;
                            }
                            if (SleepFlag == 0)
                            {
                                ThreadYield.YieldOnly();
                                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                                {
#if DEBUG
                                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                                    return;
                                }
                            }
                        }
                    }
                }
#if DEBUG
                caller.CheckTimeout(callerMemberName);
#endif
                System.Threading.Thread.Sleep(0);
            }
            while (true);
        }
        /// <summary>
        /// 申请锁并设置休眠标志，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnterSleepFlag(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            Enter(
#if DEBUG
                callerMemberName, callerFilePath, callerLineNumber
#endif
                );
            SleepFlag = 1;
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频高冲突场景（不检测休眠标识，和 SpinLock.Enter4 效果一样）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnterNotCheckSleepFlag(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
            {
#if DEBUG
                caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#endif
                return;
            }
            enterNotCheckSleepFlag(
#if DEBUG
                callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber
#endif
                );

        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频高冲突场景（不检测休眠标识，和 SpinLock.Enter4 效果一样）
        /// </summary>
        private void enterNotCheckSleepFlag(
#if DEBUG
             string callerMemberName, string callerFilePath, int callerLineNumber
#endif
            )
        {
            ThreadYield.YieldOnly();
            do
            {
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
#if DEBUG
                caller.CheckTimeout(callerMemberName);
#endif
                System.Threading.Thread.Sleep(0);
            }
            while (true);
        }
        /// <summary>
        /// Release the lock
        /// 释放锁
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Exit()
        {
#if DEBUG
            caller.Exit();
#endif
            System.Threading.Interlocked.Exchange(ref lockValue, 0);
        }
        /// <summary>
        /// 重置休眠标志并释放锁
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ExitSleepFlag()
        {
            SleepFlag = 0;
            Exit();
        }
    }
}
