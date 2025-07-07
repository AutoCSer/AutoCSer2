using System;
/*ulong,ULong,GetSize,FillSize;long,Long,GetSize,FillSize;uint,UInt,GetSize,FillSize;int,Int,GetSize,FillSize;ushort,UShort,GetSize4,FillSize2;short,Short,GetSize4,FillSize2;byte,Byte,GetSize4,FillSize4;sbyte,SByte,GetSize4,FillSize4*/

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Enumeration value array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumULong<T>(ListArray<T> array) where T : struct, IConvertible
        {
            enumULongArray(array);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Enumeration value array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumULong<T>(LeftArray<T> array) where T : struct, IConvertible
        {
            enumULongArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Enumeration value array</param>
        public static void EnumULongArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.EnumULong((T[]?)array);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Enumeration value array</param>
        public static void EnumULongListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct, IConvertible
        {
            binarySerializer.enumULongArray((ListArray<T>?)array);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value">Enumeration value array</param>
        public static void EnumULongLeftArrayReflection<T>(BinarySerializer binarySerializer, object value) where T : struct, IConvertible
        {
            LeftArray<T> array = (LeftArray<T>)value;
            binarySerializer.enumULongArrayOnly(array.Array, array.Length);
        }
        /// <summary>
        /// Enumeration serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod;
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongArrayMethod;
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongListArrayMethod;
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongLeftArrayMethod;
        /// <summary>
        /// Enumeration serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo PrimitiveMemberULongReflectionMethod;
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongArrayReflectionMethod;
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongListArrayReflectionMethod;
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongLeftArrayReflectionMethod;
#endif
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Enumeration value array</param>
        /// <param name="count"></param>
        private unsafe void enumULongArrayOnly<T>(T[] array, int count) where T : struct, IConvertible
        {
            if (count == 0) Stream.Write(0);
            else
            {
                int size = count * sizeof(ulong);
                byte* write = Stream.GetBeforeMove(GetSize(size + sizeof(int)));
                if (write != null)
                {
                    *(int*)write = count;
#if NET8
#pragma warning disable CS8500
                    fixed (T* arrayFixed = array) AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), size);
#pragma warning restore CS8500
                    FillSize(write + (size + sizeof(int)), count);
#else
                    byte* end = (write += sizeof(int)) + size;
                    foreach (T value in array)
                    {
                        *(ulong*)write = AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value);
                        if ((write += sizeof(ulong)) == end) break;
                    }
                    FillSize(end, count);
#endif
                }
            }
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Enumeration value array</param>
#if NetStandard21
        public void EnumULong<T>(T[]? array) where T : struct, IConvertible
#else
        public void EnumULong<T>(T[] array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumULongArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumULongArrayOnly(array, array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumULongArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Enumeration value array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULongArray<T>(BinarySerializer binarySerializer, T[] array) where T : struct, IConvertible
        {
            binarySerializer.EnumULong(array);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Enumeration value array</param>
#if NetStandard21
        private void enumULongArray<T>(ListArray<T>? array) where T : struct, IConvertible
#else
        private void enumULongArray<T>(ListArray<T> array) where T : struct, IConvertible
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        enumULongArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.NotReferenceCount:
                        enumULongArrayOnly(array.Array.Array, array.Array.Length);
                        ClearNotReferenceCount();
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) enumULongArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Enumeration value array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULongListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumULongArray(array);
        }
        /// <summary>
        /// Serialization of enumeration arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Enumeration value array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULongLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : struct, IConvertible
        {
            binarySerializer.enumULongArrayOnly(array.Array, array.Length);
        }
    }
}
