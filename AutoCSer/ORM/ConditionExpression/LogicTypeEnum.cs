using System;

namespace AutoCSer.ORM.ConditionExpression
{
    /// <summary>
    /// 逻辑值类型
    /// </summary>
    public enum LogicTypeEnum : byte
    {
        /// <summary>
        /// 逻辑真值
        /// </summary>
        False,
        /// <summary>
        /// 逻辑假值
        /// </summary>
        True,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 不支持的表达式
        /// </summary>
        NotSupport,
    }
}
