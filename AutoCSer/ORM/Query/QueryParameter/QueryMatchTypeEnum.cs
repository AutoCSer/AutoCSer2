using System;

namespace AutoCSer.ORM.QueryParameter
{
    /// <summary>
    /// 查询匹配类型
    /// </summary>
    public enum QueryMatchTypeEnum : byte
    {
        /// <summary>
        /// 默认匹配类型，大多数情况为 Equal
        /// </summary>
        Default,
        /// <summary>
        /// 等于
        /// </summary>
        Equal,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,
        /// <summary>
        /// 小于
        /// </summary>
        Less,
        /// <summary>
        /// 大于
        /// </summary>
        Greater,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessOrEqual,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterOrEqual,
        /// <summary>
        /// IN 表达式
        /// </summary>
        In,
        /// <summary>
        /// NOT IN 表达式
        /// </summary>
        NotIn,
        /// <summary>
        /// LIKE %value%（查询参数字段必须为 string）
        /// </summary>
        Like,
        /// <summary>
        /// NOT LIKE %value%（查询参数字段必须为 string）
        /// </summary>
        NotLike,
        /// <summary>
        /// LIKE value%（查询参数字段必须为 string）
        /// </summary>
        StartsWith,
        /// <summary>
        /// NOT LIKE value%（查询参数字段必须为 string）
        /// </summary>
        NotStartsWith,
        /// <summary>
        /// LIKE %value（查询参数字段必须为 string）
        /// </summary>
        EndsWith,
        /// <summary>
        /// NOT LIKE %value（查询参数字段必须为 string）
        /// </summary>
        NotEndsWith,
        /// <summary>
        /// CONTAINS(value,matchValue)（查询参数字段必须为 string）
        /// </summary>
        Contains,
        /// <summary>
        /// NOT CONTAINS(value,matchValue)（查询参数字段必须为 string）
        /// </summary>
        NotContains,
    }
}
