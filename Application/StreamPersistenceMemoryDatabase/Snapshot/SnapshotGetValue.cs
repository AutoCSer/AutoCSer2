using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Single-valued snapshot
    /// 单值快照
    /// </summary>
    /// <typeparam name="T">Snapshot data type
    /// 快照数据类型</typeparam>
    public class SnapshotGetValue<T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// Get snapshot data
        /// 获取快照数据
        /// </summary>
        private readonly Func<T> getValue;
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T value;
        /// <summary>
        /// Single-valued snapshot
        /// 单值快照
        /// </summary>
        /// <param name="getValue">快照数据</param>
        public SnapshotGetValue(Func<T> getValue)
        {
            this.getValue = getValue;
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
        public void GetSnapshotValueArray() { }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        public void GetSnapshotResult() { value = getValue(); }
        /// <summary>
        /// Close the snapshot operation
        /// 关闭快照操作
        /// </summary>
        public void CloseSnapshot() { }
    }
}
