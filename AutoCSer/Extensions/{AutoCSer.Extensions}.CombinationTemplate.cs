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
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this LeftArray<long> array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort(array.GetFixedBuffer(), 0, array.Length);
            else if(array.Length > 1) QuickSort(array);
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
}

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ULongQuickRangeSorter
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal ulong* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal ulong* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LongQuickRangeSorterDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal long* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal long* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LongQuickRangeSorter
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal long* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal long* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UIntQuickRangeSorterDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal uint* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal uint* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UIntQuickRangeSorter
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal uint* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal uint* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IntQuickRangeSorterDesc
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal int* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal int* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct IntQuickRangeSorter
    {
        /// <summary>
        /// 跳过数据指针
        /// </summary>
        internal int* SkipCount;
        /// <summary>
        /// 最后一条记录指针-1
        /// </summary>
        internal int* GetEndIndex;
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
                    if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                    if (leftIndex > GetEndIndex) break;
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                    if (rightIndex < SkipCount) break;
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
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal partial struct LongSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal long Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(long))]
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
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal partial struct UIntSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal uint Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(uint))]
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
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal partial struct IntSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal int Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int))]
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
        /// <param name="endIndex">结束位置 - sizeof(long)</param>
        internal unsafe static void SortLong(byte* startIndex, byte* endIndex)
        {
            do
            {
                long distance = endIndex - startIndex;
                if (distance == sizeof(long))
                {
                    if (*(long*)endIndex < *(long*)startIndex)
                    {
                        long startValue = *(long*)startIndex;
                        *(long*)startIndex = *(long*)endIndex;
                        *(long*)endIndex = startValue;
                    }
                    break;
                }

                byte* averageIndex = startIndex + ((distance / (sizeof(long) * 2)) * sizeof(long));
                long value = *(long*)averageIndex, swapValue = *(long*)endIndex;
                if (value < *(long*)startIndex)
                {
                    if (swapValue < *(long*)startIndex)
                    {
                        *(long*)endIndex = *(long*)startIndex;
                        if (swapValue < value) *(long*)startIndex = swapValue;
                        else
                        {
                            *(long*)startIndex = value;
                            *(long*)averageIndex = value = swapValue;
                        }
                    }
                    else
                    {
                        *(long*)averageIndex = *(long*)startIndex;
                        *(long*)startIndex = value;
                        value = *(long*)averageIndex;
                    }
                }
                else if (*(long*)endIndex < value)
                {
                    *(long*)endIndex = value;
                    if (swapValue < *(long*)startIndex)
                    {
                        value = *(long*)startIndex;
                        *(long*)startIndex = swapValue;
                        *(long*)averageIndex = value;
                    }
                    else
                    {
                        *(long*)averageIndex = swapValue;
                        value = swapValue;
                    }
                }
                byte* leftIndex = startIndex + sizeof(long), rightIndex = endIndex - sizeof(long);
                do
                {
                    while (value > *(long*)leftIndex) leftIndex += sizeof(long);
                    while (*(long*)rightIndex > value) rightIndex -= sizeof(long);
                    if (leftIndex < rightIndex)
                    {
                        swapValue = *(long*)leftIndex;
                        *(long*)leftIndex = *(long*)rightIndex;
                        *(long*)rightIndex = swapValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            leftIndex += sizeof(long);
                            rightIndex -= sizeof(long);
                        }
                        break;
                    }
                }
                while ((leftIndex += sizeof(long)) <= (rightIndex -= sizeof(long)));
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) SortLong(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) SortLong(leftIndex, endIndex);
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
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this LeftArray<long> array)
        {
            fixed (long* arrayFixed = array.GetFixedBuffer()) AutoCSer.Algorithm.UnsafeQuickSort.SortLong((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
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
        public void PrimitiveSerialize(float value)
        {
            int size = CustomConfig.Write(CharStream, value);
            if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
        }
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
        public void PrimitiveSerialize(double value)
        {
            int size = CustomConfig.Write(CharStream, value);
            if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
        }
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            else primitiveSerializeNotEmpty(value.ToString());
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, long value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, long? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, uint value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, uint? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, int value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, int? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, ushort value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, ushort? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, short value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, short? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, byte value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, byte? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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
            AutoCSer.Extensions.NumberExtension.ToString(value, CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, sbyte value)
        {
            AutoCSer.Extensions.NumberExtension.ToString(value, serializer.CharStream);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value">数字</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(XmlSerializer serializer, sbyte? value)
        {
            if (value.HasValue) AutoCSer.Extensions.NumberExtension.ToString(value.Value, serializer.CharStream);
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