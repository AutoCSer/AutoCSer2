using AutoCSer.Metadata;
using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图成员自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ViewMemberAttribute : IgnoreMemberAttribute
    {
        /// <summary>
        /// 绑定输出成员名称（客户端存在依赖逻辑）
        /// </summary>
        public string BindingName;
        /// <summary>
        /// 默认为 true 表示自动输出当前成员
        /// </summary>
        public bool IsAuto = true;
        /// <summary>
        /// 默认为 false 表示不忽略默认值输出，基于 IEquatable{T}
        /// </summary>
        public bool IsIgnoreDefault;
        /// <summary>
        /// 默认为 false 表示仅输出当前成员，否则输出所有子级成员
        /// </summary>
        public bool IsAllMember;

        /// <summary>
        /// 默认数据视图成员自定义属性
        /// </summary>
        internal static readonly ViewMemberAttribute Default = new ViewMemberAttribute();
    }
}
