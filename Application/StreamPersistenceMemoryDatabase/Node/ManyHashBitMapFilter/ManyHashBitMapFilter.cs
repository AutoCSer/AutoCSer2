using AutoCSer.Algorithm;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Multi-hash bitmap filtering node client
    /// 多哈希位图过滤节点客户端
    /// </summary>
    public abstract class ManyHashBitMapFilter
    {
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapFilterNodeClientNode> NodeCache;
        /// <summary>
        /// The operation of rounding off the number of bits
        /// 位数量取余操作
        /// </summary>
        internal IntegerDivision SizeDivision;
        /// <summary>
        /// Bitmap size (number of bits)
        /// 位图大小（位数量）
        /// </summary>
        protected int size { get { return (int)SizeDivision.Divisor; } }
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        /// <param name="size"></param>
        protected ManyHashBitMapFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapFilterNodeClientNode> nodeCache, int size)
        {
            NodeCache = nodeCache;
            SizeDivision.Set(Math.Max(size, 2));
        }
        /// <summary>
        /// Hash value to bitmap index position
        /// 哈希值转位图索引位置
        /// </summary>
        /// <param name="hashCodes">Hash value collection
        /// 哈希值集合</param>
        protected void hashCodeToBits(uint[] hashCodes)
        {
            for (int index = hashCodes.Length; index != 0;)
            {
                --index;
                hashCodes[index] = SizeDivision.GetMod(hashCodes[index]);
                //int hashCode = hashCodes[--index], bit = hashCode % size;
                //hashCodes[index] = bit >= 0 ? bit : (bit + size);
            }
        }

        /// <summary>
        /// Hash value to bitmap index position
        /// 哈希值转位图索引位置
        /// </summary>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="hashCodes">Hash value collection
        /// 哈希值集合</param>
        public static void HashCodeToBits(int size, uint[] hashCodes)
        {
            IntegerDivision sizeDivision = new IntegerDivision(size);
            for (int index = hashCodes.Length; index != 0;)
            {
                --index;
                hashCodes[index] = sizeDivision.GetMod(hashCodes[index]);
            }
        }
        /// <summary>
        /// Gets two hash values of 32b
        /// 获取 2 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint[] GetHashCode2(string value)
        {
            return new HashCode128(value).GetHashCodeArray2();
        }
        /// <summary>
        /// Gets three hash values of 32b
        /// 获取 3 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint[] GetHashCode3(string value)
        {
            return new HashCode128(value).GetHashCodeArray3();
        }
        /// <summary>
        /// Gets four hash values of 32b
        /// 获取 4 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint[] GetHashCode4(string value)
        {
            return new HashCode128(value).GetHashCodeArray4();
        }
    }
    /// <summary>
    /// Multi-hash bitmap filtering node client
    /// 多哈希位图过滤节点客户端
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public sealed class ManyHashBitMapFilter<T> : ManyHashBitMapFilter
    {
        /// <summary>
        /// Hash calculation
        /// 哈希计算
        /// </summary>
        private readonly Func<T, uint[]> getHashCodes;
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="getHashCodes">Hash calculation must adopt a stable hash algorithm to ensure that the calculation results of different machines or processes are consistent
        /// 哈希计算委托集合，必须采用稳定哈希算法保证不同机器或者进程计算结果一致</param>
        public ManyHashBitMapFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapFilterNodeClientNode> nodeCache, int size, Func<T, uint[]> getHashCodes) : base(nodeCache, size)
        {
            if (getHashCodes == null) throw new ArgumentNullException(nameof(getHashCodes));
            this.getHashCodes = getHashCodes;
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Set(T value)
        {
            ResponseResult<IManyHashBitMapFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            IManyHashBitMapFilterNodeClientNode node = nodeResult.Value.notNull();
            do
            {
                if (size == 0)
                {
                    ResponseResult<int> sizeResult = await node.GetSize();
                    if (!sizeResult.IsSuccess) return sizeResult;
                    SizeDivision.Set(sizeResult.Value);
                }
                uint[] hashCodes = getHashCodes(value);
                hashCodeToBits(hashCodes);
                ResponseResult<bool> setResult = await node.SetBits(size, hashCodes);
                if (!setResult.IsSuccess) return setResult;
                if (setResult.Value) return CallStateEnum.Success;
                SizeDivision.Divisor = 0;
            }
            while (true);
        }
        /// <summary>
        /// Check the bitmap data
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the data does not exist
        /// 返回 false 表示数据不存在</returns>
        public async Task<ResponseResult<bool>> Check(T value)
        {
            ResponseResult<IManyHashBitMapFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<bool>();
            IManyHashBitMapFilterNodeClientNode node = nodeResult.Value.notNull();
            do
            {
                if (size == 0)
                {
                    ResponseResult<int> sizeResult = await node.GetSize();
                    if (!sizeResult.IsSuccess) return sizeResult.Cast<bool>();
                    SizeDivision.Set(sizeResult.Value);
                }
                uint[] hashCodes = getHashCodes(value);
                hashCodeToBits(hashCodes);
                ResponseResult<NullableBoolEnum> setResult = await node.CheckBits(size, hashCodes);
                if (!setResult.IsSuccess) return setResult.Cast<bool>();
                switch (setResult.Value)
                {
                    case NullableBoolEnum.True: return true;
                    case NullableBoolEnum.False: return false;
                    default: SizeDivision.Divisor = 0; break;
                }
            }
            while (true);
        }
    }
}
