using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Multi-hash bitmap filter node interface (similar to Bloom Filter)
    /// 多哈希位图过滤节点接口（类似布隆过滤器）
    /// </summary>
    [ServerNode(IsLocalClient = true)]
    public partial interface IManyHashBitMapFilterNode
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
        /// Set bit (Check the input parameters before the persistence operation)
        /// 设置位（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="bits">Binary bit set
        /// 位置集合</param>
        /// <returns>Returning false indicates that the bitmap size does not match
        /// 返回 false 表示位图大小不匹配</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<bool> SetBitsBeforePersistence(int size, uint[] bits);
        /// <summary>
        /// Set bit
        /// 设置位
        /// </summary>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="bits">Binary bit set
        /// 位置集合</param>
        /// <returns>Returning false indicates that the bitmap size does not match
        /// 返回 false 表示位图大小不匹配</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetBits(int size, uint[] bits);
        /// <summary>
        /// Binary bit set matching
        /// 位置集合匹配
        /// </summary>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="bits">Binary bit set
        /// 位置集合</param>
        /// <returns>Returning Null indicates that the bitmap does not match
        /// 返回 Null 表示位图不匹配</returns>
        [ServerMethod(IsPersistence = false)]
        NullableBoolEnum CheckBits(int size, uint[] bits);
    }
}
