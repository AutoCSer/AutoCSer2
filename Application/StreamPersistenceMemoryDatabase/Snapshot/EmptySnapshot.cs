using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Empty snapshot
    /// 空快照
    /// </summary>
    /// <typeparam name="T">Snapshot data type
    /// 快照数据类型</typeparam>
    public sealed class EmptySnapshot<T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// Empty snapshot
        /// 空快照
        /// </summary>
        public EmptySnapshot() { }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues { get { return EmptyArray<T>.Array; } }
        /// <summary>
        /// Get the array of pre-applied snapshot containers
        /// 获取预申请快照容器数组
        /// </summary>
        void ISnapshotEnumerable<T>.GetSnapshotValueArray() { }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        void ISnapshotEnumerable<T>.GetSnapshotResult() { }
        /// <summary>
        /// Close the snapshot operation
        /// 关闭快照操作
        /// </summary>
        void ISnapshotEnumerable<T>.CloseSnapshot() { }

        /// <summary>
        /// Empty snapshot
        /// 空快照
        /// </summary>
        public static readonly EmptySnapshot<T> Empty = new EmptySnapshot<T>();
    }
}
