using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 位图节点
    /// </summary>
    public sealed class BitmapNode : IBitmapNode, ISnapshot<byte[]>
    {
        /// <summary>
        /// 位图
        /// </summary>
        private byte[] map;
        /// <summary>
        /// 二进制位数量
        /// </summary>
        private readonly uint capacity;
        /// <summary>
        /// 位图节点
        /// </summary>
        /// <param name="capacity">The number of binary bits
        /// 二进制位数量</param>
        public BitmapNode(uint capacity)
        {
            this.capacity = capacity;
            map = new byte[((ulong)capacity + 7) >> 3];
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            customObject = AutoCSer.Common.GetUninitializedArray<byte>(map.Length);
            return 1;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        public SnapshotResult<byte[]> GetSnapshotResult(byte[][] snapshotArray, object customObject)
        {
            snapshotArray[0] = (byte[])customObject;
            Buffer.BlockCopy(map, 0, snapshotArray[0], 0, map.Length);
            return new SnapshotResult<byte[]>(1);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<byte[]> array, ref LeftArray<byte[]> newArray) { }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="map">位图</param>
        public void SnapshotSet(byte[] map)
        {
            this.map = map;
        }
        /// <summary>
        /// Get the number of bitmap binary bits
        /// 获取位图二进制位数量
        /// </summary>
        public uint GetCapacity() { return capacity; }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void ClearMap()
        {
            Array.Clear(map,0, map.Length);
        }
        /// <summary>
        /// Read bit status
        /// 读取位状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>A non-0 indicates that the binary bit is in the set state. If the index exceeds, there will be no return value
        /// 非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBit(uint index)
        {
            if (index < capacity) return map[(int)(index >> 3)] & (1 << (int)(index & 7));
            return default(ValueResult<int>);
        }
        /// <summary>
        /// Set bit status
        /// 设置位状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        public bool SetBit(uint index)
        {
            if (index < capacity)
            {
                map[(int)(index >> 3)] |= (byte)(1 << (int)(index & 7));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Set the bit state and return the state before setting
        /// 设置位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>The state before setting: A non-0 indicates that the binary bit was in the set state before, and there is no return value if the index exceeds
        /// 设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBitSetBit(uint index)
        {
            if (index < capacity)
            {
                int mapIndex = (int)(index >> 3), bitValue = 1 << (int)(index & 7), value = map[mapIndex] & bitValue;
                map[mapIndex] |= (byte)bitValue;
                return value;
            }
            return default(ValueResult<int>);
        }
        /// <summary>
        /// Clear bit status
        /// 清除位状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        public bool ClearBit(uint index)
        {
            if (index < capacity)
            {
                map[(int)(index >> 3)] &= (byte)((1 << (int)(index & 7)) ^ byte.MaxValue);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Clear the bit state and return to the state before setting
        /// 清除位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Clear the state before the operation. A non-0 state indicates that the binary bit was in the set state before. If the index exceeds, there will be no return value
        /// 清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBitClearBit(uint index)
        {
            if (index < capacity)
            {
                int mapIndex = (int)(index >> 3), bitValue = 1 << (int)(index & 7), value = map[mapIndex] & bitValue;
                map[mapIndex] &= (byte)(bitValue ^ byte.MaxValue);
                return value;
            }
            return default(ValueResult<int>);
        }
        /// <summary>
        /// Reverse the bit state
        /// 位状态取反
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        public bool InvertBit(uint index)
        {
            if (index < capacity)
            {
                map[(int)(index >> 3)] ^= (byte)(1 << (int)(index & 7));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Reverse the bit state and return the state before the operation
        /// 状态取反并返回操作之前的状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Take the state before the reverse operation. If it is not 0, it indicates that the binary bit is in the set state before. If the index exceeds, there will be no return value
        /// 取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBitInvertBit(uint index)
        {
            if (index < capacity)
            {
                int mapIndex = (int)(index >> 3), bitValue = 1 << (int)(index & 7), value = map[mapIndex] & bitValue;
                map[mapIndex] ^= (byte)bitValue;
                return value;
            }
            return default(ValueResult<int>);
        }
    }
}
