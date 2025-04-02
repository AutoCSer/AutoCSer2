using System;
/*long,Long;int,Int*/

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
            fixed (long* arrayFixed = array.GetFixedBuffer()) AutoCSer.Algorithm.QuickSort.SortLong((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
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
        internal static void QuickSort(this long[] array)
        {
            fixed (long* arrayFixed = array) AutoCSer.Algorithm.QuickSort.SortLong((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
}
