using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 基于均匀概率的客户端同步总量统计节点（类似 HyperLogLog，适合小容器）
    /// </summary>
    public sealed class UniformProbabilityClientStatisticsNode : IUniformProbabilityClientStatisticsNode, ISnapshot<byte[]>
    {
        /// <summary>
        /// 最后连续 1 的数量的集合
        /// </summary>
        private byte[] bitCountArray;
        /// <summary>
        /// 获取新设置的数据的委托集合
        /// </summary>
        private LeftArray<MethodKeepCallback<int>> callbacks;
        /// <summary>
        /// 索引数组大小
        /// </summary>
        private readonly int indexCount;
        /// <summary>
        /// 索引二进制位数量
        /// </summary>
        private readonly byte indexBits;
        /// <summary>
        /// 基于均匀概率的客户端同步总量统计节点（类似 HyperLogLog，适合小容器）
        /// </summary>
        /// <param name="indexBits">The number of binary bits in the index must be even, with a minimum of 8 and a maximum of 20
        /// 索引二进制位数量，必须为偶数，最小值为 8，最大值为 20</param>
        internal unsafe UniformProbabilityClientStatisticsNode(byte indexBits)
        {
            if (indexBits <= 20)
            {
                if (indexBits >= UniformProbabilityTotalStatisticsNode.MinIndexBits) this.indexBits = (byte)(indexBits & 0x1e);
                else this.indexBits = UniformProbabilityTotalStatisticsNode.MinIndexBits;
            }
            else this.indexBits = 20;
            callbacks.SetEmpty();
            indexCount = 1 << this.indexBits;
            bitCountArray = new byte[indexCount + (65 - UniformProbabilityTotalStatisticsNode.MinIndexBits) * sizeof(int)];
            bitCountArray[bitCountArray.Length - 1] = this.indexBits;
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
            customObject = AutoCSer.Common.GetUninitializedArray<byte>(bitCountArray.Length);
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
            Buffer.BlockCopy(bitCountArray, 0, snapshotArray[0], 0, bitCountArray.Length);
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
        /// <param name="bitCountArray"></param>
        public void SnapshotSet(byte[] bitCountArray)
        {
            this.bitCountArray = bitCountArray;
        }
        /// <summary>
        /// Get data
        /// 获取数据
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        public void GetData(MethodCallback<byte[]> callback, MethodKeepCallback<int> keepCallback)
        {
            if (callback.Callback(bitCountArray))
            {
                bool isAdd = false;
                try
                {
                    callbacks.Add(keepCallback);
                    isAdd = true;
                }
                finally
                {
                    if (!isAdd) keepCallback.CancelKeep();
                }
            }
        }
        /// <summary>
        /// Try to modify the number of binary bits at the specified index position (Initialize and load the persistent data)
        /// 尝试修改指定索引位置的二进制位数量（初始化加载持久化数据）
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="bits">The number of the last consecutive binary bits 1
        /// 最后连续的二进制位 1 的数量</param>
        public void SetIndexBitLoadPersistence(int index, byte bits)
        {
            if ((uint)index < (uint)indexCount && bitCountArray[index] < bits) bitCountArray[index] = bits;
        }
        /// <summary>
        /// Try to modify the number of binary bits at the specified index position
        /// 尝试修改指定索引位置的二进制位数量
        /// </summary>
        /// <param name="index">Index position
        /// 索引位置</param>
        /// <param name="bits">The number of the last consecutive binary bits 1
        /// 最后连续的二进制位 1 的数量</param>
        public unsafe void SetIndexBit(int index, byte bits)
        {
            if ((uint)index < (uint)indexCount && bitCountArray[index] < bits)
            {
                bitCountArray[index] = bits;
                MethodKeepCallback<int>.Callback(ref callbacks, (index << 8) | bits);
            }
        }
    }
}
