using System;
using System.Collections.Generic;

namespace AutoCSer.Extensions
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
        /// <typeparam name="VT">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> CreateOnly<KT, VT>()
             where KT : class
        {
            return new Dictionary<KT, VT>();
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="KT">关键字类型</typeparam>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> CreateAny<KT, VT>(int capacity)
        {
#if AOT
            return new Dictionary<KT, VT>(capacity, AutoCSer.AOT.EqualityComparer<KT>.Default);
#else
            return new Dictionary<KT, VT>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<char, T> CreateChar<T>()
        {
#if AOT
            return new Dictionary<char, T>(AutoCSer.AOT.CharComparer.Default);
#else
            return new Dictionary<char, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<long, T> CreateLong<T>()
        {
#if AOT
            return new Dictionary<long, T>(AutoCSer.AOT.LongComparer.Default);
#else
            return new Dictionary<long, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static Dictionary<long, T> CreateLong<T>(int capacity)
        {
#if AOT
            return new Dictionary<long, T>(capacity, AutoCSer.AOT.LongComparer.Default);
#else
            return new Dictionary<long, T>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<ulong, T> CreateULong<T>()
        {
#if AOT
            return new Dictionary<ulong, T>(AutoCSer.AOT.ULongComparer.Default);
#else
            return new Dictionary<ulong, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<HashSubString, T> CreateHashSubString<T>()
        {
#if AOT
            return new Dictionary<HashSubString, T>(AutoCSer.AOT.HashSubStringComparer.Default);
#else
            return new Dictionary<HashSubString, T>();
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>字典</returns>
        public static Dictionary<HashSubString, T> CreateHashSubString<T>(int capacity)
        {
#if AOT
            return new Dictionary<HashSubString, T>(capacity, AutoCSer.AOT.HashSubStringComparer.Default);
#else
            return new Dictionary<HashSubString, T>(capacity);
#endif
        }
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<AutoCSer.Net.HostEndPoint, T> CreateEndPoint<T>()
        {
#if AOT
            return new Dictionary<AutoCSer.Net.HostEndPoint, T>(AutoCSer.AOT.EndPointComparer.Default);
#else
            return new Dictionary<AutoCSer.Net.HostEndPoint, T>();
#endif
        }
    }
}
