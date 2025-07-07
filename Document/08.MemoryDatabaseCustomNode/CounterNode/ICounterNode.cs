using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Counter node interface example
    /// 计数器节点接口示例
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface ICounterNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(long value);
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        void Increment();
        /// <summary>
        /// Get the current count
        /// 获取当前计数
        /// </summary>
        /// <returns>Current count
        /// 当前计数</returns>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsPersistence = false)]
        long GetCount();
    }
}
