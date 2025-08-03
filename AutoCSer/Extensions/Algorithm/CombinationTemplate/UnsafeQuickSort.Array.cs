using System;
/*ulong,ULong;uint,UInt*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class LeftArraySort
    {
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this LeftArray<ulong> array)
        {
            fixed (ulong* arrayFixed = array.GetFixedBuffer()) AutoCSer.Algorithm.UnsafeQuickSort.SortULong((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
    /// <summary>
    /// 数组扩展
    /// </summary>
    internal static unsafe partial class ArraySort
    {
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this ulong[] array)
        {
            fixed (ulong* arrayFixed = array) AutoCSer.Algorithm.UnsafeQuickSort.SortULong((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
}
