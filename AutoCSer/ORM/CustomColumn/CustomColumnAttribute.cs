using AutoCSer.Metadata;
using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 自定义数据列配置（只支持 struct 可嵌套）
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class CustomColumnAttribute : Attribute
    {
        /// <summary>
        /// 成员选择类型，默认为字段 InstanceField
        /// </summary>
        public MemberFiltersEnum MemberFilters = MemberFiltersEnum.InstanceField;
        /// <summary>
        /// 自定义数据列名称连接类型
        /// </summary>
        public CustomColumnNameConcatTypeEnum NameConcatType;
        /// <summary>
        /// 名称连接分割符，默认为空字符串
        /// </summary>
        public string NameConcatSplit = string.Empty;
    }
}
