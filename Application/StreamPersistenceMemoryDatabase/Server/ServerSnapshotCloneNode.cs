using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 支持快照克隆的服务端节点
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    /// <typeparam name="ST">快照数据类型</typeparam>
    public sealed class ServerSnapshotCloneNode<T, ST> : ServerNode<T, ST>
        where ST : SnapshotCloneObject<ST>
    {
        /// <summary>
        /// 支持快照克隆的服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="index"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="target"></param>
        /// <param name="isPersistence">默认为 true 表示持久化为数据库，设置为 false 为纯内存模式在重启服务是数据将丢失</param>
        internal ServerSnapshotCloneNode(StreamPersistenceMemoryDatabaseService service, NodeIndex index, string key, T target, bool isPersistence = true) : base(service, index, key, target, isPersistence)
        {
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <returns>是否成功</returns>
        internal override bool SetSnapshotArray()
        {
            if (base.SetSnapshotArray())
            {
                foreach (ST value in snapshotArray) value.SnapshotValue = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        internal override void Rebuild(PersistenceRebuilder rebuilder)
        {
            LeftArray<ST> array = snapshotArray;
            snapshotArray.SetEmpty();
            rebuilder.RebuildSnapshotClone(ref array);
        }
    }
}
