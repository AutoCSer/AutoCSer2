using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点接口（类似布隆过滤器）
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IManyHashBitMapFilterNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="map">多哈希位图数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ManyHashBitMap map);
        /// <summary>
        /// 获取位图大小（位数量）
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetSize();
        /// <summary>
        /// 设置位 持久化前检查
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示位图大小不匹配</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<bool> SetBitsBeforePersistence(int size, uint[] bits);
        /// <summary>
        /// 设置位
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示位图大小不匹配</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetBits(int size, uint[] bits);
        /// <summary>
        /// 检查位图数据
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示数据不存在</returns>
        [ServerMethod(IsPersistence = false)]
        NullableBoolEnum CheckBits(int size, uint[] bits);
    }
}
