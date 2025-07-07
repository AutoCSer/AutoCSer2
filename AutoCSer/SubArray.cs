using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer
{
    /// <summary>
    /// Array substring
    /// 数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SubArray<T>// : IList<T>
    {
        /// <summary>
        /// Original array
        /// 原数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// The starting position in the original array
        /// 原数组中的起始位置
        /// </summary>
        internal int Start;
        /// <summary>
        /// Effective data length
        /// 有效数据长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// Data end position
        /// 数据结束位置
        /// </summary>
        internal int EndIndex
        {
            get
            {
                return Start + Length;
            }
        }
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        /// <param name="array"></param>
        internal SubArray(T[] array)
        {
            Array = array;
            Start = 0;
            Length = array != null ? array.Length : 0;
        }
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="array"></param>
        internal SubArray(int startIndex, int length, T[] array)
        {
            Array = array;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// Used for the command service to return the error type
        /// 用于命令服务返回错误类型
        /// </summary>
        /// <param name="startIndex"></param>
        internal SubArray(int startIndex)
        {
            Start = startIndex;
            Length = int.MinValue;
            Array = EmptyArray<T>.Array;
        }
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        /// <param name="array">Original array
        /// 原数组</param>
        /// <param name="startIndex">The starting position in the original array
        /// 原数组中的起始位置</param>
        /// <param name="length">Effective data length
        /// 有效数据长度</param>
        public SubArray(T[] array, int startIndex, int length)
        {
            if (array == null) throw new ArgumentNullException();
            if ((uint)startIndex >= (uint)array.Length || length < 0 || startIndex + length > array.Length) throw new IndexOutOfRangeException();
            Array = array;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="array"></param>
        public static implicit operator SubArray<T>(T[] array) { return new SubArray<T>(array); }
        /// <summary>
        /// Get the original array object
        /// 获取原始数组对象
        /// </summary>
        /// <param name="startIndex">The starting position in the original array
        /// 原数组中的起始位置</param>
        /// <param name="length">Effective data length
        /// 有效数据长度</param>
        /// <returns>Original array
        /// 原数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] GetArray(out int startIndex, out int length)
        {
            startIndex = Start;
            length = Length;
            return Array;
        }
        /// <summary>
        /// Empty and release the array
        /// 置空并释放数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetEmpty()
        {
            Array = EmptyArray<T>.Array;
            Length = Start = 0;
        }
        /// <summary>
        /// Reset the array data
        /// 重置数组数据
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Set(T[] array)
        {
            Array = array;
            Start = 0;
            Length = array.Length;
        }
        /// <summary>
        /// Reset the array position data
        /// 重置数组位置数据
        /// </summary>
        /// <param name="startIndex">The starting position in the original array
        /// 原数组中的起始位置</param>
        /// <param name="length">Effective data length
        /// 有效数据长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int startIndex, int length)
        {
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// Reset the array data
        /// 重置数组数据
        /// </summary>
        /// <param name="array">Original array
        /// 原数组</param>
        /// <param name="startIndex">The starting position in the original array
        /// 原数组中的起始位置</param>
        /// <param name="length">Effective data length
        /// 有效数据长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T[] array, int startIndex, int length)
        {
            Array = array;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// Move the starting position
        /// 移动起始位置
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveStart(int size)
        {
            Start += size;
            Length -= size;
        }
        /// <summary>
        /// Return the length of the valid data after moving the starting position
        /// 移动起始位置后返回有效数据长度
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetMoveStartLength(int size)
        {
            Start += size;
            return Length -= size;
        }
        /// <summary>
        /// Create and convert to an array object
        /// 创建转换为数组对象
        /// </summary>
        /// <returns></returns>
        public T[] GetArray()
        {
            if (Length == 0) return EmptyArray<T>.Array;
            T[] newArray = new T[Length];
            System.Array.Copy(Array, Start, newArray, 0, Length);
            return newArray;
        }
        /// <summary>
        /// Create and convert to an array object
        /// 创建转换为数组对象
        /// </summary>
        /// <typeparam name="VT">Array data type
        /// 数组数据类型</typeparam>
        /// <param name="getValue">Delegate for converting data
        /// 转换数据委托</param>
        /// <returns></returns>
        public VT[] GetArray<VT>(Func<T, VT> getValue)
        {
            if (Length == 0) return EmptyArray<VT>.Array;
            VT[] newArray = new VT[Length];
            int count = 0, index = Start;
            do
            {
                newArray[count] = getValue(Array[index++]);
            }
            while (++count != Length);
            return newArray;
        }
        ///// <summary>
        ///// 比较是否一致
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal bool ReferenceEquals(ref SubArray<T> other)
        //{
        //    return ((Length ^ other.Length) | (Start ^ other.Start)) == 0 && object.ReferenceEquals(Array, other.Array);
        //}

        /// <summary>
        /// Get the fixed buffer, DEBUG mode to detect the data range
        /// 获取 fixed 缓冲区，DEBUG 模式对数据范围进行检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T[] GetFixedBuffer()
        {
#if DEBUG
            DebugCheckFixed();
#endif
            return Array;
        }
#if DEBUG
        /// <summary>
        /// Check the data before the fixed operation
        /// fixed 操作之前检查数据
        /// </summary>
        internal void DebugCheckFixed()
        {
            if (Start < 0) throw new Exception(Start.toString() + " < 0");
            if (Length < 0) throw new Exception(Length.toString() + " < 0");
            if (Start + Length > Array.Length) throw new Exception(Start.toString() + " + " + Length.toString() + " > " + Array.Length.toString());
        }
#endif
    }
}
