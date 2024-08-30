using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 参数约束类型
    /// </summary>
    public enum ParameterConstraintTypeEnum
    {
        /// <summary>
        /// 无约束
        /// </summary>
        None,
        /// <summary>
        /// 自定义接口约束
        /// </summary>
        ParameterConstraint,
        /// <summary>
        /// 非空字符串
        /// </summary>
        NotEmptyString,
        /// <summary>
        /// 非空集合
        /// </summary>
        NotEmptyCollection,
        /// <summary>
        /// 非默认值 IEquatable{T}
        /// </summary>
        NotDefault,
        /// <summary>
        /// 非 null 值
        /// </summary>
        NotNull,
    }
}
