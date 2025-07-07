using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序器
    /// </summary>
    /// <typeparam name="T">排序数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct QuickSort<T>
    {
        /// <summary>
        /// 待排序数组
        /// </summary>
        private readonly T[] Array;
        /// <summary>
        /// 排序比较器
        /// </summary>
        private readonly Func<T, T, int> Comparer;
        /// <summary>
        /// 排序器
        /// </summary>
        /// <param name="array"></param>
        /// <param name="comparer"></param>
        internal QuickSort(T[] array, Func<T, T, int> comparer)
        {
            Array = array;
            Comparer = comparer;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal void Sort(int startIndex, int endIndex)
        {
            do
            {
                T leftValue = Array[startIndex], rightValue = Array[endIndex];
                int average = (endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (Comparer(leftValue, rightValue) > 0)
                    {
                        Array[startIndex] = rightValue;
                        Array[endIndex] = leftValue;
                    }
                    break;
                }
                int leftIndex = startIndex, rightIndex = endIndex;
                T value = Array[average += startIndex];
                if (Comparer(leftValue, value) <= 0)
                {
                    if (Comparer(value, rightValue) > 0)
                    {
                        Array[rightIndex] = value;
                        if (Comparer(leftValue, rightValue) <= 0) Array[average] = value = rightValue;
                        else
                        {
                            Array[leftIndex] = rightValue;
                            Array[average] = value = leftValue;
                        }
                    }
                }
                else if (Comparer(leftValue, rightValue) <= 0)
                {
                    Array[leftIndex] = value;
                    Array[average] = value = leftValue;
                }
                else
                {
                    Array[rightIndex] = leftValue;
                    if (Comparer(value, rightValue) <= 0)
                    {
                        Array[leftIndex] = value;
                        Array[average] = value = rightValue;
                    }
                    else Array[leftIndex] = rightValue;
                }
                ++leftIndex;
                --rightIndex;
                do
                {
                    while (Comparer(Array[leftIndex], value) < 0) ++leftIndex;
                    while (Comparer(value, Array[rightIndex]) < 0) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = Array[leftIndex];
                        Array[leftIndex] = Array[rightIndex];
                        Array[rightIndex] = leftValue;
                    }
                    else
                    {
                        //if (leftIndex == rightIndex)
                        //{
                        //    ++leftIndex;
                        //    --rightIndex;
                        //}
                        int indexValue = (leftIndex ^ rightIndex).logicalInversion();
                        leftIndex += indexValue;
                        rightIndex -= indexValue;
                        break;
                    }
                }
                while (++leftIndex <= --rightIndex);
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) Sort(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) Sort(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Sort(T[] array, Func<T, T, int> comparer)
        {
            if (array.Length > 1) new QuickSort<T>(array, comparer).Sort(0, array.Length - 1);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Sort(T[] array, Func<T, T, int> comparer, int startIndex, int count)
        {
#if DEBUG
            array.debugCheckRange(startIndex, count);
#endif
            if (count > 1) new QuickSort<T>(array, comparer).Sort(startIndex, (startIndex + count) - 1);
        }
    }
}
