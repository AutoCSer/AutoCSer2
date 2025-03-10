using AutoCSer.Algorithm;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class LeftArraySort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this LeftArray<int> array)
        {
            fixed (int* arrayFixed = array.GetFixedBuffer()) AutoCSer.Algorithm.QuickSort.SortInt((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量</param>
        public static void QuickRangeSort<T>(this LeftArray<T> array, Func<T, T, int> comparer, int skipCount, int count)
        {
            if (count > 0)
            {
                if (count != 1 || array.Length != 1 || skipCount != 0) new QuickRangeSort<T>(array.Array, comparer, skipCount, skipCount + count - 1).Sort(0, array.Length - 1);
            }
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量</param>
        /// <returns>已排序数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static SubArray<T> GetQuickRangeSort<T>(this LeftArray<T> array, Func<T, T, int> comparer, int skipCount, int count)
        {
            QuickRangeSort(array, comparer, skipCount, count);
            return new SubArray<T>(array.Array, skipCount, count);
        }
    }
}
