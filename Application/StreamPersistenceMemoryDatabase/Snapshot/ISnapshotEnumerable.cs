using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照集合
    /// </summary>
    /// <typeparam name="T">快照对象类星体</typeparam>
    public interface ISnapshotEnumerable<T>
    {
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> SnapshotValues { get; }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        void GetSnapshotValueArray();
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        void GetSnapshotResult();
        /// <summary>
        /// 关闭快照操作
        /// </summary>
        void CloseSnapshot();
    }
}
