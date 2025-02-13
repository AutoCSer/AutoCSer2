using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
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
        /// 多哈希位图客户端同步过滤节点 客户端示例
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> Test()
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
