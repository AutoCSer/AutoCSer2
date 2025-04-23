//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
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
        public static T EnumLong<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(CreateLong(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumLongMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumLong), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
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
        public static T EnumUInt<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(CreateUInt(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumUIntMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumUInt), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
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
        public static T EnumInt<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(CreateInt(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumIntMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumInt), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
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
        public static T EnumUShort<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(CreateUShort(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumUShortMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumUShort), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
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
        public static T EnumShort<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(CreateShort(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumShortMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumShort), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
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
        public static T EnumByte<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(CreateByte(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumByteMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumByte), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
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
        public static T EnumSByte<T>(Config config) where T : struct, IConvertible
        {
            var customCreator = AutoCSer.Extensions.NullableReferenceExtension.castType<Func<Config, bool, T>>(config.GetCustomCreator(typeof(T)));
            return customCreator == null ? AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(CreateSByte(config)) : customCreator(config, false);
        }
#if AOT
        /// <summary>
        /// 创建可空随机对象
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CreateEnumSByteMethod = AutoCSer.Extensions.NullableReferenceExtension.notNull(typeof(Creator).GetMethod(nameof(EnumSByte), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public));
#endif
    }
}

#endif