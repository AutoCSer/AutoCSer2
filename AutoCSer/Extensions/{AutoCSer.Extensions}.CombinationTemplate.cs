//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer
{
    /// <summary>
    /// 数据反向比较
    /// </summary>
    internal static partial class CompareFromExtensions
    {
        /// <summary>
        /// 数据反向比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int CompareFrom(this long left, long right)
        {
            return right.CompareTo(left);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 数据反向比较
    /// </summary>
    internal static partial class CompareFromExtensions
    {
        /// <summary>
        /// 数据反向比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int CompareFrom(this uint left, uint right)
        {
            return right.CompareTo(left);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// 数据反向比较
    /// </summary>
    internal static partial class CompareFromExtensions
    {
        /// <summary>
        /// 数据反向比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int CompareFrom(this int left, int right)
        {
            return right.CompareTo(left);
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
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this LeftArray<int> array)
        {
            fixed (int* arrayFixed = array.GetFixedBuffer()) AutoCSer.Algorithm.QuickSort.SortInt((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this int[] array)
        {
            fixed (int* arrayFixed = array) AutoCSer.Algorithm.QuickSort.SortInt((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
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
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this LeftArray<long> array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort(array.GetFixedBuffer(), 0, array.Length);
            else if(array.Length > 1) QuickSort(array);
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this long[] array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length);
            else if (array.Length > 1) QuickSort(array);
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
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this LeftArray<uint> array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.Sort(array.GetFixedBuffer(), 0, array.Length);
            else if(array.Length > 1) QuickSort(array);
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this uint[] array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length);
            else if (array.Length > 1) QuickSort(array);
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
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this LeftArray<int> array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.Sort(array.GetFixedBuffer(), 0, array.Length);
            else if(array.Length > 1) QuickSort(array);
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this int[] array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length);
            else if (array.Length > 1) QuickSort(array);
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct LongSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        internal long Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 设置排序索引
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="index">位置索引</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(long value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct UIntSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        internal uint Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 设置排序索引
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="index">位置索引</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(uint value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct IntSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        internal int Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 设置排序索引
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="index">位置索引</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ULongIndexQuickRangeSort
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
        internal ULongIndexQuickRangeSort(ULongSortIndex* skipCount, ULongSortIndex* getEndIndex)
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
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                ULongSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                ULongSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
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
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        public static ulong[] GetQuickRangeSortKay<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, ulong> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKay(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
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
        public static VT[] GetQuickRangeSort<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, ulong> getKey, Func<T, VT> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSort(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey, getValue);
            return AutoCSer.EmptyArray<VT>.Array;
        }
    }
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class ArraySort
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
        internal static ulong[] GetQuickRangeSortKay<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, ulong> getKey)
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
                    new AutoCSer.Algorithm.ULongIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

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
        internal static VT[] GetQuickRangeSort<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, ulong> getKey, Func<T, VT> getValue)
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
                    new AutoCSer.Algorithm.ULongIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LongIndexQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private LongSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private LongSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal LongIndexQuickRangeSortDesc(LongSortIndex* skipCount, LongSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(LongSortIndex* startIndex, LongSortIndex* endIndex)
        {
            do
            {
                LongSortIndex leftValue = *startIndex, rightValue = *endIndex;
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
                LongSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                LongSortIndex indexValue = *averageIndex;
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
                long value = indexValue.Value;
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
        public static long[] GetQuickRangeSortKayDesc<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, long> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKayDesc(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<long>.Array;
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
        public static VT[] GetQuickRangeSortDesc<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, long> getKey, Func<T, VT> getValue)
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
    public static unsafe partial class ArraySort
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
        internal static long[] GetQuickRangeSortKayDesc<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, long> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.LongSortIndex));
                try
                {
                    AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.LongSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.LongIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    long[] keys = AutoCSer.Common.GetUninitializedArray<long>(count);
                    fixed (long* keyFixed = keys)
                    {
                        long* write = keyFixed;
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
            return new long[] { getKey(array[startIndex]) };
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
        internal static VT[] GetQuickRangeSortDesc<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, long> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.LongSortIndex));
                try
                {
                    AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.LongSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.LongIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LongIndexQuickRangeSort
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private LongSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private LongSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal LongIndexQuickRangeSort(LongSortIndex* skipCount, LongSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(LongSortIndex* startIndex, LongSortIndex* endIndex)
        {
            do
            {
                LongSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                LongSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                LongSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                long value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        public static long[] GetQuickRangeSortKay<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, long> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKay(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<long>.Array;
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
        public static VT[] GetQuickRangeSort<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, long> getKey, Func<T, VT> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSort(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey, getValue);
            return AutoCSer.EmptyArray<VT>.Array;
        }
    }
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class ArraySort
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
        internal static long[] GetQuickRangeSortKay<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, long> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.LongSortIndex));
                try
                {
                    AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.LongSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.LongIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    long[] keys = AutoCSer.Common.GetUninitializedArray<long>(count);
                    fixed (long* keyFixed = keys)
                    {
                        long* write = keyFixed;
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
            return new long[] { getKey(array[startIndex]) };
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
        internal static VT[] GetQuickRangeSort<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, long> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.LongSortIndex));
                try
                {
                    AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.LongSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.LongIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UIntIndexQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private UIntSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private UIntSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal UIntIndexQuickRangeSortDesc(UIntSortIndex* skipCount, UIntSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(UIntSortIndex* startIndex, UIntSortIndex* endIndex)
        {
            do
            {
                UIntSortIndex leftValue = *startIndex, rightValue = *endIndex;
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
                UIntSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                UIntSortIndex indexValue = *averageIndex;
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
                uint value = indexValue.Value;
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
        public static uint[] GetQuickRangeSortKayDesc<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, uint> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKayDesc(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<uint>.Array;
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
        public static VT[] GetQuickRangeSortDesc<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, uint> getKey, Func<T, VT> getValue)
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
    public static unsafe partial class ArraySort
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
        internal static uint[] GetQuickRangeSortKayDesc<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, uint> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.UIntSortIndex));
                try
                {
                    AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.UIntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.UIntIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    uint[] keys = AutoCSer.Common.GetUninitializedArray<uint>(count);
                    fixed (uint* keyFixed = keys)
                    {
                        uint* write = keyFixed;
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
            return new uint[] { getKey(array[startIndex]) };
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
        internal static VT[] GetQuickRangeSortDesc<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, uint> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.UIntSortIndex));
                try
                {
                    AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.UIntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.UIntIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UIntIndexQuickRangeSort
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private UIntSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private UIntSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal UIntIndexQuickRangeSort(UIntSortIndex* skipCount, UIntSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(UIntSortIndex* startIndex, UIntSortIndex* endIndex)
        {
            do
            {
                UIntSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                UIntSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                UIntSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                uint value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        public static uint[] GetQuickRangeSortKay<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, uint> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKay(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<uint>.Array;
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
        public static VT[] GetQuickRangeSort<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, uint> getKey, Func<T, VT> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSort(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey, getValue);
            return AutoCSer.EmptyArray<VT>.Array;
        }
    }
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class ArraySort
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
        internal static uint[] GetQuickRangeSortKay<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, uint> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.UIntSortIndex));
                try
                {
                    AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.UIntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.UIntIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    uint[] keys = AutoCSer.Common.GetUninitializedArray<uint>(count);
                    fixed (uint* keyFixed = keys)
                    {
                        uint* write = keyFixed;
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
            return new uint[] { getKey(array[startIndex]) };
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
        internal static VT[] GetQuickRangeSort<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, uint> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.UIntSortIndex));
                try
                {
                    AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.UIntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.UIntIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IntIndexQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private IntSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private IntSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal IntIndexQuickRangeSortDesc(IntSortIndex* skipCount, IntSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(IntSortIndex* startIndex, IntSortIndex* endIndex)
        {
            do
            {
                IntSortIndex leftValue = *startIndex, rightValue = *endIndex;
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
                IntSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                IntSortIndex indexValue = *averageIndex;
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
                int value = indexValue.Value;
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
        public static int[] GetQuickRangeSortKayDesc<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, int> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKayDesc(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<int>.Array;
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
        public static VT[] GetQuickRangeSortDesc<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, int> getKey, Func<T, VT> getValue)
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
    public static unsafe partial class ArraySort
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
        internal static int[] GetQuickRangeSortKayDesc<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, int> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.IntSortIndex));
                try
                {
                    AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.IntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.IntIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    int[] keys = AutoCSer.Common.GetUninitializedArray<int>(count);
                    fixed (int* keyFixed = keys)
                    {
                        int* write = keyFixed;
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
            return new int[] { getKey(array[startIndex]) };
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
        internal static VT[] GetQuickRangeSortDesc<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, int> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.IntSortIndex));
                try
                {
                    AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.IntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.IntIndexQuickRangeSortDesc(skip, end - 1).Sort(sortIndex, nextSortIndex);

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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IntIndexQuickRangeSort
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private IntSortIndex* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private IntSortIndex* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal IntIndexQuickRangeSort(IntSortIndex* skipCount, IntSortIndex* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(IntSortIndex* startIndex, IntSortIndex* endIndex)
        {
            do
            {
                IntSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                IntSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                IntSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                int value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        public static int[] GetQuickRangeSortKay<T>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, int> getKey)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSortKay(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey);
            return AutoCSer.EmptyArray<int>.Array;
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
        public static VT[] GetQuickRangeSort<T, VT>(this LeftArray<T> leftArray, int skipCount, int count, Func<T, int> getKey, Func<T, VT> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] < 0");
            if (count < 0) throw new IndexOutOfRangeException($"count[{AutoCSer.Extensions.NumberExtension.toString(count)}] < 0");
            if (skipCount + count > leftArray.Length) throw new IndexOutOfRangeException($"skipCount[{AutoCSer.Extensions.NumberExtension.toString(skipCount)}] + count[{AutoCSer.Extensions.NumberExtension.toString(count)}] > Length[{AutoCSer.Extensions.NumberExtension.toString(leftArray.Length)}]");
            if (count != 0) return ArraySort.GetQuickRangeSort(leftArray.Array, 0, leftArray.Length, skipCount, count, getKey, getValue);
            return AutoCSer.EmptyArray<VT>.Array;
        }
    }
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class ArraySort
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
        internal static int[] GetQuickRangeSortKay<T>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, int> getKey)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.IntSortIndex));
                try
                {
                    AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.IntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.IntIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

                    int[] keys = AutoCSer.Common.GetUninitializedArray<int>(count);
                    fixed (int* keyFixed = keys)
                    {
                        int* write = keyFixed;
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
            return new int[] { getKey(array[startIndex]) };
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
        internal static VT[] GetQuickRangeSort<T, VT>(this T[] array, int startIndex, int arraySize, int skipCount, int count, Func<T, int> getKey, Func<T, VT> getValue)
        {
            if (count != 1 || arraySize != 1 || skipCount != 0)
            {
                AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(arraySize * sizeof(AutoCSer.Algorithm.IntSortIndex));
                try
                {
                    AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                    int endIndex = startIndex + arraySize;
                    (*nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    while (++startIndex != endIndex) (*++nextSortIndex).Set(getKey(array[startIndex]), startIndex);
                    AutoCSer.Algorithm.IntSortIndex* skip = sortIndex + skipCount, end = skip + count;
                    new AutoCSer.Algorithm.IntIndexQuickRangeSort(skip, end - 1).Sort(sortIndex, nextSortIndex);

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
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void Sort(ULongSortIndex* startIndex, ULongSortIndex* endIndex)
        {
            do
            {
                ULongSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                ULongSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                ULongSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
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
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int count, Func<T, ulong> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.ULongSortIndex));
            try
            {
                AutoCSer.Algorithm.ULongSortIndex* sortIndex = (AutoCSer.Algorithm.ULongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.ULongSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int count, AutoCSer.Algorithm.ULongSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, Func<T, ulong> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.ULongSortIndex));
            try
            {
                AutoCSer.Algorithm.ULongSortIndex* sortIndex = (AutoCSer.Algorithm.ULongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.ULongSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.ULongSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct LongSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void SortDesc(LongSortIndex* startIndex, LongSortIndex* endIndex)
        {
            do
            {
                LongSortIndex leftValue = *startIndex, rightValue = *endIndex;
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
                LongSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                LongSortIndex indexValue = *averageIndex;
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
                long value = indexValue.Value;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, Func<T, long> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.LongSortIndex));
            try
            {
                AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.LongSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, AutoCSer.Algorithm.LongSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, Func<T, long> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.LongSortIndex));
            try
            {
                AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.LongSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.LongSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct LongSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void Sort(LongSortIndex* startIndex, LongSortIndex* endIndex)
        {
            do
            {
                LongSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                LongSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                LongSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                long value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int count, Func<T, long> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.LongSortIndex));
            try
            {
                AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.LongSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int count, AutoCSer.Algorithm.LongSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, Func<T, long> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.LongSortIndex));
            try
            {
                AutoCSer.Algorithm.LongSortIndex* sortIndex = (AutoCSer.Algorithm.LongSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.LongSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.LongSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct UIntSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void SortDesc(UIntSortIndex* startIndex, UIntSortIndex* endIndex)
        {
            do
            {
                UIntSortIndex leftValue = *startIndex, rightValue = *endIndex;
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
                UIntSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                UIntSortIndex indexValue = *averageIndex;
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
                uint value = indexValue.Value;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, Func<T, uint> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.UIntSortIndex));
            try
            {
                AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.UIntSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, AutoCSer.Algorithm.UIntSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, Func<T, uint> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.UIntSortIndex));
            try
            {
                AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.UIntSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.UIntSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct UIntSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void Sort(UIntSortIndex* startIndex, UIntSortIndex* endIndex)
        {
            do
            {
                UIntSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                UIntSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                UIntSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                uint value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int count, Func<T, uint> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.UIntSortIndex));
            try
            {
                AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.UIntSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int count, AutoCSer.Algorithm.UIntSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, Func<T, uint> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.UIntSortIndex));
            try
            {
                AutoCSer.Algorithm.UIntSortIndex* sortIndex = (AutoCSer.Algorithm.UIntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.UIntSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.UIntSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct IntSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void SortDesc(IntSortIndex* startIndex, IntSortIndex* endIndex)
        {
            do
            {
                IntSortIndex leftValue = *startIndex, rightValue = *endIndex;
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
                IntSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                IntSortIndex indexValue = *averageIndex;
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
                int value = indexValue.Value;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, Func<T, int> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.IntSortIndex));
            try
            {
                AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.IntSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int count, AutoCSer.Algorithm.IntSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, Func<T, int> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.IntSortIndex));
            try
            {
                AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.IntSortIndex.SortDesc(sortIndex, nextSortIndex);
                QuickSortDesc(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSortDesc<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.IntSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    internal partial struct IntSortIndex
    {
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal unsafe static void Sort(IntSortIndex* startIndex, IntSortIndex* endIndex)
        {
            do
            {
                IntSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                IntSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                IntSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareFrom(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareFrom(rightValue.Value) < 0) *leftIndex = rightValue;
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
                    if (indexValue.Value.CompareFrom(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareFrom(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                int value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom((*rightIndex).Value) > 0) --rightIndex;
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量，大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int count, Func<T, int> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer(count * sizeof(AutoCSer.Algorithm.IntSortIndex));
            try
            {
                AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = 0;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != count) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.IntSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, count, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int count, AutoCSer.Algorithm.IntSortIndex* sortIndex)
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
        /// 数组排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组，数量大于 1</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置，数量大于 1</param>
        /// <param name="getKey">排序键值获取器</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, Func<T, int> getKey)
        {
            AutoCSer.Memory.UnmanagedPoolPointer buffer = AutoCSer.Memory.UnmanagedPool.GetPoolPointer((endIndex - startIndex) * sizeof(AutoCSer.Algorithm.IntSortIndex));
            try
            {
                AutoCSer.Algorithm.IntSortIndex* sortIndex = (AutoCSer.Algorithm.IntSortIndex*)buffer.Pointer.Data, nextSortIndex = sortIndex;
                int index = startIndex;
                (*nextSortIndex).Set(getKey(array[index]), index);
                while (++index != endIndex) (*++nextSortIndex).Set(getKey(array[index]), index);
                AutoCSer.Algorithm.IntSortIndex.Sort(sortIndex, nextSortIndex);
                QuickSort(array, startIndex, endIndex, sortIndex);
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 索引排序以后调整数组数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="startIndex">排序起始位置</param>
        /// <param name="endIndex">排序结束位置</param>
        /// <param name="sortIndex">排序索引</param>
        internal static void QuickSort<T>(this T[] array, int startIndex, int endIndex, AutoCSer.Algorithm.IntSortIndex* sortIndex)
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

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ULongQuickRangeSort
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
        internal ULongQuickRangeSort(ulong* skipCount, ulong* getEndIndex)
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
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                ulong* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                ulong value = *averageIndex;
                if (leftValue.CompareFrom(value) < 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = leftValue;
                        if (value.CompareFrom(rightValue) < 0) *leftIndex = rightValue;
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
                    if (value.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = value;
                        if (leftValue.CompareFrom(rightValue) < 0)
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
                    while ((*leftIndex).CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom(*rightIndex) > 0) --rightIndex;
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
        internal static void QuickRangeSort(this SubArray<ulong> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (ulong* arrayFixed = array.GetFixedBuffer())
                {
                    ulong* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.ULongQuickRangeSort(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSort(this LeftArray<ulong> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (ulong* arrayFixed = array.GetFixedBuffer())
                {
                    ulong* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.ULongQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSort(this ulong[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (ulong* arrayFixed = array)
                {
                    ulong* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.ULongQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LongQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly long* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly long* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal LongQuickRangeSortDesc(long* skipCount, long* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(long* startIndex, long* endIndex)
        {
            do
            {
                long leftValue = *startIndex, rightValue = *endIndex;
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
                long* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                long value = *averageIndex;
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
        internal static void QuickRangeSortDesc(this SubArray<long> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (long* arrayFixed = array.GetFixedBuffer())
                {
                    long* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.LongQuickRangeSortDesc(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSortDesc(this LeftArray<long> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (long* arrayFixed = array.GetFixedBuffer())
                {
                    long* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.LongQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSortDesc(this long[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (long* arrayFixed = array)
                {
                    long* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.LongQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LongQuickRangeSort
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly long* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly long* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal LongQuickRangeSort(long* skipCount, long* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(long* startIndex, long* endIndex)
        {
            do
            {
                long leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                long* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                long value = *averageIndex;
                if (leftValue.CompareFrom(value) < 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = leftValue;
                        if (value.CompareFrom(rightValue) < 0) *leftIndex = rightValue;
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
                    if (value.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = value;
                        if (leftValue.CompareFrom(rightValue) < 0)
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
                    while ((*leftIndex).CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom(*rightIndex) > 0) --rightIndex;
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
        internal static void QuickRangeSort(this SubArray<long> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (long* arrayFixed = array.GetFixedBuffer())
                {
                    long* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.LongQuickRangeSort(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSort(this LeftArray<long> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (long* arrayFixed = array.GetFixedBuffer())
                {
                    long* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.LongQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSort(this long[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (long* arrayFixed = array)
                {
                    long* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.LongQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UIntQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly uint* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly uint* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal UIntQuickRangeSortDesc(uint* skipCount, uint* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(uint* startIndex, uint* endIndex)
        {
            do
            {
                uint leftValue = *startIndex, rightValue = *endIndex;
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
                uint* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                uint value = *averageIndex;
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
        internal static void QuickRangeSortDesc(this SubArray<uint> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (uint* arrayFixed = array.GetFixedBuffer())
                {
                    uint* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.UIntQuickRangeSortDesc(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSortDesc(this LeftArray<uint> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (uint* arrayFixed = array.GetFixedBuffer())
                {
                    uint* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.UIntQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSortDesc(this uint[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (uint* arrayFixed = array)
                {
                    uint* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.UIntQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UIntQuickRangeSort
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly uint* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly uint* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal UIntQuickRangeSort(uint* skipCount, uint* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(uint* startIndex, uint* endIndex)
        {
            do
            {
                uint leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                uint* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                uint value = *averageIndex;
                if (leftValue.CompareFrom(value) < 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = leftValue;
                        if (value.CompareFrom(rightValue) < 0) *leftIndex = rightValue;
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
                    if (value.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = value;
                        if (leftValue.CompareFrom(rightValue) < 0)
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
                    while ((*leftIndex).CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom(*rightIndex) > 0) --rightIndex;
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
        internal static void QuickRangeSort(this SubArray<uint> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (uint* arrayFixed = array.GetFixedBuffer())
                {
                    uint* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.UIntQuickRangeSort(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSort(this LeftArray<uint> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (uint* arrayFixed = array.GetFixedBuffer())
                {
                    uint* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.UIntQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSort(this uint[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (uint* arrayFixed = array)
                {
                    uint* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.UIntQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IntQuickRangeSortDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly int* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly int* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal IntQuickRangeSortDesc(int* skipCount, int* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(int* startIndex, int* endIndex)
        {
            do
            {
                int leftValue = *startIndex, rightValue = *endIndex;
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
                int* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                int value = *averageIndex;
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
        internal static void QuickRangeSortDesc(this SubArray<int> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (int* arrayFixed = array.GetFixedBuffer())
                {
                    int* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.IntQuickRangeSortDesc(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSortDesc(this LeftArray<int> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (int* arrayFixed = array.GetFixedBuffer())
                {
                    int* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.IntQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSortDesc(this int[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (int* arrayFixed = array)
                {
                    int* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.IntQuickRangeSortDesc(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序（一般用于获取分页）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IntQuickRangeSort
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        private readonly int* skipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        private readonly int* getEndIndex;
        /// <summary>
        /// 范围排序（一般用于获取分页）
        /// </summary>
        /// <param name="skipCount">跳过数据指针</param>
        /// <param name="getEndIndex">最后一条记录指针-1</param>
        internal IntQuickRangeSort(int* skipCount, int* getEndIndex)
        {
            this.skipCount = skipCount;
            this.getEndIndex = getEndIndex;
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="startIndex">起始指针</param>
        /// <param name="endIndex">结束指针-1</param>
        internal void Sort(int* startIndex, int* endIndex)
        {
            do
            {
                int leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                int* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                int value = *averageIndex;
                if (leftValue.CompareFrom(value) < 0)
                {
                    if (leftValue.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = leftValue;
                        if (value.CompareFrom(rightValue) < 0) *leftIndex = rightValue;
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
                    if (value.CompareFrom(rightValue) < 0)
                    {
                        *rightIndex = value;
                        if (leftValue.CompareFrom(rightValue) < 0)
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
                    while ((*leftIndex).CompareFrom(value) > 0) ++leftIndex;
                    while (value.CompareFrom(*rightIndex) > 0) --rightIndex;
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
        internal static void QuickRangeSort(this SubArray<int> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (int* arrayFixed = array.GetFixedBuffer())
                {
                    int* start = arrayFixed + array.Start, skip = start + skipCount;
                    new AutoCSer.Algorithm.IntQuickRangeSort(skip, skip + (count - 1)).Sort(start, start + (array.Length - 1));
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
        internal static void QuickRangeSort(this LeftArray<int> array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (int* arrayFixed = array.GetFixedBuffer())
                {
                    int* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.IntQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
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
        internal static void QuickRangeSort(this int[] array, int skipCount, int count)
        {
            if (count != 1 || array.Length != 1 || skipCount != 0)
            {
                fixed (int* arrayFixed = array)
                {
                    int* skip = arrayFixed + skipCount;
                    new AutoCSer.Algorithm.IntQuickRangeSort(skip, skip + (count - 1)).Sort(arrayFixed, arrayFixed + (array.Length - 1));
                }
            }
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
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this LeftArray<uint> array)
        {
            fixed (uint* arrayFixed = array.GetFixedBuffer()) AutoCSer.Algorithm.UnsafeQuickSort.SortUInt((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static unsafe partial class ArraySort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this uint[] array)
        {
            fixed (uint* arrayFixed = array) AutoCSer.Algorithm.UnsafeQuickSort.SortUInt((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 指针快速排序
    /// </summary>
    internal static partial class UnsafeQuickSort
    {
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置 - sizeof(uint)</param>
        internal unsafe static void SortUInt(byte* startIndex, byte* endIndex)
        {
            do
            {
                long distance = endIndex - startIndex;
                if (distance == sizeof(uint))
                {
                    if (*(uint*)endIndex < *(uint*)startIndex)
                    {
                        uint startValue = *(uint*)startIndex;
                        *(uint*)startIndex = *(uint*)endIndex;
                        *(uint*)endIndex = startValue;
                    }
                    break;
                }

                byte* averageIndex = startIndex + ((distance / (sizeof(uint) * 2)) * sizeof(uint));
                uint value = *(uint*)averageIndex, swapValue = *(uint*)endIndex;
                if (value < *(uint*)startIndex)
                {
                    if (swapValue < *(uint*)startIndex)
                    {
                        *(uint*)endIndex = *(uint*)startIndex;
                        if (swapValue < value) *(uint*)startIndex = swapValue;
                        else
                        {
                            *(uint*)startIndex = value;
                            *(uint*)averageIndex = value = swapValue;
                        }
                    }
                    else
                    {
                        *(uint*)averageIndex = *(uint*)startIndex;
                        *(uint*)startIndex = value;
                        value = *(uint*)averageIndex;
                    }
                }
                else if (*(uint*)endIndex < value)
                {
                    *(uint*)endIndex = value;
                    if (swapValue < *(uint*)startIndex)
                    {
                        value = *(uint*)startIndex;
                        *(uint*)startIndex = swapValue;
                        *(uint*)averageIndex = value;
                    }
                    else
                    {
                        *(uint*)averageIndex = swapValue;
                        value = swapValue;
                    }
                }
                byte* leftIndex = startIndex + sizeof(uint), rightIndex = endIndex - sizeof(uint);
                do
                {
                    while (value > *(uint*)leftIndex) leftIndex += sizeof(uint);
                    while (*(uint*)rightIndex > value) rightIndex -= sizeof(uint);
                    if (leftIndex < rightIndex)
                    {
                        swapValue = *(uint*)leftIndex;
                        *(uint*)leftIndex = *(uint*)rightIndex;
                        *(uint*)rightIndex = swapValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            leftIndex += sizeof(uint);
                            rightIndex -= sizeof(uint);
                        }
                        break;
                    }
                }
                while ((leftIndex += sizeof(uint)) <= (rightIndex -= sizeof(uint)));
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) SortUInt(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) SortUInt(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序数组操作
    /// </summary>
    internal static partial class UnsafeSortArray
    {
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="count">查找数据数量，大于 0</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值之前的位置</returns>
        internal unsafe static int BinaryIndexOfLess(long* start, int count, long value)
        {
            int startIndex = 0, average;
            if (*start <= start[count - 1])
            {
                do
                {
                    if (value > start[average = startIndex + ((count - startIndex) >> 1)]) startIndex = average + 1;
                    else count = average;
                }
                while (startIndex != count);
            }
            else
            {
                do
                {
                    if (value < start[average = startIndex + ((count - startIndex) >> 1)]) startIndex = average + 1;
                    else count = average;
                }
                while (startIndex != count);
            }
            return startIndex;
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
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值之前的位置</returns>
        public static int BinaryIndexOfLess(this LeftArray<long> array, long value)
        {
            if(array.Length != 0)
            {
                fixed (long* arrayFixed = array.GetFixedBuffer()) return AutoCSer.Algorithm.UnsafeSortArray.BinaryIndexOfLess(arrayFixed, array.Count, value);
            }
            return 0;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int BinaryIndexOf(this LeftArray<long> array, long value)
        {
            int index = BinaryIndexOfLess(array, value);
            return index != array.Length && array.Array[index] == value ? index : -1;
        }
        /// <summary>
        /// 删除二分查找第一个匹配值并返回删除数据位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>删除数据位置，失败返回-1</returns>
        public static int BinaryRemove(this LeftArray<long> array, long value)
        {
            int index = BinaryIndexOfLess(array, value);
            if (index != array.Length && array.Array[index] == value)
            {
                array.RemoveAt(index);
                return index;
            }
            return -1;
        }
        /// <summary>
        /// 二分查找添加新数据并返回添加新数据位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">添加新数据</param>
        /// <returns>添加新数据位置，失败返回-1</returns>
        public static int BinaryInsertNew(this LeftArray<long> array, long value)
        {
            int index = BinaryIndexOfLess(array, value);
            if (index == array.Length || array.Array[index] != value)
            {
                array.Insert(index, value);
                return index;
            }
            return -1;
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序数组操作
    /// </summary>
    internal static partial class UnsafeSortArray
    {
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="count">查找数据数量，大于 0</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值之前的位置</returns>
        internal unsafe static int BinaryIndexOfLess(uint* start, int count, uint value)
        {
            int startIndex = 0, average;
            if (*start <= start[count - 1])
            {
                do
                {
                    if (value > start[average = startIndex + ((count - startIndex) >> 1)]) startIndex = average + 1;
                    else count = average;
                }
                while (startIndex != count);
            }
            else
            {
                do
                {
                    if (value < start[average = startIndex + ((count - startIndex) >> 1)]) startIndex = average + 1;
                    else count = average;
                }
                while (startIndex != count);
            }
            return startIndex;
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
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值之前的位置</returns>
        public static int BinaryIndexOfLess(this LeftArray<uint> array, uint value)
        {
            if(array.Length != 0)
            {
                fixed (uint* arrayFixed = array.GetFixedBuffer()) return AutoCSer.Algorithm.UnsafeSortArray.BinaryIndexOfLess(arrayFixed, array.Count, value);
            }
            return 0;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int BinaryIndexOf(this LeftArray<uint> array, uint value)
        {
            int index = BinaryIndexOfLess(array, value);
            return index != array.Length && array.Array[index] == value ? index : -1;
        }
        /// <summary>
        /// 删除二分查找第一个匹配值并返回删除数据位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>删除数据位置，失败返回-1</returns>
        public static int BinaryRemove(this LeftArray<uint> array, uint value)
        {
            int index = BinaryIndexOfLess(array, value);
            if (index != array.Length && array.Array[index] == value)
            {
                array.RemoveAt(index);
                return index;
            }
            return -1;
        }
        /// <summary>
        /// 二分查找添加新数据并返回添加新数据位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">添加新数据</param>
        /// <returns>添加新数据位置，失败返回-1</returns>
        public static int BinaryInsertNew(this LeftArray<uint> array, uint value)
        {
            int index = BinaryIndexOfLess(array, value);
            if (index == array.Length || array.Array[index] != value)
            {
                array.Insert(index, value);
                return index;
            }
            return -1;
        }
    }
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序数组操作
    /// </summary>
    internal static partial class UnsafeSortArray
    {
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="count">查找数据数量，大于 0</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值之前的位置</returns>
        internal unsafe static int BinaryIndexOfLess(int* start, int count, int value)
        {
            int startIndex = 0, average;
            if (*start <= start[count - 1])
            {
                do
                {
                    if (value > start[average = startIndex + ((count - startIndex) >> 1)]) startIndex = average + 1;
                    else count = average;
                }
                while (startIndex != count);
            }
            else
            {
                do
                {
                    if (value < start[average = startIndex + ((count - startIndex) >> 1)]) startIndex = average + 1;
                    else count = average;
                }
                while (startIndex != count);
            }
            return startIndex;
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
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值之前的位置</returns>
        public static int BinaryIndexOfLess(this LeftArray<int> array, int value)
        {
            if(array.Length != 0)
            {
                fixed (int* arrayFixed = array.GetFixedBuffer()) return AutoCSer.Algorithm.UnsafeSortArray.BinaryIndexOfLess(arrayFixed, array.Count, value);
            }
            return 0;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int BinaryIndexOf(this LeftArray<int> array, int value)
        {
            int index = BinaryIndexOfLess(array, value);
            return index != array.Length && array.Array[index] == value ? index : -1;
        }
        /// <summary>
        /// 删除二分查找第一个匹配值并返回删除数据位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">匹配值</param>
        /// <returns>删除数据位置，失败返回-1</returns>
        public static int BinaryRemove(this LeftArray<int> array, int value)
        {
            int index = BinaryIndexOfLess(array, value);
            if (index != array.Length && array.Array[index] == value)
            {
                array.RemoveAt(index);
                return index;
            }
            return -1;
        }
        /// <summary>
        /// 二分查找添加新数据并返回添加新数据位置
        /// </summary>
        /// <param name="array">数组处于已排序状态</param>
        /// <param name="value">添加新数据</param>
        /// <returns>添加新数据位置，失败返回-1</returns>
        public static int BinaryInsertNew(this LeftArray<int> array, int value)
        {
            int index = BinaryIndexOfLess(array, value);
            if (index == array.Length || array.Array[index] != value)
            {
                array.Insert(index, value);
                return index;
            }
            return -1;
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref long value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref long? value)
        {
            if (IsValue() == 0) value = default(long);
            else
            {
                long newValue = default(long);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref long? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref uint value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref uint? value)
        {
            if (IsValue() == 0) value = default(uint);
            else
            {
                uint newValue = default(uint);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref uint? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref int value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref int? value)
        {
            if (IsValue() == 0) value = default(int);
            else
            {
                int newValue = default(int);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref int? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref ushort value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref ushort? value)
        {
            if (IsValue() == 0) value = default(ushort);
            else
            {
                ushort newValue = default(ushort);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref ushort? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref short value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref short? value)
        {
            if (IsValue() == 0) value = default(short);
            else
            {
                short newValue = default(short);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref short? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref byte value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref byte? value)
        {
            if (IsValue() == 0) value = default(byte);
            else
            {
                byte newValue = default(byte);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref byte? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref sbyte value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref sbyte? value)
        {
            if (IsValue() == 0) value = default(sbyte);
            else
            {
                sbyte newValue = default(sbyte);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref sbyte? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref bool value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref bool? value)
        {
            if (IsValue() == 0) value = default(bool);
            else
            {
                bool newValue = default(bool);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref bool? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref float value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref float? value)
        {
            if (IsValue() == 0) value = default(float);
            else
            {
                float newValue = default(float);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref float? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref double value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref double? value)
        {
            if (IsValue() == 0) value = default(double);
            else
            {
                double newValue = default(double);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref double? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref decimal value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref decimal? value)
        {
            if (IsValue() == 0) value = default(decimal);
            else
            {
                decimal newValue = default(decimal);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref decimal? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref char value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref char? value)
        {
            if (IsValue() == 0) value = default(char);
            else
            {
                char newValue = default(char);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref char? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref DateTime value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref DateTime? value)
        {
            if (IsValue() == 0) value = default(DateTime);
            else
            {
                DateTime newValue = default(DateTime);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref DateTime? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref TimeSpan value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref TimeSpan? value)
        {
            if (IsValue() == 0) value = default(TimeSpan);
            else
            {
                TimeSpan newValue = default(TimeSpan);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref TimeSpan? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref Guid value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
        /// <summary>
        /// 逻辑值解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref Guid? value)
        {
            if (IsValue() == 0) value = default(Guid);
            else
            {
                Guid newValue = default(Guid);
                PrimitiveDeserialize(ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(XmlDeserializer serializer, ref Guid? value)
        {
            serializer.PrimitiveDeserialize(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref DateTime value)
        {
            getValue();
            if (State == AutoCSer.Xml.DeserializeStateEnum.Success)
            {
                if (valueSize != 0)
                {
                    if (XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = AutoCSer.Xml.DeserializeStateEnum.NotDateTime;
            }
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref TimeSpan value)
        {
            getValue();
            if (State == AutoCSer.Xml.DeserializeStateEnum.Success)
            {
                if (valueSize != 0)
                {
                    if (XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = AutoCSer.Xml.DeserializeStateEnum.NotTimeSpan;
            }
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumLongDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberSigned())
            {
                long intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        long intValue = enumInts.Long[index];
                        intValue |= enumInts.Long[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.Long[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, long>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumLongDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(long), false);
            long* data = enumInts.Long;
            foreach (T value in enumValues) *(long*)data++ = AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUIntDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberUnsigned())
            {
                uint intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        uint intValue = enumInts.UInt[index];
                        intValue |= enumInts.UInt[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.UInt[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, uint>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumUIntDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(uint), false);
            uint* data = enumInts.UInt;
            foreach (T value in enumValues) *(uint*)data++ = AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumIntDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberSigned())
            {
                int intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        int intValue = enumInts.Int[index];
                        intValue |= enumInts.Int[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.Int[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, int>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumIntDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(int), false);
            int* data = enumInts.Int;
            foreach (T value in enumValues) *(int*)data++ = AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumUShortDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberUnsigned())
            {
                ushort intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ushort intValue = enumInts.UShort[index];
                        intValue |= enumInts.UShort[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.UShort[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumUShortDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(ushort), false);
            ushort* data = enumInts.UShort;
            foreach (T value in enumValues) *(ushort*)data++ = AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumShortDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberSigned())
            {
                short intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        short intValue = enumInts.Short[index];
                        intValue |= enumInts.Short[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.Short[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumShortDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(short), false);
            short* data = enumInts.Short;
            foreach (T value in enumValues) *(short*)data++ = AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumByteDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberUnsigned())
            {
                byte intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        byte intValue = enumInts.Byte[index];
                        intValue |= enumInts.Byte[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.Byte[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumByteDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(byte), false);
            byte* data = enumInts.Byte;
            foreach (T value in enumValues) *(byte*)data++ = AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value);
        }
    }
}

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumSByteDeserialize<T> : EnumDeserialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeserializeNumber(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsEnumNumberSigned())
            {
                sbyte intValue = 0;
                deserializer.DeserializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
            }
            else if (deserializer.State == DeserializeStateEnum.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Deserialize(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value)) deserialize(deserializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeserializeFlags(XmlDeserializer deserializer, ref T value)
        {
            if (!tryDeserializeNumber(deserializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (deserializer.Config.IsMatchEnum) deserializer.State = DeserializeStateEnum.NoFoundEnumValue;
                    else deserializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(deserializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        sbyte intValue = enumInts.SByte[index];
                        intValue |= enumInts.SByte[nextIndex];
                        while (deserializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(deserializer)) != -1) intValue |= enumInts.SByte[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt(intValue);
                    }
                }
            }
        }
        static EnumSByteDeserialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(sbyte), false);
            sbyte* data = enumInts.SByte;
            foreach (T value in enumValues) *(sbyte*)data++ = AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref long value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref uint value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref int value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref ushort value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref short value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref byte value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void PrimitiveDeserialize(ref sbyte value)
        {
            searchValue();
            DeserializeNumber(ref value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, float value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, float? value)
        {
            if (value.HasValue) serializer.PrimitiveSerialize(value.Value);
        }
    }
}

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, double value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, double? value)
        {
            if (value.HasValue) serializer.PrimitiveSerialize(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(TimeSpan value)
        {
            int size = CustomConfig.Write(this, value);
            if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, TimeSpan value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, TimeSpan? value)
        {
            if (value.HasValue) serializer.PrimitiveSerialize(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumLong<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumLong<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumLong(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumUInt<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumUInt<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumUInt(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumInt<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumInt<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumInt(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumUShort<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumUShort<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumUShort(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumShort<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumShort<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumShort(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumByte<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumByte<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumByte(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void enumSByte<T>(T value) where T : struct, IConvertible
        {
            if (!Config.IsEnumToString) PrimitiveSerialize(AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value));
            else primitiveSerializeNotEmpty(AutoCSer.Extensions.NullableReferenceExtension.notNull(value.ToString()));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void EnumSByte<T>(XmlSerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.enumSByte(value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(long value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, long value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, long? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(uint value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, uint value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, uint? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(int value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, int value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, int? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(ushort value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, ushort value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, ushort? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(short value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, short value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, short? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(byte value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, byte value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, byte? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void PrimitiveSerialize(sbyte value)
        {
            CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, sbyte value)
        {
            serializer.CharStream.WriteString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, sbyte? value)
        {
            if (value.HasValue) serializer.CharStream.WriteString(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, bool value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, bool? value)
        {
            if (value.HasValue) serializer.PrimitiveSerialize(value.Value);
        }
    }
}

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public sealed unsafe partial class XmlSerializer
    {
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, Guid value)
        {
            serializer.PrimitiveSerialize(value);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, Guid? value)
        {
            if (value.HasValue) serializer.PrimitiveSerialize(value.Value);
        }
    }
}

#endif