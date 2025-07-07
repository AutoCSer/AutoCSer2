using System;
using System.Collections.Generic;

namespace AutoCSer
{
    /// <summary>
    /// Create the HashSet
    /// 创建 HashSet
    /// </summary>
    public static class HashSetCreator
    {
        /// <summary>
        /// Create a HashSet
        /// 创建 HashSet
        /// </summary>
        /// <returns>HashSet</returns>
        public static HashSet<int> CreateInt()
        {
#if AOT
            return new HashSet<int>(AutoCSer.IntComparer.Default);
#else
            return new HashSet<int>();
#endif
        }
        /// <summary>
        /// Create a HashSet
        /// 创建 HashSet
        /// </summary>
        /// <returns>HashSet</returns>
        public static HashSet<long> CreateLong()
        {
#if AOT
            return new HashSet<long>(AutoCSer.LongComparer.Default);
#else
            return new HashSet<long>();
#endif
        }
        /// <summary>
        /// Create a HashSet
        /// 创建 HashSet
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <returns>HashSet</returns>
        internal static HashSet<HashObject<T>> CreateHashObject<T>()
            where T : class
        {
#if AOT
            return new HashSet<HashObject<T>>(AutoCSer.EquatableComparer<HashObject<T>>.Default);
#else
            return new HashSet<HashObject<T>>();
#endif
        }
    }
    /// <summary>
    /// Create the HashSet
    /// 创建 HashSet
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public static class HashSetCreator<T> where T : IEquatable<T>
    {
#if AOT
        /// <summary>
        /// Whether it is a value type
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(T).IsValueType;
#endif
        /// <summary>
        /// Create a HashSet
        /// 创建 HashSet
        /// </summary>
        /// <returns>HashSet</returns>
        public static HashSet<T> Create()
        {
#if AOT
            if (isValueType) return new HashSet<T>(AutoCSer.EquatableComparer<T>.Default);
#endif
            return new HashSet<T>();
        }
    }
}
