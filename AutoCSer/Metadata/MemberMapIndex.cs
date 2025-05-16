using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图索引
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct MemberMapIndex<T>
    {
        /// <summary>
        /// 成员位图索引，负数为无效索引
        /// </summary>
        public readonly int MemberIndex;
        /// <summary>
        /// 成员位图索引
        /// </summary>
        /// <param name="memberName">成员名称</param>
#if NetStandard21
        public MemberMapIndex(string? memberName)
#else
        public MemberMapIndex(string memberName)
#endif
        {
            MemberIndex = memberName != null ? MemberMapData<T>.GetMemberIndex(memberName) : -1;
        }
    }
}
