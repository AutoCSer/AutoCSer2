using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// 多哈希位图客户端同步过滤节点 客户端示例
    /// </summary>
    internal static class ManyHashBitMapClientFilterNode
    {
        /// <summary>
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter<string> client = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter<string>(CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateManyHashBitMapClientFilterNode(nameof(ManyHashBitMapClientFilterNode), 1 << 16)), AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter.GetHashCode4);
        /// <summary>
        /// 客户端节点单例（支持并发读取操作）
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter<string> readWriteQueueClient = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter<string>(CommandClientSocketEvent.StreamPersistenceMemoryDatabaseReadWriteQueueClientCache.CreateNode(client => client.GetOrCreateManyHashBitMapClientFilterNode(nameof(ManyHashBitMapClientFilterNode), 1 << 16)), AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter.GetHashCode4);
        /// <summary>
        /// 多哈希位图客户端同步过滤节点 客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
        {
            if (!await test(client)) return false;
            if (!await test(readWriteQueueClient)) return false;
            return true;
        }
        /// <summary>
        /// 多哈希位图客户端同步过滤节点 客户端示例
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> test(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapClientFilter<string> client)
        {
            var result = await client.Set("AAA");
            if (!result.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            var boolResult = await client.Check("AAA");
            if (!boolResult.Value)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
