using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Client statistics node interface based on uniform probability (similar to HyperLogLog, suitable for small containers)
    /// 基于均匀概率的客户端同步总量统计节点接口（类似 HyperLogLog，适合小容器）
    /// </summary>
    [ServerNode]
    public partial interface IUniformProbabilityClientStatisticsNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="bitCountArray"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(byte[] bitCountArray);
        /// <summary>
        /// Get the array of binary bits
        /// 获取二进制位数量的数组
        /// </summary>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetBitArray(MethodCallback<byte[]> callback);
        /// <summary>
        /// Get the newly set data
        /// 获取新设置的数据
        /// </summary>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetIndexBit(MethodKeepCallback<int> callback);
        /// <summary>
        /// Try to modify the number of binary bits at the specified index position (Initialize and load the persistent data)
        /// 尝试修改指定索引位置的二进制位数量（初始化加载持久化数据）
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="bits">The number of the last consecutive binary bits 1
        /// 最后连续的二进制位 1 的数量</param>
        void SetIndexBitLoadPersistence(int index, byte bits);
        /// <summary>
        /// Try to modify the number of binary bits at the specified index position
        /// 尝试修改指定索引位置的二进制位数量
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="bits">The number of the last consecutive binary bits 1
        /// 最后连续的二进制位 1 的数量</param>
        void SetIndexBit(int index, byte bits);
    }
}
