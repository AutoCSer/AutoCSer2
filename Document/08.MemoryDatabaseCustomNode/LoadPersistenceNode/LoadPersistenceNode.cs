using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// The API sample node for initialize and load the persistent data
    /// 初始化加载持久化数据 API 示例节点
    /// </summary>
    public sealed class LoadPersistenceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ContextNode<ILoadPersistenceNode>, ILoadPersistenceNode, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IEnumerableSnapshot<long>
    {
        /// <summary>
        /// Current count
        /// 当前计数
        /// </summary>
        private long count;
        /// <summary>
        /// Single-valued snapshot
        /// 单值快照
        /// </summary>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshotEnumerable<long> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IEnumerableSnapshot<long>.SnapshotEnumerable { get { return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotGetValue<long>(GetCount); } }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(long value)
        {
            count = value;
        }
        /// <summary>
        /// Count +1 (Initialize and load the persistent data)
        /// 计数 +1（初始化加载持久化数据）
        /// </summary>
        public void IncrementLoadPersistence()
        {
            ++count;
            //output false
            Console.WriteLine($"IsLoaded {base.StreamPersistenceMemoryDatabaseService.IsLoaded} in {nameof(IncrementLoadPersistence)}");
        }
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        public void Increment()
        {
            ++count;
            //output true
            Console.WriteLine($"IsLoaded {base.StreamPersistenceMemoryDatabaseService.IsLoaded} in {nameof(Increment)}");
        }
        /// <summary>
        /// Get the current count
        /// 获取当前计数
        /// </summary>
        /// <returns>Current count
        /// 当前计数</returns>
        public long GetCount()
        {
            return count;
        }
    }
}
