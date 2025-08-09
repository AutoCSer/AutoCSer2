using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// A client example of a total statistics node based on uniform probability
    /// 基于均匀概率的总量统计节点客户端示例
    /// </summary>
    internal static class UniformProbabilityTotalStatisticsNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IUniformProbabilityTotalStatisticsNodeClientNode> nodeCache = CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateUniformProbabilityTotalStatisticsNode(nameof(UniformProbabilityTotalStatisticsNode), 8));
        /// <summary>
        /// A client example of a total statistics node based on uniform probability
        /// 基于均匀概率的总量统计节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            var nodeResult = await nodeCache.GetNode();
            if (!nodeResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IUniformProbabilityTotalStatisticsNodeClientNode node = nodeResult.Value.AutoCSerClassGenericTypeExtensions().NotNull();
            var result = await node.Append(AutoCSer.Random.Default.NextULong());
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var countResult = await node.Count();
            if (!countResult.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
