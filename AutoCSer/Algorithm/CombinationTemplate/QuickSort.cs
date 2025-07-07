using System;
/*long,Long;int,Int*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 指针快速排序
    /// </summary>
    internal static partial class QuickSort
    {
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="endIndex">结束位置 - sizeof(long)</param>
        internal unsafe static void SortLong(byte* startIndex, byte* endIndex)
        {
            do
            {
                System.Int64 distance = endIndex - startIndex;
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
