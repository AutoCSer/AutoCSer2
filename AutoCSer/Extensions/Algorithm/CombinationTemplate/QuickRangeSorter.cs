using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 范围排序器(一般用于获取分页)
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ULongQuickRangeSorterDesc
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
