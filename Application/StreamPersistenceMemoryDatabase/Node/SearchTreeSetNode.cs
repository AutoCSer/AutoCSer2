using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Binary search tree set node
    /// 二叉搜索树集合节点
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if AOT
    public abstract class SearchTreeSetNode<T> : ISnapshot<T>
#else
    public sealed class SearchTreeSetNode<T> : ISearchTreeSetNode<T>, ISnapshot<T>
#endif
        where T : IComparable<T>
    {
        /// <summary>
        /// Binary search tree set
        /// 二叉搜索树集合
        /// </summary>
        private AutoCSer.SearchTree.Set<T> searchTreeSet;
        /// <summary>
        /// Binary search tree set node
        /// 二叉搜索树集合节点
        /// </summary>
        public SearchTreeSetNode()
        {
            searchTreeSet = new AutoCSer.SearchTree.Set<T>();
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
            return searchTreeSet.Count;
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
            return new SnapshotResult<T>(snapshotArray, searchTreeSet.Values, searchTreeSet.Count);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<T> array, ref LeftArray<T> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return searchTreeSet.Count;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && searchTreeSet.Add(value);
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
                    if (value != null && searchTreeSet.Add(value)) ++count;
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
            searchTreeSet.Clear();
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && searchTreeSet.Contains(value);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(T value)
        {
            return value != null && searchTreeSet.Remove(value);
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
                    if (value != null && searchTreeSet.Remove(value)) ++count;
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// Get the first data
        /// 获取第一个数据
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        public ValueResult<T> GetFrist()
        {
            if (searchTreeSet.Count != 0) return searchTreeSet.Frist;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        public ValueResult<T> GetLast()
        {
            if (searchTreeSet.Count != 0) return searchTreeSet.Last;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        public int IndexOf(T value)
        {
            return value != null && searchTreeSet.Count != 0 ? searchTreeSet.IndexOf(value) : -1;
        }
        /// <summary>
        /// Get the number of nodes smaller than the specified keyword
        /// 获取比指定关键字小的节点数量
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        public int CountLess(T value)
        {
            return value != null ? searchTreeSet.CountLess(value) : -1;
        }
        /// <summary>
        /// Get the number of nodes larger than the specified keyword
        /// 获取比指定关键字大的节点数量
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        public int CountThan(T value)
        {
            return value != null ? searchTreeSet.CountThan(value) : -1;
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        public ValueResult<T> GetByIndex(int index)
        {
            if ((uint)index < (uint)searchTreeSet.Count) return searchTreeSet.At(index);
            return default(ValueResult<T>);
        }
    }
}
