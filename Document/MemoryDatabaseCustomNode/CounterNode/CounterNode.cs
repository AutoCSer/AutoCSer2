using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 计数器节点
    /// </summary>
    public sealed class CounterNode : ICounterNode, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>
    {
        /// <summary>
        /// 当前计数
        /// </summary>
        private long count;
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>.GetSnapshotCapacity(ref object customObject)
        {
            return 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<long> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>.GetSnapshotResult(long[] snapshotArray, object customObject)
        {
            snapshotArray[0] = count;
            return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<long>(1);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>.SetSnapshotResult(ref LeftArray<long> array, ref LeftArray<long> newArray) { }
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(long value)
        {
            count = value;
        }
        /// <summary>
        /// 计数 +1
        /// </summary>
        public void Increment()
        {
            ++count;
        }
        /// <summary>
        /// 获取当前计数
        /// </summary>
        /// <returns>当前计数</returns>
        public long GetCount()
        {
            return count;
        }
    }
}
