using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Dictionary counter node example
    /// 字典计数器节点示例
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    public sealed class DictionaryCounterNode<T> : IDictionaryCounterNode<T>, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<BinarySerializeKeyValue<T, long>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Dictionary counter
        /// 字典计数器
        /// </summary>
        private Dictionary<T, long> counts;
        /// <summary>
        /// Dictionary counter node example
        /// 字典计数器节点示例
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        internal DictionaryCounterNode(int capacity)
        {
            counts = DictionaryCreator<T>.Create<long>(capacity);
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<BinarySerializeKeyValue<T, long>>.GetSnapshotCapacity(ref object customObject)
        {
            return counts.Count;
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
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<BinarySerializeKeyValue<T, long>> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<BinarySerializeKeyValue<T, long>>.GetSnapshotResult(BinarySerializeKeyValue<T, long>[] snapshotArray, object customObject)
        {
            return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode.GetSnapshotResult(counts, snapshotArray);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<BinarySerializeKeyValue<T, long>>.SetSnapshotResult(ref LeftArray<BinarySerializeKeyValue<T, long>> array, ref LeftArray<BinarySerializeKeyValue<T, long>> newArray) { }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(BinarySerializeKeyValue<T, long> value)
        {
            counts.Add(value.Key, value.Value);
        }
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        /// <param name="key">The keyword for counting
        /// 计数关键字</param>
        /// <returns>If the key is null, it returns false
        /// key 为 null 则返回 false</returns>
        public bool Increment(T key)
        {
            if (key != null)
            {
                if (counts.TryGetValue(key, out long count)) counts[key] = count + 1;
                else counts.Add(key, 1);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get the current count
        /// 获取当前计数
        /// </summary>
        /// <param name="key">The keyword for counting
        /// 计数关键字</param>
        /// <returns>If the key is null, -1 will be returned
        /// key 为 null 则返回 -1</returns>
        public long GetCount(T key)
        {
            if (key != null) return counts.TryGetValue(key, out long count) ? count : 0;
            return -1;
        }
        /// <summary>
        /// Delete the count
        /// 删除计数
        /// </summary>
        /// <param name="key">The keyword for counting
        /// 计数关键字</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(T key)
        {
            return key != null && counts.Remove(key);
        }
    }
}
