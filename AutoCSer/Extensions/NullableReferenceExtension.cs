using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 可空引用类型扩展操作
    /// </summary>
    public static class NullableReferenceExtension
    {
        /// <summary>
        /// object 转换为指定可空引用类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? castClass<T>(this object? value) where T : class
#else
        public static T castClass<T>(this object value) where T : class
#endif
        {
#if NetStandard21
            return (T?)value;
#else
            return (T)value;
#endif
        }
        /// <summary>
        /// object 转换为指定可空类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? castType<T>(this object? value)
#else
        public static T castType<T>(this object value)
#endif
        {
            if (value != null) return (T)value;
            return default(T);
        }
        /// <summary>
        /// object 转换为值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T castValue<T>(this object? value) where T : struct
#else
        public static T castValue<T>(this object value) where T : struct
#endif
        {
            if (value != null) return (T)value;
            return default(T);
        }
        /// <summary>
        /// 非空对象转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T notNullCastType<T>(this object? value) where T : class
#else
        public static T notNullCastType<T>(this object value) where T : class
#endif
        {
#if DEBUG
            if (value == null) throw new ArgumentNullException();
#endif
#if NetStandard21
#pragma warning disable CS8603
            return (T?)value;
#pragma warning restore CS8603
#else
            return (T)value;
#endif
        }
        /// <summary>
        /// 非空对象类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T notNull<T>(this T? value) where T : class
#else
        public static T notNull<T>(this T value) where T : class
#endif
        {
#if DEBUG
            if (value == null) throw new ArgumentNullException();
#endif
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
        /// <summary>
        /// 可空引用类型转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T?[] castArray<T>(this T? value) where T : class
#else
        public static T[] castArray<T>(this T value) where T : class
#endif
        {
#if NetStandard21
            return new T?[] { value };
#else
            return new T[] { value };
#endif
        }
        /// <summary>
        /// 泛型转 object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>

        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static object castObject<T>(this T value)
        {
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
        /// <summary>
        /// 设置数组元素为默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void setDefault<T>(this T[] array, int index)
        {
#pragma warning disable CS8601
            array[index] = default(T);
#pragma warning restore CS8601
        }
        /// <summary>
        /// 设置数组元素为默认值并返回原始值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T getSetDefault<T>(this T[] array, int index)
        {
            T value = array[index];
#pragma warning disable CS8601
            array[index] = default(T);
#pragma warning restore CS8601
            return value;
        }
    }
}
