using System;
/*ulong,ULong,Unsigned;long,Long,Signed;uint,UInt,Unsigned;int,Int,Signed;ushort,UShort,Unsigned;short,Short,Signed;byte,Byte,Unsigned;sbyte,SByte,Signed*/

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumULongDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberUnsigned())
            {
                ulong intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
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
                        ulong intValue = enumInts.ULong[index];
                        intValue |= enumInts.ULong[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.ULong[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumULongDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(ulong), false);
            ulong* data = enumInts.ULong;
            foreach (T value in enumValues) *(ulong*)data++ = AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value);
        }
    }
}
