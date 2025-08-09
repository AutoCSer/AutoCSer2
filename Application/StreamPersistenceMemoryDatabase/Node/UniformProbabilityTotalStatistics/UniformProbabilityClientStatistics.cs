using AutoCSer.Algorithm;
using AutoCSer.Extensions;
using AutoCSer.Extensions.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Total statistics node client based on uniform probability
    /// 基于均匀概率的总量统计节点客户端
    /// </summary>
    public sealed class UniformProbabilityClientStatistics : IDisposable
    {
        /// <summary>
        /// Total statistics client nodes based on uniform probability
        /// 基于均匀概率的总量统计客户端节点
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IUniformProbabilityClientStatisticsNodeClientNode> NodeCache;
        /// <summary>
        /// Current statistics instance
        /// 当前统计实例
        /// </summary>
#if NetStandard21
        private UniformProbabilityClientStatisticsCallback? callback;
#else
        private UniformProbabilityClientStatisticsCallback callback;
#endif
        /// <summary>
        /// Whether to release resources
        /// 是否释放资源
        /// </summary>
        internal bool IsDispose;
        /// <summary>
        /// Total statistics node client based on uniform probability
        /// 基于均匀概率的总量统计节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        public UniformProbabilityClientStatistics(StreamPersistenceMemoryDatabaseClientNodeCache<IUniformProbabilityClientStatisticsNodeClientNode> nodeCache)
        {
            NodeCache = nodeCache;
            new UniformProbabilityClientStatisticsCallback(this).Start().AutoCSerNotWait();
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
        /// Set the current statistical instance
        /// 设置当前统计实例
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(UniformProbabilityClientStatisticsCallback callback)
        {
            this.callback?.Cancel();
            this.callback = callback;
        }
        /// <summary>
        /// Add statistical data
        /// 添加统计数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult> Append(ulong value)
        {
            return callback?.Append(value) ?? ResponseResult.ClientLoadUnfinishedTask;
        }
        /// <summary>
        /// Add string data
        /// 添加字符串数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult> Append(string value)
        {
            return Append(value != null ? value.getHashCode64() : 1);
        }
        /// <summary>
        /// Add byte array data
        /// 添加字节数组数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult> Append(byte[] data)
        {
            return Append(data != null ? data.getHashCode64() : 1);
        }
        /// <summary>
        /// Get the statistical quantity
        /// 获取统计数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double Count()
        {
            return callback != null ? callback.Count() : double.MinValue;
        }
    }
}
