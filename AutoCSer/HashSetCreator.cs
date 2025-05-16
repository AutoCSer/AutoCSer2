using System;
using System.Collections.Generic;

namespace AutoCSer
{
    /// <summary>
    /// 创建 HashSet
    /// </summary>
    public static class HashSetCreator
    {
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        public static HashSet<int> CreateInt()
        {
#if AOT
            return new HashSet<int>(AutoCSer.IntComparer.Default);
#else
            return new HashSet<int>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        public static HashSet<long> CreateLong()
        {
#if AOT
            return new HashSet<long>(AutoCSer.LongComparer.Default);
#else
            return new HashSet<long>();
#endif
        }
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>HASH表</returns>
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
    /// 创建 HashSet表
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public static class HashSetCreator<T> where T : IEquatable<T>
    {
#if AOT
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(T).IsValueType;
#endif
        /// <summary>
        /// 创建 HashSet 表
        /// </summary>
        /// <returns>HashSet 表</returns>
        public static HashSet<T> Create()
        {
#if AOT
            if (isValueType) return new HashSet<T>(AutoCSer.EquatableComparer<T>.Default);
#endif
            return new HashSet<T>();
        }
    }
}
