using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员序列化（AOT 反射模式）
    /// </summary>
    internal sealed class SerializeMemberMap
    {
        /// <summary>
        /// 序列化方法集合
        /// </summary>
        private readonly SerializeMemberMapMethod[] fixedFields;
        /// <summary>
        /// 序列化方法集合
        /// </summary>
        private readonly SerializeMemberMapMethod[] fields;
        /// <summary>
        /// 固定字节数
        /// </summary>
        private readonly int fixedSize;
        /// <summary>
        /// 泛型类型成员序列化
        /// </summary>
        /// <param name="fieldSizeArray"></param>
        internal SerializeMemberMap(ref FieldSizeArray fieldSizeArray)
        {
            fixedSize = (fieldSizeArray.FixedSize + (sizeof(int) + 3)) & (int.MaxValue - 3);
            fixedFields = fieldSizeArray.FixedFields.Length != 0 ? new SerializeMemberMapMethod[fieldSizeArray.FixedFields.Length] : EmptyArray<SerializeMemberMapMethod>.Array;
            fields = fieldSizeArray.FieldArray.Length != 0 ? new SerializeMemberMapMethod[fieldSizeArray.FieldArray.Length] : EmptyArray<SerializeMemberMapMethod>.Array;
            int index = 0;
            foreach (FieldSize field in fieldSizeArray.FixedFields) fixedFields[index++].Set(field.FieldIndex);
            index = 0;
            foreach (FieldSize field in fieldSizeArray.FieldArray) fields[index++].Set(field.FieldIndex);
        }
        /// <summary>
        /// 序列化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Serialize<T>(MemberMap<T> memberMap, BinarySerializer serializer, T value)
        {
            if (serializer.Stream.PrepSize(fixedSize)) serialize(memberMap, serializer, value.castObject());
        }
        /// <summary>
        /// 序列化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private void serialize<T>(MemberMap<T> memberMap, BinarySerializer serializer, object value)
        {
            int startIndex = serializer.Stream.Data.Pointer.CurrentIndex;
            foreach (SerializeMemberMapMethod field in fixedFields) field.Serialize(memberMap, serializer, value);
            serializer.Stream.Data.Pointer.SerializeFillWithStartIndex(startIndex);
            foreach (SerializeMemberMapMethod field in fields) field.Serialize(memberMap, serializer, value);
        }
    }
}
