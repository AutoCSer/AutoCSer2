using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 分页数组数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PageArray<T>
    {
        /// <summary>
        /// 跳过数据
        /// </summary>
        internal int SkipCount;
        /// <summary>
        /// 数组位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// 数组写入是否完成
        /// </summary>
        internal bool IsArray
        {
            get { return Index == Array.Length; }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组写入是否完成</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Add(T value)
        {
            Array[Index++] = value;
            return IsArray;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int AddDesc(T value)
        {
            Array[--Index] = value;
            return Index;
        }
    }
    /// <summary>
    /// 分页数组数据
    /// </summary>
    /// <typeparam name="VT"></typeparam>
    /// <typeparam name="AT"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PageArray<VT, AT>
    {
        /// <summary>
        /// 跳过数据
        /// </summary>
        internal int SkipCount;
        /// <summary>
        /// 数组位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 数组
        /// </summary>
        internal AT[] Array;
        /// <summary>
        /// 获取数据
        /// </summary>
        internal Func<VT, AT> GetValue;
        /// <summary>
        /// 数组写入是否完成
        /// </summary>
        internal bool IsArray
        {
            get { return Index == Array.Length; }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组写入是否完成</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Add(VT value)
        {
            Array[Index++] = GetValue(value);
            return IsArray;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数组位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int AddDesc(VT value)
        {
            Array[--Index] = GetValue(value);
            return Index;
        }
    }
}
