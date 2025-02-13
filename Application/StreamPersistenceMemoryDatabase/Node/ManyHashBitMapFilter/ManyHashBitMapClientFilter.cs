using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点客户端
    /// </summary>
    public abstract class ManyHashBitMapClientFilter : IDisposable
    {
        /// <summary>
        /// 多哈希位图过滤节点客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> NodeCache;
        /// <summary>
        /// 多哈希位图数据
        /// </summary>
        protected ManyHashBitMap map;
        /// <summary>
        /// 多哈希位图数据访问锁
        /// </summary>
        protected readonly object mapLock;
        /// <summary>
        /// 获取设置新位操作保存回调
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? keepCallback;
#else
        private CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// 未处理新位置集合
        /// </summary>
        private LeftArray<int> getBits;
        /// <summary>
        /// 是否释放资源
        /// </summary>
        protected bool isDispose;
        /// <summary>
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDispose = true;
            keepCallback?.Dispose();
        }
        /// <summary>
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
        /// 获取 4 个 16b 的哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetHashCode4(string value)
        {
            ulong hashCode = value.getHashCode64();
            yield return (int)((uint)hashCode & 0xffff);
            yield return (int)((uint)(hashCode >> 16) & 0xffff);
            yield return (int)((uint)(hashCode >> 32) & 0xffff);
            yield return (int)(uint)(hashCode >> 48);
        }
    }
    /// <summary>
    /// 多哈希位图过滤节点客户端
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class ManyHashBitMapClientFilter<T> : ManyHashBitMapClientFilter
    {
        /// <summary>
        /// 哈希计算委托集合
        /// </summary>
        private readonly Func<T, IEnumerable<int>> getHashCodes;
        /// <summary>
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        /// <param name="getHashCodes">哈希计算委托集合，必须采用稳定哈希算法保证不同机器或者进程计算结果一致</param>
        public ManyHashBitMapClientFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> nodeCache, Func<T, IEnumerable<int>> getHashCodes) : base(nodeCache)
        {
            if (getHashCodes == null) throw new ArgumentNullException(nameof(getHashCodes));
            this.getHashCodes = getHashCodes;
            getMap().NotWait();
        }
        /// <summary>
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<ResponseResult> Set(T value)
        {
            if (map.Size != 0)
            {
                foreach (int hashCode in getHashCodes(value))
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
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task<ResponseResult> set(T value, IManyHashBitMapClientFilterNodeClientNode node)
        {
            foreach (int hashCode in getHashCodes(value))
            {
                int bit = map.GetBitByHashCode(hashCode);
                if (map.GetBitValue(bit) == 0)
                {
                    if (isDispose) return new ResponseResult(CallStateEnum.Disposed);
                    ResponseResult result = await node.SetBit(bit);
                    if (!result.IsSuccess) return result;

                    Monitor.Enter(mapLock);
                    map.SetBit(bit);
                    Monitor.Exit(mapLock);
                }
            }
            return new ResponseResult(CallStateEnum.Success);
        }
        /// <summary>
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
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回 false 表示数据不存在</returns>
        public Task<ResponseResult<bool>> Check(T value)
        {
            if (isDispose) return Task.FromResult(new ResponseResult<bool>(CallStateEnum.Disposed));
            if (map.Size != 0)
            {
                foreach (int hashCode in getHashCodes(value))
                {
                    if (map.GetBitValueByHashCode(hashCode) == 0) return ResponseResult.FalseTask;
                }
                return ResponseResult.TrueTask;
            }
            return check(value);
        }
        /// <summary>
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
            if (isDispose) return new ResponseResult<bool>(CallStateEnum.Disposed);
            foreach (int hashCode in getHashCodes(value))
            {
                if (map.GetBitValueByHashCode(hashCode) == 0) return false;
            }
            return true;
        }
    }
}
