using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 获取快照集合
    /// </summary>
    /// <typeparam name="T">Snapshot data type
    /// 快照数据类型</typeparam>
    public class SnapshotGetEnumerable<T> : SnapshotGetValue<IEnumerable<T>>, ISnapshotEnumerable<T>
    {
        /// <summary>
        /// Single-valued snapshot
        /// 单值快照
        /// </summary>
        /// <param name="getValue">快照数据</param>
        public SnapshotGetEnumerable(Func<IEnumerable<T>> getValue) : base(getValue) { }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues { get { return value; } }
    }
}
