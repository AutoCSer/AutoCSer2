using AutoCSer.Metadata;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer
{
    /// <summary>
    /// 自定义序列化成员位图
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct BinarySerializeMemberMap
    {
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        internal MemberMap? MemberMap;
#else
        internal MemberMap MemberMap;
#endif
        /// <summary>
        /// 成员位图
        /// </summary>
#if NetStandard21
        internal MemberMap? CurrentMemberMap;
#else
        internal MemberMap CurrentMemberMap;
#endif
        /// <summary>
        /// JSON 序列化成员位图
        /// </summary>
#if NetStandard21
        internal MemberMap? JsonMemberMap;
#else
        internal MemberMap JsonMemberMap;
#endif

        /// <summary>
        /// 自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="currentMemberMap"></param>
        /// <param name="jsonMemberMap"></param>
#if NetStandard21
        internal BinarySerializeMemberMap(MemberMap? memberMap, MemberMap? currentMemberMap, MemberMap? jsonMemberMap)
#else
        internal BinarySerializeMemberMap(MemberMap memberMap, MemberMap currentMemberMap, MemberMap jsonMemberMap)
#endif
        {
            MemberMap = memberMap;
            CurrentMemberMap = currentMemberMap;
            JsonMemberMap = jsonMemberMap;
        }
    }
}
