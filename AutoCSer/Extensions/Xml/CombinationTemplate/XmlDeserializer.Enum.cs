using System;
/*ulong,ULong,Unsigned;long,Long,Signed;uint,UInt,Unsigned;int,Int,Signed;ushort,UShort,Unsigned;short,Short,Signed;byte,Byte,Unsigned;sbyte,SByte,Signed*/

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed partial class XmlDeserializer
    {
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryDeserializeEnumULong<T>(ref T value) where T : struct, IConvertible
        {
            if (IsEnumNumberUnsigned())
            {
                ulong intValue = 0;
                DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
            }
            else if (State == AutoCSer.Xml.DeserializeStateEnum.Success) return false;
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
            AutoCSer.Xml.EnumULongDeserialize<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void EnumFlagsULong<T>(ref T value) where T : struct, IConvertible
        {
            AutoCSer.Xml.EnumULongDeserialize<T>.DeserializeFlags(this, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULong<T>(XmlDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Xml.EnumULongDeserialize<T>.Deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumFlagsULong<T>(XmlDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            AutoCSer.Xml.EnumULongDeserialize<T>.DeserializeFlags(deserializer, ref value);
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
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumFlagsULong<T>(XmlDeserializer deserializer, ref T value) { }
#endif
    }
}
namespace AutoCSer.Xml
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
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!deserializer.TryDeserializeEnumULong(ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!deserializer.TryDeserializeEnumULong(ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ulong intValue = AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(enumValues[index]);
                        intValue |= AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(enumValues[nextIndex]);
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(enumValues[index]);
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
                    }
                }
            }
        }
    }
}
