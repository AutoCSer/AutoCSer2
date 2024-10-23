using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 位图节点
    /// </summary>
    public class BitmapNode : IBitmapNode, ISnapshot<byte[]>
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
        /// <param name="capacity">二进制位数量</param>
        public BitmapNode(uint capacity)
        {
            this.capacity = capacity;
            map = new byte[((ulong)capacity + 7) >> 3];
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<byte[]> GetSnapshotArray()
        {
            LeftArray<byte[]> array = new LeftArray<byte[]>(1);
            array.Add(AutoCSer.Common.Config.GetUninitializedArray(map, map.Length));
            return array;
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="map">位图</param>
        public void SnapshotSet(byte[] map)
        {
            this.map = map;
        }
        /// <summary>
        /// 获取二进制位数量
        /// </summary>
        public uint GetCapacity() { return capacity; }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void ClearMap()
        {
            Array.Clear(map,0, map.Length);
        }
        /// <summary>
        /// 读取位
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBit(uint index)
        {
            if (index < capacity) return map[(int)(index >> 3)] & (1 << (int)(index & 7));
            return default(ValueResult<int>);
        }
        /// <summary>
        /// 设置位状态 持久化参数检查
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetBitBeforePersistence(uint index)
        {
            if (index < capacity) return default(ValueResult<bool>);
            return false;
        }
        /// <summary>
        /// 设置位状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>是否设置成功，失败表示索引超出范围</returns>
        public bool SetBit(uint index)
        {
            map[(int)(index >> 3)] |= (byte)(1 << (int)(index & 7));
            return true;
        }
        /// <summary>
        /// 设置位状态并返回设置之前的状态 持久化参数检查
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<int>> GetBitSetBitBeforePersistence(uint index)
        {
            if (index < capacity) return default(ValueResult<ValueResult<int>>);
            return default(ValueResult<int>);
        }
        /// <summary>
        /// 设置位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBitSetBit(uint index)
        {
            int mapIndex = (int)(index >> 3), bitValue = 1 << (int)(index & 7), value = map[mapIndex] & bitValue;
            map[mapIndex] |= (byte)bitValue;
            return value;
        }
        /// <summary>
        /// 清除位状态 持久化参数检查
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> ClearBitBeforePersistence(uint index)
        {
            return SetBitBeforePersistence(index);
        }
        /// <summary>
        /// 清除位状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>是否设置成功，失败表示索引超出范围</returns>
        public bool ClearBit(uint index)
        {
            map[(int)(index >> 3)] &= (byte)((1 << (int)(index & 7)) ^ byte.MaxValue);
            return true;
        }
        /// <summary>
        /// 清除位状态并返回设置之前的状态 持久化参数检查
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<int>> GetBitClearBitBeforePersistence(uint index)
        {
            return GetBitSetBitBeforePersistence(index);
        }
        /// <summary>
        /// 清除位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBitClearBit(uint index)
        {
            int mapIndex = (int)(index >> 3), bitValue = 1 << (int)(index & 7), value = map[mapIndex] & bitValue;
            map[mapIndex] &= (byte)(bitValue ^ byte.MaxValue);
            return value;
        }
        /// <summary>
        /// 状态取反 持久化参数检查
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> InvertBitBeforePersistence(uint index)
        {
            return SetBitBeforePersistence(index);
        }
        /// <summary>
        /// 状态取反
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>是否设置成功，失败表示索引超出范围</returns>
        public bool InvertBit(uint index)
        {
            map[(int)(index >> 3)] ^= (byte)(1 << (int)(index & 7));
            return true;
        }
        /// <summary>
        /// 状态取反并返回操作之前的状态 持久化参数检查
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<int>> GetBitInvertBitBeforePersistence(uint index)
        {
            return GetBitSetBitBeforePersistence(index);
        }
        /// <summary>
        /// 状态取反并返回操作之前的状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        public ValueResult<int> GetBitInvertBit(uint index)
        {
            int mapIndex = (int)(index >> 3), bitValue = 1 << (int)(index & 7), value = map[mapIndex] & bitValue;
            map[mapIndex] ^= (byte)bitValue;
            return value;
        }
    }
}
