﻿using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 对象对比
    /// </summary>
    public static partial class Comparor
    {
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool EnumULong<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumULongMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumULong), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}
