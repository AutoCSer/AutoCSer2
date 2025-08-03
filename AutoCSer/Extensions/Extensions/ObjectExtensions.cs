using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// object expansion operation
    /// object 扩展操作
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ObjectExtensions
    {
        /// <summary>
        /// object
        /// </summary>
#if NetStandard21
        private readonly object? value;
#else
        private readonly object value;
#endif
        /// <summary>
        /// object expansion operation
        /// object 扩展操作
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public ObjectExtensions(object? value)
#else
        public ObjectExtensions(object value)
#endif
        {
            this.value = value;
        }
        /// <summary>
        /// The object is converted to a specified nullable reference type
        /// object 转换为指定可空引用类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? CastClass<T>() where T : class
#else
        public T CastClass<T>() where T : class
#endif
        {
            return value.castClass<T>();
        }
        /// <summary>
        /// The object is converted to a specified nullable type
        /// object 转换为指定可空类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? CastType<T>()
#else
        public T CastType<T>()
#endif
        {
            return value.castType<T>();
        }
        /// <summary>
        /// Convert object to a value type
        /// object 转换为值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T CastValue<T>() where T : struct
#else
        public T CastValue<T>() where T : struct
#endif
        {
            return value.castValue<T>();
        }
        /// <summary>
        /// Non-empty object conversion type
        /// 非空 object 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T NotNullCastType<T>() where T : class
#else
        public T NotNullCastType<T>() where T : class
#endif
        {
            return value.notNullCastType<T>();
        }
    }
}
