using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 持久化前置检查示例节点
    /// </summary>
    public sealed class BeforePersistenceNode : IBeforePersistenceNode
        , AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>
        , AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>
    {
        /// <summary>
        /// 带有 ID 的数据集合
        /// </summary>
        private Dictionary<long, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity>> entities;
        /// <summary>
        /// 当前分配 ID
        /// </summary>
        private long currentIdentity;
        /// <summary>
        /// 字典计数器节点
        /// </summary>
        /// <param name="capacity">初始化容器尺寸</param>
        internal BeforePersistenceNode(int capacity)
        {
            entities = DictionaryCreator<long>.Create<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity>>(capacity);
        }
        #region 带有 ID 的数据集合快照
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>.GetSnapshotCapacity(ref object customObject)
        {
            return entities.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<IdentityEntity> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>.GetSnapshotResult(IdentityEntity[] snapshotArray, object customObject)
        {
            return IdentityEntity.GetSnapshotResult(snapshotArray, entities.Values);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<IdentityEntity>.SetSnapshotResult(ref LeftArray<IdentityEntity> array, ref LeftArray<IdentityEntity> newArray) { }
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSetEntity(IdentityEntity value)
        {
            entities.Add(value.Identity, value);
        }
        #endregion
        #region 当前分配 ID 快照
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>.GetSnapshotCapacity(ref object customObject)
        {
            return 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<long> AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>.GetSnapshotResult(long[] snapshotArray, object customObject)
        {
            snapshotArray[0] = currentIdentity;
            return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotResult<long>(1);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISnapshot<long>.SetSnapshotResult(ref LeftArray<long> array, ref LeftArray<long> newArray) { }
        /// <summary>
        /// 快照设置数据，从快照数据恢复内存数据
        /// </summary>
        /// <param name="identity">当前分配 ID</param>
        public void SnapshotSetIdentity(long identity)
        {
            currentIdentity = identity;
        }
        #endregion
        /// <summary>
        /// 添加一个新数据（持久化前置检查，客户端不可见）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<long> AppendEntityBeforePersistence(IdentityEntity value)
        {
            if (value != null)
            {
                value.Identity = 0;//清理客户端传参
                return default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<long>);//返回默认值则继续执行 AppendEntity 方法
            }
            return 0;//非默认值则直接返回，不再执行 AppendEntity 方法，也不会持久化该请求数据
        }
        /// <summary>
        /// 添加一个新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>新数据 ID，失败返回 0</returns>
        public long AppendEntity(IdentityEntity value)
        {
            if (value.Identity == 0) value.Identity = ++currentIdentity;
            entities.Add(value.Identity, value);
            return value.Identity;
        }
        /// <summary>
        /// 计数 +1
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>没有找到 ID 则返回 false</returns>
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
        /// 获取当前计数
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>没有找到 ID 则返回 -1</returns>
        public long GetCount(long identity)
        {
            if (entities.TryGetValue(identity, out AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CheckSnapshotCloneObject<IdentityEntity> count))
            {
                return count.NoCheckNotNull().Count;
            }
            return -1;
        }
        /// <summary>
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
