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
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumULong<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
            else primitiveSerializeNotEmpty(value.ToString());
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumULong<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumULong(value);
        }
    }
}
