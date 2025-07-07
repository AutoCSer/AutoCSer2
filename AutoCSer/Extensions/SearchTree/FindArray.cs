using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// Search for data
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
        /// The data collection
        /// 数据集合
        /// </summary>
        internal LeftArray<T> Array;
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(T value)
        {
            if (IsValue(value)) Array.Add(value);
        }
    }
}
