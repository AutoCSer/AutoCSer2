using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图客户端同步过滤节点（类似布隆过滤器，适合小容器）
    /// </summary>
    public sealed class ManyHashBitMapClientFilterNode : IManyHashBitMapClientFilterNode, IEnumerableSnapshot<ManyHashBitMap>
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
        /// 设置新位回调委托集合
        /// </summary>
        private LeftArray<MethodKeepCallback<int>> callbacks;
        /// <summary>
        /// 多哈希位图客户端同步过滤节点（类似布隆过滤器，适合小容器）
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        public ManyHashBitMapClientFilterNode(int size)
        {
            map.Set(size);
            callbacks.SetEmpty();
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
        /// 获取设置新位操作
        /// </summary>
        /// <param name="callback">设置位置</param>
        public void GetBit(MethodKeepCallback<int> callback)
        {
            callbacks.Add(callback);
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
        /// 获取当前位图数据
        /// </summary>
        /// <returns></returns>
        public ManyHashBitMap GetData()
        {
            return map;
        }
        /// <summary>
        /// 设置位 持久化前检查
        /// </summary>
        /// <param name="bit">位置</param>
        /// <returns>是否继续持久化操作</returns>
        public bool SetBitBeforePersistence(int bit)
        {
            return map.GetBitValueBeforePersistence(bit) == 0;
        }
        /// <summary>
        /// 设置位
        /// </summary>
        /// <param name="bit">位置</param>
        public void SetBit(int bit)
        {
            if (map.CheckSetBit(bit)) MethodKeepCallback<int>.Callback(ref callbacks, bit);
        }
    }
}
