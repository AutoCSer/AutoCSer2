using System;
/*ulong,SortSize64;long,SortSize64;uint,SortSize32;int,SortSize32*/

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
        public static void Sort(this LeftArray<ulong> array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort(array.GetFixedBuffer(), 0, array.Length);
            else if(array.Length > 1) QuickSort(array);
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
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Sort(this ulong[] array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length);
            else if (array.Length > 1) QuickSort(array);
        }
    }
}
