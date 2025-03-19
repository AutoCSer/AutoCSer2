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

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="array">排序数组</param>
        public static void RandomSort<T>(this T[] array)
        {
            int count = array.Length;
            if (count > 1)
            {
                int index;
                AutoCSer.Random random = AutoCSer.Random.Default;
                T value;
                while (count > 1)
                {
                    index = (int)((uint)random.Next() % (uint)count);
                    value = array[--count];
                    array[count] = array[index];
                    array[index] = value;
                }
            }
        }
    }
}
