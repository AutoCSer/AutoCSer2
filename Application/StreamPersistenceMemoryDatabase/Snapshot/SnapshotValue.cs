using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 固定值快照
    /// </summary>
    /// <typeparam name="T">快照数据类型</typeparam>
    public sealed class SnapshotValue<T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// 快照数据
        /// </summary>
        private readonly T value;
        /// <summary>
        /// 固定值快照
        /// </summary>
        /// <param name="value">快照数据</param>
        public SnapshotValue(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues { get { yield return value; } }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        void ISnapshotEnumerable<T>.GetSnapshotValueArray() { }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        void ISnapshotEnumerable<T>.GetSnapshotResult() { }
        /// <summary>
        /// 关闭快照操作
        /// </summary>
        void ISnapshotEnumerable<T>.CloseSnapshot() { }
    }
}
