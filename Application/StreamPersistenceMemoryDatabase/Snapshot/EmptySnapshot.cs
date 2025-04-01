using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 空快照
    /// </summary>
    /// <typeparam name="T">快照数据类型</typeparam>
    public sealed class EmptySnapshot<T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// 空快照
        /// </summary>
        public EmptySnapshot() { }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues { get { return EmptyArray<T>.Array; } }
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

        /// <summary>
        /// 空快照
        /// </summary>
        public static readonly EmptySnapshot<T> Empty = new EmptySnapshot<T>();
    }
}
