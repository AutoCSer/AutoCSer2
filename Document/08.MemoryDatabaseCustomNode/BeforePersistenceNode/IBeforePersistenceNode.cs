using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Example of a persistent pre-check node interface
    /// 持久化前置检查节点接口示例
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface IBeforePersistenceNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSetEntity(IdentityEntity value);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="identity">Current allocation identity
        /// 当前分配 ID</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetIdentity(long identity);
        /// <summary>
        /// Add a new data (Check the input parameters before the persistence operation)
        /// 添加一个新数据（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<long> AppendEntityBeforePersistence(IdentityEntity value);
        /// <summary>
        /// Add a new data
        /// 添加一个新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>New data identity. Return 0 if failed
        /// 新数据 ID，失败返回 0</returns>
        public long AppendEntity(IdentityEntity value);
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>Return false if the identity is not found
        /// 没有找到 ID 则返回 false</returns>
        bool Increment(long identity);
        /// <summary>
        /// Get the current count
        /// 获取当前计数
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>If the identity is not found, return -1
        /// 没有找到 ID 则返回 -1</returns>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsPersistence = false)]
        long GetCount(long identity);
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        bool Remove(long identity);
    }
}
