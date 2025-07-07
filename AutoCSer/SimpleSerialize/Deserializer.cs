using AutoCSer.BinarySerialize;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">Logical value</param>
        /// <returns></returns>
        public static byte* Deserialize(byte* data, ref bool? value)
        {
            switch(*data)
            {
                case 0: value = null; return data + 1;
                case 1: value = false; return data + 1;
                case 2: value = true; return data + 1;
            }
            return null;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static byte* Deserialize(byte* data, ref byte? value)
        {
            if (*(data + sizeof(byte)) == 0)
            {
                value = *(byte*)data;
                return data + sizeof(ushort);
            }
            if (*(short*)data == short.MinValue)
            {
                value = null;
                return data + sizeof(ushort);
            }
            return null;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static byte* Deserialize(byte* data, ref sbyte? value)
        {
            if (*(data + sizeof(byte)) == 0)
            {
                value = *(sbyte*)data;
                return data + sizeof(ushort);
            }
            if (*(short*)data == short.MinValue)
            {
                value = null;
                return data + sizeof(ushort);
            }
            return null;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static byte* Deserialize(byte* data, ref short? value)
        {
            if (*(ushort*)(data + sizeof(ushort)) == 0)
            {
                value = *(short*)data;
                return data + sizeof(int);
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static byte* Deserialize(byte* data, ref ushort? value)
        {
            if (*(ushort*)(data + sizeof(ushort)) == 0)
            {
                value = *(ushort*)data;
                return data + sizeof(int);
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
        /// <summary>
        /// 数值反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static byte* Deserialize(byte* data, ref char? value)
        {
            if (*(ushort*)(data + sizeof(char)) == 0)
            {
                value = *(char*)data;
                return data + sizeof(int);
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <param name="end"></param>
        /// <returns></returns>
#if NetStandard21
        public static byte* Deserialize(byte* start, ref string? value, byte* end)
#else
        public static byte* Deserialize(byte* start, ref string value, byte* end)
#endif
        {
            if (start != null)
            {
                if (*(int*)start == BinarySerializer.NullValue)
                {
                    value = null;
                    return start + sizeof(int);
                }
                string stringValue = string.Empty;
                end = NotNull(start, ref stringValue, end);
                if (end != null)
                {
                    value = stringValue;
                    return end;
                }
            }
            return null;
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal static byte* NotNull(byte* start, ref string value, byte* end)
        {
            int length = *(int*)start;
            if ((length & 1) == 0)
            {
                if (length > 0)
                {
                    int dataLength = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                    if (dataLength <= (int)(end - start))
                    {
                        value = new string((char*)(start + sizeof(int)), 0, length >> 1);
                        return start + dataLength;
                    }
                }
                else if (length == 0)
                {
                    value = string.Empty;
                    return start + sizeof(int);
                }
            }
            else if ((length >>= 1) > 0)
            {
                int lengthSize = AutoCSer.Memory.UnmanagedStreamBase.GetSerializeStringLengthSize(length);
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(end - start))
                {
                    value = AutoCSer.Common.AllocateString(length);
                    fixed (char* valueFixed = value) return AutoCSer.BinaryDeserializer.Deserialize(start, end, valueFixed, length, lengthSize);
                }
            }
            return null;
        }
        /// <summary>
        /// 字节数组反序列化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <param name="end"></param>
        /// <returns></returns>
#if NetStandard21
        public static byte* Deserialize(byte* start, ref byte[]? value, byte* end)
#else
        public static byte* Deserialize(byte* start, ref byte[] value, byte* end)
#endif
        {
            if (start != null)
            {
                int length = *(int*)start;
                if (length > 0)
                {
                    int size = (length + (3 + sizeof(int))) & (int.MaxValue - 3);
                    if (size <= end - start)
                    {
                        value = AutoCSer.Common.GetUninitializedArray<byte>(length);
                        AutoCSer.Common.CopyTo(start + sizeof(int), value);
                        return start + size;
                    }
                }
                else
                {
                    if (length == 0)
                    {
                        value = EmptyArray<byte>.Array;
                        return start + sizeof(int);
                    }
                    if (length == BinarySerializer.NullValue)
                    {
                        value = null;
                        return start + sizeof(int);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">Target data</param>
        internal delegate byte* DeserializeDelegate<T>(byte* data, ref T value);
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">Target data</param>
        /// <param name="end"></param>
        internal delegate byte* DeserializeDefaultDelegate<T>(byte* data, ref T value, byte* end);
        /// <summary>
        /// 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static byte* Deserialize<T>(byte* data, ref T value) { return null; }
#if !AOT
        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, Delegate> deserializeDelegates;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Delegate? GetDeserializeDelegate(Type type)
#else
        internal static Delegate GetDeserializeDelegate(Type type)
#endif
        {
            var deserializeDelegate = default(Delegate);
            return deserializeDelegates.TryGetValue(type, out deserializeDelegate) ? deserializeDelegate : null;
        }
        static Deserializer()
        {
            deserializeDelegates = DictionaryCreator.CreateHashObject<System.Type, Delegate>();
            deserializeDelegates.Add(typeof(bool), (DeserializeDelegate<bool>)Deserialize);
            deserializeDelegates.Add(typeof(bool?), (DeserializeDelegate<bool?>)Deserialize);
            deserializeDelegates.Add(typeof(byte), (DeserializeDelegate<byte>)Deserialize);
            deserializeDelegates.Add(typeof(byte?), (DeserializeDelegate<byte?>)Deserialize);
            deserializeDelegates.Add(typeof(sbyte), (DeserializeDelegate<sbyte>)Deserialize);
            deserializeDelegates.Add(typeof(sbyte?), (DeserializeDelegate<sbyte?>)Deserialize);
            deserializeDelegates.Add(typeof(short), (DeserializeDelegate<short>)Deserialize);
            deserializeDelegates.Add(typeof(short?), (DeserializeDelegate<short?>)Deserialize);
            deserializeDelegates.Add(typeof(ushort), (DeserializeDelegate<ushort>)Deserialize);
            deserializeDelegates.Add(typeof(ushort?), (DeserializeDelegate<ushort?>)Deserialize);
            deserializeDelegates.Add(typeof(int), (DeserializeDelegate<int>)Deserialize);
            deserializeDelegates.Add(typeof(int?), (DeserializeDelegate<int?>)Deserialize);
            deserializeDelegates.Add(typeof(uint), (DeserializeDelegate<uint>)Deserialize);
            deserializeDelegates.Add(typeof(uint?), (DeserializeDelegate<uint?>)Deserialize);
            deserializeDelegates.Add(typeof(long), (DeserializeDelegate<long>)Deserialize);
            deserializeDelegates.Add(typeof(long?), (DeserializeDelegate<long?>)Deserialize);
            deserializeDelegates.Add(typeof(ulong), (DeserializeDelegate<ulong>)Deserialize);
            deserializeDelegates.Add(typeof(ulong?), (DeserializeDelegate<ulong?>)Deserialize);
            deserializeDelegates.Add(typeof(float), (DeserializeDelegate<float>)Deserialize);
            deserializeDelegates.Add(typeof(float?), (DeserializeDelegate<float?>)Deserialize);
            deserializeDelegates.Add(typeof(double), (DeserializeDelegate<double>)Deserialize);
            deserializeDelegates.Add(typeof(double?), (DeserializeDelegate<double?>)Deserialize);
            deserializeDelegates.Add(typeof(decimal), (DeserializeDelegate<decimal>)Deserialize);
            deserializeDelegates.Add(typeof(decimal?), (DeserializeDelegate<decimal?>)Deserialize);
            deserializeDelegates.Add(typeof(char), (DeserializeDelegate<char>)Deserialize);
            deserializeDelegates.Add(typeof(char?), (DeserializeDelegate<char?>)Deserialize);
            deserializeDelegates.Add(typeof(DateTime), (DeserializeDelegate<DateTime>)Deserialize);
            deserializeDelegates.Add(typeof(DateTime?), (DeserializeDelegate<DateTime?>)Deserialize);
            deserializeDelegates.Add(typeof(TimeSpan), (DeserializeDelegate<TimeSpan>)Deserialize);
            deserializeDelegates.Add(typeof(TimeSpan?), (DeserializeDelegate<TimeSpan?>)Deserialize);
            deserializeDelegates.Add(typeof(Guid), (DeserializeDelegate<Guid>)Deserialize);
            deserializeDelegates.Add(typeof(Guid?), (DeserializeDelegate<Guid?>)Deserialize);
#if NetStandard21
            deserializeDelegates.Add(typeof(string), (DeserializeDefaultDelegate<string?>)Deserialize);
            deserializeDelegates.Add(typeof(byte[]), (DeserializeDefaultDelegate<byte[]?>)Deserialize);
#else
            deserializeDelegates.Add(typeof(string), (DeserializeDefaultDelegate<string>)Deserialize);
            deserializeDelegates.Add(typeof(byte[]), (DeserializeDefaultDelegate<byte[]>)Deserialize);
#endif
            deserializeDelegates.Add(typeof(Half), (DeserializeDelegate<Half>)Deserialize);
            deserializeDelegates.Add(typeof(Int128), (DeserializeDelegate<Int128>)Deserialize);
            deserializeDelegates.Add(typeof(UInt128), (DeserializeDelegate<UInt128>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Complex), (DeserializeDelegate<System.Numerics.Complex>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Plane), (DeserializeDelegate<System.Numerics.Plane>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Quaternion), (DeserializeDelegate<System.Numerics.Quaternion>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Matrix3x2), (DeserializeDelegate<System.Numerics.Matrix3x2>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Matrix4x4), (DeserializeDelegate<System.Numerics.Matrix4x4>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Vector2), (DeserializeDelegate<System.Numerics.Vector2>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Vector3), (DeserializeDelegate<System.Numerics.Vector3>)Deserialize);
            deserializeDelegates.Add(typeof(System.Numerics.Vector4), (DeserializeDelegate<System.Numerics.Vector4>)Deserialize);
        }
#endif
    }
    /// <summary>
    /// 简单反序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    internal unsafe static partial class Deserializer<T> where T : struct
    {
        /// <summary>
        /// 简单反序列化委托
        /// </summary>
        internal static readonly Deserializer.DeserializeDefaultDelegate<T> DefaultDeserializer;

        static Deserializer()
        {
#if AOT
            var method = typeof(T).GetMethod(AutoCSer.CodeGenerator.SimpleSerializeAttribute.SimpleDeserializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(byte*), typeof(T).MakeByRefType(), typeof(byte*) });
            if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(byte*))
            {
                DefaultDeserializer = (Deserializer.DeserializeDefaultDelegate<T>)method.CreateDelegate(typeof(Deserializer.DeserializeDefaultDelegate<T>));
                return;
            }
            throw new MissingMethodException(typeof(T).fullName(), AutoCSer.CodeGenerator.SimpleSerializeAttribute.SimpleDeserializeMethodName);
#else
            int memberCountVerify;
            FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetFields(typeof(T), MemberFiltersEnum.InstanceField), false, out memberCountVerify);
            DeserializeDynamicMethod dynamicMethod = new DeserializeDynamicMethod(typeof(T));
            foreach (FieldSize member in fields.FixedFields) dynamicMethod.Push(member);
            dynamicMethod.FixedFill(-fields.FixedSize & 3);
            foreach (FieldSize member in fields.FieldArray) dynamicMethod.Push(member);
            DefaultDeserializer = (Deserializer.DeserializeDefaultDelegate<T>)dynamicMethod.Create(typeof(Deserializer.DeserializeDefaultDelegate<T>));
#endif
        }
    }
}
