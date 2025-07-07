using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点（类似布隆过滤器）
    /// </summary>
    public sealed class ManyHashBitMapFilterNode : IManyHashBitMapFilterNode, IEnumerableSnapshot<ManyHashBitMap>
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
        /// 多哈希位图过滤节点（类似布隆过滤器）
        /// </summary>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        public ManyHashBitMapFilterNode(int size)
        {
            map.Set(size);
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
        /// Get the bitmap size (number of bits)
        /// 获取位图大小（位数量）
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return map.Size;
        }
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
        public ValueResult<bool> SetBitsBeforePersistence(int size, uint[] bits)
        {
            if (size == map.Size && bits != null)
            {
                foreach (uint bit in bits)
                {
                    if (map.GetBitValueBeforePersistence((int)bit) == 0) return default(ValueResult<bool>);
                }
                return true;
            }
            return false;
        }
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
        public bool SetBits(int size, uint[] bits)
        {
            foreach (uint bit in bits) map.SetBit((int)bit);
            return true;
        }
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
        public NullableBoolEnum CheckBits(int size, uint[] bits)
        {
            if (size == map.Size && bits != null)
            {
                foreach (uint bit in bits)
                {
                    if (map.GetBitValue((int)bit) == 0) return NullableBoolEnum.False;
                }
                return NullableBoolEnum.True;
            }
            return NullableBoolEnum.Null;
        }
    }
}
