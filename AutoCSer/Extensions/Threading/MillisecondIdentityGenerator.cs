using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Threading
{
    /// <summary>
    /// 毫秒ID生成器
    /// </summary>
    public abstract class MillisecondIdentityGenerator
    {
        /// <summary>
        /// 开始计数时间
        /// </summary>
        protected static readonly DateTime startTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 初始化时间戳
        /// </summary>
        protected static readonly long startTimestamp = AutoCSer.Date.GetTimestampByTicks(AutoCSer.Date.StartTime.Ticks - startTime.Ticks);

        /// <summary>
        /// 毫秒内计数掩码
        /// </summary>
        protected readonly long mask;
        /// <summary>
        /// 当前最大时间戳
        /// </summary>
        protected long maxTimestamp;
        /// <summary>
        /// 当前ID
        /// </summary>
        protected long currentIdentity;
        /// <summary>
        /// ID生成访问锁
        /// </summary>
        internal AutoCSer.Threading.SpinLock IdentityLock;
        /// <summary>
        /// 允许连续时间戳数量
        /// </summary>
        protected ushort timestampCount;
        /// <summary>
        /// 毫秒内计数 2 进制位数 + 分布式编号 2 进制位数
        /// </summary>
        protected readonly byte bits;
        /// <summary>
        /// 分布式编号 2 进制位数
        /// </summary>
        private readonly byte distributedBits;
        /// <summary>
        /// 毫秒ID生成器
        /// </summary>
        /// <param name="mask">毫秒内计数掩码</param>
        /// <param name="bits">毫秒内计数 2 进制位数 + 分布式编号 2 进制位数</param>
        /// <param name="distributedBits">分布式编号 2 进制位数</param>
        protected MillisecondIdentityGenerator(long mask, byte bits, byte distributedBits)
        {
            this.mask = mask;
            this.bits = bits;
            this.distributedBits = distributedBits;
        }
        /// <summary>
        /// 根据 ID 获取时间
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DateTime GetTime(long identity)
        {
            return startTime.AddTicks((long)(identity >> bits) * TimeSpan.TicksPerMillisecond);
        }
        /// <summary>
        /// 根据 ID 获取时间
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DateTime GetLocalTime(long identity)
        {
            return startTime.AddTicks((long)(identity >> bits) * TimeSpan.TicksPerMillisecond).ToLocalTime();
        }
    }
}
