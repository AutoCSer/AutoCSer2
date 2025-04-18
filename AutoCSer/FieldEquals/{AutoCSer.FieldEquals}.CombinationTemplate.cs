//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

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
        public static bool EnumLong<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumLongMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumLong), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

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
        public static bool EnumUInt<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUIntMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumUInt), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

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
        public static bool EnumInt<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumIntMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumInt), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

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
        public static bool EnumUShort<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumUShortMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumUShort), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

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
        public static bool EnumShort<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumShortMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumShort), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

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
        public static bool EnumByte<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumByteMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumByte), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

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
        public static bool EnumSByte<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
#if AOT
        /// <summary>
        /// 枚举值比较
        /// </summary>
        internal static readonly System.Reflection.MethodInfo EnumSByteMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Comparor).GetMethod(nameof(EnumSByte), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

#endif