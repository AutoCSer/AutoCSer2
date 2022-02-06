using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机生成枚举数据
    /// </summary>
    public static partial class Creator
    {
        /// <summary>
        /// 创建随机枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static T EnumULong<T>(Config config) where T : struct, IConvertible
        {
            return AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(CreateULong(config));
        }
    }
}
