using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Node collection enumeration snapshot function interface
    /// 节点集合枚举快照功能接口
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public interface IEnumerableSnapshot<T>
    {
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<T> SnapshotEnumerable { get; }
    }
}
