using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 内存数据库回调
    /// </summary>
    internal sealed class ServiceCallback : ReadWriteQueueNode
    {
        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 内存数据库回调类型
        /// </summary>
        private readonly ServiceCallbackTypeEnum type;
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        /// <param name="type">内存数据库回调类型</param>
        internal ServiceCallback(StreamPersistenceMemoryDatabaseService service, ServiceCallbackTypeEnum type)
        {
            this.service = service;
            this.type = type;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            switch (type)
            {
                case ServiceCallbackTypeEnum.Load: service.Load(); return;
                case ServiceCallbackTypeEnum.NodeDispose: service.NodeDispose(); return;
            }
        }
    }
}
