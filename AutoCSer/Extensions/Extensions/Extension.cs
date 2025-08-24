using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Get the extended operation encapsulation
    /// 获取扩展操作封装
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// Get the AutoCSer array extension encapsulation
        /// 获取 AutoCSer 数组扩展封装
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.ULongArrayExtensions AutoCSerExtensions(this ulong[] array)
        {
            return new AutoCSer.Extensions.ULongArrayExtensions(array);
        }
        /// <summary>
        /// Get the AutoCSer array extension encapsulation
        /// 获取 AutoCSer 数组扩展封装
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.LongArrayExtensions AutoCSerExtensions(this long[] array)
        {
            return new AutoCSer.Extensions.LongArrayExtensions(array);
        }
        /// <summary>
        /// Get the AutoCSer array extension encapsulation
        /// 获取 AutoCSer 数组扩展封装
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.UIntArrayExtensions AutoCSerExtensions(this uint[] array)
        {
            return new AutoCSer.Extensions.UIntArrayExtensions(array);
        }
        /// <summary>
        /// Get the AutoCSer array extension encapsulation
        /// 获取 AutoCSer 数组扩展封装
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.IntArrayExtensions AutoCSerExtensions(this int[] array)
        {
            return new AutoCSer.Extensions.IntArrayExtensions(array);
        }
        /// <summary>
        /// Get the AutoCSer array extension encapsulation
        /// 获取 AutoCSer 数组扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.ArrayExtensions<T> AutoCSerExtensions<T>(this T[] array)
        {
            return new AutoCSer.Extensions.ArrayExtensions<T>(array);
        }
        /// <summary>
        /// Get the AutoCSer dictionary extension encapsulation
        /// 获取 AutoCSer 字典扩展封装
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.DictionaryExtensions<KT, VT> AutoCSerExtensions<KT, VT>(this Dictionary<KT, VT> dictionary)
#if NetStandard21
             where KT : notnull
#endif
        {
            return new AutoCSer.Extensions.DictionaryExtensions<KT, VT>(dictionary);
        }
        /// <summary>
        /// Get the AutoCSer collection extension encapsulation
        /// 获取 AutoCSer 集合扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.CollectionExtensions<T> AutoCSerExtensions<T>(this ICollection<T> collection)
        {
            return new AutoCSer.Extensions.CollectionExtensions<T>(collection);
        }
        /// <summary>
        /// 获取 AutoCSer 可枚举集合扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.EnumerableExtensions<T> AutoCSerExtensions<T>(this IEnumerable<T> values)
        {
            return new AutoCSer.Extensions.EnumerableExtensions<T>(values);
        }
        /// <summary>
        /// 获取 AutoCSer 同步上下文扩展封装
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.SynchronizationContextExtensions AutoCSerExtensions(this SynchronizationContext context)
        {
            return new AutoCSer.Extensions.SynchronizationContextExtensions(context);
        }
        /// <summary>
        /// Get the AutoCSer Task extension encapsulation
        /// 获取 AutoCSer Task 扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.TaskExtensions<T> AutoCSerExtensions<T>(this Task<T> task)
        {
            return new AutoCSer.Extensions.TaskExtensions<T>(task);
        }
        /// <summary>
        /// Get the AutoCSer Task extension encapsulation
        /// 获取 AutoCSer Task 扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.TaskExtensions AutoCSerTaskExtensions<T>(this Task<T> task)
        {
            return new AutoCSer.Extensions.TaskExtensions(task);
        }
        /// <summary>
        /// Get the AutoCSer Task extension encapsulation
        /// 获取 AutoCSer Task 扩展封装
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.TaskExtensions AutoCSerExtensions(this Task task)
        {
            return new AutoCSer.Extensions.TaskExtensions(task);
        }
        /// <summary>
        /// Get the AutoCSer ValueTask extension encapsulation
        /// 获取 AutoCSer ValueTask 扩展封装
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.ValueTaskExtensions AutoCSerExtensions(this ValueTask task)
        {
            return new AutoCSer.Extensions.ValueTaskExtensions(task);
        }
        /// <summary>
        /// 获取 AutoCSer 泛型扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static AutoCSer.Extensions.ClassGenericTypeExtensions<T> AutoCSerExtensions<T>(this T? value) where T : class
#else
        public static AutoCSer.Extensions.ClassGenericTypeExtensions<T> AutoCSerExtensions<T>(this T value) where T : class
#endif
        {
            return new AutoCSer.Extensions.ClassGenericTypeExtensions<T>(value);
        }
        /// <summary>
        /// 获取 AutoCSer 泛型扩展封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.GenericTypeExtensions<T> AutoCSerGenericTypeExtensions<T>(this T value)
        {
            return new AutoCSer.Extensions.GenericTypeExtensions<T>(value);
        }
        /// <summary>
        /// Get the AutoCSer object extension encapsulation
        /// 获取 AutoCSer object 扩展封装
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static AutoCSer.Extensions.ObjectExtensions AutoCSerObjectExtensions(this object? value)
#else
        public static AutoCSer.Extensions.ObjectExtensions AutoCSerObjectExtensions(this object value)
#endif
        {
            return new AutoCSer.Extensions.ObjectExtensions(value);
        }
    }
}
