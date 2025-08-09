using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 基于均匀概率的总量统计客户端节点扩展
    /// </summary>
    public static class UniformProbabilityTotalStatisticsClientNodeExtension
    {
        /// <summary>
        /// Add string data
        /// 添加字符串数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseResultAwaiter Append(this IUniformProbabilityTotalStatisticsNodeClientNode node, string value)
        {
            return node.Append(value != null ? value.getHashCode64() : 1);
        }
        /// <summary>
        /// 添加字节数组数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseResultAwaiter Append(this IUniformProbabilityTotalStatisticsNodeClientNode node, byte[] data)
        {
            return node.Append(data != null ? data.getHashCode64() : 1);
        }

        /// <summary>
        /// Add string data
        /// 添加字符串数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<LocalResult> Append(this IUniformProbabilityTotalStatisticsNodeLocalClientNode node, string value)
        {
            return node.Append(value != null ? value.getHashCode64() : 1);
        }
        /// <summary>
        /// Add byte array data
        /// Add byte array data
        /// 添加字节数组数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<LocalResult> Append(this IUniformProbabilityTotalStatisticsNodeLocalClientNode node, byte[] data)
        {
            return node.Append(data != null ? data.getHashCode64() : 1);
        }
    }
}
