using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Nullable reference type extension operation (Used to eliminate IDE warnings)
    /// 可空引用类型扩展操作（用于消除 IDE 警告）
    /// </summary>
    internal static class NullableReferenceExtension
    {
        /// <summary>
        /// The object is converted to a specified nullable reference type
        /// object 转换为指定可空引用类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T? castClass<T>(this object? value) where T : class
#else
        internal static T castClass<T>(this object value) where T : class
#endif
        {
#if NetStandard21
            return (T?)value;
#else
            return (T)value;
#endif
        }
        /// <summary>
        /// The object is converted to a specified nullable type
        /// object 转换为指定可空类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T? castType<T>(this object? value)
#else
        internal static T castType<T>(this object value)
#endif
        {
            if (value != null) return (T)value;
            return default(T);
        }
        /// <summary>
        /// Convert object to a value type
        /// object 转换为值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T castValue<T>(this object? value) where T : struct
#else
        internal static T castValue<T>(this object value) where T : struct
#endif
        {
            if (value != null) return (T)value;
            return default(T);
        }
        /// <summary>
        /// Non-empty object conversion type
        /// 非空 object 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T notNullCastType<T>(this object? value) where T : class
#else
        internal static T notNullCastType<T>(this object value) where T : class
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
        /// Non-empty object type conversion
        /// 非空对象类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T notNull<T>(this T? value) where T : class
#else
        internal static T notNull<T>(this T value) where T : class
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
        /// Convert an array of nullable reference types to an array
        /// 可空引用类型数组转换为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T?[] castArray<T>(this T? value) where T : class
#else
        internal static T[] castArray<T>(this T value) where T : class
#endif
        {
#if NetStandard21
            return new T?[] { value };
#else
            return new T[] { value };
#endif
        }
        /// <summary>
        /// Generic values are converted to object
        /// 泛型值转换为 object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>

        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static object castObject<T>(this T value)
        {
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
        /// <summary>
        /// Set the array elements to the default values
        /// 设置数组元素为默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void setDefault<T>(this T[] array, int index)
        {
#pragma warning disable CS8601
            array[index] = default(T);
#pragma warning restore CS8601
        }
        /// <summary>
        /// Set the array elements as default values and return the original values
        /// 设置数组元素为默认值并返回原始值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T getSetDefault<T>(this T[] array, int index)
        {
            T value = array[index];
#pragma warning disable CS8601
            array[index] = default(T);
#pragma warning restore CS8601
            return value;
        }
    }
}
