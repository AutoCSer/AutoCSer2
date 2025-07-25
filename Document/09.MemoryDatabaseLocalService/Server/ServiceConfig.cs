﻿using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Server
{
    /// <summary>
    /// Log stream persistence memory database local service configuration
    /// 日志流持久化内存数据库本地服务配置
    /// </summary>
    internal sealed class ServiceConfig : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceConfig
    {
        /// <summary>
        /// The test environment deletes historical persistent files from the previous 15 minutes. The production environment processes the files based on site requirements
        /// 测试环境删除 15 分钟以前的历史持久化文件，生产环境根据实际需求处理
        /// </summary>
        /// <returns></returns>
        public override DateTime GetRemoveHistoryFileTime()
        {
            return AutoCSer.Threading.SecondTimer.UtcNow.AddMinutes(-15);
        }
        /// <summary>
        /// The test environment deletes persistent files once a minute. The production environment deletes persistent files based on site requirements
        /// 测试环境每分钟执行一次删除历史持久化文件操作，生产环境根据实际需求处理
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override void RemoveHistoryFile(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).Catch();
        }
        /// <summary>
        /// Set the rebuild file size to at least 10MB
        /// 重建文件大小设置为至少 10MB
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override bool CheckRebuild(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 10 << 20;
        }

        /// <summary>
        /// Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalService localService = new AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig
        {
            PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService)),
            PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService) + nameof(AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig.PersistenceSwitchPath))
        }.Create();
        ///// <summary>
        ///// Log stream persistence memory database local service (Support concurrent read operations)
        ///// 日志流持久化内存数据库本地服务（支持并发读取操作）
        ///// </summary>
        //private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalService localService = new AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig
        //{
        //    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService)),
        //    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService) + nameof(AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig.PersistenceSwitchPath))
        //}.Create(-1);
        /// <summary>
        /// Log stream persistence in-memory database local client
        /// 日志流持久化内存数据库本地客户端
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode> Client = localService.CreateClient();
    }
}
