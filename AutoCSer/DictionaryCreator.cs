using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 创建字典
    /// </summary>
    public static partial class DictionaryCreator
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
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
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
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
    /// 创建字典
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public static class DictionaryCreator<KT> where KT : IEquatable<KT>
    {
#if AOT
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(KT).IsValueType;
#endif
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> Create<VT>()
        {
#if AOT
            if (isValueType) return new Dictionary<KT, VT>(AutoCSer.EquatableComparer<KT>.Default);
#endif
            return new Dictionary<KT, VT>();
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> Create<VT>(int capacity)
        {
#if AOT
            if (isValueType) return new Dictionary<KT, VT>(capacity, AutoCSer.EquatableComparer<KT>.Default);
#endif
            return new Dictionary<KT, VT>(capacity);
        }
    }
}
