using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 计数器节点接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface ICounterNode
    {
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(long value);
        /// <summary>
        /// 计数 +1
        /// </summary>
        void Increment();
        /// <summary>
        /// 获取当前计数
        /// </summary>
        /// <returns>当前计数</returns>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsPersistence = false)]
        long GetCount();
    }
}
