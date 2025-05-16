using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value">枚举值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static byte* EnumULong<T>(byte* data, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(data);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(*(ulong*)data);
#endif
            return data + sizeof(ulong);
        }
    }
}
