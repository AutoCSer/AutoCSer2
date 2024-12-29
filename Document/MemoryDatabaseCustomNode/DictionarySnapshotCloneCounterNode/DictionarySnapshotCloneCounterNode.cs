using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 支持快照克隆的字典计数器节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public sealed class DictionarySnapshotCloneCounterNode<T> : IDictionarySnapshotCloneCounterNode<T>, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<SnapshotCloneCounter<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 字典计数器
        /// </summary>
        private Dictionary<T, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<SnapshotCloneCounter<T>>> counts;
        /// <summary>
        /// 字典计数器节点
        /// </summary>
        /// <param name="capacity">初始化容器尺寸</param>
        internal DictionarySnapshotCloneCounterNode(int capacity)
        {
            counts = DictionaryCreator<T>.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<SnapshotCloneCounter<T>>>(capacity);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<SnapshotCloneCounter<T>>.GetSnapshotCapacity(ref object customObject)
        {
            return counts.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<SnapshotCloneCounter<T>> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<SnapshotCloneCounter<T>>.GetSnapshotResult(SnapshotCloneCounter<T>[] snapshotArray, object customObject)
        {
            return SnapshotCloneCounter<T>.GetSnapshotResult(snapshotArray, counts.Values);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<SnapshotCloneCounter<T>>.SetSnapshotResult(ref LeftArray<SnapshotCloneCounter<T>> array, ref LeftArray<SnapshotCloneCounter<T>> newArray) { }
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(SnapshotCloneCounter<T> value)
        {
            counts.Add(value.Key, value);
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
                if (counts.TryGetValue(key, out AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<SnapshotCloneCounter<T>> count)) count.GetNotNull().Increment();
                else counts.Add(key, new SnapshotCloneCounter<T>(key));
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
            if (key != null) return counts.TryGetValue(key, out AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<SnapshotCloneCounter<T>> count) ? count.NoCheckNotNull().Count : 0;
            return -1;
        }
    }
}
