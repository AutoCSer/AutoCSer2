using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 数字类型
    /// </summary>
    internal enum NumberTypeEnum : byte
    {
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// NaN
        /// </summary>
        NaN,
        /// <summary>
        /// 正无穷
        /// </summary>
        PositiveInfinity,
        /// <summary>
        /// 负无穷
        /// </summary>
        NegativeInfinity,
        /// <summary>
        /// null 值
        /// </summary>
        Null,
        /// <summary>
        /// 对象
        /// </summary>
        Object,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
    }
}
