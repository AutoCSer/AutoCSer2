using System;
using System.Reflection;

namespace AutoCSer.FieldEquals
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
        /// 成员信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="nameMember"></param>
        internal Member(FieldInfo field, MemberInfo nameMember)
        {
            this.Field = field;
            this.NameMember = nameMember;
        }
    }
}
