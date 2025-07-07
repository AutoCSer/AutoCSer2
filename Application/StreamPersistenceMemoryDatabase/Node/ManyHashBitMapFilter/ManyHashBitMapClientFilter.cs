using AutoCSer.Algorithm;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Multi-hash bitmap filtering node client
    /// 多哈希位图过滤节点客户端
    /// </summary>
    public abstract class ManyHashBitMapClientFilter : IDisposable
    {
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> NodeCache;
        /// <summary>
        /// Multi-hash bitmap data
        /// 多哈希位图数据
        /// </summary>
        protected ManyHashBitMap map;
        /// <summary>
        /// Multi-hash bitmap data access lock
        /// 多哈希位图数据访问锁
        /// </summary>
        protected readonly object mapLock;
        /// <summary>
        /// Keep callback for get the operation of setting a new bit 
        /// 获取设置新位操作保持回调
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? keepCallback;
#else
        private CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// A collection of unprocessed new locations
        /// 未处理新位置集合
        /// </summary>
        private LeftArray<int> getBits;
        /// <summary>
        /// Whether to release resources
        /// 是否释放资源
        /// </summary>
        protected bool isDispose;
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        protected ManyHashBitMapClientFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> nodeCache)
        {
            NodeCache = nodeCache;
            getBits.SetEmpty();
            mapLock = new object();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            isDispose = true;
            keepCallback?.Dispose();
        }
        /// <summary>
        /// Initialize to get the multi-Hash bitmap data
        /// 初始化获取多哈希位图数据
        /// </summary>
        /// <returns></returns>
        protected async Task getMap()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                ResponseResult<IManyHashBitMapClientFilterNodeClientNode> nodeResult = await NodeCache.GetSynchronousNode();
                if (nodeResult.IsSuccess)
                {
                    IManyHashBitMapClientFilterNodeClientNode node = nodeResult.Value.notNull();
                    keepCallback = await node.GetBit(getBit);
                    if (keepCallback != null)
                    {
                        if (map.Size == 0) await getData(node);
                        return;
                    }
                }
                await Task.Delay(1000);
            }
            while (!isDispose);
        }
        /// <summary>
        /// Initialize to get the multi-Hash bitmap data
        /// 初始化获取多哈希位图数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task getData(IManyHashBitMapClientFilterNodeClientNode node)
        {
            do
            {
                if (map.Size != 0) return;
                ResponseResult<ManyHashBitMap> mapResult = await node.GetData();
                if (mapResult.IsSuccess)
                {
                    set(mapResult.Value);
                    return;
                }
                await Task.Delay(1000);
            }
            while (!isDispose);
        }
        /// <summary>
        /// Set the multi-Hash bitmap data
        /// 设置多哈希位图数据
        /// </summary>
        /// <param name="map"></param>
        protected void set(ManyHashBitMap map)
        {
            Monitor.Enter(mapLock);
            if (this.map.Size == 0)
            {
                this.map = map;
                int bitCount = getBits.Length;
                if (bitCount == 0)
                {
                    Monitor.Exit(mapLock);
                    return;
                }
                try
                {
                    foreach(var bit in getBits.Array)
                    {
                        map.SetBit(bit);
                        if (--bitCount == 0) break;
                    }
                    getBits.SetEmpty();
                }
                finally { Monitor.Exit(mapLock); }
                return;
            }
            bool isMerge = this.map.Merge(map);
            Monitor.Exit(mapLock);
            if (!isMerge) Dispose();
            return;
        }
        /// <summary>
        /// Get the operation of setting a new bit
        /// 获取设置新位操作
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        private void getBit(ResponseResult<int> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                Monitor.Enter(mapLock);
                try
                {
                    if (map.Size != 0) map.SetBit(result.Value);
                    else getBits.Add(result.Value);
                }
                finally { Monitor.Exit(mapLock); }
            }
            else if (!isDispose) getMap().NotWait();
        }

        /// <summary>
        /// Gets two hash values of 32b
        /// 获取 2 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<uint> GetHashCode2(string value)
        {
            return new HashCode128(value).GetHashCode2();
        }
        /// <summary>
        /// Gets three hash values of 32b
        /// 获取 3 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<uint> GetHashCode3(string value)
        {
            return new HashCode128(value).GetHashCode3();
        }
        /// <summary>
        /// Gets four hash values of 32b
        /// 获取 4 个 32b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<uint> GetHashCode4(string value)
        {
            return new HashCode128(value).GetHashCode4();
        }
    }
    /// <summary>
    /// Multi-hash bitmap filtering node client
    /// 多哈希位图过滤节点客户端
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public sealed class ManyHashBitMapClientFilter<T> : ManyHashBitMapClientFilter
    {
        /// <summary>
        /// Hash calculation
        /// 哈希计算
        /// </summary>
        private readonly Func<T, IEnumerable<uint>> getHashCodes;
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        /// <param name="getHashCodes">Hash calculation must adopt a stable hash algorithm to ensure that the calculation results of different machines or processes are consistent
        /// 哈希计算委托集合，必须采用稳定哈希算法保证不同机器或者进程计算结果一致</param>
        public ManyHashBitMapClientFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> nodeCache, Func<T, IEnumerable<uint>> getHashCodes) : base(nodeCache)
        {
            if (getHashCodes == null) throw new ArgumentNullException(nameof(getHashCodes));
            this.getHashCodes = getHashCodes;
            getMap().NotWait();
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<ResponseResult> Set(T value)
        {
            if (map.Size != 0)
            {
                foreach (uint hashCode in getHashCodes(value))
                {
                    if (map.GetBitValueByHashCode(hashCode) == 0)
                    {
                        if(isDispose) return ResponseResult.DisposedTask;
                        return set(value);
                    }
                }
                return ResponseResult.SuccessTask;
            }
            if (isDispose) return ResponseResult.DisposedTask;
            return setMap(value);
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task<ResponseResult> set(T value)
        {
            ResponseResult<IManyHashBitMapClientFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            return await set(value, nodeResult.Value.notNull());
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task<ResponseResult> set(T value, IManyHashBitMapClientFilterNodeClientNode node)
        {
            foreach (uint hashCode in getHashCodes(value))
            {
                int bit = map.GetBitByHashCode(hashCode);
                if (map.GetBitValue(bit) == 0)
                {
                    if (isDispose) return CallStateEnum.Disposed;
                    ResponseResult result = await node.SetBit(bit);
                    if (!result.IsSuccess) return result;

                    Monitor.Enter(mapLock);
                    map.SetBit(bit);
                    Monitor.Exit(mapLock);
                }
            }
            return CallStateEnum.Success;
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task<ResponseResult> setMap(T value)
        {
            ResponseResult<IManyHashBitMapClientFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult;
            IManyHashBitMapClientFilterNodeClientNode node = nodeResult.Value.notNull();
            if (map.Size == 0)
            {
                ResponseResult<ManyHashBitMap> mapResult = await node.GetData();
                if (!mapResult.IsSuccess) return mapResult;
                set(mapResult.Value);
            }
            return await set(value, node);
        }
        /// <summary>
        /// Check the bitmap data
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the data does not exist
        /// 返回 false 表示数据不存在</returns>
        public Task<ResponseResult<bool>> Check(T value)
        {
            if (isDispose) return Task.FromResult(new ResponseResult<bool>(CallStateEnum.Disposed));
            if (map.Size != 0)
            {
                foreach (uint hashCode in getHashCodes(value))
                {
                    if (map.GetBitValueByHashCode(hashCode) == 0) return ResponseResult.FalseTask;
                }
                return ResponseResult.TrueTask;
            }
            return check(value);
        }
        /// <summary>
        /// Check the bitmap data
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task<ResponseResult<bool>> check(T value)
        {
            ResponseResult<IManyHashBitMapClientFilterNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<bool>();
            IManyHashBitMapClientFilterNodeClientNode node = nodeResult.Value.notNull();
            if (map.Size == 0)
            {
                ResponseResult<ManyHashBitMap> mapResult = await node.GetData();
                if (!mapResult.IsSuccess) return mapResult.Cast<bool>();
                set(mapResult.Value);
            }
            if (isDispose) return CallStateEnum.Disposed;
            foreach (uint hashCode in getHashCodes(value))
            {
                if (map.GetBitValueByHashCode(hashCode) == 0) return false;
            }
            return true;
        }
    }
}
