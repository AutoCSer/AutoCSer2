using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点快照功能接口
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public interface IEnumerableSnapshot<T>
    {
        /// <summary>
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<T> SnapshotEnumerable { get; }
    }
}
