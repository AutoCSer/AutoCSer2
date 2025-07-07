using AutoCSer.Extensions;
using AutoCSer.SearchTree;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if AOT
    public abstract class HashSetNode<T> : IEnumerableSnapshot<T>
#else
    public sealed class HashSetNode<T> : IHashSetNode<T>, IEnumerableSnapshot<T>
#endif
        where T : IEquatable<T>
    {
        /// <summary>
        /// 哈希表
        /// </summary>
        private SnapshotHashSet<T> hashSet;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<T> IEnumerableSnapshot<T>.SnapshotEnumerable { get { return hashSet.Nodes; } }
        /// <summary>
        /// 哈希表节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        public HashSetNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex)
        {
            hashSet = new SnapshotHashSet<T>(capacity, groupType);
        }
        /// <summary>
        /// Clear all data and rebuild the container (to solve the problem of low performance of the clear call when the data volume is large)
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
            hashSet.Renew(capacity);
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return hashSet.Count;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && hashSet.Add(value, (uint)value.GetHashCode());
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The quantity of the added data
        /// 添加数据数量</returns>
        public int AddValues(T[] values)
        {
            if (values != null)
            {
                int count = 0;
                foreach (T value in values)
                {
                    if (value != null && hashSet.Add(value, (uint)value.GetHashCode())) ++count;
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            hashSet.ClearArray();
        }
        /// <summary>
        /// Reusable dictionaries reset data locations (The presence of reference type data can cause memory leaks)
        /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        public void ReusableClear()
        {
            hashSet.ClearCount();
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && hashSet.Contains(value, (uint)value.GetHashCode());
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(T value)
        {
            return value != null && hashSet.Remove(value, (uint)value.GetHashCode());
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The quantity of deleted data
        /// 删除数据数量</returns>
        public int RemoveValues(T[] values)
        {
            if (values != null)
            {
                int count = 0;
                foreach (T value in values)
                {
                    if (value != null && hashSet.Remove(value, (uint)value.GetHashCode())) ++count;
                }
                return count;
            }
            return 0;
        }
    }
}
