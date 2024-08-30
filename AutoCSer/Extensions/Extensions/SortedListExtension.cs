using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 排序数组相关扩展
    /// </summary>
    public static class SortedListExtension
    {
        /// <summary>
        /// 关键字不存在时添加数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回 false 表示已经存在关键字添加失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryAdd<KT, VT>(this SortedList<KT, VT> list, KT key, VT value)
        {
            if (!list.ContainsKey(key))
            {
                list.Add(key, value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据关键字删除数据并返回被删除数据
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>返回 false 表示关键字不存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool Remove<KT, VT>(this SortedList<KT, VT> list, KT key, out VT value)
        {
            if (list.TryGetValue(key, out value))
            {
                list.Remove(key);
                return true;
            }
            value = default(VT);
            return false;
        }
    }
}
