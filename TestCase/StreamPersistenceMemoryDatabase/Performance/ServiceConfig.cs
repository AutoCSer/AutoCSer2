using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 日志流持久化内存数据库服务端配置
    /// </summary>
    public sealed class ServiceConfig : StreamPersistenceMemoryDatabaseServiceConfig
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
        public override void RemoveHistoryFile(StreamPersistenceMemoryDatabaseService service)
        {
            new RemoveHistoryFile(service).Remove(new AutoCSer.Threading.TaskRunTimer(60.0)).NotWait();
        }
#if !DEBUG
        /// <summary>
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
