using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Interlocked.CompareExchange 自旋锁（相对 System.Threading.Monitor 减少一个 object 对象）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SpinLock
    {
        /// <summary>
        /// 锁数据
        /// </summary>
        internal int Lock;
        /// <summary>
        /// 保留
        /// </summary>
        internal int Reserve;
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
            if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
            {
                caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
                return true;
            }
            return false;
#else
            return System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0;
#endif
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnterYield(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
            {
#if DEBUG
                caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#endif
                return;
            }
            enterYield(
#if DEBUG
                callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber
#endif
                );
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        private void enterYield(
#if DEBUG
             string callerMemberName, string callerFilePath, int callerLineNumber
#endif
            )
        {
            ThreadYield.YieldOnly();
            do
            {
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
                {
#if DEBUG
                    caller.Set(callerMemberName, callerFilePath, callerLineNumber);
#endif
                    return;
                }
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
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
        /// 申请锁，一直调用 Thread.Sleep(0)，用于低频场景
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnterSleep(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0)
            {
#if DEBUG
                caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#endif
                return;
            }
            ThreadYield.YieldOnly();

            while (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) != 0)
            {
#if DEBUG
                caller.CheckTimeout(callerMemberName ?? string.Empty);
#endif
                System.Threading.Thread.Sleep(0);
            }
#if DEBUG
            caller.Set(callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#endif
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
            System.Threading.Interlocked.Exchange(ref Lock, 0);
        }
    }
}
