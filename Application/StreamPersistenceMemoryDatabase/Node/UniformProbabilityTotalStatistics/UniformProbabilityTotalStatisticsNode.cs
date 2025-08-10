using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 基于均匀概率的总量统计节点（类似 HyperLogLog）
    /// </summary>
    public sealed class UniformProbabilityTotalStatisticsNode : ContextNode<IUniformProbabilityTotalStatisticsNode, byte[]>, IUniformProbabilityTotalStatisticsNode, ISnapshot<byte[]>
    {
        /// <summary>
        /// 索引二进制位最小数量
        /// </summary>
        internal const byte MinIndexBits = 8;

        /// <summary>
        /// 最后连续 1 的数量的集合
        /// </summary>
        private byte[] bitCountArray;
        /// <summary>
        /// 获取索引的二进制位的低位
        /// </summary>
        private readonly uint indexLow;
        /// <summary>
        /// 获取索引的二进制位的高位
        /// </summary>
        private readonly uint indexHigh;
        /// <summary>
        /// 数据的二进制位
        /// </summary>
        private readonly ulong valueMark;
        /// <summary>
        /// 获取索引的二进制位的低位的数量
        /// </summary>
        private readonly byte indexLowBits;
        /// <summary>
        /// 获取索引的二进制位的高位需要右移位的数量
        /// </summary>
        private readonly byte indexShiftBits;
        /// <summary>
        /// 索引二进制位数量
        /// </summary>
        private readonly byte indexBits;
        /// <summary>
        /// 当前最大连续数量
        /// </summary>
        private byte maxBits;
        /// <summary>
        /// 索引数组大小
        /// </summary>
        private readonly int indexCount;
        /// <summary>
        /// 当前倒数和
        /// </summary>
        private double bitReciprocalSum;
        /// <summary>
        /// 倒数和的倒数数量
        /// </summary>
        private double sumCount;
        /// <summary>
        /// 统计数量
        /// </summary>
        private double totalCount;
        /// <summary>
        /// 基于均匀概率的总量统计节点（类似 HyperLogLog）
        /// </summary>
        /// <param name="indexBits">The number of binary bits in the index must be even, with a minimum of 8 and a maximum of 20
        /// 索引二进制位数量，必须为偶数，最小值为 8，最大值为 20</param>
        public UniformProbabilityTotalStatisticsNode(byte indexBits)
        {
            if (indexBits <= 20)
            {
                if (indexBits >= MinIndexBits) indexLowBits = (byte)(indexBits >> 1);
                else indexLowBits = (MinIndexBits >> 1);
            }
            else indexLowBits = 10;
            this.indexBits = (byte)(indexLowBits << 1);
            indexLow = (1U << indexLowBits) - 1;
            indexShiftBits = (byte)(64 - (indexLowBits << 1));
            indexCount = 1 << this.indexBits;
            indexHigh = indexLow << indexLowBits;
            valueMark = (1UL << indexShiftBits) - 1;
            bitCountArray = new byte[indexCount + (65 - MinIndexBits) * sizeof(int)];
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public unsafe override IUniformProbabilityTotalStatisticsNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public unsafe override IUniformProbabilityTotalStatisticsNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            fixed (byte* bitFixed = bitCountArray)
            {
                int* counts = (int*)(bitFixed + indexCount);
                byte bits = 64 - MinIndexBits, maxBits = 0;
                do
                {
                    int count = counts[bits];
                    if (count != 0)
                    {
                        if (maxBits == 0) maxBits = bits;
                        sumCount += (count >> 8) * bits + (count & 0xff);
                    }
                }
                while (--bits != 0);
                if (maxBits != 0) setMaxBits(maxBits, counts);
            }
            return this;
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
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="bitCountArray"></param>
        public unsafe void SnapshotSet(byte[] bitCountArray)
        {
            this.bitCountArray = bitCountArray;
        }
        /// <summary>
        /// Get the number of index binary bits
        /// 获取索引二进制位数量
        /// </summary>
        /// <returns></returns>
        public byte GetIndexBits()
        {
            return indexBits;
        }
        /// <summary>
        /// Add statistical data
        /// 添加统计数据
        /// </summary>
        /// <param name="value"></param>
        public void Append(ulong value)
        {
            //value += AutoCSer.Memory.Common.AddHashCode;
            int index = (int)(((uint)value & indexLow) | ((uint)(value >> indexShiftBits) & indexHigh));
            value = (value >> indexLowBits) & valueMark;
            ulong markValue = (1UL << (bitCountArray[index] + 1)) - 1;
            if ((value & markValue) != markValue) return;
            //计算最后连续 1 的数量
            value ^= value + 1;
            if ((uint)value != uint.MaxValue) set(index, (byte)(((uint)value + 1).deBruijnLog2() - 1));
            else if ((value & 0xffffffff00000000UL) != 0) set(index, (byte)(((uint)(value >> 32) + 1).deBruijnLog2() + 31));
            else set(index, 31);
        }
        /// <summary>
        /// 设置最后连续 1 的数量
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bits"></param>
        private unsafe void set(int index, byte bits)
        {
            double* reciprocal = (double*)BitReciprocal.Data;
            fixed (byte* bitFixed = bitCountArray)
            {
                int* counts = (int*)(bitFixed + indexCount);
                byte lastBits = bitFixed[index];
                bool isLoaded = StreamPersistenceMemoryDatabaseService == null || StreamPersistenceMemoryDatabaseService.IsLoaded;
                //bool isLoaded = StreamPersistenceMemoryDatabaseService.IsLoaded;
                int* incrementCount = counts + bits, decrementCount = counts + lastBits;
                bitFixed[index] = bits;
                *incrementCount = CountIncrement(*incrementCount, bits);
                *decrementCount = CountDecrement(*decrementCount, lastBits);
                if (isLoaded)
                {
                    if (lastBits == 0) ++sumCount;
                    if (bits > maxBits) setMaxBits(bits, counts);
                    else setSum(bitReciprocalSum + reciprocal[bits] - reciprocal[lastBits]);
                }
            }
        }
        /// <summary>
        /// 设置倒数和
        /// </summary>
        /// <param name="bitReciprocalSum"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setSum(double bitReciprocalSum)
        {
            this.bitReciprocalSum = bitReciprocalSum;
            totalCount = Math.Pow(2, sumCount / bitReciprocalSum) * sumCount;
        }
        /// <summary>
        /// 重新计算倒数和
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="counts"></param>
        private unsafe void setMaxBits(byte bits, int* counts)
        {
            maxBits = bits;
            double sum = counts[1] >> 8;
            for (double* reciprocal = (double*)BitReciprocal.Data; bits != 1; --bits)
            {
                int count = counts[bits];
                sum += (count >> 8) + (byte)count * reciprocal[bits];
            }
            setSum(sum);
        }
        /// <summary>
        /// Get the statistical quantity
        /// 获取统计数量
        /// </summary>
        /// <returns></returns>
        public double Count()
        {
            return totalCount;
        }

        /// <summary>
        /// 获取默认匹配的索引二进制位数量（非精确的最小统计误差）
        /// </summary>
        /// <param name="count">预计统计总数量（二进制位限制在 8 - 20 位之间，支持 10W - 10Y 级别的数量统计）</param>
        /// <returns></returns>
        public static byte GetDefaultIndexBits(int count)
        {
            if (count < (3 << 16)) return 8;
            if (count < (3 << 18)) return 10;
            if (count < (1 << 22)) return 12;
            if (count < (1 << 24)) return 14;
            if (count < (1 << 26)) return 16;
            if (count < (1 << 28)) return 18;
            return 20;
        }
        /// <summary>
        /// 增加计数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int CountIncrement(int count, byte bits)
        {
            if ((++count & 0xff) != bits) return count;
            return (count + 0x100) & 0x7fffff00;
        }
        /// <summary>
        /// 减少计数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int CountDecrement(int count, byte bits)
        {
            if ((count & 0xff) != 0) return count - 1;
            return (count - 0x100) | (bits - 1);
        }
        /// <summary>
        /// 基于均匀概率的总量统计的二进制位数量的倒数
        /// </summary>
        internal static Pointer BitReciprocal;
        unsafe static UniformProbabilityTotalStatisticsNode()
        {
            BitReciprocal = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Memory.Unmanaged.GetPowerReciprocal();
            double* reciprocal = (double*)BitReciprocal.Data;
            for(int index = 64 - MinIndexBits; index != 0; --index) reciprocal[index] = (double)1 / index;
            *reciprocal = 0;
        }
    }
}
