using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 多哈希位图过滤节点客户端实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ManyHashBitMapClientFilterCallback<T>
    {
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        private readonly ManyHashBitMapClientFilter<T> client;
        /// <summary>
        /// 获取设置新数据保持回调
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? keepCallback;
#else
        private CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// Multi-hash bitmap data
        /// 多哈希位图数据
        /// </summary>
        private ManyHashBitMap map;
        /// <summary>
        /// 新设置的位集合
        /// </summary>
        private LeftArray<int> setBits;
        /// <summary>
        /// 新设置的位集合访问锁
        /// </summary>
        private readonly object setBitLock;
        /// <summary>
        /// 重建实例访问锁
        /// </summary>
        private int restartLock;
        /// <summary>
        /// 是否已经取消回调操作
        /// </summary>
        private bool isCancel;
        /// <summary>
        /// 多哈希位图过滤节点客户端实例
        /// </summary>
        /// <param name="client">Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端</param>
        internal ManyHashBitMapClientFilterCallback(ManyHashBitMapClientFilter<T> client)
        {
            this.client = client;
            setBitLock = new object();
            setBits.SetEmpty();
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
                ResponseResult<IManyHashBitMapClientFilterNodeClientNode> nodeResult = await client.NodeCache.GetSynchronousNode();
                if (nodeResult.IsSuccess)
                {
                    IManyHashBitMapClientFilterNodeClientNode node = nodeResult.Value.notNull();
                    keepCallback = await node.GetData(getData, getBit);
                    if (keepCallback != null) return;
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
                new ManyHashBitMapClientFilterCallback<T>(client).Start().AutoCSerNotWait();
            }
        }
        /// <summary>
        /// Set the multi-Hash bitmap data
        /// 设置多哈希位图数据
        /// </summary>
        /// <param name="result"></param>
        private void getData(ResponseResult<ManyHashBitMap> result)
        {
            if (result.IsSuccess)
            {
                this.map = result.Value;
                if (!isCancel) client.Set(this);
            }
            else Cancel();
        }
        /// <summary>
        /// Get the operation of setting a new bit
        /// 获取设置新位操作
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        private void getBit(ResponseResult<int> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess) map.SetBit(result.Value);
            else Cancel();
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Task<ResponseResult> Set(T value)
        {
            foreach (uint hashCode in client.GetHashCodes(value))
            {
                if (map.GetBitValueByHashCode(hashCode) == 0)
                {
                    if (!client.IsDispose) return set(value);
                    return ResponseResult.DisposedTask;
                }
            }
            return ResponseResult.SuccessTask;
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task<ResponseResult> set(T value)
        {
            ResponseResult<IManyHashBitMapClientFilterNodeClientNode> nodeResult = await client.NodeCache.GetNode();
            if (nodeResult.IsSuccess)
            {
                IManyHashBitMapClientFilterNodeClientNode node = nodeResult.Value.notNull();
                foreach (uint hashCode in client.GetHashCodes(value))
                {
                    int bit = map.GetBitByHashCode(hashCode);
                    if (map.GetBitValue(bit) == 0)
                    {
                        if (!client.IsDispose)
                        {
                            ResponseResult result = await node.SetBit(bit);
                            if (result.IsSuccess)
                            {
                                Monitor.Enter(setBitLock);
                                try
                                {
                                    setBits.Add(bit);
                                }
                                finally { Monitor.Exit(setBitLock); }
                            }
                            else return result;
                        }
                        else return CallStateEnum.Disposed;
                    }
                }
                return CallStateEnum.Success;
            }
            return nodeResult;
        }
        /// <summary>
        /// Check the bitmap data
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the data does not exist
        /// 返回 false 表示数据不存在</returns>
        internal bool Check(T value)
        {
            foreach (uint hashCode in client.GetHashCodes(value))
            {
                int bit = map.GetBitByHashCode(hashCode);
                if (map.GetBitValue(bit) == 0)
                {
                    if (setBits.Length == 0)
                    {
                        if (map.GetBitValue(bit) == 0) return false;
                    }
                    else
                    {
                        int setBit = int.MinValue;
                        Monitor.Enter(setBitLock);
                        try
                        {
                            int count = setBits.Length;
                            int[] bitArray = setBits.Array;
                            while (count != 0)
                            {
                                setBit = bitArray[--count];
                                if (bit != setBit)
                                {
                                    if (map.GetBitValue(setBit) != 0) setBits.UnsafeRemoveAtToEnd(count);
                                }
                                else break;
                            }
                        }
                        finally { Monitor.Exit(setBitLock); }
                        if (bit != setBit && map.GetBitValue(bit) == 0) return false;
                    }
                }
            }
            return true;
        }
    }
}
