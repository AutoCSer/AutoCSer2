using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员序列化（AOT 反射模式）
    /// </summary>
    internal sealed class SerializeMember
    {
        /// <summary>
        /// 序列化方法集合
        /// </summary>
        private readonly SerializeMemberMethod[] fixedFields;
        /// <summary>
        /// 序列化方法集合
        /// </summary>
        private readonly SerializeMemberMethod[] fields;
        /// <summary>
        /// 成员数量
        /// </summary>
        private readonly int memberCountVerify;
        /// <summary>
        /// 固定字节数
        /// </summary>
        private readonly int fixedSize;
        /// <summary>
        /// 固定补全字节数
        /// </summary>
        private readonly int fixedFillSize;
        /// <summary>
        /// 泛型类型成员序列化
        /// </summary>
        /// <param name="fieldSizeArray"></param>
        /// <param name="memberCountVerify"></param>
        internal SerializeMember(ref FieldSizeArray fieldSizeArray, int memberCountVerify)
        {
            this.memberCountVerify = memberCountVerify;
            fixedFillSize = -fieldSizeArray.FixedSize & 3;
            fixedSize = (fieldSizeArray.FixedSize + (sizeof(int) + 3)) & (int.MaxValue - 3);
            fixedFields = fieldSizeArray.FixedFields.Length != 0 ? new SerializeMemberMethod[fieldSizeArray.FixedFields.Length] : EmptyArray<SerializeMemberMethod>.Array;
            fields = fieldSizeArray.FieldArray.Length != 0 ? new SerializeMemberMethod[fieldSizeArray.FieldArray.Length] : EmptyArray<SerializeMemberMethod>.Array;
            int index = 0;
            foreach (FieldSize field in fieldSizeArray.FixedFields) fixedFields[index++].Set(field.Field);
            index = 0;
            foreach (FieldSize field in fieldSizeArray.FieldArray) fields[index++].Set(field.Field);
        }
        /// <summary>
        /// 序列化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        internal void Serialize<T>(BinarySerializer serializer, T value)
        {
            if (serializer.WriteMemberCountVerify(fixedSize, memberCountVerify)) serialize(serializer, value.castObject());
        }
        /// <summary>
        /// 序列化操作
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private void serialize(BinarySerializer serializer, object value)
        {
            foreach (SerializeMemberMethod field in fixedFields) field.Serialize(serializer, value);
            serializer.FixedFillSize(fixedFillSize);
            foreach (SerializeMemberMethod field in fields) field.Serialize(serializer, value);
        }
    }
}
