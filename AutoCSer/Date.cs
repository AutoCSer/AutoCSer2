using AutoCSer.Memory;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace AutoCSer
{
    /// <summary>
    /// 日期相关操作
    /// </summary>
    public unsafe static partial class Date
    {
        /// <summary>
        /// 默认基础时间值 1900/1/1
        /// </summary>
        public static readonly DateTime BaseTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// 初始化时间 Utc
        /// </summary>
        public static readonly DateTime StartTime;
        /// <summary>
        /// 初始化时钟周期
        /// </summary>
        public static readonly long StartTimestamp;
        /// <summary>
        /// 获取初始化时间差
        /// </summary>
        /// <returns></returns>
        public static long TimestampDifference
        {
            get
            {
                if (Stopwatch.IsHighResolution) return Stopwatch.GetTimestamp() - AutoCSer.Date.StartTimestamp;
                return DateTime.UtcNow.Ticks - AutoCSer.Date.StartTimestamp;
            }
        }
        /// <summary>
        /// 1毫秒时间戳
        /// </summary>
        public static readonly long TimestampByMilliseconds;
        /// <summary>
        /// 本地时钟周期
        /// </summary>
        public static readonly long LocalTimeTicks;
        /// <summary>
        /// 时区小时字符串 +HH:
        /// </summary>
        internal static readonly long ZoneHourString;
        /// <summary>
        /// 时区f分钟字符串 mm"
        /// </summary>
        internal static readonly long ZoneMinuteString;
        /// <summary>
        /// 时钟周期与时间戳是否一致
        /// </summary>
        private static readonly bool isTimestampTicks;

        /// <summary>
        /// 32位除以60转乘法的乘数
        /// </summary>
        public const ulong Div60_32Mul = ((1L << Div60_32Shift) + 59) / 60;
        /// <summary>
        /// 32位除以60转乘法的位移
        /// </summary>
        public const int Div60_32Shift = 37;
        /// <summary>
        /// 16位除以60转乘法的乘数
        /// </summary>
        public const uint Div60_16Mul = ((1U << Div60_16Shift) + 59) / 60;
        /// <summary>
        /// 16位除以60转乘法的位移
        /// </summary>
        public const int Div60_16Shift = 21;
        ///// <summary>
        ///// 32位除以 TimeSpan.TicksPerSecond 转乘法的乘数
        ///// </summary>
        //public const ulong DivTicksPerSecondMul = ((1L << DivTicksPerSecondShift) + (TimeSpan.TicksPerSecond - 1)) / TimeSpan.TicksPerSecond;
        ///// <summary>
        ///// 32位除以 TimeSpan.TicksPerSecond 转乘法的位移
        ///// </summary>
        //public const int DivTicksPerSecondShift = 55;

        /// <summary>
        /// 星期
        /// </summary>
        private static AutoCSer.Memory.Pointer weekData;
        /// <summary>
        /// 月份
        /// </summary>
        private static AutoCSer.Memory.Pointer monthData;

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static DateTime localToUniversalTime(this DateTime date)
        {
            return new DateTime(date.Ticks - LocalTimeTicks, DateTimeKind.Utc);
        }

        /// <summary>
        /// 时间转换成日期字符串(yyyy/MM/dd)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="chars">时间字符串</param>
        /// <param name="split">分隔符</param>
        internal static void ToDateString(DateTime time, char* chars, char split = DefaultDateSplit)
        {
            int data0 = time.Year, data1 = (data0 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 4) = split;
            int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 7) = split;
            int data3 = (data2 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(int*)(chars + 2) = ((data1 - data2 * 10) + ((data0 - data1 * 10) << 16)) + 0x300030;
            *(int*)chars = (data3 + ((data2 - data3 * 10) << 16)) + 0x300030;
            data0 = time.Month;
            data2 = time.Day;
            data1 = (data0 + 6) >> 4;
            data3 = (data2 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 5) = (char)(data1 + '0');
            *(chars + 6) = (char)((data0 - data1 * 10) + '0');
            *(int*)(chars + 8) = (data3 + ((data2 - data3 * 10) << 16)) + 0x300030;
        }
        /// <summary>
        /// 时间转换成字符串(HH:mm:ss)
        /// </summary>
        /// <param name="second">当天的计时秒数</param>
        /// <param name="chars">时间字符串</param>
        private static void toTimeString(int second, char* chars)
        {
            int minute = (int)(((ulong)second * Div60_32Mul) >> Div60_32Shift);
            int hour = (minute * (int)Div60_16Mul) >> Div60_16Shift;
            second -= minute * 60;
            int high = (hour * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            minute -= hour * 60;
            *chars = (char)(high + '0');
            *(chars + 1) = (char)((hour - high * 10) + '0');
            *(chars + 2) = ':';
            high = (minute * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(int*)(chars + 3) = (high + ((minute - high * 10) << 16)) + 0x300030;
            *(chars + 5) = ':';
            high = (second * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 6) = (char)(high + '0');
            *(chars + 7) = (char)((second - high * 10) + '0');
        }
        /// <summary>
        /// 时间转换字符串最大字节长度 yyyy-MM-ddTHH:mm:ss.fffffff
        /// </summary>
        public const int ToStringSize = 27;
        /// <summary>
        /// 默认年月日之间的分隔符
        /// </summary>
        public const char DefaultDateSplit = '-';
        /// <summary>
        /// 时间转换成字符串 yyyy-MM-ddTHH:mm:ss.fffffff
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFixed"></param>
        /// <param name="dateTimeSplit">日期与时间之间的分隔符</param>
        /// <param name="dateSplit">年月日之间的分隔符</param>
        /// <returns>字符串长度，可能返回 19/23/27</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int ToString(this DateTime time, char* timeFixed, char dateTimeSplit = 'T', char dateSplit = DefaultDateSplit)
        {
            return toTicksString(timeFixed + 19, ToSecondString(time, timeFixed, dateTimeSplit, dateSplit)) + 19;
        }
        /// <summary>
        /// 时间转换成字符串 .fffffff
        /// </summary>
        /// <param name="timeFixed"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private static int toTicksString(char* timeFixed, long ticks)
        {
            if (ticks == 0) return 0;
            int low = (int)(uint)ticks, high = (int)(((uint)low * AutoCSer.Extensions.NumberExtension.Div10000Mul) >> AutoCSer.Extensions.NumberExtension.Div10000Shift);
            int data1 = (high * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(long*)(timeFixed) = '.' + (data2 << 16) + ((long)(data1 - data2 * 10) << 32) + ((long)(high - data1 * 10) << 48) + 0x30003000300000L;
            if ((low -= high * 10000) == 0) return 4;
            high = (low * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            data1 = (high * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(long*)(timeFixed + 4) = data2 + ((data1 - data2 * 10) << 16) + ((long)(high - data1 * 10) << 32) + ((long)(low - high * 10) << 48) + 0x30003000300030L;
            return 8;
        }
        /// <summary>
        /// 时间转换成字符串 yyyy-MM-ddTHH:mm:ss.fff
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFixed"></param>
        /// <param name="dateTimeSplit">日期与时间之间的分隔符</param>
        /// <param name="dateSplit">年月日之间的分隔符</param>
        /// <returns>字符串长度，可能返回 19/23</returns>
        internal static int ToString3(this DateTime time, char* timeFixed, char dateTimeSplit = 'T', char dateSplit = DefaultDateSplit)
        {
            long ticks = ToSecondString(time, timeFixed, dateTimeSplit, dateSplit);
            if (ticks == 0) return 19;
            int high = (int)(((uint)(int)(uint)ticks * AutoCSer.Extensions.NumberExtension.Div10000Mul) >> AutoCSer.Extensions.NumberExtension.Div10000Shift);
            int data1 = (high * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(long*)(timeFixed + 19) = '.' + (data2 << 16) + ((long)(data1 - data2 * 10) << 32) + ((long)(high - data1 * 10) << 48) + 0x30003000300000L;
            return 23;
        }
        /// <summary>
        /// 时间转换成字符串 yyyy-MM-ddTHH:mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFixed"></param>
        /// <param name="dateTimeSplit">日期与时间之间的分隔符</param>
        /// <param name="dateSplit">年月日之间的分隔符</param>
        /// <returns>不足1秒的周期数</returns>
        internal static long ToSecondString(DateTime time, char* timeFixed, char dateTimeSplit = 'T', char dateSplit = DefaultDateSplit)
        {
            ToDateString(time, timeFixed, dateSplit);
            *(timeFixed + 10) = dateTimeSplit;
            long ticks = time.Ticks % TimeSpan.TicksPerDay, seconds = ticks / TimeSpan.TicksPerSecond;
            toTimeString((int)seconds, timeFixed + 11);
            return ticks - seconds * TimeSpan.TicksPerSecond;
        }
        /// <summary>
        /// 时间转换成字符串
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="charStream">字符流</param>
        /// <param name="dateSplit">年月日之间的分隔符</param>
        internal static void toString(this DateTime time, CharStream charStream, char dateSplit = DefaultDateSplit)
        {
            char* timeFixed = (char*)charStream.GetPrepSizeCurrent(ToStringSize * sizeof(char));
            if (timeFixed != null) charStream.Data.Pointer.MoveSize(ToString(time, timeFixed, time.Kind == DateTimeKind.Utc ? 'T' : ' ', dateSplit) << 1);
        }
        /// <summary>
        /// 时间转换成字符串 yyyy-MM-ddTHH:mm:ss.fffffff
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dateSplit">年月日之间的分隔符</param>
        /// <returns></returns>
        public static string toString(this DateTime time, char dateSplit = DefaultDateSplit)
        {
            char* chars = stackalloc char[ToStringSize];
            return new string(chars, 0, ToString(time, chars, time.Kind == DateTimeKind.Utc ? 'T' : ' ', dateSplit));
        }
        ///// <summary>
        ///// 时间转换成字符串(yyyy/MM/dd HH:mm:ss)
        ///// </summary>
        ///// <param name="time">时间</param>
        ///// <param name="dateSplit">日期分隔符</param>
        ///// <returns>时间字符串</returns>
        //public static string toSecondString(this DateTime time, char dateSplit = '-')
        //{
        //    string timeString = AutoCSer.Common.Config.AllocateString(19);
        //    fixed (char* timeFixed = timeString)
        //    {
        //        toDateString(time, timeFixed, dateSplit);
        //        *(timeFixed + 10) = ' ';
        //        toTimeString((int)((time.Ticks % TimeSpan.TicksPerDay) /? TimeSpan.TicksPerSecond), timeFixed + 11);
        //    }
        //    return timeString;
        //}
        ///// <summary>
        ///// 时间转换成字符串(精确到毫秒) yyyy-MM-ddTHH:mm:ss.fff
        ///// </summary>
        ///// <param name="time">时间</param>
        ///// <param name="chars">时间字符串</param>
        //internal unsafe static void ToMillisecondString(DateTime time, char* chars)
        //{
        //    long dayTiks = time.Ticks % TimeSpan.TicksPerDay;
        //    toDateString(time, chars, DateSplitChar);
        //    long seconds = dayTiks /? TimeSpan.TicksPerSecond;
        //    *(chars + 19) = '.';
        //    *(chars + 10) = ' ';
        //    toTimeString((int)seconds, chars + 11);
        //    int data0 = (int)(((ulong)(dayTiks - seconds * TimeSpan.TicksPerSecond) * AutoCSer.Extensions.NumberExtension.Div10000Mul) >> AutoCSer.Extensions.NumberExtension.Div10000Shift);
        //    int data1 = (data0 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
        //    int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
        //    *(chars + 22) = (char)(data0 - data1 * 10 + '0');
        //    *(int*)(chars + 20) = (data2 + ((data1 - data2 * 10) << 16)) + 0x300030;
        //}
        /// <summary>
        /// 时间转换成字符串 HH:mm:ss.fffffff
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFixed"></param>
        /// <returns>字符串长度</returns>
        internal static int ToString(this TimeSpan time, char* timeFixed)
        {
            long ticks = time.Ticks;
            int timeIndex = 0;
            if (ticks < 0)
            {
                *timeFixed = '-';
                ticks = -ticks;
                timeIndex = 1;
            }
            long hour100 = ticks / (TimeSpan.TicksPerHour * 100);
            if (hour100 != 0)
            {
                timeIndex += AutoCSer.Extensions.NumberExtension.ToString((uint)(int)hour100, timeFixed + timeIndex);
                ticks -= hour100 * (TimeSpan.TicksPerHour * 100);
            }
            long seconds = ticks / TimeSpan.TicksPerSecond;
            toTimeString((int)seconds, timeFixed + timeIndex);
            timeIndex += 8;
            return toTicksString(timeFixed + timeIndex, ticks - seconds * TimeSpan.TicksPerSecond) + timeIndex;
        }
        /// <summary>
        /// 时间转换成字符串 HH:mm:ss.fffffff
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string toString(this TimeSpan time)
        {
            char* chars = stackalloc char[12 + 16];
            return new string(chars, 0, ToString(time, chars));
        }
        /// <summary>
        /// 时间转整数值 Year[23b] + Month[4b] + Day[5b]
        /// </summary>
        /// <param name="time"></param>
        /// <returns>Year[23b] + Month[4b] + Day[5b]</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint toIntDate(this DateTime time)
        {
            return ((uint)time.Year << 9) + ((uint)time.Month << 5) + (uint)time.Day;
        }
        /// <summary>
        /// 整数值转时间
        /// </summary>
        /// <param name="date">Year[23b] + Month[4b] + Day[5b]</param>
        /// <param name="kind"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static DateTime fromIntDate(this uint date, DateTimeKind kind = DateTimeKind.Utc)
        {
            return new DateTime((int)(date >> 9), (int)((date >> 5) & 15), (int)(date & 31), 0, 0, 0, kind);
        }

        /// <summary>
        /// 时间戳转毫秒数
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long GetMillisecondsByTimestamp(long timestamp)
        {
            if (isTimestampTicks) return timestamp / TimeSpan.TicksPerMillisecond;
            return timestamp * 1000 / Stopwatch.Frequency;
        }

        ///// <summary>
        ///// 毫秒数转时间戳乘数
        ///// </summary>
        //private static readonly double millisecondsToTimestamp = Stopwatch.IsHighResolution ? (double)Stopwatch.Frequency / 1000 : TimeSpan.TicksPerMillisecond;
        /// <summary>
        /// 毫秒数转时间戳
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long GetTimestampByMilliseconds(long milliseconds)
        {
            if (isTimestampTicks) return milliseconds * TimeSpan.TicksPerMillisecond; 
            return milliseconds * Stopwatch.Frequency / 1000;
        }
        /// <summary>
        /// 每秒始终周期数量
        /// </summary>
        public static long TimestampPerSecond { get { return Stopwatch.IsHighResolution ? Stopwatch.Frequency : TimeSpan.TicksPerSecond; } }
        /// <summary>
        /// 秒数转时间戳
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long GetTimestampBySeconds(long seconds)
        {
            return seconds * TimestampPerSecond;
        }
        ///// <summary>
        ///// 时钟周期转时间戳乘数
        ///// </summary>
        //private static readonly double ticksToTimestamp = Stopwatch.IsHighResolution ? (double)Stopwatch.Frequency / TimeSpan.TicksPerSecond : 1;
        /// <summary>
        /// 时钟周期转时间戳
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long GetTimestampByTicks(long ticks)
        {
            if (isTimestampTicks) return ticks; 
            return ticks * Stopwatch.Frequency / TimeSpan.TicksPerSecond;//可能溢出
        }
        static Date()
        {
            if (Stopwatch.IsHighResolution)
            {
                StartTime = DateTime.UtcNow;
                StartTimestamp = Stopwatch.GetTimestamp();
                TimestampByMilliseconds = Stopwatch.Frequency / 1000;
                isTimestampTicks = Stopwatch.Frequency == TimeSpan.TicksPerSecond;
            }
            else
            {
                StartTime = DateTime.UtcNow;
                StartTimestamp = StartTime.Ticks;
                TimestampByMilliseconds = TimeSpan.TicksPerMillisecond;
                isTimestampTicks = true;
            }
            LocalTimeTicks = TimeZoneInfo.Local.BaseUtcOffset.Ticks;

            long zoneChar0, localTimeTicks;
            if (LocalTimeTicks >= 0)
            {
                zoneChar0 = '+' + ((long)':' << 48);
                localTimeTicks = LocalTimeTicks;
            }
            else
            {
                zoneChar0 = '-' + ((long)':' << 48);
                localTimeTicks = -LocalTimeTicks;
            }
            long minute = (int)(LocalTimeTicks / TimeSpan.TicksPerMinute);
            int hour = (int)(((ulong)minute * Div60_32Mul) >> Div60_32Shift), high = (hour * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            ZoneHourString = zoneChar0 + ((high + '0') << 16) + ((long)((hour - high * 10) + '0') << 32);
            minute -= hour * 60;
            high = ((int)minute * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            ZoneMinuteString = (high + '0') + ((((int)minute - high * 10) + '0') << 16) + ((long)'"' << 32);

            weekData = Unmanaged.GetDateWeekData();
            int* write = weekData.Int;
            *write = 'S' + ('u' << 8) + ('n' << 16) + (',' << 24);
            *++write = 'M' + ('o' << 8) + ('n' << 16) + (',' << 24);
            *++write = 'T' + ('u' << 8) + ('e' << 16) + (',' << 24);
            *++write = 'W' + ('e' << 8) + ('d' << 16) + (',' << 24);
            *++write = 'T' + ('h' << 8) + ('u' << 16) + (',' << 24);
            *++write = 'F' + ('r' << 8) + ('i' << 16) + (',' << 24);
            *++write = 'S' + ('a' << 8) + ('t' << 16) + (',' << 24);
            monthData = Unmanaged.GetDateMonthData();
            write = monthData.Int;
            *write = 'J' + ('a' << 8) + ('n' << 16) + (' ' << 24);
            *++write = 'F' + ('e' << 8) + ('b' << 16) + (' ' << 24);
            *++write = 'M' + ('a' << 8) + ('r' << 16) + (' ' << 24);
            *++write = 'A' + ('p' << 8) + ('r' << 16) + (' ' << 24);
            *++write = 'M' + ('a' << 8) + ('y' << 16) + (' ' << 24);
            *++write = 'J' + ('u' << 8) + ('n' << 16) + (' ' << 24);
            *++write = 'J' + ('u' << 8) + ('l' << 16) + (' ' << 24);
            *++write = 'A' + ('u' << 8) + ('g' << 16) + (' ' << 24);
            *++write = 'S' + ('e' << 8) + ('p' << 16) + (' ' << 24);
            *++write = 'O' + ('c' << 8) + ('t' << 16) + (' ' << 24);
            *++write = 'N' + ('o' << 8) + ('v' << 16) + (' ' << 24);
            *++write = 'D' + ('e' << 8) + ('c' << 16) + (' ' << 24);
        }
    }
}
