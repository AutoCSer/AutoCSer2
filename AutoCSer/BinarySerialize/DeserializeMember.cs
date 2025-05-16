using AutoCSer.Extensions;
using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员反序列化（AOT 反射模式）
    /// </summary>
    internal sealed class DeserializeMember
    {
        /// <summary>
        /// 反序列化方法集合
        /// </summary>
        private readonly DeserializeMemberMethod[] fixedFields;
        /// <summary>
        /// 反序列化方法集合
        /// </summary>
        private readonly DeserializeMemberMethod[] fields;
        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        private readonly int fixedFillSize;
        /// <summary>
        /// 泛型类型成员反序列化
        /// </summary>
        /// <param name="fieldSizeArray"></param>
        internal DeserializeMember(ref FieldSizeArray fieldSizeArray)
        {
            fixedFillSize = -fieldSizeArray.FixedSize & 3;
            fixedFields = fieldSizeArray.FixedFields.Length != 0 ? new DeserializeMemberMethod[fieldSizeArray.FixedFields.Length] : EmptyArray<DeserializeMemberMethod>.Array;
            fields = fieldSizeArray.FieldArray.Length != 0 ? new DeserializeMemberMethod[fieldSizeArray.FieldArray.Length] : EmptyArray<DeserializeMemberMethod>.Array;
            int index = 0;
            foreach (FieldSize field in fieldSizeArray.FixedFields) fixedFields[index++].Set(field.Field);
            index = 0;
            foreach (FieldSize field in fieldSizeArray.FieldArray) fields[index++].Set(field.Field);
        }
        /// <summary>
        /// 反序列化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        internal unsafe void Deserialize<T>(BinaryDeserializer deserializer, ref T value)
        {
            object objectValue = value.castObject();
            foreach (DeserializeMemberMethod field in fixedFields) field.Deserialize(deserializer, objectValue);
            deserializer.Current += fixedFillSize;
            foreach (DeserializeMemberMethod field in fields) field.Deserialize(deserializer, objectValue);
            value = (T)objectValue;
        }
    }
}
