using System;

namespace AutoCSer.ORM.ConditionExpression
{
    /// <summary>
    /// 表达式类型
    /// </summary>
    internal enum ConvertTypeEnum : byte
    {
        /// <summary>
        /// 原始表达式
        /// </summary>
        Expression,
        /// <summary>
        /// 转换表达式
        /// </summary>
        ConvertExpression,
        /// <summary>
        /// 不支持的表达式
        /// </summary>
        NotSupport,
    }
}
