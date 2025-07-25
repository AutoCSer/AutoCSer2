﻿using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// Enumeration value serialization (for AOT code generation, not allowed for developers to call)
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
            if (!Config.IsEnumToString) XmlSerialize(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// Serialization of enumeration values
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
        /// Enumeration value serialization method information
        /// 枚举值序列化方法信息
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod;
#endif
    }
}
