using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点（类似布隆过滤器）
    /// </summary>
    public sealed class ManyHashBitMapFilterNode : IManyHashBitMapFilterNode, IEnumerableSnapshot<ManyHashBitMap>
    {
        /// <summary>
        /// 多哈希位图数据
        /// </summary>
        private ManyHashBitMap map;
        /// <summary>
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<ManyHashBitMap> IEnumerableSnapshot<ManyHashBitMap>.SnapshotEnumerable { get { return new SnapshotValue<ManyHashBitMap>(map); } }
        /// <summary>
        /// 多哈希位图过滤节点（类似布隆过滤器）
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        public ManyHashBitMapFilterNode(int size)
        {
            map.Set(size);
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="map">多哈希位图数据</param>
        public void SnapshotSet(ManyHashBitMap map)
        {
            this.map = map;
        }
        /// <summary>
        /// 获取位图大小（位数量）
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return map.Size;
        }
        /// <summary>
        /// 设置位 持久化前检查
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示位图大小不匹配</returns>
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
        /// 设置位
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示位图大小不匹配</returns>
        public bool SetBits(int size, uint[] bits)
        {
            foreach (uint bit in bits) map.SetBit((int)bit);
            return true;
        }
        /// <summary>
        /// 检查位图数据
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示数据不存在</returns>
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
