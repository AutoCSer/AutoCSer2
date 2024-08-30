using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 自定义数据列名称
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CustomColumnName
    {
        /// <summary>
        /// 成员类型
        /// </summary>
        internal Member Member;
        /// <summary>
        /// 数据列名称
        /// </summary>
        internal string Name;
        /// <summary>
        /// 自定义数据列名称
        /// </summary>
        /// <param name="member"></param>
        /// <param name="name"></param>
        internal CustomColumnName(Member member, string name)
        {
            Member = member;
            Name = name;
        }
        /// <summary>
        /// 设置自定义数据列名称
        /// </summary>
        /// <param name="member"></param>
        /// <param name="name"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(Member member, string name)
        {
            Member = member;
            Name = name;
        }
    }
}
