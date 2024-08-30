using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 日期时间类型
    /// </summary>
    public enum DateTimeTypeEnum : byte
    {
        /// <summary>
        /// 默认日期时间 1753/1/1~9999/12/31 DateTime yyyy/MM/dd HH:mm:ss.fff
        /// </summary>
        DateTime,
        /// <summary>
        /// 高精度日期时间 DateTime yyyy/MM/dd HH:mm:ss.fffffff
        /// </summary>
        DateTime2,
        /// <summary>
        /// 日期时间 DateTime 1900/1/1~2079/6/6
        /// </summary>
        SmallDateTime,
        /// <summary>
        /// 日期 yyyy/MM/dd
        /// </summary>
        Date,
    }
}
