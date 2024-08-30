using System;
/*ulong;long;uint;int*/

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
        internal unsafe static int BinaryIndexOfLess(ulong* start, int count, ulong value)
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
        public static int BinaryIndexOfLess(this LeftArray<ulong> array, ulong value)
        {
            if(array.Length != 0)
            {
                fixed (ulong* arrayFixed = array.GetFixedBuffer()) return AutoCSer.Algorithm.UnsafeSortArray.BinaryIndexOfLess(arrayFixed, array.Count, value);
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
        public static int BinaryIndexOf(this LeftArray<ulong> array, ulong value)
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
        public static int BinaryRemove(this LeftArray<ulong> array, ulong value)
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
        public static int BinaryInsertNew(this LeftArray<ulong> array, ulong value)
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
