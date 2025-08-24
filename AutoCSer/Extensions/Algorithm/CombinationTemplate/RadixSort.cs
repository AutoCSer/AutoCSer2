using System;
/*ulong,ULong,SortSize64;long,Long,SortSize64;uint,UInt,SortSize32;int,Int,SortSize32*/

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
    internal static unsafe partial class ArraySort
    {
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Sort(this ulong[] array)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length);
            else if (array.Length > 1) QuickSort(array);
        }
    }
    /// <summary>
    /// ulong array expansion operation
    /// ulong 数组扩展操作
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct ULongArrayExtensions
    {
        /// <summary>
        /// ulong array
        /// </summary>
        private readonly ulong[] array;
        /// <summary>
        /// ulong array expansion operation
        /// ulong 数组扩展操作
        /// </summary>
        /// <param name="array"></param>
        public ULongArrayExtensions(ulong[] array)
        {
            this.array = array;
        }
        /// <summary>
        /// ulong array sorting
        /// ulong 数组排序
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort()
        {
            array.Sort();
        }
        /// <summary>
        /// 随机排序
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RandomSort()
        {
            array.RandomSort();
        }
    }
}
