using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.TestCase.ProcessGuardClient
{
    /// <summary>
    /// 进程守护节点客户端
    /// </summary>
    internal static class CommandClientSocketEvent
    {
        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ProcessGuardCommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ProcessGuardCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ProcessGuard),
            GetSocketEventDelegate = (client) => new ProcessGuardCommandClientSocketEvent(client)
        });
        /// <summary>
        /// 进程守护节点客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IProcessGuardNodeClientNode> ProcessGuardNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateProcessGuardNode());
    }
}
