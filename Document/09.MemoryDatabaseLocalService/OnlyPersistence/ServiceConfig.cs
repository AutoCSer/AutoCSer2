using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.OnlyPersistence
{
    /// <summary>
    /// Local service configuration for archive-only data
    /// 仅存档数据本地服务配置
    /// </summary>
    internal sealed class ServiceConfig : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceConfig
    {
        /// <summary>
        /// The test environment deletes historical persistent files from 7 days ago, and the production environment handles them based on actual needs
        /// 测试环境删除 7 天以前的历史持久化文件，生产环境根据实际需求处理
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
        /// The test environment is set to rebuild the file size at least 10MB (excluding the restart operation), and the production environment is handled according to actual needs
        /// 测试环境设置重建文件大小为至少 10MB（不包括重启操作），生产环境根据实际需求处理
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override bool CheckRebuild(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 10 << 20;
        }

        /// <summary>
        /// A local database service that archive-only data
        /// 仅存档数据的本地数据库服务
        /// </summary>
        private static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalService localService = new AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig
        {
            PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService) + nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceTypeEnum.OnlyPersistence)),
            PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseLocalService) + nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceTypeEnum.OnlyPersistence) + nameof(AutoCSer.Document.MemoryDatabaseLocalService.Server.ServiceConfig.PersistenceSwitchPath)),
            PersistenceType = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceTypeEnum.OnlyPersistence
        }.Create();
        /// <summary>
        /// A local database client that archive-only data
        /// 仅存档数据的本地数据库客户端
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode> Client = localService.CreateClient();
    }
}
