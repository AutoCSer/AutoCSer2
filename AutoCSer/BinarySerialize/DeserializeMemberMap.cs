using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 泛型类型成员反序列化（AOT 反射模式）
    /// </summary>
    internal sealed class DeserializeMemberMap
    {
        /// <summary>
        /// 反序列化方法集合
        /// </summary>
        private readonly DeserializeMemberMapMethod[] fixedFields;
        /// <summary>
        /// 反序列化方法集合
        /// </summary>
        private readonly DeserializeMemberMapMethod[] fields;
        /// <summary>
        /// 泛型类型成员反序列化
        /// </summary>
        /// <param name="fieldSizeArray"></param>
        internal DeserializeMemberMap(ref FieldSizeArray fieldSizeArray)
        {
            fixedFields = fieldSizeArray.FixedFields.Length != 0 ? new DeserializeMemberMapMethod[fieldSizeArray.FixedFields.Length] : EmptyArray<DeserializeMemberMapMethod>.Array;
            fields = fieldSizeArray.FieldArray.Length != 0 ? new DeserializeMemberMapMethod[fieldSizeArray.FieldArray.Length] : EmptyArray<DeserializeMemberMapMethod>.Array;
            int index = 0;
            foreach (FieldSize field in fieldSizeArray.FixedFields) fixedFields[index++].Set(field.FieldIndex);
            index = 0;
            foreach (FieldSize field in fieldSizeArray.FieldArray) fields[index++].Set(field.FieldIndex);
        }
        /// <summary>
        /// 反序列化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        internal unsafe void Deserialize<T>(MemberMap<T> memberMap, BinaryDeserializer deserializer, ref T value)
        {
            object objectValue = value.castObject();
            byte* start = deserializer.Current;
            foreach (DeserializeMemberMapMethod field in fixedFields) field.Deserialize(memberMap, deserializer, objectValue);
            deserializer.Current += (int)(start - deserializer.Current) & 3;
            foreach (DeserializeMemberMapMethod field in fields) field.Deserialize(memberMap, deserializer, objectValue);
            value = (T)objectValue;
        }
    }
}
