using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序集合节点
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if AOT
    public abstract class SortedSetNode<T> : ISnapshot<T>
#else
    public sealed class SortedSetNode<T> : ISortedSetNode<T>, ISnapshot<T>
#endif
        where T : IComparable<T>
    {
        /// <summary>
        /// 排序集合
        /// </summary>
        private SortedSet<T> sortedSet;
        /// <summary>
        /// 排序集合节点
        /// </summary>
        public SortedSetNode()
        {
            sortedSet = new SortedSet<T>();
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return sortedSet.Count;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            return new SnapshotResult<T>(snapshotArray, sortedSet);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<T> array, ref LeftArray<T> newArray) { }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return sortedSet.Count;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && sortedSet.Add(value);
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
                    if (value != null && sortedSet.Add(value)) ++count;
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
            sortedSet.Clear();
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && sortedSet.Contains(value);
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
            return value != null && sortedSet.Remove(value);
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
                    if (value != null && sortedSet.Remove(value)) ++count;
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// Get the minimum value
        /// 获取最小值
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        public ValueResult<T> GetMin()
        {
#pragma warning disable CS8604
            if (sortedSet.Count != 0) return sortedSet.Min;
#pragma warning restore CS8604
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Get the maximum value
        /// 获取最大值
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        public ValueResult<T> GetMax()
        {
#pragma warning disable CS8604
            if (sortedSet.Count != 0) return sortedSet.Max;
#pragma warning restore CS8604
            return default(ValueResult<T>);
        }
    }
}
