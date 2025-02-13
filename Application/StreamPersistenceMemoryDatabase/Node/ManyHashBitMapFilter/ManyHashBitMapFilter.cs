using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点客户端
    /// </summary>
    public abstract class ManyHashBitMapFilter
    {
        /// <summary>
        /// 多哈希位图过滤节点客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapFilterNodeClientNode> NodeCache;
        /// <summary>
        /// 位图大小（位数量）
        /// </summary>
        protected int size;
        /// <summary>
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        /// <param name="size"></param>
        protected ManyHashBitMapFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapFilterNodeClientNode> nodeCache, int size)
        {
            NodeCache = nodeCache;
            this.size = Math.Max(size, 2);
        }

        /// <summary>
        /// 哈希值转位图索引位置
        /// </summary>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="hashCodes">哈希值集合</param>
        public static void HashCodeToBits(int size, int[] hashCodes)
        {
            for (int index = hashCodes.Length; index != 0;)
            {
                int hashCode = hashCodes[--index], bit = hashCode % size;
                hashCodes[index] = bit >= 0 ? bit : (bit + size);
            }
        }
        /// <summary>
        /// 获取 2 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] GetHashCode2(string value)
        {
            ulong hashCode = value.getHashCode64();
            return new int[] { (int)(uint)hashCode, (int)(uint)(hashCode >> 32) };
        }
        /// <summary>
        /// 获取 3 个 21b+21b+22b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] GetHashCode3(string value)
        {
            ulong hashCode = value.getHashCode64();
            return new int[] { (int)((uint)hashCode & ((1 << 21) - 1)), (int)((uint)(hashCode >> 21) & ((1 << 21) - 1)), (int)(uint)(hashCode >> 42) };
        }
    }
    /// <summary>
    /// 多哈希位图过滤节点客户端
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class ManyHashBitMapFilter<T> : ManyHashBitMapFilter
    {
        /// <summary>
        /// 哈希计算委托集合
        /// </summary>
        private readonly Func<T, int[]> getHashCodes;
        /// <summary>
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        /// <param name="size">位图大小（位数量）</param>
        /// <param name="getHashCodes">哈希计算委托集合，必须采用稳定哈希算法保证不同机器或者进程计算结果一致</param>
        public ManyHashBitMapFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapFilterNodeClientNode> nodeCache, int size, Func<T, int[]> getHashCodes) : base(nodeCache, size)
        {
            if (getHashCodes == null) throw new ArgumentNullException(nameof(getHashCodes));
            this.getHashCodes = getHashCodes;
        }
        /// <summary>
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Set(T value)
        {
            ResponseResult<IManyHashBitMapFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            IManyHashBitMapFilterNodeClientNode node = nodeResult.Value.notNull();
            int size = this.size;
            do
            {
                if (size == 0)
                {
                    ResponseResult<int> sizeResult = await node.GetSize();
                    if (!sizeResult.IsSuccess) return sizeResult;
                    this.size = size = sizeResult.Value;
                }
                int[] hashCodes = getHashCodes(value);
                HashCodeToBits(size, hashCodes);
                ResponseResult<bool> setResult = await node.SetBits(size, hashCodes);
                if (!setResult.IsSuccess) return setResult;
                if (setResult.Value) return new ResponseResult(CallStateEnum.Success);
                size = 0;
            }
            while (true);
        }
        /// <summary>
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回 false 表示数据不存在</returns>
        public async Task<ResponseResult<bool>> Check(T value)
        {
            ResponseResult<IManyHashBitMapFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<bool>();
            IManyHashBitMapFilterNodeClientNode node = nodeResult.Value.notNull();
            int size = this.size;
            do
            {
                if (size == 0)
                {
                    ResponseResult<int> sizeResult = await node.GetSize();
                    if (!sizeResult.IsSuccess) return sizeResult.Cast<bool>();
                    this.size = size = sizeResult.Value;
                }
                int[] hashCodes = getHashCodes(value);
                HashCodeToBits(size, hashCodes);
                ResponseResult<NullableBoolEnum> setResult = await node.CheckBits(size, hashCodes);
                if (!setResult.IsSuccess) return setResult.Cast<bool>();
                switch (setResult.Value)
                {
                    case NullableBoolEnum.True: return true;
                    case NullableBoolEnum.False: return false;
                    default: size = 0; break;
                }
            }
            while (true);
        }
    }
}
