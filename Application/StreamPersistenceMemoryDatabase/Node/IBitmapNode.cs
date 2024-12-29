using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 位图节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IBitmapNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="map"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(byte[] map);
        /// <summary>
        /// 获取二进制位数量
        /// </summary>
        uint GetCapacity();
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void ClearMap();
        /// <summary>
        /// 读取位状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<int> GetBit(uint index);
        /// <summary>
        /// 设置位状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>是否设置成功，失败表示索引超出范围</returns>
        bool SetBit(uint index);
        /// <summary>
        /// 设置位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        ValueResult<int> GetBitSetBit(uint index);
        /// <summary>
        /// 清除位状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>是否设置成功，失败表示索引超出范围</returns>
        bool ClearBit(uint index);
        /// <summary>
        /// 清除位状态并返回设置之前的状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        ValueResult<int> GetBitClearBit(uint index);
        /// <summary>
        /// 状态取反
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>是否设置成功，失败表示索引超出范围</returns>
        bool InvertBit(uint index);
        /// <summary>
        /// 状态取反并返回操作之前的状态
        /// </summary>
        /// <param name="index">位索引</param>
        /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
        ValueResult<int> GetBitInvertBit(uint index);
    }
}
