using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 字典计数器节点接口
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
    public partial interface IDictionaryCounterNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(KeyValue<T, long> value);
        /// <summary>
        /// 计数 +1
        /// </summary>
        /// <param name="key">计数关键字</param>
        /// <returns>key 为 null 则返回 false</returns>
        bool Increment(T key);
        /// <summary>
        /// 获取当前计数
        /// </summary>
        /// <param name="key">计数关键字</param>
        /// <returns>key 为 null 则返回 -1</returns>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethod(IsPersistence = false)]
        long GetCount(T key);
    }
}
