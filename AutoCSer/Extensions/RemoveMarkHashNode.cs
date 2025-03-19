using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 带移除标记的可重用哈希表节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = sizeof(uint) * 2)]
    internal struct RemoveMarkHashNode
    {
        /// <summary>
        /// 索引数据 1b[删除数据标记]+1b[首节点标记]+10b[节点来源]+10b[哈希索引]+10b[下一个数据索引位置]
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        private uint indexData;
        /// <summary>
        /// 哈希索引位置
        /// </summary>
        internal int HashIndex
        {
            get { return (int)((indexData >> 10) & RemoveMarkHashSetCapacity.MaxCapacity); }
        }
        /// <summary>
        /// 带首节点标记的节点来源
        /// </summary>
        internal uint Source
        {
            get { return (indexData >> 20) & ((RemoveMarkHashSetCapacity.MaxCapacity << 1) | 1); }
        }
        /// <summary>
        /// 节点来源
        /// </summary>
        internal int SourceIndex
        {
            get { return (int)((indexData >> 20) & RemoveMarkHashSetCapacity.MaxCapacity); }
        }
        /// <summary>
        /// 下一个数据索引位置，0x3ff 表示最后一个
        /// </summary>
        internal int Next
        {
            get { return (int)(indexData & RemoveMarkHashSetCapacity.MaxCapacity); }
        }
        /// <summary>
        /// 0 表示首节点
        /// </summary>
        internal uint SourceHigh
        {
            get { return indexData & 0x40000000U; }
        }
        /// <summary>
        /// 0 表示添加数据
        /// </summary>
        internal uint IsRemove
        {
            get { return indexData & 0x80000000U; }
        }
        /// <summary>
        /// 关键字数据哈希值
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(uint))]
        internal uint HashCode;
        /// <summary>
        /// 带移除标记的可重用哈希表节点
        /// </summary>
        /// <param name="hashCode">关键字数据哈希值</param>
        internal RemoveMarkHashNode(uint hashCode)
        {
            HashCode = hashCode;
            indexData = 0;
        }
        /// <summary>
        /// 带移除标记的可重用哈希表节点
        /// </summary>
        /// <param name="hashCode">关键字数据哈希值</param>
        /// <param name="indexData">索引数据</param>
        internal RemoveMarkHashNode(uint hashCode, uint indexData)
        {
            HashCode = hashCode;
            this.indexData = indexData;
        }
        /// <summary>
        /// 设置下一个数据索引位置
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNext()
        {
            indexData |= RemoveMarkHashSetCapacity.MaxCapacity;
        }
        /// <summary>
        /// 设置下一个数据索引位置
        /// </summary>
        /// <param name="next">下一个数据索引位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNext(int next)
        {
            indexData = (indexData & (uint.MaxValue ^ RemoveMarkHashSetCapacity.MaxCapacity)) | (uint)next;
        }
        /// <summary>
        /// 设置哈希索引位置为 0
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetHashIndex()
        {
            indexData &= uint.MaxValue ^ (RemoveMarkHashSetCapacity.MaxCapacity << 10);
        }
        /// <summary>
        /// 设置哈希索引位置
        /// </summary>
        /// <param name="hashIndex">哈希索引位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetHashIndex(int hashIndex)
        {
            indexData = (indexData & (uint.MaxValue ^ (RemoveMarkHashSetCapacity.MaxCapacity << 10))) | ((uint)hashIndex << 10);
        }
        /// <summary>
        /// 设置节点来源
        /// </summary>
        /// <param name="source">节点来源</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetSource(int source)
        {
            indexData = (indexData & (uint.MaxValue ^ (((RemoveMarkHashSetCapacity.MaxCapacity << 1) | 1) << 20))) | ((uint)source << 20);
        }
        /// <summary>
        /// 设置节点来源
        /// </summary>
        /// <param name="source">节点来源</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNextSource(int source)
        {
            indexData = (indexData & (uint.MaxValue ^ (RemoveMarkHashSetCapacity.MaxCapacity << 20))) | ((uint)source << 20) | 0x40000000U;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNode(int source, uint hashCode)
        {
            indexData = ((uint)source << 20) | (indexData & (RemoveMarkHashSetCapacity.MaxCapacity << 10)) | RemoveMarkHashSetCapacity.MaxCapacity;
            HashCode = hashCode;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNextNode(int source, uint hashCode)
        {
            indexData = ((uint)source << 20) | (indexData & (RemoveMarkHashSetCapacity.MaxCapacity << 10)) | (RemoveMarkHashSetCapacity.MaxCapacity | 0x40000000U);
            HashCode = hashCode;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRemoveNode(int source, uint hashCode)
        {
            indexData = ((uint)source << 20) | (indexData & (RemoveMarkHashSetCapacity.MaxCapacity << 10)) | (RemoveMarkHashSetCapacity.MaxCapacity | 0x80000000U);
            HashCode = hashCode;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRemoveNextNode(int source, uint hashCode)
        {
            indexData = ((uint)source << 20) | (indexData & (RemoveMarkHashSetCapacity.MaxCapacity << 10)) | (RemoveMarkHashSetCapacity.MaxCapacity | 0xc0000000U);
            HashCode = hashCode;
        }
    }
    /// <summary>
    /// 带移除标记的可重用哈希表节点
    /// </summary>
    /// <typeparam name="T">关键字数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct RemoveMarkHashNode<T>
    {
        /// <summary>
        /// 带移除标记的可重用哈希表节点
        /// </summary>
        internal RemoveMarkHashNode HashIndex;
        /// <summary>
        /// 关键字数据
        /// </summary>
        internal T Value;
        /// <summary>
        /// 带移除标记的可重用哈希表节点
        /// </summary>
        /// <param name="value">关键字数据</param>
        /// <param name="hashCode">关键字数据哈希值</param>
        internal RemoveMarkHashNode(T value, uint hashCode)
        {
            Value = value;
            HashIndex = new RemoveMarkHashNode(hashCode);
        }
        /// <summary>
        /// 带移除标记的可重用哈希表节点
        /// </summary>
        /// <param name="value">关键字数据</param>
        /// <param name="hashCode">关键字数据哈希值</param>
        /// <param name="indexData">索引数据</param>
        internal RemoveMarkHashNode(T value, uint hashCode, uint indexData)
        {
            Value = value;
            HashIndex = new RemoveMarkHashNode(hashCode, indexData);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNode(T value, int source, uint hashCode)
        {
            Value = value;
            HashIndex.SetNode(source, hashCode);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNextNode(T value, int source, uint hashCode)
        {
            Value = value;
            HashIndex.SetNextNode(source, hashCode);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRemoveNode(T value, int source, uint hashCode)
        {
            Value = value;
            HashIndex.SetRemoveNode(source, hashCode);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="hashCode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRemoveNextNode(T value, int source, uint hashCode)
        {
            Value = value;
            HashIndex.SetRemoveNextNode(source, hashCode);
        }
    }
}
