using System;

namespace AutoCSer.ORM.ConditionExpression
{
    /// <summary>
    /// 异常类型
    /// </summary>
    internal enum ExceptionTypeEnum : byte
    {
        /// <summary>
        /// 目标对象为 null
        /// </summary>
        TargetIsNull = 1,
        /// <summary>
        /// 数组不是常量
        /// </summary>
        ArrayNotConstant,
        /// <summary>
        /// 目标对象不是数组
        /// </summary>
        TargetNotArray,
        /// <summary>
        /// 数组索引不是 int 常量
        /// </summary>
        ArrayIndexNotInt,
        /// <summary>
        /// 数组索引超出范围
        /// </summary>
        ArrayIndexOutOfRange,
    }
}
