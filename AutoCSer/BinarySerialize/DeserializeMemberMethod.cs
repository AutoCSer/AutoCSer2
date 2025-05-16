using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员反序列化方法
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DeserializeMemberMethod
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
        /// 设置字段信息
        /// </summary>
        /// <param name="field"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(FieldInfo field)
        {
            this.field = field;
            method = Common.GetMemberDeserializeDelegate(field.FieldType);
        }
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Deserialize(BinaryDeserializer deserializer, object value)
        {
            field.SetValue(value, method(deserializer));
        }
    }
}
