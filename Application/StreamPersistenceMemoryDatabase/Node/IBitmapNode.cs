using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Bitmap node interface
    /// 位图节点接口
    /// </summary>
    [ServerNode(IsLocalClient = true)]
    public partial interface IBitmapNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="map"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(byte[] map);
        /// <summary>
        /// Get the number of bitmap binary bits
        /// 获取位图二进制位数量
        /// </summary>
        uint GetCapacity();
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void ClearMap();
        /// <summary>
        /// Read bit status
        /// 读取位状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>A non-0 indicates that the binary bit is in the set state. If the index exceeds, there will be no return value
        /// 非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<int> GetBit(uint index);
        /// <summary>
        /// Set bit status
        /// 设置位状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool SetBit(uint index);
        /// <summary>
        /// Set the bit state and return the state before setting
        /// 设置位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>The state before setting: A non-0 indicates that the binary bit was in the set state before, and there is no return value if the index exceeds
        /// 设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<int> GetBitSetBit(uint index);
        /// <summary>
        /// Clear bit status
        /// 清除位状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool ClearBit(uint index);
        /// <summary>
        /// Clear the bit state and return to the state before setting
        /// 清除位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Clear the state before the operation. A non-0 state indicates that the binary bit was in the set state before. If the index exceeds, there will be no return value
        /// 清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<int> GetBitClearBit(uint index);
        /// <summary>
        /// Reverse the bit state
        /// 位状态取反
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool InvertBit(uint index);
        /// <summary>
        /// Reverse the bit state and return the state before the operation
        /// 位状态取反并返回操作之前的状态
        /// </summary>
        /// <param name="index">Bit index position
        /// 位索引位置</param>
        /// <returns>Take the state before the reverse operation. If it is not 0, it indicates that the binary bit is in the set state before. If the index exceeds, there will be no return value
        /// 取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<int> GetBitInvertBit(uint index);
    }
}
