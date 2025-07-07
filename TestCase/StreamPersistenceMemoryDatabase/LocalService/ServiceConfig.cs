using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseLocalService
{
    /// <summary>
    /// Log stream persistence memory database local service configuration
    /// 日志流持久化内存数据库本地服务配置
    /// </summary>
    public sealed class ServiceConfig : LocalServiceConfig
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
        public override void RemoveHistoryFile(StreamPersistenceMemoryDatabaseService service)
        {
            new RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).Catch();
        }
#if !DEBUG
        /// <summary>
        /// The size of the reconstructed file is set to at least 300MB because a round of test writes exceed 200MB
        /// 重建文件大小设置为至少 300MB，因为一轮测试写入超过 200MB
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override bool CheckRebuild(StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 300 << 20;
        }
#endif
    }
}
