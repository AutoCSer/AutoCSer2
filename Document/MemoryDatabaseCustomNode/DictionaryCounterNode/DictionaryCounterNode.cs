using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 字典计数器节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public sealed class DictionaryCounterNode<T> : IDictionaryCounterNode<T>, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<KeyValue<T, long>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 字典计数器
        /// </summary>
        private Dictionary<T, long> counts;
        /// <summary>
        /// 字典计数器节点
        /// </summary>
        /// <param name="capacity">初始化容器尺寸</param>
        internal DictionaryCounterNode(int capacity)
        {
            counts = DictionaryCreator<T>.Create<long>();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<KeyValue<T, long>>.GetSnapshotCapacity(ref object customObject)
        {
            return counts.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<KeyValue<T, long>> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<KeyValue<T, long>>.GetSnapshotResult(KeyValue<T, long>[] snapshotArray, object customObject)
        {
            return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode.GetSnapshotResult(counts, snapshotArray);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<KeyValue<T, long>>.SetSnapshotResult(ref LeftArray<KeyValue<T, long>> array, ref LeftArray<KeyValue<T, long>> newArray) { }
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(KeyValue<T, long> value)
        {
            counts.Add(value.Key, value.Value);
        }
        /// <summary>
        /// 计数 +1
        /// </summary>
        /// <param name="key">计数关键字</param>
        /// <returns>key 为 null 则返回 false</returns>
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
        /// 获取当前计数
        /// </summary>
        /// <param name="key">计数关键字</param>
        /// <returns>key 为 null 则返回 -1</returns>
        public long GetCount(T key)
        {
            if (key != null) return counts.TryGetValue(key, out long count) ? count : 0;
            return -1;
        }
    }
}
