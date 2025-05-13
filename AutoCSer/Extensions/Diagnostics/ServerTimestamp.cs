using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 服务端时间戳
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct ServerTimestamp
    {
        /// <summary>
        /// 获取时间戳的服务端时间
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 获取服务端时间的时间戳
        /// </summary>
        public long Timestamp;
        /// <summary>
        /// 每秒始终周期数量
        /// </summary>
        public double TimestampPerSecond;
        /// <summary>
        /// 服务端时间戳
        /// </summary>
        /// <param name="timestampPerSecond">每秒始终周期数量</param>
        public ServerTimestamp(double timestampPerSecond)
        {
            Time = DateTime.UtcNow;
            Timestamp = Stopwatch.GetTimestamp();
            TimestampPerSecond = timestampPerSecond;
        }
        /// <summary>
        /// 服务端时间戳
        /// </summary>
        /// <param name="timestamp">获取服务端时间的时间戳</param>
        private ServerTimestamp(ServerTimestamp timestamp)
        {
            Time = timestamp.Time.ToLocalTime();
            Timestamp = timestamp.Timestamp;
            TimestampPerSecond = timestamp.TimestampPerSecond;
        }
        /// <summary>
        /// UTC 时间转本地时间
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ServerTimestamp UtcToLocalTime()
        {
            return Time.Kind == DateTimeKind.Utc ? new ServerTimestamp(this) : this;
        }
        /// <summary>
        /// 获取服务端时间
        /// </summary>
        /// <param name="timestamp">服务端时间戳</param>
        /// <returns>服务端时间</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DateTime GetTime(long timestamp)
        {
            return Time.AddSeconds((timestamp - Timestamp) / TimestampPerSecond);
        }
    }
}
