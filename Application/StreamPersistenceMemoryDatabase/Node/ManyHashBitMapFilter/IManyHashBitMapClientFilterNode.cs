using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器）
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IManyHashBitMapClientFilterNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="map">多哈希位图数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ManyHashBitMap map);
        /// <summary>
        /// 获取设置新位操作
        /// </summary>
        /// <param name="callback">设置位置</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetBit(MethodKeepCallback<int> callback);
        /// <summary>
        /// 获取位图大小（位数量）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetSize();
        /// <summary>
        /// 获取当前位图数据
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ManyHashBitMap GetData();
        /// <summary>
        /// 设置位 持久化前检查
        /// </summary>
        /// <param name="bit">位置</param>
        /// <returns>是否继续持久化操作</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetBitBeforePersistence(int bit);
        /// <summary>
        /// 设置位
        /// </summary>
        /// <param name="bit">位置</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void SetBit(int bit);
    }
}
