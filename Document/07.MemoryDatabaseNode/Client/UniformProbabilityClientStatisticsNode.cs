
using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// An example of a client-side synchronous total statistics node client based on uniform probability
    /// 基于均匀概率的客户端同步总量统计节点客户端示例
    /// </summary>
    internal static class UniformProbabilityClientStatisticsNode
    {
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.UniformProbabilityClientStatistics client = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.UniformProbabilityClientStatistics(CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateUniformProbabilityClientStatisticsNode(nameof(UniformProbabilityClientStatisticsNode), 8)));
        /// <summary>
        /// An example of a client-side synchronous total statistics node client based on uniform probability
        /// 基于均匀概率的客户端同步总量统计节点客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult result = default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult);
            for (int count = 10; count != 0; --count)
            {
                result = await client.Append(AutoCSer.Random.Default.NextULong());
                if (result.IsSuccess) break;
                if (result.CallState != AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum.ClientLoadUnfinished) break;
                await Task.Delay(100);
            }
            Console.WriteLine($"{result.IsSuccess} {client.Count()}");
            return true;
        }
    }
}
