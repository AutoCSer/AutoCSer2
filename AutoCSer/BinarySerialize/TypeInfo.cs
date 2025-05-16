using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制序列化类型信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TypeInfo
    {
        /// <summary>
        /// 成员类型集合
        /// </summary>
        internal AutoCSer.LeftArray<Type> MemberTypes;
        /// <summary>
        /// 成员数量验证数据
        /// </summary>
        internal readonly int MemberCountVerify;
        /// <summary>
        /// 是否简单二进制序列化
        /// </summary>
        internal readonly bool IsSimpleSerialize;
        /// <summary>
        /// 二进制序列化类型信息
        /// </summary>
        /// <param name="isSimpleSerialize">是否简单二进制序列化</param>
        /// <param name="count">成员类型数量</param>
        /// <param name="memberCountVerify">成员数量验证数据</param>
        public TypeInfo(bool isSimpleSerialize, int count, int memberCountVerify)
        {
            IsSimpleSerialize = isSimpleSerialize;
            MemberCountVerify = memberCountVerify;
            MemberTypes = new LeftArray<Type>(count);
        }
        /// <summary>
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
