using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 单向动态数组
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class ListArray<T> : IList<T>
    {
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        internal LeftArray<T> Array;
        /// <summary>
        /// 长度
        /// </summary>
        public int Count
        {
            get { return Array.Length; }
        }
        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get { return Array.IsReadOnly; } }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>Data value</returns>
        public T this[int index]
        {
            get
            {
                return Array[index];
            }
            set
            {
                Array[index] = value;
            }
        }
        /// <summary>
        /// 单向动态数组
        /// </summary>
        public ListArray() : this(0) { }
        /// <summary>
        /// 单向动态数组
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public ListArray(int capacity)
        {
            Array = new LeftArray<T>(capacity);
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="array">数据数组</param>
        internal ListArray(T[] array)
        {
            Array = new LeftArray<T>(array);
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="array">数据数组</param>
        internal ListArray(LeftArray<T> array)
        {
            Array = array;
        }
        /// <summary>
        /// 单向动态数据
        /// </summary>
        /// <param name="length">Initialize the data length
        /// 初始化数据长度</param>
        /// <param name="array">Original array
        /// 原数组</param>
        internal ListArray(int length, T[] array)
        {
            Array = new LeftArray<T>(length, array);
        }
        /// <summary>
        /// Get the enumerator
        /// 获取枚举器
        /// </summary>
        /// <returns>Enumerator
        /// 枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Array.Length != 0) return new Enumerator<T>.Array(Array.Array, 0, Array.Length);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// Get the enumerator
        /// 获取枚举器
        /// </summary>
        /// <returns>Enumerator
        /// 枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Array.Length != 0) return new Enumerator<T>.Array(Array.Array, 0, Array.Length);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear();
        }
        ///// <summary>
        ///// Empty and release the array
        ///// 置空并释放数组
        ///// </summary>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public void SetEmpty()
        //{
        //    Array.SetEmpty();
        //}
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            Array.Add(value);
        }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array">Data collection
        /// 数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T[] array)
        {
            Array.Add(array);
        }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array">Data collection
        /// 数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Append(params T[] array)
        {
            Array.Add(array);
        }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array">Data collection
        /// 数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(ListArray<T> array)
        {
            Array.Add(ref array.Array);
        }
        /// <summary>
        /// Insert data
        /// 插入数据
        /// </summary>
        /// <param name="index">Insert position
        /// 插入位置</param>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T value)
        {
            Array.Insert(index, value);
        }
        /// <summary>
        /// Determine whether there is data
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns>Returning false indicates that there is no matching data
        /// 返回 false 表示不存在匹配数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            return Array.Contains(value);
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="destinationArray">Target array
        /// 目标数组</param>
        /// <param name="index">Target starting position
        /// 目标起始位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] destinationArray, int index)
        {
            Array.CopyTo(destinationArray, index);
        }
        /// <summary>
        /// Reverse the array
        /// 逆转列表
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            Array.Reverse();
        }
        /// <summary>
        /// Get the matching data location
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns>Returning -1 indicates a matching failure
        /// 返回 -1 表示匹配失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T value)
        {
            return Array.IndexOf(value);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Returning false indicates that there is no data match
        /// 返回 false 表示不存在数据匹配</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(T value)
        {
            return Array.Remove(value);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        /// <returns>被移除数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            Array.RemoveAt(index);
        }
        /// <summary>
        /// Array data sorting
        /// 数组数据排序
        /// </summary>
        /// <param name="comparer">Data sorting comparator
        /// 数据排序比较器</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, T, int> comparer)
        {
            Array.Sort(comparer);
        }
        /// <summary>
        /// Connect string
        /// 连接字符串
        /// </summary>
        /// <param name="toString">The delegate that gets the string
        /// 获取字符串的委托</param>
        /// <param name="join">String concatenation symbol
        /// 字符串连接符号</param>
        /// <returns>The string generated by the concatenation operation
        /// 连接操作产生的字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string JoinString(string join, Func<T, string> toString)
        {
            return Array.JoinString(join, toString);
        }
    }
}
