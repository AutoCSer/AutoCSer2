﻿using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// 日志流持久化内存数据库客户端（支持并发读取操作）
    /// </summary>
    internal sealed class ReadWriteQueueClientSocketEvent : AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        private readonly CommandClientSocketEvent client;
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get { return client.StreamPersistenceMemoryDatabaseReadWriteQueueClient; } }
        /// <summary>
        /// 日志流持久化内存数据库客户端（支持并发读取操作）
        /// </summary>
        /// <param name="client"></param>
        internal ReadWriteQueueClientSocketEvent(CommandClientSocketEvent client)
        {
            this.client = client;
        }
    }
}
