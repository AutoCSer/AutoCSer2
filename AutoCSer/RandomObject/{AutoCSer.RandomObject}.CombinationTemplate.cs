//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

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
        internal static T EnumLong<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(CreateLong(config)) : customCreator(config, false);

        }
    }
}

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
        internal static T EnumUInt<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(CreateUInt(config)) : customCreator(config, false);

        }
    }
}

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
        internal static T EnumInt<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(CreateInt(config)) : customCreator(config, false);

        }
    }
}

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
        internal static T EnumUShort<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(CreateUShort(config)) : customCreator(config, false);

        }
    }
}

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
        internal static T EnumShort<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(CreateShort(config)) : customCreator(config, false);

        }
    }
}

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
        internal static T EnumByte<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(CreateByte(config)) : customCreator(config, false);

        }
    }
}

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
        internal static T EnumSByte<T>(Config config) where T : struct, IConvertible
        {
            Func<Config, bool, T> customCreator = (Func<Config, bool, T>)config.GetCustomCreator(typeof(T));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(CreateSByte(config)) : customCreator(config, false);

        }
    }
}

#endif