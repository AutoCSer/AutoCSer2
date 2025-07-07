using AutoCSer.Extensions;
using System;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    /// <typeparam name="T">排序数据类型</typeparam>
    internal struct QuickRangeSort<T>
    {
        /// <summary>
        /// 待排序数组
        /// </summary>
        private readonly T[] array;
        /// <summary>
        /// 排序比较器
        /// </summary>
        private readonly Func<T, T, int> comparer;
        /// <summary>
        /// 跳过数据数量
        /// </summary>
        private readonly int skipCount;
        /// <summary>
        /// 最后一条记录位置-1
        /// </summary>
        private readonly int getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="getEndIndex">最后一条获取记录位置-1</param>
        internal QuickRangeSort(T[] array, Func<T, T, int> comparer, int skipCount, int getEndIndex)
        {
            this.array = array;
            this.comparer = comparer;
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">数组起始位置</param>
        /// <param name="endIndex">数组结束位置-1</param>
        internal void Sort(int startIndex, int endIndex)
        {
            do
            {
                T leftValue = array[startIndex], rightValue = array[endIndex];
                int average = (endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (comparer(leftValue, rightValue) > 0)
                    {
                        array[startIndex] = rightValue;
                        array[endIndex] = leftValue;
                    }
                    break;
                }
                average += startIndex;
                //if (average > getEndIndex) average = getEndIndex;
                //else if (average < skipCount) average = skipCount;
                int leftIndex = startIndex, rightIndex = endIndex;
                T value = array[average];
                if (comparer(leftValue, value) <= 0)
                {
                    if (comparer(value, rightValue) > 0)
                    {
                        array[rightIndex] = value;
                        if (comparer(leftValue, rightValue) <= 0) array[average] = value = rightValue;
                        else
                        {
                            array[leftIndex] = rightValue;
                            array[average] = value = leftValue;
                        }
                    }
                }
                else if (comparer(leftValue, rightValue) <= 0)
                {
                    array[leftIndex] = value;
                    array[average] = value = leftValue;
                }
                else
                {
                    array[rightIndex] = leftValue;
                    if (comparer(value, rightValue) <= 0)
                    {
                        array[leftIndex] = value;
                        array[average] = value = rightValue;
                    }
                    else array[leftIndex] = rightValue;
                }
                ++leftIndex;
                --rightIndex;
                do
                {
                    while (comparer(array[leftIndex], value) < 0) ++leftIndex;
                    while (comparer(value, array[rightIndex]) < 0) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = array[leftIndex];
                        array[leftIndex] = array[rightIndex];
                        array[rightIndex] = leftValue;
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
                    if (startIndex < rightIndex && rightIndex >= skipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > getEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= getEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < skipCount) break;
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
    }
}
