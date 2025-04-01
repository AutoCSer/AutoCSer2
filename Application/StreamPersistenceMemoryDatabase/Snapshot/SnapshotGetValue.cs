using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 单值快照
    /// </summary>
    /// <typeparam name="T">快照数据类型</typeparam>
    public class SnapshotGetValue<T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// 获取快照数据
        /// </summary>
        private readonly Func<T> getValue;
        /// <summary>
        /// 快照数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T value;
        /// <summary>
        /// 单值快照
        /// </summary>
        /// <param name="getValue">快照数据</param>
        public SnapshotGetValue(Func<T> getValue)
        {
            this.getValue = getValue;
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues { get { yield return value; } }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        public void GetSnapshotValueArray() { }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        public void GetSnapshotResult() { value = getValue(); }
        /// <summary>
        /// 关闭快照操作
        /// </summary>
        public void CloseSnapshot() { }
    }
}
