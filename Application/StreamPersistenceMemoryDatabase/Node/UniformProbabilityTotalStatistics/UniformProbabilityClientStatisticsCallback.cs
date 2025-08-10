using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 基于均匀概率的总量统计节点客户端实例
    /// </summary>
    internal sealed class UniformProbabilityClientStatisticsCallback
    {
        /// <summary>
        /// Total statistics node client based on uniform probability
        /// 基于均匀概率的总量统计节点客户端
        /// </summary>
        private readonly UniformProbabilityClientStatistics client;
        /// <summary>
        /// 获取设置新数据保持回调
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? keepCallback;
#else
        private CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// 最后连续 1 的数量的集合
        /// </summary>
        private byte[] bitCountArray;
        /// <summary>
        /// 获取索引的二进制位的低位
        /// </summary>
        private uint indexLow;
        /// <summary>
        /// 获取索引的二进制位的高位
        /// </summary>
        private uint indexHigh;
        /// <summary>
        /// 数据的二进制位
        /// </summary>
        private ulong valueMark;
        /// <summary>
        /// 获取索引的二进制位的低位的数量
        /// </summary>
        private byte indexLowBits;
        /// <summary>
        /// 获取索引的二进制位的高位需要右移位的数量
        /// </summary>
        private byte indexShiftBits;
        ///// <summary>
        ///// 索引二进制位数量
        ///// </summary>
        //private byte indexBits;
        /// <summary>
        /// 当前最大连续数量
        /// </summary>
        private byte maxBits;
        /// <summary>
        /// 是否已经取消回调操作
        /// </summary>
        private bool isCancel;
        /// <summary>
        /// 索引数组大小
        /// </summary>
        private int indexCount;
        /// <summary>
        /// 当前倒数和
        /// </summary>
        private double bitReciprocalSum;
        /// <summary>
        /// 统计数量
        /// </summary>
        internal double TotalCount;
        /// <summary>
        /// 倒数和的倒数数量
        /// </summary>
        private int sumCount;
        /// <summary>
        /// 重建实例访问锁
        /// </summary>
        private int restartLock;
        /// <summary>
        /// 基于均匀概率的总量统计节点客户端实例
        /// </summary>
        /// <param name="client">基于均匀概率的总量统计节点客户端</param>
        internal UniformProbabilityClientStatisticsCallback(UniformProbabilityClientStatistics client)
        {
            this.client = client;
            bitCountArray = EmptyArray<byte>.Array;
        }
        /// <summary>
        /// 开始获取数据
        /// </summary>
        /// <returns></returns>
        internal async Task Start()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                ResponseResult<IUniformProbabilityClientStatisticsNodeClientNode> nodeResult = await client.NodeCache.GetSynchronousNode();
                if (nodeResult.IsSuccess)
                {
                    IUniformProbabilityClientStatisticsNodeClientNode node = nodeResult.Value.notNull();
                    KeepCallbackCommand keepCommand = node.GetIndexBit(getIndexBit);
                    if (await node.GetBitArray(getBitArray))
                    {
                        keepCallback = await keepCommand;
                        if (keepCallback != null)
                        {
                            client.Set(this);
                            return;
                        }
                    }
                }
                await Task.Delay(1000);
            }
            while (!client.IsDispose && !isCancel);
            Cancel();
        }
        /// <summary>
        /// 取消回调
        /// </summary>
        internal void Cancel()
        {
            isCancel = true;
            keepCallback?.Dispose();
            if (!client.IsDispose && System.Threading.Interlocked.CompareExchange(ref restartLock, 1, 0) == 0)
            {
                new UniformProbabilityClientStatisticsCallback(client).Start().AutoCSerNotWait();
            }
        }
        /// <summary>
        /// Get the array of binary bits
        /// 获取二进制位数量的数组
        /// </summary>
        /// <param name="result"></param>
        private unsafe void getBitArray(ResponseResult<byte[]> result)
        {
            if (result.IsSuccess)
            {
                bitCountArray = result.Value.notNull();
                fixed (byte* bitFixed = bitCountArray)
                {
                    byte* start = bitFixed + (bitCountArray.Length - 1);
                    byte indexBits = *start, bits;
                    indexLowBits = (byte)(indexBits >> 1);
                    indexCount = 1 << indexBits;
                    *start = 0;
                    indexShiftBits = (byte)(64 - (indexLowBits << 1));
                    indexLow = (1U << indexLowBits) - 1;
                    int* counts = (int*)(bitFixed + indexCount);
                    indexHigh = indexLow << indexLowBits;
                    valueMark = (1UL << indexShiftBits) - 1;
                    start = bitFixed;
                    do
                    {
                        if ((bits = *start) != 0)
                        {
                            ++sumCount;
                            int* incrementCount = counts + bits;
                            if (bits > maxBits) maxBits = bits;
                            *incrementCount = UniformProbabilityTotalStatisticsNode.CountIncrement(*incrementCount, bits);
                        }
                    }
                    while (++start != counts);
                    if (maxBits != 0) setMaxBits(maxBits, counts);
                }
            }
            else Cancel();
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
            for (double* reciprocal = (double*)UniformProbabilityTotalStatisticsNode.BitReciprocal.Data; bits != 1; --bits)
            {
                int count = counts[bits];
                sum += (count >> 8) + (byte)count * reciprocal[bits];
            }
            setSum(sum);
        }
        /// <summary>
        /// Get the newly set data
        /// 获取新设置的数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        private unsafe void getIndexBit(ResponseResult<int> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                int index = result.Value;
                byte bits = (byte)index, lastBits;
                index >>= 8;
                double* reciprocal = (double*)UniformProbabilityTotalStatisticsNode.BitReciprocal.Data;
                fixed (byte* bitFixed = bitCountArray)
                {
                    int* counts = (int*)(bitFixed + indexCount);
                    lastBits = bitFixed[index];
                    int* incrementCount = counts + bits, decrementCount = counts + lastBits;
                    bitFixed[index] = bits;
                    *incrementCount = UniformProbabilityTotalStatisticsNode.CountIncrement(*incrementCount, bits);
                    *decrementCount = UniformProbabilityTotalStatisticsNode.CountDecrement(*decrementCount, lastBits);
                    if (lastBits == 0) ++sumCount;
                    if (bits > maxBits) setMaxBits(bits, counts);
                    else setSum(bitReciprocalSum + reciprocal[bits] - reciprocal[lastBits]);
                }
            }
            else Cancel();
        }
        /// <summary>
        /// 设置倒数和
        /// </summary>
        /// <param name="bitReciprocalSum"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setSum(double bitReciprocalSum)
        {
            this.bitReciprocalSum = bitReciprocalSum;
            TotalCount = Math.Pow(2, sumCount / bitReciprocalSum) * sumCount;
        }
        /// <summary>
        /// Add statistical data
        /// 添加统计数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Task<ResponseResult> Append(ulong value)
        {
            if (!client.IsDispose)
            {
                int index = (int)(((uint)value & indexLow) | ((uint)(value >> indexShiftBits) & indexHigh));
                value = (value >> indexLowBits) & valueMark;
                ulong markValue = (1UL << (bitCountArray[index] + 1)) - 1;
                if ((value & markValue) != markValue) return ResponseResult.SuccessTask;
                //计算最后连续 1 的数量
                value ^= value + 1;
                if ((uint)value != uint.MaxValue) return set(index, (byte)(((uint)value + 1).deBruijnLog2() - 1));
                if ((value & 0xffffffff00000000UL) != 0) return set(index, (byte)(((uint)(value >> 32) + 1).deBruijnLog2() + 31));
                return set(index, 31);
            }
            return ResponseResult.DisposedTask;
        }
        /// <summary>
        /// 设置最后连续 1 的数量
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        private async Task<ResponseResult> set(int index, byte bits)
        {
            ResponseResult<IUniformProbabilityClientStatisticsNodeClientNode> nodeResult = await client.NodeCache.GetNode();
            if (nodeResult.IsSuccess) return await nodeResult.Value.notNull().SetIndexBit(index, bits);
            return nodeResult;
        }
    }
}
