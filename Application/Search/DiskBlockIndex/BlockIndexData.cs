using AutoCSer.CommandService.DiskBlock;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 索引数据磁盘块索引信息节点
    /// </summary>
    /// <typeparam name="T">索引数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct BlockIndexData<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 上一块数据磁盘块索引信息
        /// </summary>
        internal readonly BlockIndex BlockIndex;
        /// <summary>
        /// 历史数据磁盘块索引信息中的数据总数量
        /// </summary>
        internal readonly int BlockIndexTotalCount;
        /// <summary>
        /// 历史数据磁盘块索引信息中的新增数据数量
        /// </summary>
        internal readonly int BlockIndexValueCount;
        /// <summary>
        /// 未持久化数据集合
        /// </summary>
        internal T[] Values;
        /// <summary>
        /// 未持久化数据集合中的新增数据数量
        /// </summary>
        internal int ValueCount;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        internal int CacheValueCount
        {
            get
            {
                if (Values != null) return Values.Length;
                return 0;
            }
        }
        /// <summary>
        /// 预估数据数量
        /// </summary>
        internal int EstimatedCount
        {
            get { return BlockIndexValueCount + ValueCount; }
        }
        /// <summary>
        /// 空节点
        /// </summary>
        /// <param name="blockIndex"></param>
        internal BlockIndexData(BlockIndex blockIndex)
        {
            BlockIndex = blockIndex;
            BlockIndexTotalCount = BlockIndexValueCount = ValueCount = 0;
            Values = EmptyArray<T>.Array;
        }
        /// <summary>
        /// 关键字数据磁盘块索引信息节点
        /// </summary>
        /// <param name="index">带移除标记的可重用哈希索引</param>
        internal BlockIndexData(RemoveMarkHashIndex<T> index)
        {
            BlockIndex = index.BlockIndex;
            BlockIndexTotalCount = index.BlockIndexTotalCount;
            BlockIndexValueCount = index.BlockIndexValueCount;
            Values = index.Values.GetArray(out ValueCount);
        }
        /// <summary>
        /// 关键字数据磁盘块索引信息节点
        /// </summary>
        /// <param name="index">带移除标记的可重用哈希索引</param>
        /// <param name="values">未持久化数据集合</param>
        /// <param name="valueCount">未持久化数据集合中的新增数据数量</param>
        internal BlockIndexData(RemoveMarkHashKeyIndex index, T[] values, int valueCount)
        {
            BlockIndex = index.BlockIndex;
            BlockIndexTotalCount = index.BlockIndexTotalCount;
            BlockIndexValueCount = index.BlockIndexValueCount;
            Values = values;
            ValueCount = valueCount;
        }
        /// <summary>
        /// 加载更新数据
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="values"></param>
        internal BlockIndexData(BlockIndex blockIndex, T[] values)
        {
            BlockIndex = blockIndex;
            BlockIndexTotalCount = BlockIndexValueCount = 0;
            Values = values;
            ValueCount = values.Length;
        }
        /// <summary>
        /// 设置空数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetEmptyValues()
        {
            Values = EmptyArray<T>.Array;
            ValueCount = 0;
        }
        /// <summary>
        /// 获取未持久化数据集合的带移除标记的可重用哈希表
        /// </summary>
        /// <returns></returns>
        internal RemoveMarkHashSet<T> GetRemoveMarkHashSet()
        {
            RemoveMarkHashSet<T> hashSet = new RemoveMarkHashSet<T>(Values.Length);
            int index = 0;
            foreach (T value in Values)
            {
                if (index == ValueCount) hashSet.AddRemove(value);
                else hashSet.Add(value);
            }
            return hashSet;
        }
        /// <summary>
        /// 获取持久化数据节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal PersistenceNode<T> GetPersistenceNode()
        {
            return new PersistenceNode<T>(BlockIndex, Values, ValueCount);
        }
    }
}
