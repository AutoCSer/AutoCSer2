using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 单向动态数组
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ListArray<T> : IList<T>
    {
        /// <summary>
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
        /// <returns>数据值</returns>
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
        /// <param name="capacity">容器大小</param>
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
        /// <param name="length">初始化数据长度</param>
        /// <param name="array">原数组</param>
        internal ListArray(int length, T[] array)
        {
            Array = new LeftArray<T>(length, array);
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Array.Length != 0) return new Enumerator<T>.Array(Array.Array, 0, Array.Length);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Array.Length != 0) return new Enumerator<T>.Array(Array.Array, 0, Array.Length);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear();
        }
        ///// <summary>
        ///// 置空并释放数组
        ///// </summary>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public void SetEmpty()
        //{
        //    Array.SetEmpty();
        //}
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            Array.Add(value);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T[] array)
        {
            Array.Add(array);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Append(params T[] array)
        {
            Array.Add(array);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(ListArray<T> array)
        {
            Array.Add(ref array.Array);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T value)
        {
            Array.Insert(index, value);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            return Array.Contains(value);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="destinationArray">目标数据</param>
        /// <param name="index">目标位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] destinationArray, int index)
        {
            Array.CopyTo(destinationArray, index);
        }
        /// <summary>
        /// 逆转列表
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            Array.Reverse();
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T value)
        {
            return Array.IndexOf(value);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(T value)
        {
            return Array.Remove(value);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            Array.RemoveAt(index);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, T, int> comparer)
        {
            Array.Sort(comparer);
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string JoinString(string join, Func<T, string> toString)
        {
            return Array.JoinString(join, toString);
        }
    }
}
