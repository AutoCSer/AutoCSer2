using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Snapshot collection
    /// 快照集合
    /// </summary>
    /// <typeparam name="T">Snapshot object type
    /// 快照对象类型</typeparam>
    public interface ISnapshotEnumerable<T>
    {
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> SnapshotValues { get; }
        /// <summary>
        /// Get the array of pre-applied snapshot containers
        /// 获取预申请快照容器数组
        /// </summary>
        void GetSnapshotValueArray();
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        void GetSnapshotResult();
        /// <summary>
        /// Close the snapshot operation
        /// 关闭快照操作
        /// </summary>
        void CloseSnapshot();
    }
}
