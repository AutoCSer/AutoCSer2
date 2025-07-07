using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 固定值快照
    /// </summary>
    /// <typeparam name="T">Snapshot data type
    /// 快照数据类型</typeparam>
    public sealed class SnapshotValue<T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
        private readonly T value;
        /// <summary>
        /// 固定值快照
        /// </summary>
        /// <param name="value">Snapshot data
        /// 快照数据</param>
        public SnapshotValue(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues { get { yield return value; } }
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
    }
}
