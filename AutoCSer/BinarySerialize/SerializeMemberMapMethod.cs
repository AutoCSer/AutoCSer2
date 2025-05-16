using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员序列化方法
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberMapMethod
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        private FieldInfo field;
        /// <summary>
        /// 序列化方法
        /// </summary>
        private Action<BinarySerializer, object?> method;
        /// <summary>
        /// 成员索引
        /// </summary>
        private int memberIndex;
        /// <summary>
        /// 设置字段信息
        /// </summary>
        /// <param name="field"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(FieldIndex field)
        {
            this.field = field.Member;
            method = Common.GetMemberSerializeDelegate(this.field.FieldType);
            memberIndex = field.MemberIndex;
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Serialize<T>(MemberMap<T> memberMap, BinarySerializer serializer, object value)
        {
            if (memberMap.IsMember(memberIndex)) method(serializer, field.GetValue(value));
        }
    }
}
