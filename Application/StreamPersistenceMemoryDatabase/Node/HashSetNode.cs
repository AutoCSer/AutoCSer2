using AutoCSer.Extensions;
using AutoCSer.SearchTree;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
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
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<T> IEnumerableSnapshot<T>.SnapshotEnumerable { get { return hashSet.Nodes; } }
        /// <summary>
        /// 哈希表节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        public HashSetNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex)
        {
            hashSet = new SnapshotHashSet<T>(capacity, groupType);
        }
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
            hashSet.Renew(capacity);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return hashSet.Count;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && hashSet.Add(value, (uint)value.GetHashCode());
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns>添加数据数量</returns>
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
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            hashSet.ClearArray();
        }
        /// <summary>
        /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        public void ReusableClear()
        {
            hashSet.ClearCount();
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && hashSet.Contains(value, (uint)value.GetHashCode());
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(T value)
        {
            return value != null && hashSet.Remove(value, (uint)value.GetHashCode());
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="values"></param>
        /// <returns>删除数据数量</returns>
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
