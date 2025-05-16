using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 一次性等待锁
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct OnceAutoWaitHandle
    {
        /// <summary>
        /// 同步等待锁
        /// </summary>
        private object waitLock;
        /// <summary>
        /// 是否等待中
        /// </summary>
        internal volatile int IsWait;
        /// <summary>
        /// 保留
        /// </summary>
        internal int Reserved;
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="waitLock">同步等待锁</param>
        /// <param name="isWait">是否等待中</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(object waitLock, int isWait = 0)
        {
            this.waitLock = waitLock;
            this.IsWait = isWait;
        }
        /// <summary>
        /// 等待结束
        /// </summary>
        internal void Wait()
        {
            Monitor.Enter(waitLock);
            if (IsWait == 0)
            {
                IsWait = 1;
                Monitor.Wait(waitLock);
            }
            IsWait = 0;
            Monitor.Exit(waitLock);
        }
        /// <summary>
        /// 结束等待
        /// </summary>
        internal void Set()
        {
            Monitor.Enter(waitLock);
            if (IsWait == 0) IsWait = 1;
            else Monitor.Pulse(waitLock);
            Monitor.Exit(waitLock);
        }
    }
}
