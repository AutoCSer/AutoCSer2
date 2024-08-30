using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<ValueResult<T>> Cast<T>(this IEnumerable<T> values)
        {
            foreach (T value in values) yield return value;
        }
    }
}
