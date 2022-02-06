using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 日期相关操作
    /// </summary>
    public static class Date
    {
        /// <summary>
        /// 每毫秒时间戳
        /// </summary>
        internal static readonly long TimestampPerMillisecond = Stopwatch.IsHighResolution ? Stopwatch.Frequency / 1000 : TimeSpan.TicksPerMillisecond;
        /// <summary>
        /// 每秒 毫秒时间戳误差
        /// </summary>
        internal static readonly long MillisecondTimestampDifferencePerSecond = Stopwatch.IsHighResolution ? Stopwatch.Frequency - Stopwatch.Frequency / 1000 * 1000 : 0;

        /// <summary>
        /// 时间戳转毫秒数乘数
        /// </summary>
        private static readonly double timestampToMilliseconds = Stopwatch.IsHighResolution ? 1000 / (double)Stopwatch.Frequency : (1 / (double)TimeSpan.TicksPerMillisecond);
        /// <summary>
        /// 时间戳转毫秒数
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static long GetMillisecondsByTimestamp(long timestamp)
        {
            if (Stopwatch.IsHighResolution) return (long)(timestamp * timestampToMilliseconds);
            return timestamp / TimeSpan.TicksPerMillisecond;
        }
    }
}
