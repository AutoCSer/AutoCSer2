using AutoCSer.Algorithm;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 可重用哈希节点
    /// </summary>
    /// <typeparam name="T">节点数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReusableHashNode<T>
    {
        /// <summary>
        /// 哈希索引
        /// </summary>
        internal int HashIndex;
        /// <summary>
        /// 节点来源，最高位为 0 表示首节点，否则表示后续节点
        /// </summary>
        internal uint Source;
        /// <summary>
        /// 0 表示首节点
        /// </summary>
        internal uint SourceHigh
        {
            get { return Source & 0x80000000U; }
        }
        /// <summary>
        /// 获取节点来源
        /// </summary>
        internal int SourceIndex
        {
            get { return (int)(Source & int.MaxValue); }
        }
        /// <summary>
        /// 设置后续节点的节点来源
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNextSource(int index)
        {
            Source = (uint)index | 0x80000000U;
        }

        /// <summary>
        /// 关键字哈希值
        /// </summary>
        internal uint HashCode;
        /// <summary>
        /// 下一个数据索引位置，int.MaxValue 表示最后一个
        /// </summary>
        internal int Next;
        /// <summary>
        /// 数据
        /// </summary>
        internal T Value;
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(uint source, uint hashCode, T value)
        {
            Source = source;
            HashCode = hashCode;
            Next = int.MaxValue;
            Value = value;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        /// <param name="value"></param>
        /// <param name="next"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(uint source, uint hashCode, T value, int next)
        {
            Source = source;
            HashCode = hashCode;
            Next = next;
            Value = value;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="hashCode"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(uint hashCode, T value)
        {
            HashCode = hashCode;
            Value = value;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(uint source, ref ReusableHashNode<T> value)
        {
            Source = source;
            HashCode = value.HashCode;
            Next = int.MaxValue;
            Value = value.Value;
        }
        /// <summary>
        /// 设置后续节点数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNext(uint source, ref ReusableHashNode<T> value)
        {
            Source = source | 0x80000000U;
            HashCode = value.HashCode;
            Next = int.MaxValue;
            Value = value.Value;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="hashIndex"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetHashIndex(int hashIndex, ref ReusableHashNode<T> value)
        {
            HashIndex = hashIndex;
            Source = (uint)hashIndex;
            HashCode = value.HashCode;
            Next = int.MaxValue;
            Value = value.Value;
        }
    }
}
