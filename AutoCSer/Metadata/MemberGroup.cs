using System;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员分组
    /// </summary>
    internal sealed class MemberGroup
    {
        /// <summary>
        /// 公有动态字段
        /// </summary>
        internal LeftArray<FieldInfo> PublicFields;
        /// <summary>
        /// 非公有动态字段
        /// </summary>
        internal LeftArray<FieldInfo> NonPublicFields;
        /// <summary>
        /// 公有动态属性
        /// </summary>
        internal LeftArray<PropertyInfo> PublicProperties;
        /// <summary>
        /// 非公有动态属性
        /// </summary>
        internal LeftArray<PropertyInfo> NonPublicProperties;
        /// <summary>
        /// 成员分组
        /// </summary>
        internal MemberGroup()
        {
            PublicFields.SetEmpty();
            NonPublicFields.SetEmpty();
            PublicProperties.SetEmpty();
            NonPublicProperties.SetEmpty();
        }
    }
}
