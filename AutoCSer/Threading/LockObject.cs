using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 锁对象
    /// </summary>
#if DEBUG
    public sealed class LockObject
#else
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LockObject
#endif
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object lockObject;
#if DEBUG
        /// <summary>
        /// 锁调用信息
        /// </summary>
        private LockCaller caller;
#endif
        /// <summary>
        /// 锁对象
        /// </summary>
        /// <param name="lockObject"></param>
        public LockObject(object lockObject)
        {
            this.lockObject = lockObject;
        }
        /// <summary>
        /// 等待锁
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
            caller.Enter(lockObject, callerMemberName ?? string.Empty, callerFilePath ?? string.Empty, callerLineNumber);
#else
            System.Threading.Monitor.Enter(lockObject);
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
            System.Threading.Monitor.Exit(lockObject);
        }
#if DEBUG
        /// <summary>
        /// 启动线程检测锁占用状态
        /// </summary>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckThread(string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            caller.Set(callerMemberName, callerFilePath, callerLineNumber);
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(checkThread);
        }
        /// <summary>
        /// 线程检测锁占用状态
        /// </summary>
        private void checkThread()
        {
            caller.Enter(lockObject, string.Empty, string.Empty, 0);
        }
#endif
    }
}
