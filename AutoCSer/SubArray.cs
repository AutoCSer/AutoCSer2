using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SubArray<T>// : IList<T>
    {
        /// <summary>
        /// 原数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// 原数组中的起始位置
        /// </summary>
        internal int Start;
        /// <summary>
        /// 数据长度
        /// </summary>
        internal int Length;
        /// <summary>
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
        /// 用于命令服务返回类型
        /// </summary>
        /// <param name="startIndex"></param>
        internal SubArray(int startIndex)
        {
            Start = startIndex;
            Length = int.MinValue;
            Array = EmptyArray<T>.Array;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array">原数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">数据数量</param>
        public SubArray(T[] array, int startIndex, int length)
        {
            if (array == null) throw new ArgumentNullException();
            if ((uint)startIndex >= (uint)array.Length || length < 0 || startIndex + length > array.Length) throw new IndexOutOfRangeException();
            Array = array;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="array"></param>
        public static implicit operator SubArray<T>(T[] array) { return new SubArray<T>(array); }
        /// <summary>
        /// 获取数组子串原始数组数据
        /// </summary>
        /// <param name="dataIndex">原数组中的起始位置</param>
        /// <param name="dataSize">数据长度</param>
        /// <returns>原数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] GetArray(out int dataIndex, out int dataSize)
        {
            dataIndex = Start;
            dataSize = Length;
            return Array;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetEmpty()
        {
            Array = EmptyArray<T>.Array;
            Length = Start = 0;
        }
        /// <summary>
        /// 重置数据
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
        /// 重置数据
        /// </summary>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int startIndex, int length)
        {
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="array">数组,不能为null</param>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T[] array, int startIndex, int length)
        {
            Array = array;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 修改起始位置
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void MoveStart(int size)
        {
            Start += size;
            Length -= size;
        }
        /// <summary>
        /// 修改起始位置并返回数据长度
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
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        public T[] GetArray()
        {
            if (Length == 0) return EmptyArray<T>.Array;
            T[] newArray = new T[Length];
            System.Array.Copy(Array, Start, newArray, 0, Length);
            return newArray;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="VT">数组类型</typeparam>
        /// <param name="getValue">数据获取委托</param>
        /// <returns>数组</returns>
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
        /// fixed 之前检查数据
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
