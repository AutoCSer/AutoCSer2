using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照接口节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class EnumerableSnapshotNode<T> : SnapshotNode
    {
        /// <summary>
        /// 操作节点对象
        /// </summary>
        private readonly IEnumerableSnapshot<T> target;
        /// <summary>
        /// 快照集合
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private ISnapshotEnumerable<T> values;
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target">操作节点对象</param>
        internal EnumerableSnapshotNode(IEnumerableSnapshot<T> target)
        {
            this.target = target;
        }
        /// <summary>
        /// 关闭重建
        /// </summary>
        internal override void Close()
        {
            if (values != null)
            {
                values.CloseSnapshot();
                values = null;
            }
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        internal override void GetArray()
        {
            (values = target.SnapshotEnumerable).GetSnapshotValueArray();
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        internal override void GetResult()
        {
            values.GetSnapshotResult();
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        /// <returns></returns>
        internal override bool Rebuild(PersistenceRebuilder rebuilder)
        {
            return rebuilder.Rebuild(values);
        }
    }
}
