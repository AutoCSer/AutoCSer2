using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Single-valued snapshot
    /// 单值快照
    /// </summary>
    /// <typeparam name="T">Snapshot data type
    /// 快照数据类型</typeparam>
    public sealed class SnapshotGetValueEmpty<T> : SnapshotGetValue<KeyValue<bool, T>>, ISnapshotEnumerable<T>
    {
        /// <summary>
        /// Single-valued snapshot
        /// 单值快照
        /// </summary>
        /// <param name="getValue">快照数据</param>
        public SnapshotGetValueEmpty(Func<KeyValue<bool, T>> getValue) : base(getValue) { }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues
        {
            get
            {
                if (value.Key) yield return value.Value;
            }
        }
    }
}
