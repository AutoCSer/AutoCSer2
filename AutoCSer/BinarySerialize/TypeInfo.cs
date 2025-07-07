using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// Binary serialization type information
    /// 二进制序列化类型信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TypeInfo
    {
        /// <summary>
        /// Collection of member types
        /// 成员类型集合
        /// </summary>
        internal AutoCSer.LeftArray<Type> MemberTypes;
        /// <summary>
        /// Member quantity verification data
        /// 成员数量验证数据
        /// </summary>
        internal readonly int MemberCountVerify;
        /// <summary>
        /// Whether it is simple binary serialization
        /// 是否简单二进制序列化
        /// </summary>
        internal readonly bool IsSimpleSerialize;
        /// <summary>
        /// Binary serialization type information
        /// 二进制序列化类型信息
        /// </summary>
        /// <param name="isSimpleSerialize">Whether it is simple binary serialization
        /// 是否简单二进制序列化</param>
        /// <param name="count">Number of member types
        /// 成员类型数量</param>
        /// <param name="memberCountVerify">Member quantity verification data
        /// 成员数量验证数据</param>
        public TypeInfo(bool isSimpleSerialize, int count, int memberCountVerify)
        {
            IsSimpleSerialize = isSimpleSerialize;
            MemberCountVerify = memberCountVerify;
            MemberTypes = new LeftArray<Type>(count);
        }
        /// <summary>
        /// Add member type
        /// 添加成员类型
        /// </summary>
        /// <param name="memberType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(Type memberType)
        {
            MemberTypes.Add(memberType);
        }
    }
}
