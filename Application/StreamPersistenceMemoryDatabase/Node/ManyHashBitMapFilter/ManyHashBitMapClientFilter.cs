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
    public abstract class ManyHashBitMapClientFilter
    {
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> NodeCache;
        /// <summary>
        /// Whether to release resources
        /// 是否释放资源
        /// </summary>
        internal bool IsDispose;
        /// <summary>
        /// Multi-hash bitmap filtering node client
        /// 多哈希位图过滤节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        protected ManyHashBitMapClientFilter(StreamPersistenceMemoryDatabaseClientNodeCache<IManyHashBitMapClientFilterNodeClientNode> nodeCache)
        {
            NodeCache = nodeCache;
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
    public sealed class ManyHashBitMapClientFilter<T> : ManyHashBitMapClientFilter, IDisposable
    {
        /// <summary>
        /// Hash calculation
        /// 哈希计算
        /// </summary>
        internal readonly Func<T, IEnumerable<uint>> GetHashCodes;
        /// <summary>
        /// Current instance
        /// 当前实例
        /// </summary>
#if NetStandard21
        private ManyHashBitMapClientFilterCallback<T>? callback;
#else
        private ManyHashBitMapClientFilterCallback<T> callback;
#endif
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
            this.GetHashCodes = getHashCodes;
            new ManyHashBitMapClientFilterCallback<T>(this).Start().AutoCSerNotWait();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            IsDispose = true;
            callback?.Cancel();
        }
        /// <summary>
        /// Set the current instance
        /// 设置当前实例
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ManyHashBitMapClientFilterCallback<T> callback)
        {
            this.callback?.Cancel();
            this.callback = callback;
        }
        /// <summary>
        /// Set the bitmap data
        /// 设置位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult> Set(T value)
        {
            return callback?.Set(value) ?? ResponseResult.ClientLoadUnfinishedTask;
        }
        /// <summary>
        /// Check the bitmap data
        /// 检查位图数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the data does not exist
        /// 返回 false 表示数据不存在</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Check(T value)
        {
            return callback != null && callback.Check(value);
        }
    }
}
