using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ULongQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly ulong* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly ulong* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal ULongQuickRangeSortDesc(ulong* skipCount, ulong* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(ulong* startIndex, ulong* endIndex)
        {
            do
            {
                ulong leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.CompareTo(rightValue) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                ulong* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                ulong value = *averageIndex;
                if (leftValue.CompareTo(value) < 0)
                {
                    if (leftValue.CompareTo(rightValue) < 0)
                    {
                        *rightIndex = leftValue;
                        if (value.CompareTo(rightValue) < 0) *leftIndex = rightValue;
                        else
                        {
                            *leftIndex = value;
                            *averageIndex = value = rightValue;
                        }
                    }
                    else
                    {
                        *leftIndex = value;
                        *averageIndex = value = leftValue;
                    }
                }
                else
                {
                    if (value.CompareTo(rightValue) < 0)
                    {
                        *rightIndex = value;
                        if (leftValue.CompareTo(rightValue) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = value = leftValue;
                        }
                        else *averageIndex = value = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                do
                {
                    while ((*leftIndex).CompareTo(value) > 0) ++leftIndex;
                    while (value.CompareTo(*rightIndex) > 0) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = *leftIndex;
                        *leftIndex = *rightIndex;
                        *rightIndex = leftValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            ++leftIndex;
                            --rightIndex;
                        }
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
namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class SubArraySort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量，大于 0</param>
        internal static void QuickRangeSortDesc(this SubArray<ulong> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (ulong* arrayFixed = array.GetFixedBuffer())
                {
                    ulong* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.ULongQuickRangeSortDesc(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
                }
            }
        }
    }
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class LeftArraySort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量，大于 0</param>
        internal static void QuickRangeSortDesc(this LeftArray<ulong> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (ulong* arrayFixed = array.GetFixedBuffer())
                {
                    ulong* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.ULongQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量，大于 0</param>
        internal static void QuickRangeSortDesc(this ulong[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (ulong* arrayFixed = array)
                {
                    ulong* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.ULongQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}
