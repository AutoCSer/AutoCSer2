using AutoCSer.Extensions.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 可枚举集合扩展操作
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// 单个数据转换为可枚举集合
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="value">枚举数据</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static IEnumerable<T> valueToEnumerable<T>(this T value)
        {
            yield return value;
        }

        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="values">Data collection
        /// 数据集合</param>
        /// <param name="capacity">初始空间大小</param>
        /// <returns>Array</returns>
        internal static LeftArray<T> getLeftArray<T>(this IEnumerable<T> values, int capacity = 0)
        {
            LeftArray<T> array = new LeftArray<T>(0);
            foreach (T value in values) array.Add(value);
            return array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="values">Data collection
        /// 数据集合</param>
        /// <param name="capacity">初始空间大小</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ListArray<T> getListArray<T>(this IEnumerable<T> values, int capacity = 0)
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
        internal static Task<EnumerableTask<T>> enumerableTask<T>(this IEnumerable<T> source, int taskCount, Func<T, Task> getTask)
        {
            return new EnumerableTask<T>(source, taskCount, getTask).Start();
        }
        /// <summary>
        /// 并发任务，用于替代 Parallel.ForEachAsync
        /// </summary>
        /// <param name="source">获取任务参数集合</param>
        /// <param name="taskCount">最大并发任务数量，最小值为 1</param>
        /// <param name="getTask">获取任务委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Task<EnumerableValueTask<T>> enumerableValueTask<T>(this IEnumerable<T> source, int taskCount, Func<T, ValueTask> getTask)
        {
            return new EnumerableValueTask<T>(source, taskCount, getTask).Start();
        }
    }
    /// <summary>
    /// 可枚举集合扩展操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct EnumerableExtensions<T>
    {
        /// <summary>
        /// 可枚举集合
        /// </summary>
        private readonly IEnumerable<T> values;
        /// <summary>
        /// 可枚举集合扩展操作
        /// </summary>
        /// <param name="values"></param>
        public EnumerableExtensions(IEnumerable<T> values)
        {
            this.values = values;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <param name="capacity">初始空间大小</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LeftArray<T> GetLeftArray(int capacity = 0)
        {
            return values.getLeftArray(capacity);
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <param name="capacity">初始空间大小</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ListArray<T> GetListArray(int capacity = 0)
        {
            return values.getListArray(capacity);
        }
        /// <summary>
        /// 并发任务，用于替代 Parallel.ForEachAsync
        /// </summary>
        /// <param name="taskCount">最大并发任务数量，最小值为 1</param>
        /// <param name="getTask">获取任务委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<EnumerableTask<T>> EnumerableTask(int taskCount, Func<T, Task> getTask)
        {
            return values.enumerableTask(taskCount, getTask);
        }
        /// <summary>
        /// 并发任务，用于替代 Parallel.ForEachAsync
        /// </summary>
        /// <param name="taskCount">最大并发任务数量，最小值为 1</param>
        /// <param name="getTask">获取任务委托</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<EnumerableValueTask<T>> EnumerableValueTask(int taskCount, Func<T, ValueTask> getTask)
        {
            return values.enumerableValueTask(taskCount, getTask);
        }
    }
}
