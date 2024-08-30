using System;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 成员配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MemberAttribute : Attribute
    {
        /// <summary>
        /// 默认为 true 表示允许 null 值
        /// </summary>
        public bool IsNullable = true;

        /// <summary>
        /// 默认成员配置
        /// </summary>
        internal static readonly MemberAttribute Default = new MemberAttribute();
    }
}
