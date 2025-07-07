using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// An example of the dictionary counter node interface supporting snapshot cloning
    /// 支持快照克隆的字典计数器节点接口示例
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface IDictionarySnapshotCloneCounterNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(SnapshotCloneCounter<T> value);
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        /// <param name="key">The keyword for counting
        /// 计数关键字</param>
        /// <returns>If the key is null, it returns false
        /// key 为 null 则返回 false</returns>
        bool Increment(T key);
        /// <summary>
        /// Get the current count
        /// 获取当前计数
        /// </summary>
        /// <param name="key">The keyword for counting
        /// 计数关键字</param>
        /// <returns>If the key is null, -1 will be returned
        /// key 为 null 则返回 -1</returns>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsPersistence = false)]
        long GetCount(T key);
        /// <summary>
        /// Delete the count
        /// 删除计数
        /// </summary>
        /// <param name="key">The keyword for counting
        /// 计数关键字</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        bool Remove(T key);
    }
}
