using System;

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
        /// <param name="array">长度大于 1</param>
        internal static void QuickSort(this int[] array)
        {
            fixed (int* arrayFixed = array) AutoCSer.Algorithm.QuickSort.SortInt((byte*)arrayFixed, (byte*)(arrayFixed + (array.Length - 1)));
        }
    }
}
