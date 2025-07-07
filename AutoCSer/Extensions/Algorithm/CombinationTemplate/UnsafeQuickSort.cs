using System;
/*ulong,ULong;uint,UInt*/

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
        /// <param name="startIndex">Starting position
        /// 起始位置</param>
        /// <param name="endIndex">结束位置 - sizeof(ulong)</param>
        internal unsafe static void SortULong(byte* startIndex, byte* endIndex)
        {
            do
            {
                long distance = endIndex - startIndex;
                if (distance == sizeof(ulong))
                {
                    if (*(ulong*)endIndex < *(ulong*)startIndex)
                    {
                        ulong startValue = *(ulong*)startIndex;
                        *(ulong*)startIndex = *(ulong*)endIndex;
                        *(ulong*)endIndex = startValue;
                    }
                    break;
                }

                byte* averageIndex = startIndex + ((distance / (sizeof(ulong) * 2)) * sizeof(ulong));
                ulong value = *(ulong*)averageIndex, swapValue = *(ulong*)endIndex;
                if (value < *(ulong*)startIndex)
                {
                    if (swapValue < *(ulong*)startIndex)
                    {
                        *(ulong*)endIndex = *(ulong*)startIndex;
                        if (swapValue < value) *(ulong*)startIndex = swapValue;
                        else
                        {
                            *(ulong*)startIndex = value;
                            *(ulong*)averageIndex = value = swapValue;
                        }
                    }
                    else
                    {
                        *(ulong*)averageIndex = *(ulong*)startIndex;
                        *(ulong*)startIndex = value;
                        value = *(ulong*)averageIndex;
                    }
                }
                else if (*(ulong*)endIndex < value)
                {
                    *(ulong*)endIndex = value;
                    if (swapValue < *(ulong*)startIndex)
                    {
                        value = *(ulong*)startIndex;
                        *(ulong*)startIndex = swapValue;
                        *(ulong*)averageIndex = value;
                    }
                    else
                    {
                        *(ulong*)averageIndex = swapValue;
                        value = swapValue;
                    }
                }
                byte* leftIndex = startIndex + sizeof(ulong), rightIndex = endIndex - sizeof(ulong);
                do
                {
                    while (value > *(ulong*)leftIndex) leftIndex += sizeof(ulong);
                    while (*(ulong*)rightIndex > value) rightIndex -= sizeof(ulong);
                    if (leftIndex < rightIndex)
                    {
                        swapValue = *(ulong*)leftIndex;
                        *(ulong*)leftIndex = *(ulong*)rightIndex;
                        *(ulong*)rightIndex = swapValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            leftIndex += sizeof(ulong);
                            rightIndex -= sizeof(ulong);
                        }
                        break;
                    }
                }
                while ((leftIndex += sizeof(ulong)) <= (rightIndex -= sizeof(ulong)));
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) SortULong(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) SortULong(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
    }
}
