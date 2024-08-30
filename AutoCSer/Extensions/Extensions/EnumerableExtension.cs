using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="capacity">初始空间大小</param>
        /// <returns>数组</returns>
        public static LeftArray<T> getLeftArray<T>(this IEnumerable<T> values, int capacity = 0)
        {
            LeftArray<T> array = new LeftArray<T>(0);
            foreach (T value in values) array.Add(value);
            return array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="capacity">初始空间大小</param>
        /// <returns>数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ListArray<T> getListArray<T>(this IEnumerable<T> values, int capacity = 0)
        {
            return new ListArray<T>(getLeftArray(values, capacity));
        }
    }
}
