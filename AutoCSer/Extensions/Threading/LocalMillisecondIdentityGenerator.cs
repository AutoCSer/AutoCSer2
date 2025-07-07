using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 本地毫秒ID生成器（毫秒内超出计算范围时自动移动到下一个毫秒数据）
    /// </summary>
    public sealed class LocalMillisecondIdentityGenerator : AutoCSer.Extensions.Threading.MillisecondIdentityGenerator
    {
        /// <summary>
        /// 毫秒ID生成器
        /// </summary>
        /// <param name="bits">毫秒内计数 2 进制位数，默认为 20 表示支持持续 278 年每秒 10亿 个 ID</param>
        public LocalMillisecondIdentityGenerator(byte bits = 20) : base((1L << bits) - 1, bits, 0) { }
        /// <summary>
        /// 获取下一个ID
        /// </summary>
        /// <returns></returns>
        public long GetNext()
        {
            IdentityLock.EnterYield();
            long timestamp = startTimestamp + AutoCSer.Date.TimestampDifference;
            if (timestamp < maxTimestamp)
            {
                long identity = ++currentIdentity;
                if ((identity & mask) != 0)
                {
                    IdentityLock.Exit();
                    return identity;
                }
                if (--timestampCount == 0)
                {
                    maxTimestamp += AutoCSer.Extensions.Date.MillisecondTimestampDifferencePerSecond;
                    timestampCount = 1000;
                }
                maxTimestamp += AutoCSer.Extensions.Date.TimestampPerMillisecond;
                IdentityLock.Exit();
                return identity;
            }
            if (timestamp == maxTimestamp)
            {
                long identity = ((currentIdentity >> bits) + 1) << bits;
                if (--timestampCount == 0)
                {
                    maxTimestamp += AutoCSer.Extensions.Date.MillisecondTimestampDifferencePerSecond;
                    timestampCount = 1000;
                }
                currentIdentity = identity;
                maxTimestamp += AutoCSer.Extensions.Date.TimestampPerMillisecond;
                IdentityLock.Exit();
                return identity;
            }
            else
            {
                long identity = AutoCSer.Extensions.Date.GetMillisecondsByTimestamp(timestamp), lastIdentity = currentIdentity >> bits;
                //while (identity <= lastIdentity) ++identity;
                if (identity <= lastIdentity) identity = lastIdentity + 1;
                timestampCount = 1000;
                maxTimestamp = Date.GetTimestampByMilliseconds(identity + 1);
                currentIdentity = (identity <<= bits);
                IdentityLock.Exit();
                return identity;
            }
        }
    }
}
