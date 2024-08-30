using System;

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
}
