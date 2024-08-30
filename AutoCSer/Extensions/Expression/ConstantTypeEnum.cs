using System;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 常量类型
    /// </summary>
    internal enum ConstantTypeEnum : byte
    {
        /// <summary>
        /// 非常量
        /// </summary>
        None,
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
    }
}
