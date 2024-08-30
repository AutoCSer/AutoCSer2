using System;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据类型
    /// </summary>
    internal enum DataType : byte
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// int
        /// </summary>
        Int,
        /// <summary>
        /// 逻辑值
        /// </summary>
        Bool,
        /// <summary>
        /// 时间值
        /// </summary>
        DateTime,
        /// <summary>
        /// 时间值
        /// </summary>
        TimeSpan,
        /// <summary>
        /// long
        /// </summary>
        Long,
        /// <summary>
        /// 小数
        /// </summary>
        Decimal,
        /// <summary>
        /// Guid
        /// </summary>
        Guid,
        /// <summary>
        /// byte
        /// </summary>
        Byte,
        /// <summary>
        /// short
        /// </summary>
        Short,
        /// <summary>
        /// 浮点数
        /// </summary>
        Float,
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        Double,
    }
}
