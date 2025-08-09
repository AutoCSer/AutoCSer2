using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Total statistics node interface based on uniform probability (similar to HyperLogLog)
    /// 基于均匀概率的总量统计节点接口（类似 HyperLogLog）
    /// </summary>
    [ServerNode(IsLocalClient = true)]
    public partial interface IUniformProbabilityTotalStatisticsNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="bitCountArray"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(byte[] bitCountArray);
        /// <summary>
        /// Get the number of index binary bits
        /// 获取索引二进制位数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        byte GetIndexBits();
        /// <summary>
        /// Add statistical data
        /// 添加统计数据
        /// </summary>
        /// <param name="value"></param>
        void Append(ulong value);
        /// <summary>
        /// Get the statistical quantity
        /// 获取统计数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        double Count();
    }
}
