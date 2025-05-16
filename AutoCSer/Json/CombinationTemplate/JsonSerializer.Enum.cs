using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public void EnumULong<T>(T value) where T : struct, IConvertible
#else
        internal void EnumULong<T>(T value) where T : struct, IConvertible
#endif
        {
            if (IsBinaryMix || !Config.IsEnumToString) JsonSerialize(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULong<T>(JsonSerializer jsonSerializer, T value) where T : struct, IConvertible
        {
            jsonSerializer.EnumULong(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod;
#endif
    }
}
