using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unmanagedStream">二进制数据序列化</param>
        /// <param name="value">Enumeration value</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumULong<T>(AutoCSer.Memory.UnmanagedStream unmanagedStream, T value) where T : struct, IConvertible
        {
            unmanagedStream.Data.Pointer.Write(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
        }
    }
}
