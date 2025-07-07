using AutoCSer.BinarySerialize;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        private static void serialize(UnmanagedStream stream, byte[] value)
        {
            if (value != null)
            {
                if (value.Length == 0) stream.Write(0);
                else
                {
                    fixed (byte* valueFixed = value) stream.Serialize(valueFixed, value.Length, value.Length);
                }
            }
            else stream.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        private static void serialize(UnmanagedStream stream, string value)
        {
            if (value != null)
            {
                if (value.Length == 0) stream.Write(0);
                else
                {
                    fixed (char* valueFixed = value) stream.Serialize(valueFixed, value.Length);
                }
            }
            else stream.Write(BinarySerializer.NullValue);
        }
#if AOT
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, bool? value)
        {
            stream.Write(value.HasValue ? (value.Value ? (byte)2 : (byte)1) : (byte)0);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, byte? value)
        {
            if (value.HasValue) stream.Write((ushort)value.Value);
            else stream.Write(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, sbyte? value)
        {
            if (value.HasValue) stream.Write((ushort)(byte)value.Value);
            else stream.Write(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, short? value)
        {
            if (value.HasValue) stream.Write((uint)(ushort)value.Value);
            else stream.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, ushort? value)
        {
            if (value.HasValue) stream.Write((uint)value.Value);
            else stream.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">字符</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, char? value)
        {
            if (value.HasValue) stream.Write((uint)value.Value);
            else stream.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Guid</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, Guid value)
        {
            stream.Write(ref value);
        }
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, string value)
        {
            serialize(stream, value);
        }
        /// <summary>
        /// 字节数组序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(UnmanagedStream stream, byte[] value)
        {
            serialize(stream, value);
        }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly HashSet<HashObject<System.Type>> serializeDelegates;
        /// <summary>
        /// 判断是否可序列化类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsType(Type type)
        {
            return type == typeof(void) || type.IsEnum || serializeDelegates.Contains(type);
        }
        static Serializer()
        {
            serializeDelegates = HashSetCreator.CreateHashObject<Type>();
            serializeDelegates.Add(typeof(bool));
            serializeDelegates.Add(typeof(bool?));
            serializeDelegates.Add(typeof(byte));
            serializeDelegates.Add(typeof(byte?));
            serializeDelegates.Add(typeof(sbyte));
            serializeDelegates.Add(typeof(sbyte?));
            serializeDelegates.Add(typeof(short));
            serializeDelegates.Add(typeof(short?));
            serializeDelegates.Add(typeof(ushort));
            serializeDelegates.Add(typeof(ushort?));
            serializeDelegates.Add(typeof(int));
            serializeDelegates.Add(typeof(int?));
            serializeDelegates.Add(typeof(uint));
            serializeDelegates.Add(typeof(uint?));
            serializeDelegates.Add(typeof(long));
            serializeDelegates.Add(typeof(long?));
            serializeDelegates.Add(typeof(ulong));
            serializeDelegates.Add(typeof(ulong?));
            serializeDelegates.Add(typeof(float));
            serializeDelegates.Add(typeof(float?));
            serializeDelegates.Add(typeof(double));
            serializeDelegates.Add(typeof(double?));
            serializeDelegates.Add(typeof(decimal));
            serializeDelegates.Add(typeof(decimal?));
            serializeDelegates.Add(typeof(char));
            serializeDelegates.Add(typeof(char?));
            serializeDelegates.Add(typeof(DateTime));
            serializeDelegates.Add(typeof(DateTime?));
            serializeDelegates.Add(typeof(TimeSpan));
            serializeDelegates.Add(typeof(TimeSpan?));
            serializeDelegates.Add(typeof(Guid));
            serializeDelegates.Add(typeof(Guid?));
            serializeDelegates.Add(typeof(string));
            serializeDelegates.Add(typeof(byte[]));
            
            serializeDelegates.Add(typeof(Half));
            serializeDelegates.Add(typeof(Int128));
            serializeDelegates.Add(typeof(UInt128));
            serializeDelegates.Add(typeof(System.Numerics.Complex));
            serializeDelegates.Add(typeof(System.Numerics.Plane));
            serializeDelegates.Add(typeof(System.Numerics.Quaternion));
            serializeDelegates.Add(typeof(System.Numerics.Matrix3x2));
            serializeDelegates.Add(typeof(System.Numerics.Matrix4x4));
            serializeDelegates.Add(typeof(System.Numerics.Vector2));
            serializeDelegates.Add(typeof(System.Numerics.Vector3));
            serializeDelegates.Add(typeof(System.Numerics.Vector4));
        }
#else
        /// <summary>
        /// 逻辑值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, bool? value)
        {
            stream.Data.Pointer.Write(value.HasValue ? (value.Value ? (byte)2 : (byte)1) : (byte)0);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, byte? value)
        {
            if (value.HasValue) stream.Data.Pointer.Write((ushort)value.Value);
            else stream.Data.Pointer.Write(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, sbyte? value)
        {
            if (value.HasValue) stream.Data.Pointer.Write((ushort)(byte)value.Value);
            else stream.Data.Pointer.Write(short.MinValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, short? value)
        {
            if (value.HasValue) stream.Data.Pointer.Write((uint)(ushort)value.Value);
            else stream.Data.Pointer.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 数值序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, ushort? value)
        {
            if (value.HasValue) stream.Data.Pointer.Write((uint)value.Value);
            else stream.Data.Pointer.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 字符序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">字符</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, char? value)
        {
            if (value.HasValue) stream.Data.Pointer.Write((uint)value.Value);
            else stream.Data.Pointer.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// Guid序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Guid</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(UnmanagedStream stream, Guid value)
        {
            stream.Data.Pointer.Write(ref value);
        }
        /// <summary>
        /// 序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        internal static void Serialize<T>(UnmanagedStream stream, T value) { }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, Delegate> serializeDelegates;
        /// <summary>
        /// 获取基本类型转换函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>转换函数</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Delegate? GetSerializeDelegate(Type type)
#else
        internal static Delegate GetSerializeDelegate(Type type)
#endif
        {
            var serializeDelegate = default(Delegate);
            return serializeDelegates.TryGetValue(type, out serializeDelegate) ? serializeDelegate : null;
        }
        /// <summary>
        /// 判断是否可序列化类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsType(Type type)
        {
            return type == typeof(void) || type.IsEnum || serializeDelegates.ContainsKey(type);
        }
        static Serializer()
        {
            serializeDelegates = DictionaryCreator.CreateHashObject<Type, Delegate>();
            serializeDelegates.Add(typeof(bool), (Action<UnmanagedStream, bool>)serialize);
            serializeDelegates.Add(typeof(bool?), (Action<UnmanagedStream, bool?>)serialize);
            serializeDelegates.Add(typeof(byte), (Action<UnmanagedStream, byte>)serialize);
            serializeDelegates.Add(typeof(byte?), (Action<UnmanagedStream, byte?>)serialize);
            serializeDelegates.Add(typeof(sbyte), (Action<UnmanagedStream, sbyte>)serialize);
            serializeDelegates.Add(typeof(sbyte?), (Action<UnmanagedStream, sbyte?>)serialize);
            serializeDelegates.Add(typeof(short), (Action<UnmanagedStream, short>)serialize);
            serializeDelegates.Add(typeof(short?), (Action<UnmanagedStream, short?>)serialize);
            serializeDelegates.Add(typeof(ushort), (Action<UnmanagedStream, ushort>)serialize);
            serializeDelegates.Add(typeof(ushort?), (Action<UnmanagedStream, ushort?>)serialize);
            serializeDelegates.Add(typeof(int), (Action<UnmanagedStream, int>)serialize);
            serializeDelegates.Add(typeof(int?), (Action<UnmanagedStream, int?>)serialize);
            serializeDelegates.Add(typeof(uint), (Action<UnmanagedStream, uint>)serialize);
            serializeDelegates.Add(typeof(uint?), (Action<UnmanagedStream, uint?>)serialize);
            serializeDelegates.Add(typeof(long), (Action<UnmanagedStream, long>)serialize);
            serializeDelegates.Add(typeof(long?), (Action<UnmanagedStream, long?>)serialize);
            serializeDelegates.Add(typeof(ulong), (Action<UnmanagedStream, ulong>)serialize);
            serializeDelegates.Add(typeof(ulong?), (Action<UnmanagedStream, ulong?>)serialize);
            serializeDelegates.Add(typeof(float), (Action<UnmanagedStream, float>)serialize);
            serializeDelegates.Add(typeof(float?), (Action<UnmanagedStream, float?>)serialize);
            serializeDelegates.Add(typeof(double), (Action<UnmanagedStream, double>)serialize);
            serializeDelegates.Add(typeof(double?), (Action<UnmanagedStream, double?>)serialize);
            serializeDelegates.Add(typeof(decimal), (Action<UnmanagedStream, decimal>)serialize);
            serializeDelegates.Add(typeof(decimal?), (Action<UnmanagedStream, decimal?>)serialize);
            serializeDelegates.Add(typeof(char), (Action<UnmanagedStream, char>)serialize);
            serializeDelegates.Add(typeof(char?), (Action<UnmanagedStream, char?>)serialize);
            serializeDelegates.Add(typeof(DateTime), (Action<UnmanagedStream, DateTime>)serialize);
            serializeDelegates.Add(typeof(DateTime?), (Action<UnmanagedStream, DateTime?>)serialize);
            serializeDelegates.Add(typeof(TimeSpan), (Action<UnmanagedStream, TimeSpan>)serialize);
            serializeDelegates.Add(typeof(TimeSpan?), (Action<UnmanagedStream, TimeSpan?>)serialize);
            serializeDelegates.Add(typeof(Guid), (Action<UnmanagedStream, Guid>)serialize);
            serializeDelegates.Add(typeof(Guid?), (Action<UnmanagedStream, Guid?>)serialize);
            serializeDelegates.Add(typeof(string), (Action<UnmanagedStream, string>)serialize);
            serializeDelegates.Add(typeof(byte[]), (Action<UnmanagedStream, byte[]>)serialize);

            serializeDelegates.Add(typeof(Half), (Action<UnmanagedStream, Half>)serialize);
            serializeDelegates.Add(typeof(Int128), (Action<UnmanagedStream, Int128>)serialize);
            serializeDelegates.Add(typeof(UInt128), (Action<UnmanagedStream, UInt128>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Complex), (Action<UnmanagedStream, System.Numerics.Complex>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Plane), (Action<UnmanagedStream, System.Numerics.Plane>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Quaternion), (Action<UnmanagedStream, System.Numerics.Quaternion>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Matrix3x2), (Action<UnmanagedStream, System.Numerics.Matrix3x2>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Matrix4x4), (Action<UnmanagedStream, System.Numerics.Matrix4x4>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Vector2), (Action<UnmanagedStream, System.Numerics.Vector2>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Vector3), (Action<UnmanagedStream, System.Numerics.Vector3>)serialize);
            serializeDelegates.Add(typeof(System.Numerics.Vector4), (Action<UnmanagedStream, System.Numerics.Vector4>)serialize);
        }
#endif
    }
    /// <summary>
    /// 简单序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal partial class Serializer<T> where T : struct
    {
        /// <summary>
        /// 简单序列化委托
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">Target data</param>
        internal delegate void SimpleSerializer(UnmanagedStream stream, ref T value);
        /// <summary>
        /// 成员序列化
        /// </summary>
        internal static readonly SimpleSerializer DefaultSerializer;
        static Serializer()
        {
#if AOT
            var method = typeof(T).GetMethod(AutoCSer.CodeGenerator.SimpleSerializeAttribute.SimpleSerializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(UnmanagedStream), typeof(T).MakeByRefType() });
            if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
            {
                DefaultSerializer = (SimpleSerializer)method.CreateDelegate(typeof(SimpleSerializer));
                return;
            }
            throw new MissingMethodException(typeof(T).fullName(), AutoCSer.CodeGenerator.SimpleSerializeAttribute.SimpleSerializeMethodName);
#else
            int memberCountVerify;
            FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetFields(typeof(T), MemberFiltersEnum.InstanceField), false, out memberCountVerify);
            SerializeDynamicMethod dynamicMethod = new SerializeDynamicMethod(typeof(T), (fields.FixedSize + fields.FieldArray.Length * sizeof(int) + 3) & (int.MaxValue - 3));
            foreach (FieldSize member in fields.FixedFields) dynamicMethod.Push(member);
            dynamicMethod.FixedFill(-fields.FixedSize & 3);
            foreach (FieldSize member in fields.FieldArray) dynamicMethod.Push(member);
            DefaultSerializer = (SimpleSerializer)dynamicMethod.Create(typeof(SimpleSerializer));
#endif
        }
    }
}
