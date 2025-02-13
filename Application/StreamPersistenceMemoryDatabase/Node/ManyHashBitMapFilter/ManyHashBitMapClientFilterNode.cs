using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图客户端同步过滤节点（类似布隆过滤器，适合小容器）
    /// </summary>
    public sealed class ManyHashBitMapClientFilterNode : IManyHashBitMapClientFilterNode, ISnapshot<ManyHashBitMap>
    {
        /// <summary>
        /// 多哈希位图数据
        /// </summary>
        private ManyHashBitMap map;
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
        /// 设置新位回调
        /// </summary>
        /// <param name="bit"></param>
        private void callback(int bit)
        {
            int count = callbacks.Length;
            if (count != 0)
            {
                MethodKeepCallback<int>[] callbackArray = callbacks.Array;
                do
                {
                    if (!callbackArray[--count].Callback(bit)) callbacks.RemoveToEnd(count);
                }
                while (count != 0);
            }
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
            if (map.CheckSetBit(bit)) callback(bit);
        }
    }
}
