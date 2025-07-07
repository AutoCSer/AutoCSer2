using System;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Multi-hash bitmap filtering node client example
    /// 多哈希位图过滤节点 客户端示例
    /// </summary>
    internal static class ManyHashBitMapFilterNode
    {
        /// <summary>
        /// Test the bitmap size (number of bits)
        /// 测试位图大小（位数量）
        /// </summary>
        private static readonly int size = new Algorithm.ManyHashBitMapCapacity(1 << 10, 2).GetHashCapacity();
        /// <summary>
        /// Client node singleton
        /// 客户端节点单例
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilter<string> client = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilter<string>(CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateManyHashBitMapFilterNode(nameof(ManyHashBitMapFilterNode), size)), size, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMapFilter.GetHashCode2);
        /// <summary>
        /// Multi-hash bitmap filtering node client example
        /// 多哈希位图过滤节点 客户端示例
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
