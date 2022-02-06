using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 查找数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FindArray<T>
    {
        /// <summary>
        /// 数据匹配委托
        /// </summary>
        internal Func<T, bool> IsValue;
        /// <summary>
        /// 数据集合
        /// </summary>
        internal LeftArray<T> Array;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(T value)
        {
            if (IsValue(value)) Array.Add(value);
        }
    }
}
