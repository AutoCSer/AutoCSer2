﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字典相关操作
    /// </summary>
    public static partial class DictionaryExtension
    {
        /// <summary>
        /// 当关键字不存在时添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回 false 表示已经存在关键字添加失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryAdd<KT, VT>(this Dictionary<KT, VT> dictionary, KT key, VT value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 当关键字存在时删除数据并返回
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>返回 false 表示关键字不存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool Remove<KT, VT>(this Dictionary<KT, VT> dictionary, KT key, out VT value)
        {
            if (dictionary.TryGetValue(key, out value))
            {
                dictionary.Remove(key);
                return true;
            }
            return false;
        }
    }
}
