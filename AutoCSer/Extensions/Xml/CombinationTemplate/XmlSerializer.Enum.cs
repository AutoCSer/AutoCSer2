using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化（用于代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void EnumULong<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) XmlSerialize(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULong<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.EnumULong(value);
        }
#if AOT
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod;
#endif
    }
}
