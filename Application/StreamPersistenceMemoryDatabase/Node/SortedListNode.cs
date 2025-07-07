using AutoCSer.Extensions;
using AutoCSer.SearchTree;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序列表节点
    /// </summary>
    /// <typeparam name="KT">Sort keyword type
    /// 排序关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
#if AOT
    public abstract class SortedListNode<KT, VT> : ISnapshot<KeyValue<KT, VT>>
#else
    public sealed class SortedListNode<KT, VT> : ISortedListNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
#endif
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 排序列表
        /// </summary>
        private SortedList<KT, VT> list;
        /// <summary>
        /// 排序列表
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public SortedListNode(int capacity)
        {
            list = new SortedList<KT, VT>(capacity);
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
            return list.Count;
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
        public SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult(KeyValue<KT, VT>[] snapshotArray, object customObject)
        {
            return ServerNode.GetSnapshotResult(list, snapshotArray);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<KeyValue<KT, VT>> array, ref LeftArray<KeyValue<KT, VT>> newArray) { }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            list[value.Key] = value.Value;
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return list.Count;
        }
        /// <summary>
        /// Get the container size
        /// 获取容器大小
        /// </summary>
        /// <returns></returns>
        public int GetCapacity()
        {
            return list.Capacity;
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && list.TryAdd(key, value);
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && list.ContainsKey(key);
        }
        /// <summary>
        /// To determine whether the data exists, the time complexity is O(n). It is not recommended to call (since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(VT value)
        {
            return list.ContainsValue(value);
        }
        /// <summary>
        /// Get the ranking position of the key word
        /// 获取关键字排序位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A negative number indicates that the keyword was not found
        /// 负数表示没有找到关键字</returns>
        public int IndexOfKey(KT key)
        {
            return key != null ? list.IndexOfKey(key) : -1;
        }
        /// <summary>
        /// Get the first matching data sort position (since cached data is a serialized copy of the object, the equality test is done by implementing IEquatable{VT})
        /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A negative number indicates that no matching data was found
        /// 负数表示没有找到匹配数据</returns>
        public int IndexOfValue(VT value)
        {
            return list.IndexOfValue(value);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(KT key)
        {
            return key != null && list.Remove(key);
        }
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            if (key != null)
            {
                var value = default(VT);
                if (list.Remove(key, out value)) return value;
            }
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
            if (key != null && list.TryGetValue(key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Delete the data at the specified sort index position
        /// 删除指定排序索引位置数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        public bool RemoveAt(int index)
        {
            if ((uint)index < (uint)list.Count)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}
