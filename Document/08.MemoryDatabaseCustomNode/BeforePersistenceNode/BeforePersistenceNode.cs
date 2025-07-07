using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Example of a persistent pre-check node
    /// 持久化前置检查节点示例
    /// </summary>
    public sealed class BeforePersistenceNode : IBeforePersistenceNode
        , AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>
        , AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IEnumerableSnapshot<long>
    {
        /// <summary>
        /// A data collection with identity
        /// 带有 ID 的数据集合
        /// </summary>
        private Dictionary<long, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity>> entities;
        /// <summary>
        /// Current allocation identity
        /// 当前分配 ID
        /// </summary>
        private long currentIdentity;
        /// <summary>
        /// Example of a persistent pre-check node
        /// 持久化前置检查节点示例
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        internal BeforePersistenceNode(int capacity)
        {
            entities = DictionaryCreator<long>.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity>>(capacity);
        }
        #region A snapshot of the data collection with identity
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>.GetSnapshotCapacity(ref object customObject)
        {
            return entities.Count;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<IdentityEntity> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>.GetSnapshotResult(IdentityEntity[] snapshotArray, object customObject)
        {
            return IdentityEntity.GetSnapshotResult(snapshotArray, entities.Values);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>.SetSnapshotResult(ref LeftArray<IdentityEntity> array, ref LeftArray<IdentityEntity> newArray) { }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetEntity(IdentityEntity value)
        {
            entities.Add(value.Identity, value);
        }
        #endregion
        #region A snapshot of the currently assigned identity
        /// <summary>
        /// Single-valued snapshot
        /// 单值快照
        /// </summary>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshotEnumerable<long> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IEnumerableSnapshot<long>.SnapshotEnumerable { get { return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotGetValue<long>(getCurrentIdentity); } }
        /// <summary>
        /// Get the current allocation identity
        /// 获取当前分配 ID
        /// </summary>
        /// <returns></returns>
        private long getCurrentIdentity()
        {
            return currentIdentity;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="identity">Current allocation identity
        /// 当前分配 ID</param>
        public void SnapshotSetIdentity(long identity)
        {
            currentIdentity = identity;
        }
        #endregion
        /// <summary>
        /// Add a new data (Check the input parameters before the persistence operation)
        /// 添加一个新数据（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<long> AppendEntityBeforePersistence(IdentityEntity value)
        {
            if (value != null)
            {
                //Clean up the parameters passed by the client
                //清理客户端传参
                value.Identity = 0;
                //Returning the default value will continue to execute the AppendEntity method
                //返回默认值则继续执行 AppendEntity 方法
                return default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<long>);
            }
            //If a non-default value is returned, it will be returned directly without executing the AppendEntity method, nor will the requested data be persisted
            //返回非默认值则直接返回，不再执行 AppendEntity 方法，也不会持久化该请求数据
            return 0;
        }
        /// <summary>
        /// Add a new data
        /// 添加一个新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>New data identity. Return 0 if failed
        /// 新数据 ID，失败返回 0</returns>
        public long AppendEntity(IdentityEntity value)
        {
            if (value.Identity == 0) value.Identity = ++currentIdentity;
            entities.Add(value.Identity, value);
            return value.Identity;
        }
        /// <summary>
        /// Count +1
        /// 计数 +1
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>Return false if the identity is not found
        /// 没有找到 ID 则返回 false</returns>
        public bool Increment(long identity)
        {
            if (entities.TryGetValue(identity, out AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity> count))
            {
                count.GetNotNull().Increment();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get the current count
        /// 获取当前计数
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>If the identity is not found, return -1
        /// 没有找到 ID 则返回 -1</returns>
        public long GetCount(long identity)
        {
            if (entities.TryGetValue(identity, out AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity> count))
            {
                return count.NoCheckNotNull().Count;
            }
            return -1;
        }
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool Remove(long identity)
        {
            return entities.Remove(identity);
        }
    }
}
