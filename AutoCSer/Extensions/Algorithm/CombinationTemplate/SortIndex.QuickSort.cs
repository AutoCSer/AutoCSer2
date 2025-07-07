using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct ULongSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void SortDesc(ULongSortIndex* startIndex, ULongSortIndex* endIndex)
        {
            do
            {
                ULongSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareTo(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                ULongSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                ULongSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareTo(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareTo(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareTo(rightValue.Value) < 0) *leftIndex = rightValue;
                        else
                        {
                            *leftIndex = indexValue;
                            *averageIndex = indexValue = rightValue;
                        }
                    }
                    else
                    {
                        *leftIndex = indexValue;
                        *averageIndex = indexValue = leftValue;
                    }
                }
                else
                {
                    if (indexValue.Value.CompareTo(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareTo(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                ulong value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareTo(value) > 0) ++leftIndex;
                    while (value.CompareTo((*rightIndex).Value) > 0) --rightIndex;
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
                    if (startIndex < rightIndex) SortDesc(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) SortDesc(leftIndex, endIndex);
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
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, Func<T, ulong> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.ULongSortIndex));
            try
            {
                AutoCSer.Algorithm.ULongSortIndex* sortIndex = (AutoCSer.Algorithm.ULongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.ULongSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="count">The quantity of data to be sorted
        /// 排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, AutoCSer.Algorithm.ULongSortIndex* sortIndex)
        {
            int index = 0, readIndex;
            do
            {
                readIndex = sortIndex[index].Index;
                if (readIndex != index)
                {
                    T value = array[index];
                    int writeIndex = index;
                    do
                    {
                        sortIndex[writeIndex].Index = writeIndex;
                        array[writeIndex] = array[readIndex];
                        writeIndex = readIndex;
                        readIndex = sortIndex[readIndex].Index;
                    }
                    while (readIndex != index);
                    sortIndex[writeIndex].Index = writeIndex;
                    array[writeIndex] = value;
                }
            }
            while (++index != count);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, Func<T, ulong> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.ULongSortIndex));
            try
            {
                AutoCSer.Algorithm.ULongSortIndex* sortIndex = (AutoCSer.Algorithm.ULongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.ULongSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="array">Array</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.ULongSortIndex* sortIndex)
        {
            int index = startIndex, readIndex;
            do
            {
                readIndex = sortIndex[index - startIndex].Index;
                if (readIndex != index)
                {
                    T value = array[index];
                    int writeIndex = index;
                    do
                    {
                        sortIndex[writeIndex - startIndex].Index = writeIndex;
                        array[writeIndex] = array[readIndex];
                        writeIndex = readIndex;
                        readIndex = sortIndex[readIndex - startIndex].Index;
                    }
                    while (readIndex != index);
                    sortIndex[writeIndex - startIndex].Index = writeIndex;
                    array[writeIndex] = value;
                }
            }
            while (++index != endIndex);
        }
    }
}
