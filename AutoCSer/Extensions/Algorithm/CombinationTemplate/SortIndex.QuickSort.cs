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
        /// <param name="startIndex">起始位置</param>
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
