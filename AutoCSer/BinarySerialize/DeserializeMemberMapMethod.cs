using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员反序列化方法
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeMemberMapMethod
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        private FieldInfo field;
        /// <summary>
        /// 反序列化方法
        /// </summary>
        private Func<BinaryDeserializer, object?> method;
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
            memberIndex = field.MemberIndex;
            method = Common.GetMemberDeserializeDelegate(this.field.FieldType);
        }
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        internal void Deserialize<T>(MemberMap<T> memberMap, BinaryDeserializer deserializer, object value)
        {
            if (memberMap.IsMember(memberIndex)) field.SetValue(value, method(deserializer));
        }
    }
}
