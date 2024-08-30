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
        internal static bool EnumLong<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
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
        internal static bool EnumUInt<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
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
        internal static bool EnumInt<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
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
        internal static bool EnumUShort<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
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
        internal static bool EnumShort<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
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
        internal static bool EnumByte<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
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
        internal static bool EnumSByte<T>(T left, T right) where T : struct, IConvertible
        {
            if (AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(left) == AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(right)) return true;
            Breakpoint(left, right);
            return false;
        }
    }
}

#endif