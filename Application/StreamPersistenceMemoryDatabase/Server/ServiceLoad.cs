using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 加载数据
    /// </summary>
    internal sealed class ServiceLoad : QueueTaskNode
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        internal ServiceLoad(StreamPersistenceMemoryDatabaseService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            service.Load();
        }
    }
}
