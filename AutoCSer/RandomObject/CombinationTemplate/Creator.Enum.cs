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
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T EnumULong<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(CreateULong(config)) : customCreator(config, false);

        }
    }
}
