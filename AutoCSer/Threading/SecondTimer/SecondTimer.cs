using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 秒级定时操作
    /// </summary>
    public static class SecondTimer
    {
        /// <summary>
        /// 刷新时间的定时器
        /// </summary>
        private readonly static Timer timer;
        /// <summary>
        /// 二维定时任务数组，用于确定性非阻塞的内部任务（队列模式无并发）
        /// </summary>
        internal readonly static SecondTimerArray InternalTaskArray;
        /// <summary>
        /// 二维定时任务数组（队列模式无并发）
        /// </summary>
        public readonly static SecondTimerTaskArray TaskArray;
        /// <summary>
        /// 当前时钟秒数计数
        /// </summary>
        internal static long CurrentSeconds;
        /// <summary>
        /// 当前时钟秒数计数
        /// </summary>
        public static long GetCurrentSeconds { get { return CurrentSeconds; } }
        /// <summary>
        /// 精确到秒的时间
        /// </summary>
        public static DateTime Now { get; private set; }
        /// <summary>
        /// 精确到秒的时间
        /// </summary>
        public static DateTime UtcNow { get; private set; }
        /// <summary>
        /// 重置时间
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static DateTime SetNow()
        {
            DateTime now = DateTime.Now;
            Now = now;
            UtcNow = now.localToUniversalTime();
            return now;
        }
        /// <summary>
        /// 重置时间
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static DateTime SetUtcNow()
        {
            DateTime now = DateTime.Now;
            Now = now;
            UtcNow = now.localToUniversalTime();
            return UtcNow;
        }
        /// <summary>
        /// 下一秒时钟周期
        /// </summary>
        internal static long NextSecondTicks;
        /// <summary>
        /// 当前时间更新间隔
        /// </summary>
        internal static long TimerInterval;
        /// <summary>
        /// 未结束刷新时间线程数量
        /// </summary>
        private static int refreshTimeThreadCount;
        /// <summary>
        /// 未结束刷新时间线程数量
        /// </summary>
        public static int RefreshTimeThreadCount { get { return refreshTimeThreadCount; } }
        /// <summary>
        /// 每秒触发一次的定时任务链表，用于确定性非阻塞的内部任务
        /// </summary>
        internal static SecondTimerNode.YieldLink SecondNodeLink;
        /// <summary>
        /// 刷新时间
        /// </summary>
        /// <param name="state"></param>
#if NetStandard21
        private static void refreshTime(object? state)
#else
        private static void refreshTime(object state)
#endif
        {
            System.Threading.Interlocked.Increment(ref refreshTimeThreadCount);
            DateTime now = DateTime.Now;
            Now = now;
            UtcNow = now.localToUniversalTime();
            timer.Change(TimerInterval = 1000L - now.Millisecond, -1);

            do
            {
                long nextSecondTicks = NextSecondTicks;
                if (nextSecondTicks <= Now.Ticks)
                {
                    if (System.Threading.Interlocked.CompareExchange(ref NextSecondTicks, nextSecondTicks + TimeSpan.TicksPerSecond, nextSecondTicks) == nextSecondTicks)
                    {
                        System.Threading.Interlocked.Increment(ref CurrentSeconds);
                        try
                        {
                            var node = SecondNodeLink.End;
                            if (node != null) SecondTimerNode.LinkOnTimer(node);

                            ThreadPool.CheckExit();
                            InternalTaskArray.OnTimer();
                            TaskArray.OnTimer().NotWait();
                        }
                        catch (Exception exception)
                        {
                            AutoCSer.LogHelper.ExceptionIgnoreException(exception, "全局定时任务错误中断", LogLevelEnum.AutoCSer | LogLevelEnum.Exception | LogLevelEnum.Fatal);
                        }
                    }
                }
                else
                {
                    System.Threading.Interlocked.Decrement(ref refreshTimeThreadCount);
                    return;
                }
            }
            while (true);
        }
        static SecondTimer()
        {
            CurrentSeconds = 1;
            UtcNow = (Now = DateTime.Now).localToUniversalTime();

            byte taskArrayBitSize = AutoCSer.Common.Config.TimeoutCapacityBitSize;
            InternalTaskArray = new SecondTimerArray(taskArrayBitSize);
            TaskArray = new SecondTimerTaskArray(taskArrayBitSize);

            NextSecondTicks = ((Now.Ticks / TimeSpan.TicksPerSecond) + 1) * TimeSpan.TicksPerSecond;
            timer = new Timer(refreshTime, null, TimerInterval = 1000L - Now.Millisecond, -1);
#if !AOT
            AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(SecondTimer));
#endif
        }
    }
}
