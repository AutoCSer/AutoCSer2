using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 持久化前置检查示例节点接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface IBeforePersistenceNode
    {
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSetEntity(IdentityEntity value);
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="identity">当前分配 ID</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetIdentity(long identity);
        /// <summary>
        /// 添加一个新数据（持久化前置检查，客户端不可见）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<long> AppendEntityBeforePersistence(IdentityEntity value);
        /// <summary>
        /// 添加一个新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>新数据 ID，失败返回 0</returns>
        public long AppendEntity(IdentityEntity value);
        /// <summary>
        /// 计数 +1
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>没有找到 ID 则返回 false</returns>
        bool Increment(long identity);
        /// <summary>
        /// 获取当前计数
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>没有找到 ID 则返回 -1</returns>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsPersistence = false)]
        long GetCount(long identity);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        bool Remove(long identity);
    }
}
