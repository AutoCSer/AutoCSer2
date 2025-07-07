using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Multi-hash bitmap client synchronization filter node (similar to Bloom filter, suitable for small containers)
    /// 多哈希位图客户端同步过滤节点（类似布隆过滤器，适合小容器）
    /// </summary>
    public sealed class ManyHashBitMapClientFilterNode : IManyHashBitMapClientFilterNode, IEnumerableSnapshot<ManyHashBitMap>
    {
        /// <summary>
        /// Multi-hash bitmap data
        /// 多哈希位图数据
        /// </summary>
        private ManyHashBitMap map;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<ManyHashBitMap> IEnumerableSnapshot<ManyHashBitMap>.SnapshotEnumerable { get { return new SnapshotValue<ManyHashBitMap>(map); } }
        /// <summary>
        /// Set the collection of delegates for the new bit callback
        /// 设置新位回调委托集合
        /// </summary>
        private LeftArray<MethodKeepCallback<int>> callbacks;
        /// <summary>
        /// Multi-hash bitmap client synchronization filter node (similar to Bloom filter, suitable for small containers)
        /// 多哈希位图客户端同步过滤节点（类似布隆过滤器，适合小容器）
        /// </summary>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        public ManyHashBitMapClientFilterNode(int size)
        {
            map.Set(size);
            callbacks.SetEmpty();
        }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="map">Multi-hash bitmap data
        /// 多哈希位图数据</param>
        public void SnapshotSet(ManyHashBitMap map)
        {
            this.map = map;
        }
        /// <summary>
        /// Get the operation of setting a new bit
        /// 获取设置新位操作
        /// </summary>
        /// <param name="callback"></param>
        public void GetBit(MethodKeepCallback<int> callback)
        {
            callbacks.Add(callback);
        }
        /// <summary>
        /// Get the bitmap size (number of bits)
        /// 获取位图大小（位数量）
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return map.Size;
        }
        /// <summary>
        /// Get the current bitmap data
        /// 获取当前位图数据
        /// </summary>
        /// <returns></returns>
        public ManyHashBitMap GetData()
        {
            return map;
        }
        /// <summary>
        /// Set bit (Check the input parameters before the persistence operation)
        /// 设置位（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="bit">The set binary bit
        /// 设置的二进制位</param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        public bool SetBitBeforePersistence(int bit)
        {
            return map.GetBitValueBeforePersistence(bit) == 0;
        }
        /// <summary>
        /// Set bit
        /// 设置位
        /// </summary>
        /// <param name="bit">The set binary bit
        /// 设置的二进制位</param>
        public void SetBit(int bit)
        {
            if (map.CheckSetBit(bit)) MethodKeepCallback<int>.Callback(ref callbacks, bit);
        }
    }
}
