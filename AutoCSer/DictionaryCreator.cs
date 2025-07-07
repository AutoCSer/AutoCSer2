using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Create the dictionary
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// Create a dictionary
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="T">Data type</typeparam>
        /// <returns>Dictionary</returns>
        public static Dictionary<HashObject<KT>, T> CreateHashObject<KT, T>()
            where KT : class
        {
#if AOT
            return new Dictionary<HashObject<KT>, T>(AutoCSer.EquatableComparer<HashObject<KT>>.Default);
#else
            return new Dictionary<HashObject<KT>, T>();
#endif
        }
        /// <summary>
        /// Create a dictionary
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">Keyword type
        /// 关键字类型</typeparam>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<HashObject<KT>, T> CreateHashObject<KT, T>(int capacity)
            where KT : class
        {
#if AOT
            return new Dictionary<HashObject<KT>, T>(capacity, AutoCSer.EquatableComparer<HashObject<KT>>.Default);
#else
            return new Dictionary<HashObject<KT>, T>(capacity);
#endif
        }
    }
    /// <summary>
    /// Create the dictionary
    /// 创建字典
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public static class DictionaryCreator<KT> where KT : IEquatable<KT>
    {
#if AOT
        /// <summary>
        /// Whether it is a value type
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(KT).IsValueType;
#endif
        /// <summary>
        /// Create a dictionary
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">Data type</typeparam>
        /// <returns>Dictionary</returns>
        public static Dictionary<KT, VT> Create<VT>()
        {
#if AOT
            if (isValueType) return new Dictionary<KT, VT>(AutoCSer.EquatableComparer<KT>.Default);
#endif
            return new Dictionary<KT, VT>();
        }
        /// <summary>
        /// Create a dictionary
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">Data type</typeparam>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<KT, VT> Create<VT>(int capacity)
        {
#if AOT
            if (isValueType) return new Dictionary<KT, VT>(capacity, AutoCSer.EquatableComparer<KT>.Default);
#endif
            return new Dictionary<KT, VT>(capacity);
        }
    }
}
