using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#if DEBUG
namespace AutoCSer.Threading
{
    /// <summary>
    /// 锁调用信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct LockCaller
    {
        /// <summary>
        /// 锁超时时间戳
        /// </summary>
        private long timeoutTimestamp;
        /// <summary>
        /// 超时检查次数
        /// </summary>
        private int timeoutCount;
        /// <summary>
        /// 锁调用行数
        /// </summary>
        private int callerLineNumber;
        /// <summary>
        ///  锁调用成员
        /// </summary>
        private string callerMemberName;
        /// <summary>
        /// 锁调用文件
        /// </summary>
        private string callerFilePath;
        /// <summary>
        /// 锁调用信息
        /// </summary>
        /// <param name="callerMemberName">锁调用成员</param>
        /// <param name="callerFilePath">锁调用文件</param>
        /// <param name="callerLineNumber">锁调用行数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            this.callerMemberName = callerMemberName;
            this.callerFilePath = callerFilePath;
            this.callerLineNumber = callerLineNumber;
            timeoutTimestamp = getLockTimeoutTimestamp();
            System.Threading.Interlocked.Exchange(ref timeoutCount, 0);
        }
        /// <summary>
        /// 锁超时检查
        /// </summary>
        /// <param name="callerMemberName"></param>
        internal void CheckTimeout(string callerMemberName)
        {
            long timeoutTimestamp = this.timeoutTimestamp;
            if (System.Diagnostics.Stopwatch.GetTimestamp() >= timeoutTimestamp && timeoutTimestamp != 0)
            {
                do
                {
                    int timeoutCount = this.timeoutCount;
                    if (System.Threading.Interlocked.CompareExchange(ref this.timeoutCount, timeoutCount + 1, timeoutCount) == timeoutCount)
                    {
                        if (System.Diagnostics.Stopwatch.GetTimestamp() >= this.timeoutTimestamp)
                        {
                            if (this.timeoutTimestamp != 0)
                            {
                                AutoCSer.LogHelper.ErrorIgnoreException($"锁未释放造成 {callerMemberName} 申请锁超时 {timeoutCount + 1} 次", LogLevelEnum.Error, this.callerMemberName, callerFilePath, callerLineNumber);
                                this.timeoutTimestamp = getLockTimeoutTimestamp();
                            }
                        }
                        else System.Threading.Interlocked.Increment(ref this.timeoutCount);
                        return;
                    }
                    ThreadYield.YieldOnly();
                }
                while (timeoutTimestamp == this.timeoutTimestamp);
            }
        }
        /// <summary>
        /// 锁申请
        /// </summary>
        /// <param name="lockObject">锁对象</param>
        /// <param name="callerMemberName">锁调用成员</param>
        /// <param name="callerFilePath">锁调用文件</param>
        /// <param name="callerLineNumber">锁调用行数</param>
        internal void Enter(object lockObject, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            while (!System.Threading.Monitor.TryEnter(lockObject, lockTimeout)) CheckTimeout(callerMemberName);
            this.callerMemberName = callerMemberName;
            this.callerFilePath = callerFilePath;
            this.callerLineNumber = callerLineNumber;
        }
        /// <summary>
        /// 锁申请
        /// </summary>
        /// <param name="lockObject">锁对象</param>
        /// <param name="callerMemberName">锁调用成员</param>
        /// <param name="callerFilePath">锁调用文件</param>
        /// <param name="callerLineNumber">锁调用行数</param>
        internal void Enter(System.Threading.SemaphoreSlim lockObject, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            while (!lockObject.Wait(lockTimeout)) CheckTimeout(callerMemberName);
            this.callerMemberName = callerMemberName;
            this.callerFilePath = callerFilePath;
            this.callerLineNumber = callerLineNumber;
        }
        /// <summary>
        /// 锁申请
        /// </summary>
        /// <param name="lockObject">锁对象</param>
        /// <param name="callerMemberName">锁调用成员</param>
        /// <param name="callerFilePath">锁调用文件</param>
        /// <param name="callerLineNumber">锁调用行数</param>
        internal async Task EnterAsync(System.Threading.SemaphoreSlim lockObject, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            while (!await lockObject.WaitAsync(lockTimeout)) CheckTimeout(callerMemberName);
            this.callerMemberName = callerMemberName;
            this.callerFilePath = callerFilePath;
            this.callerLineNumber = callerLineNumber;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Exit()
        {
            timeoutTimestamp = 0;
        }

        /// <summary>
        /// 锁超时检查时间
        /// </summary>
        private static readonly TimeSpan lockTimeout;
        /// <summary>
        /// 锁超时检查时间戳
        /// </summary>
        private static long lockTimeoutTimestamp;
        /// <summary>
        /// 获取锁超时检查时间戳
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static long getLockTimeoutTimestamp()
        {
            return System.Diagnostics.Stopwatch.GetTimestamp() + lockTimeoutTimestamp;
        }

        static LockCaller()
        {
            int seconds = Math.Max(AutoCSer.Common.Config.LockTimeoutSeconds, 1);
            lockTimeout = new TimeSpan(0, 0, seconds);

            #region 不能依赖 AutoCSer.Date 否则可能死锁
            lockTimeoutTimestamp = seconds * (Stopwatch.IsHighResolution ? Stopwatch.Frequency : TimeSpan.TicksPerSecond);
            #endregion
        }
    }
}
#endif