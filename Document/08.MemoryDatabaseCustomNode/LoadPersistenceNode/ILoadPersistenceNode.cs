using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// The API sample node interface for initialize and load the persistent data
    /// 初始化加载持久化数据 API 示例节点接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface ILoadPersistenceNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(long value);
        /// <summary>
        /// Count +1 (Initialize and load the persistent data)
        /// 计数 +1（初始化加载持久化数据）
        /// </summary>
        void IncrementLoadPersistence();
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
