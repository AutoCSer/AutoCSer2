using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// ICollection 泛型转换
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ToGenericCollection<T> : ICollection<T>
    {
        /// <summary>
        /// ICollection数据集合
        /// </summary>
        private readonly ICollection collection;
        /// <summary>
        /// ICollection泛型转换
        /// </summary>
        /// <param name="collection">ICollection数据集合</param>
        public ToGenericCollection(ICollection collection)
        {
            this.collection = collection;
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return collection != null ? collection.Count : 0; }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get { return true; } }
        /// <summary>
        /// Get the enumerator
        /// 获取枚举器
        /// </summary>
        /// <returns>Enumerator
        /// 枚举器</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (collection != null)
            {
                foreach (T value in collection) yield return value;
            }
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
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        public void Add(T value)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 移除数据(不可用)
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Returning false indicates that there is no data match
        /// 返回 false 表示不存在数据匹配</returns>
        public bool Remove(T value)
        {
            return false;
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="values">Target array
        /// 目标数组</param>
        /// <param name="index">Target starting position
        /// 目标起始位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] values, int index)
        {
            if (collection != null) collection.CopyTo(values, index);
        }
        /// <summary>
        /// Determine whether there is data
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns>Returning false indicates that there is no matching data
        /// 返回 false 表示不存在匹配数据</returns>
        public bool Contains(T value)
        {
            if (collection != null)
            {
                foreach (T nextValue in collection)
                {
                    if (nextValue.Equals(value)) return true;
                }
            }
            return false;
        }
    }
}
