using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.Server
{
    /// <summary>
    /// 日志流持久化内存数据库本地服务端配置
    /// </summary>
    internal sealed class ServiceConfig : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceConfig
    {
        /// <summary>
        /// 测试环境删除 15 分钟以前的历史持久化文件，生产环境根据实际需求处理
        /// </summary>
        /// <returns></returns>
        public override DateTime GetRemoveHistoryFileTime()
        {
            return AutoCSer.Threading.SecondTimer.UtcNow.AddMinutes(-15);
        }
        /// <summary>
        /// 测试环境每分钟执行一次删除历史持久化文件操作，生产环境根据实际需求处理
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override void RemoveHistoryFile(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).NotWait();
        }
        /// <summary>
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
        /// 日志流持久化内存数据库本地服务端
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalService localService = new AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig
        {
            PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService)),
            PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService) + nameof(AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig.PersistenceSwitchPath))
        }.Create();
        ///// <summary>
        ///// 日志流持久化内存数据库本地服务端（支持并发读取操作）
        ///// </summary>
        //private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalService localService = new AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig
        //{
        //    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService)),
        //    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService) + nameof(AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig.PersistenceSwitchPath))
        //}.Create(-1);
        /// <summary>
        /// 日志流持久化内存数据库本地客户端
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode> Client = localService.CreateClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode>();
    }
}
