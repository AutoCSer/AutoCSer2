using AutoCSer.CommandService.DiskBlock;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 关键字数据集合持久化数据节点
    /// </summary>
    /// <typeparam name="T">关键字数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PersistenceNode<T>
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
        /// 当前数据集合
        /// </summary>
        internal readonly T[] Values;
        /// <summary>
        /// 当前数据集合中的新增数据数量
        /// </summary>
        internal readonly int ValueCount;
        /// <summary>
        /// 关键字数据集合持久化数据节点
        /// </summary>
        /// <param name="index">带移除标记的可重用哈希索引</param>
        internal PersistenceNode(RemoveMarkHashIndex<T> index)
        {
            BlockIndex = index.BlockIndex;
            Values = index.Values.GetArray(out ValueCount);
        }
        /// <summary>
        /// 关键字数据集合持久化数据节点
        /// </summary>
        /// <param name="blockIndex">上一块数据磁盘块索引信息</param>
        /// <param name="values">当前数据集合</param>
        /// <param name="valueCount">当前数据集合中的新增数据数量</param>
        internal PersistenceNode(BlockIndex blockIndex, T[] values, int valueCount)
        {
            this.BlockIndex = blockIndex;
            this.Values = values;
            ValueCount = valueCount;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="hashSet"></param>
        internal void Load(HashSet<T> hashSet)
        {
            int valueCount = ValueCount;
            foreach (T value in Values)
            {
                if (valueCount != 0)
                {
                    hashSet.Add(value);
                    --valueCount;
                }
                else hashSet.Remove(value);
            }
        }
    }
}
