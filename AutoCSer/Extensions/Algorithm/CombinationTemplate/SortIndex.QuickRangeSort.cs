using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ULongIndexQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private ULongSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private ULongSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal ULongIndexQuickRangeSortDesc(ULongSortIndex* skipCount, ULongSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(ULongSortIndex* startIndex, ULongSortIndex* endIndex)
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
                ULongSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
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
    public static unsafe partial class LeftArraySort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <param name="leftArray">待排序数组</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <returns>排序关键字集合</returns>
        public static ulong[] GetQuickRangeSortKayDesc<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, ulong> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKayDesc(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<ulong>.Array;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <typeparam name="VT">返回数据类型</typeparam>
        /// <param name="leftArray">待排序数组</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量，大于 0</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="getValue">排序数据获取器</param>
        /// <returns>排序数据集合</returns>
        public static VT[] GetQuickRangeSortDesc<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, ulong> getKey, Func<T, VT> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortDesc(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey, getValue);
            return AutoCSer.EmptyArray<VT>.Array;
        }
    }
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    internal static unsafe partial class ArraySort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">排序数组起始位置</param>
        /// <param name="arraySize">排序数组数据数量</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量，大于 0</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <returns>排序关键字集合</returns>
        internal static ulong[] GetQuickRangeSortKayDesc<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, ulong> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.ULongSortIndex));
                try
                {
                    AutoCSer.Algorithm.ULongSortIndex* sortIndex = (AutoCSer.Algorithm.ULongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.ULongSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.ULongIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    ulong[] keys = AutoCSer.Common.GetUninitializedArray<ulong>(count);
                    fixed (ulong* keyFixed = keys)
                    {
                        ulong* write = keyFixed;
                        do
                        {
                            *write++ = (*skip).Value;
                        }
                        while (++skip != end);
                    }
                    return keys;
                }
                finally { buffer.PushOnly(); }
            }
            return new ulong[] { getKey(array[startIndex]) };
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="T">排序关键字类型</typeparam>
        /// <typeparam name="VT">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">排序数组起始位置</param>
        /// <param name="arraySize">排序数组数据数量</param>
        /// <param name="skipCount">获取跳过数据数量</param>
        /// <param name="count">获取数据记录数量，大于 0</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="getValue">排序数据获取器</param>
        /// <returns>排序数据集合</returns>
        internal static VT[] GetQuickRangeSortDesc<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, ulong> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.ULongSortIndex));
                try
                {
                    AutoCSer.Algorithm.ULongSortIndex* sortIndex = (AutoCSer.Algorithm.ULongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.ULongSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.ULongIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    VT[] values = new VT[count];
                    startIndex = 0;
                    do
                    {
                        values[startIndex++] = getValue(array[(*skip).Index]);
                    }
                    while (++skip != end);
                    return values;
                }
                finally { buffer.PushOnly(); }
            }
            return new VT[] { getValue(array[startIndex]) };
        }
    }
}
