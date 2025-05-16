using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员序列化方法
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeMemberMethod
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
        /// 设置字段信息
        /// </summary>
        /// <param name="field"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(FieldInfo field)
        {
            this.field = field;
            method = Common.GetMemberSerializeDelegate(field.FieldType);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Serialize(BinarySerializer serializer, object value)
        {
            method(serializer, field.GetValue(value));
        }
    }
}
