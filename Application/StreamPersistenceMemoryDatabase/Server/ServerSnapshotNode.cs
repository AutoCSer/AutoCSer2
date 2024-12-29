using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 支持快照接口的服务端节点
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    public sealed class ServerSnapshotNode<T> : ServerNode<T>
    {
        /// <summary>
        /// 当前节点是否支持重建
        /// </summary>
        internal override bool IsRebuild { get { return IsPersistence; } }
        /// <summary>
        /// 快照接口集合
        /// </summary>
        private SnapshotNode[] snapshots;
        /// <summary>
        /// 支持快照接口的服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="index"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="target"></param>
        /// <param name="isPersistence">默认为 true 表示持久化为数据库，设置为 false 为纯内存模式在重启服务是数据将丢失</param>
        internal ServerSnapshotNode(StreamPersistenceMemoryDatabaseService service, NodeIndex index, string key, T target, bool isPersistence = true) : base(service, index, key, target, isPersistence)
        {
            snapshots = getSnapshots(target);
        }
        /// <summary>
        /// 获取快照接口集合
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private SnapshotNode[] getSnapshots(T target)
        {
            LeftArray<SnapshotNode> snapshots = new LeftArray<SnapshotNode>(1);
            object targetObject = target.castObject();
            foreach (Type type in targetObject.GetType().GetInterfaces())
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ISnapshot<>))
                {
                    Type snapshotType = type.GetGenericArguments()[0];
                    for (var baseType = snapshotType.BaseType; baseType != typeof(object) && baseType != null; baseType = baseType.BaseType)
                    {
                        if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(SnapshotCloneObject<>)
                            && snapshotType == baseType.GetGenericArguments()[0])
                        {
                            snapshots.Add(SnapshotCloneObjectGenericType.Get(snapshotType).CreateSnapshotCloneNode(targetObject));
                            snapshotType = type;
                            break;
                        }
                    }
                    if (!object.ReferenceEquals(snapshotType, type))
                    {
                        snapshots.Add(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(snapshotType).CreateSnapshotNode(targetObject));
                    }
                }
            }
            return snapshots.ToArray();
        }
        /// <summary>
        /// 检查操作节点对象是否实现快照接口
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected override T checkNewTarget(T target)
        {
            SnapshotNode[] snapshots = getSnapshots(target);
            if (snapshots.Length != 0)
            {
                this.snapshots = snapshots;
                return target;
            }
            throw new InvalidCastException($"服务端节点类型 {target.castObject().GetType().fullName()} 缺少快照接口实现 {typeof(ISnapshot<>).fullName()}");
        }
        /// <summary>
        /// 关闭重建
        /// </summary>
        internal override void CloseRebuild()
        {
            Rebuilding = false;
            foreach (SnapshotNode snapshot in snapshots) snapshot.Close();
        }
        /// <summary>
        /// 检查快照重建状态
        /// </summary>
        /// <returns></returns>
        internal override bool CheckSnapshot()
        {
            Rebuilding = false;
            return !IsLoadException;
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        internal override void GetSnapshotArray()
        {
            foreach (SnapshotNode snapshot in snapshots) snapshot.GetArray();
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <returns>是否成功</returns>
        internal override bool GetSnapshotResult()
        {
            if (!IsLoadException)
            {
                foreach (SnapshotNode snapshot in snapshots) snapshot.GetResult();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        /// <returns></returns>
        internal override bool Rebuild(PersistenceRebuilder rebuilder)
        {
            foreach (SnapshotNode snapshot in snapshots)
            {
                if (!snapshot.Rebuild(rebuilder)) return false;
            }
            return true;
        }
    }
}
