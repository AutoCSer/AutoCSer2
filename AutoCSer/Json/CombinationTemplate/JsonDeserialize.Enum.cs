using System;
/*ulong,ULong,Unsigned;long,Long,Signed;uint,UInt,Unsigned;int,Int,Signed;ushort,UShort,Unsigned;short,Short,Signed;byte,Byte,Unsigned;sbyte,SByte,Signed*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 枚举数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumULong<T>(ref T value) where T : struct, IConvertible
        {
            if (IsBinaryMix || IsEnumNumberUnsigned())
            {
                ulong intValue = 0;
                JsonDeserialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
            }
            else if (State == AutoCSer.Json.DeserializeStateEnum.Success) return false;
            return true;
        }
#if AOT
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumULong<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumULongDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsULong<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumULongDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumULong<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumULongDeserialize<T>.Deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        public static void EnumFlagsULong<T>(JsonDeserializer jsonDeserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Json.EnumULongDeserialize<T>.DeserializeFlags(jsonDeserializer, ref value);
        }
        /// <summary>
        /// Deserialization of enumeration values
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod;
        /// <summary>
        /// Deserialization of enumeration values
        /// 枚举值反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumFlagsULongMethod;
#else
        /// <summary>
        /// 枚举反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value"></param>
        internal static void EnumFlagsULong<T>(JsonDeserializer jsonDeserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumULongDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumULong(ref value)) deserialize(jsonDeserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        public static void DeserializeFlags(JsonDeserializer jsonDeserializer, ref T value)
        {
            if (!jsonDeserializer.TryDeserializeEnumULong(ref value))
            {
                if (enumSearcher.State == null) jsonDeserializer.CheckMatchEnumIgnore();
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ulong intValue = AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(enumValues[nextIndex]);
                        while (jsonDeserializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeserializer);
                            if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (index != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(enumValues[index]);
                            }
                            else return;
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
                    }
                }
            }
        }
    }
}
