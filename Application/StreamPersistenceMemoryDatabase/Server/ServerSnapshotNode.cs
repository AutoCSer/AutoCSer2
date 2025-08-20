using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 支持快照接口的服务端节点
    /// </summary>
    /// <typeparam name="T">Node interface type
    /// 节点接口类型</typeparam>
    public sealed class ServerSnapshotNode<T> : ServerNode<T>
    {
        /// <summary>
        /// 快照接口集合
        /// </summary>
        private SnapshotNode[] snapshots;
        /// <summary>
        /// 支持快照接口的服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="target"></param>
        /// <param name="isPersistence">默认为 true 表示持久化为数据库，设置为 false 为纯内存模式在重启服务时数据将丢失</param>
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
                if (type.IsGenericType)
                {
                    Type snapshotType = type.GetGenericTypeDefinition();
                    if (snapshotType == typeof(ISnapshot<>))
                    {
                        snapshotType = type.GetGenericArguments()[0];
                        for (var baseType = snapshotType.BaseType; baseType != typeof(object) && baseType != null; baseType = baseType.BaseType)
                        {
                            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(SnapshotCloneObject<>)
                                && snapshotType == baseType.GetGenericArguments()[0])
                            {
#if AOT
                                snapshots.Add(SnapshotCloneNode.CreateMethod.MakeGenericMethod(snapshotType).Invoke(null, new object[] { targetObject }).notNullCastType<SnapshotNode>());
#else
                                snapshots.Add(SnapshotCloneObjectGenericType.Get(snapshotType).CreateSnapshotCloneNode(targetObject));
#endif
                                snapshotType = type;
                                break;
                            }
                        }
                        if (!object.ReferenceEquals(snapshotType, type))
                        {
#if AOT
                            snapshots.Add(SnapshotNode.CreateMethod.MakeGenericMethod(snapshotType).Invoke(null, new object[] { targetObject }).notNullCastType<SnapshotNode>());
#else
                            snapshots.Add(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(snapshotType).CreateSnapshotNode(targetObject));
#endif
                        }
                    }
                    else if(snapshotType == typeof(IEnumerableSnapshot<>))
                    {
#if AOT
                        snapshots.Add(EnumerableSnapshotNode.CreateMethod.MakeGenericMethod(type.GetGenericArguments()[0]).Invoke(null, new object[] { targetObject }).notNullCastType<SnapshotNode>());
#else
                        snapshots.Add(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(type.GetGenericArguments()[0]).CreateEnumerableSnapshotNode(targetObject));
#endif
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
            throw new InvalidCastException(Culture.Configuration.Default.GetServerSnapshotNodeNotImplemented(target.castObject().GetType()));
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
        /// Get the array of pre-applied snapshot containers
        /// 获取预申请快照容器数组
        /// </summary>
        internal override void GetSnapshotArray()
        {
            if (IsPersistence)
            {
                foreach (SnapshotNode snapshot in snapshots) snapshot.GetArray();
            }
        }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        /// <returns>Return false on failure</returns>
        internal override bool GetSnapshotResult()
        {
            if (!IsLoadException)
            {
                if (IsPersistence)
                {
                    foreach (SnapshotNode snapshot in snapshots) snapshot.GetResult();
                }
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
            if (IsPersistence)
            {
                bool isCreateNode = true;
                foreach (SnapshotNode snapshot in snapshots)
                {
                    if (!snapshot.Rebuild(rebuilder, isCreateNode)) return false;
                    isCreateNode = false;
                }
                return true;
            }
            return rebuilder.Rebuild();
        }
    }
}
