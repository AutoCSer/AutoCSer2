using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点（类似布隆过滤器）
    /// </summary>
    public sealed class ManyHashBitMapFilterNode : IManyHashBitMapFilterNode, ISnapshot<ManyHashBitMap>
    {
        /// <summary>
        /// 多哈希位图数据
        /// </summary>
        private ManyHashBitMap map;
        /// <summary>
        /// 多哈希位图过滤节点（类似布隆过滤器）
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        public ManyHashBitMapFilterNode(int size)
        {
            map.Set(size);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<ManyHashBitMap> GetSnapshotResult(ManyHashBitMap[] snapshotArray, object customObject)
        {
            snapshotArray[0] = map;
            return new SnapshotResult<ManyHashBitMap>(1);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<ManyHashBitMap> array, ref LeftArray<ManyHashBitMap> newArray) { }
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
        public ValueResult<bool> SetBitsBeforePersistence(int size, int[] bits)
        {
            if (size == map.Size && bits != null)
            {
                foreach (int bit in bits)
                {
                    if (map.GetBitValueBeforePersistence(bit) == 0) return default(ValueResult<bool>);
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
        public bool SetBits(int size, int[] bits)
        {
            foreach (int bit in bits) map.SetBit(bit);
            return true;
        }
        /// <summary>
        /// 检查位图数据
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="bits">位置集合</param>
        /// <returns>返回 false 表示数据不存在</returns>
        public NullableBoolEnum CheckBits(int size, int[] bits)
        {
            if (size == map.Size && bits != null)
            {
                foreach (int bit in bits)
                {
                    if (map.GetBitValue(bit) == 0) return NullableBoolEnum.False;
                }
                return NullableBoolEnum.True;
            }
            return NullableBoolEnum.Null;
        }
    }
}
