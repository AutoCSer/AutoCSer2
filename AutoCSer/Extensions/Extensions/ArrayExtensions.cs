using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Array expansion operation
    /// 数组扩展操作
    /// </summary>
    internal static class ArrayExtensions
    {
        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        internal static void RandomSort<T>(this T[] array)
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
    /// <summary>
    /// Array expansion operation
    /// 数组扩展操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct ArrayExtensions<T>
    {
        /// <summary>
        /// Array
        /// </summary>
        private readonly T[] array;
        /// <summary>
        /// Array expansion operation
        /// 数组扩展操作
        /// </summary>
        /// <param name="array"></param>
        public ArrayExtensions(T[] array)
        {
            this.array = array;
        }
        /// <summary>
        /// 数组是否为空或者长度为0
        /// </summary>
        /// <returns>数组是否为空或者长度为0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty()
        {
            return array.isEmpty();
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <returns>复制后的新数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] Copy()
        {
            return array.copy();
        }
        /// <summary>
        /// 合并数组
        /// </summary>
        /// <param name="otherArray">追加的数组</param>
        /// <returns>合并后的数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] Concat(T[] otherArray)
        {
            return array.concat(otherArray);
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="isValue">Determine whether the data match
        /// 判断数据是否匹配</param>
        /// <returns>匹配集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LeftArray<T> GetFind(Func<T, bool> isValue)
        {
            return array.getFind(isValue);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparer">Data sorting comparator
        /// 数据排序比较器</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] Sort(Func<T, T, int> comparer)
        {
            return array.sort(comparer);
        }
        /// <summary>
        /// Connect string
        /// 连接字符串
        /// </summary>
        /// <param name="toString">The delegate that gets the string
        /// 获取字符串的委托</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string JoinString(char join, Func<T, string> toString)
        {
            return array.joinString(join, toString);
        }
        /// <summary>
        /// 随机排序
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RandomSort()
        {
            array.RandomSort();
        }
        /// <summary>
        /// Set the array elements to the default values
        /// 设置数组元素为默认值
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetDefault(int index)
        {
            array.setDefault(index);
        }
        /// <summary>
        /// Set the array elements as default values and return the original values
        /// 设置数组元素为默认值并返回原始值
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetSetDefault(int index)
        {
            return array.getSetDefault(index);
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public VT[] GetArray<VT>(Func<T, VT> getValue)
        {
            return array.getArray(getValue);
        }

        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, int> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length, getKey);
            else QuickSort(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, uint> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length, getKey);
            else QuickSort(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, long> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.KeySortSize64) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length, getKey);
            else QuickSort(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, ulong> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.KeySortSize64) AutoCSer.Algorithm.RadixSort.Sort(array, 0, array.Length, getKey);
            else QuickSort(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SortDesc(Func<T, int> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.SortDesc(array, 0, array.Length, getKey);
            else QuickSortDesc(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SortDesc(Func<T, uint> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize32) AutoCSer.Algorithm.RadixSort.SortDesc(array, 0, array.Length, getKey);
            else QuickSortDesc(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SortDesc(Func<T, long> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.KeySortSize64) AutoCSer.Algorithm.RadixSort.SortDesc(array, 0, array.Length, getKey);
            else QuickSortDesc(getKey);
        }
        /// <summary>
        /// Array sorting
        /// 数组排序
        /// </summary>
        /// <param name="getKey">排序键值获取器</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SortDesc(Func<T, ulong> getKey)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.KeySortSize64) AutoCSer.Algorithm.RadixSort.SortDesc(array, 0, array.Length, getKey);
            else QuickSortDesc(getKey);
        }
    }
}
