using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照数据类型转换
    /// </summary>
    /// <typeparam name="ST">数据类型</typeparam>
    /// <typeparam name="T">持久化类型</typeparam>
    public sealed class SnapshotEnumerableCastEmpty<ST, T> : ISnapshotEnumerable<T>
    {
        /// <summary>
        /// 快照集合
        /// </summary>
        private readonly ISnapshotEnumerable<ST> snapshot;
        /// <summary>
        /// 数据类型转换委托
        /// </summary>
        private readonly Func<ST, T> getValue;
        /// <summary>
        /// 判断快照是否有效
        /// </summary>
        private readonly Func<bool> getIsSnapshot;
        /// <summary>
        /// 快照是否有效
        /// </summary>
        private bool isSnapshot;
        /// <summary>
        /// 快照数据类型转换
        /// </summary>
        /// <param name="snapshot">快照集合</param>
        /// <param name="getValue">持久化类型</param>
        /// <param name="getIsSnapshot">持久化类型</param>
        public SnapshotEnumerableCastEmpty(ISnapshotEnumerable<ST> snapshot, Func<ST, T> getValue, Func<bool> getIsSnapshot)
        {
            this.snapshot = snapshot;
            this.getValue = getValue;
            this.getIsSnapshot = getIsSnapshot;
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues
        {
            get
            {
                return isSnapshot ? getSnapshotValues() : EmptyArray<T>.Array;
            }
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<T> getSnapshotValues()
        {
            foreach (ST value in snapshot.SnapshotValues)
            {
                yield return getValue(value);
            }
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        void ISnapshotEnumerable<T>.GetSnapshotValueArray()
        {
            if (getIsSnapshot()) snapshot.GetSnapshotValueArray();
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        void ISnapshotEnumerable<T>.GetSnapshotResult()
        {
            if (isSnapshot = getIsSnapshot()) snapshot.GetSnapshotResult();
        }
        /// <summary>
        /// 关闭快照操作
        /// </summary>
        void ISnapshotEnumerable<T>.CloseSnapshot()
        {
            snapshot.CloseSnapshot();
        }
    }
}
