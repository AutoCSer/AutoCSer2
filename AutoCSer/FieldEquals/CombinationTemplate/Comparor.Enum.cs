using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 对象对比
    /// </summary>
    internal static partial class Comparor
    {
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool EnumULong<T>(T left, T right) where T : struct, IConvertible
        {
            return check(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(right));
        }
    }
}
