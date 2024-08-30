using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据列序号
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberColumnIndex
    {
        /// <summary>
        /// 字段成员
        /// </summary>
        internal readonly Member Member;
        /// <summary>
        /// 自定义数据列序号
        /// </summary>
        internal int Index;
        /// <summary>
        /// 判断是否数据列
        /// </summary>
        /// <returns></returns>
        internal bool IsColumn
        {
            get
            {
                return Member.CustomColumnAttribute == null || Index >= 0;
            }
        }
        /// <summary>
        /// 获取数据列名称
        /// </summary>
        /// <returns></returns>
        internal string ColumnName
        {
            get
            {
                if (Member.CustomColumnAttribute == null) return Member.MemberIndex.Member.Name;
                return Index >= 0 ? Member.CustomColumnNames[Index].Name : null;
            }
        }
        /// <summary>
        /// 判断是否模型成员
        /// </summary>
        internal bool IsMember
        {
            get
            {
                return Member.CustomColumnAttribute == null || Index < 0 || Member.CustomColumnNames[Index].Name == Member.MemberIndex.Member.Name;
            }
        }
        /// <summary>
        /// 数据列序号
        /// </summary>
        /// <param name="member"></param>
        /// <param name="index"></param>
        internal MemberColumnIndex(Member member, int index)
        {
            this.Member = member;
            this.Index = index;
        }
        /// <summary>
        /// 设置数据列序号
        /// </summary>
        /// <param name="member"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Set(Member member, int index)
        {
            if (object.ReferenceEquals(this.Member, member) && this.Index < 0)
            {
                this.Index = index;
                return true;
            }
            return false;
        }
    }
}
