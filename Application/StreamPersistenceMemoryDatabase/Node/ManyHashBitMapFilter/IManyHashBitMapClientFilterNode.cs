using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Multi-hash bitmap client synchronization filter node Interface (similar to Bloom filter, suitable for small containers)
    /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器）
    /// </summary>
    [ServerNode]
    public partial interface IManyHashBitMapClientFilterNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="map">Multi-hash bitmap data
        /// 多哈希位图数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ManyHashBitMap map);
        /// <summary>
        /// Get the bitmap size (number of bits)
        /// 获取位图大小（位数量）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetSize();
        /// <summary>
        /// Get data
        /// 获取数据
        /// </summary>
        /// <param name="callback">Get the current bitmap data
        /// 获取当前位图数据</param>
        /// <param name="keepCallback">Get the operation of setting a new bit
        /// 获取设置新位操作</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetData(MethodCallback<ManyHashBitMap> callback, MethodKeepCallback<int> keepCallback);
        /// <summary>
        /// Set bit (Check the input parameters before the persistence operation)
        /// 设置位（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="bit">The set binary bit
        /// 设置的二进制位</param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetBitBeforePersistence(int bit);
        /// <summary>
        /// Set bit
        /// 设置位
        /// </summary>
        /// <param name="bit">The set binary bit
        /// 设置的二进制位</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void SetBit(int bit);
    }
}
