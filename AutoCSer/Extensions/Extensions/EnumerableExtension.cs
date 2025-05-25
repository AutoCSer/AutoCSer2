using AutoCSer.Extensions.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 单个数据转换为可枚举集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="value">枚举数据</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> valueToEnumerable<T>(this T value)
        {
            yield return value;
        }

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

        /// <summary>
        /// 并发任务，用于替代 Parallel.ForEachAsync
        /// </summary>
        /// <param name="source">获取任务参数集合</param>
        /// <param name="taskCount">最大并发任务数量，最小值为 1</param>
        /// <param name="getTask">获取任务委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<EnumerableTask<T>> enumerableTask<T>(this IEnumerable<T> source, int taskCount, Func<T, Task> getTask)
        {
            return new EnumerableTask<T>(source, taskCount, getTask).Start();
        }
#if NetStandard21
        /// <summary>
        /// 并发任务，用于替代 Parallel.ForEachAsync
        /// </summary>
        /// <param name="source">获取任务参数集合</param>
        /// <param name="taskCount">最大并发任务数量，最小值为 1</param>
        /// <param name="getTask">获取任务委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<EnumerableValueTask<T>> enumerableValueTask<T>(this IEnumerable<T> source, int taskCount, Func<T, ValueTask> getTask)
        {
            return new EnumerableValueTask<T>(source, taskCount, getTask).Start();
        }
#endif
    }
}
