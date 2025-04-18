using System;
using System.Reflection;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 成员信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Member
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        internal readonly FieldInfo Field;
        /// <summary>
        /// 成员名称信息
        /// </summary>
        internal readonly MemberInfo NameMember;
        /// <summary>
        /// 成员配置
        /// </summary>
        internal readonly MemberAttribute Attribute;
        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="nameMember"></param>
        /// <param name="attribute"></param>
        internal Member(FieldInfo field, MemberInfo nameMember, MemberAttribute attribute)
        {
            this.Field = field;
            this.NameMember = nameMember;
            this.Attribute = attribute;
        }
    }
}
