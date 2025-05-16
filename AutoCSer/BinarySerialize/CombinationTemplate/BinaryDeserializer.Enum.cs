using System;
/*ulong,ULong,GetSize;long,Long,GetSize;uint,UInt,GetSize;int,Int,GetSize;ushort,UShort,GetSize4;short,Short,GetSize4;byte,Byte,GetSize4;sbyte,SByte,GetSize4*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ulong FixedEnumULong()
        {
            ulong value = *(ulong*)Current;
            Current += sizeof(ulong);
            return value;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object PrimitiveMemberULongReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            T value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(deserializer.Current);
            deserializer.Current += sizeof(ulong);
            return value;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumULongArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(T[]);
            deserializer.EnumULong(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? EnumULongListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(ListArray<T>);
            deserializer.EnumULong(ref array);
            return array;
        }
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object EnumULongLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct, IConvertible
        {
            var array = default(LeftArray<T>);
            deserializer.EnumULong(ref array);
            return array;
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongListArrayMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongLeftArrayMethod;
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberULongReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongListArrayReflectionMethod;
        /// <summary>
        /// 枚举数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumULongMember<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(*(ulong*)deserializer.Current);
#endif
            deserializer.Current += sizeof(ulong);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumULong<T>(ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumULong<T>(ref T[]? array) where T : struct, IConvertible
#else
        internal void EnumULong<T>(ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(ulong), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        for (int index = 0; index != length; read += sizeof(ulong)) array[index++] = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(*(ulong*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumULongArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumULongArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct, IConvertible
#else
        internal static void EnumULongArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumULong(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumULong<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal void EnumULong<T>(ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal void EnumULong<T>(ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long copySize = (long)length * sizeof(ulong), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array.Array;
                        for (int index = 0; index != length; read += sizeof(ulong)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(*(ulong*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumULongListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
#if NetStandard21
        internal static void EnumULongListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct, IConvertible
#else
        internal static void EnumULongListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct, IConvertible
#endif
#endif
        {
            deserializer.EnumULong(ref array);
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">枚举数组反序列化</param>
#if AOT
        public void EnumULong<T>(ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal void EnumULong<T>(ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long copySize = (long)length * sizeof(ulong), dataLength = AutoCSer.BinarySerializer.GetSize(copySize + (sizeof(int)));
                if (dataLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
#if NET8
#pragma warning disable CS8500
                        fixed (T* arrayFixed = array.Array) AutoCSer.Common.CopyTo(Current + sizeof(int), arrayFixed, copySize);
#pragma warning restore CS8500
#else
                        byte* read = Current + sizeof(int);
                        T[] valueArray = array.Array;
                        for (int index = 0; index != length; read += sizeof(ulong)) valueArray[index++] = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(*(ulong*)read);
#endif
                        Current += dataLength;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// 枚举数组反序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">枚举数组反序列化</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void EnumULongLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#else
        internal static void EnumULongLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct, IConvertible
#endif
        {
            deserializer.EnumULong(ref array);
        }
    }
}
