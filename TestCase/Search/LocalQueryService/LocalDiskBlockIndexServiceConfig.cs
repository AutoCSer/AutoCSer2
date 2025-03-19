using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.TestCase.SearchDataSource;
using System;
using System.IO;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 索引节点服务端配置
    /// </summary>
    internal sealed class LocalDiskBlockIndexServiceConfig : LocalServiceConfig
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
        /// 重建文件大小设置为至少 1MB
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override bool CheckRebuild(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service)
        {
            long persistencePosition = service.GetPersistencePosition();
            return (persistencePosition >> 1) >= service.RebuildSnapshotPosition && persistencePosition > 1 << 20;
        }

        /// <summary>
        /// 日志流持久化内存数据库本地服务端
        /// </summary>
        private static readonly LocalService localService = new LocalDiskBlockIndexServiceConfig
        {
            PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(LocalDiskBlockIndexServiceConfig)),
            PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(LocalDiskBlockIndexServiceConfig) + nameof(PersistenceSwitchPath))
        }.Create<IQueryServiceNode>(p => new QueryServiceNode(p));
        /// <summary>
        /// 日志流持久化内存数据库本地客户端
        /// </summary>
        public static readonly LocalClient<IQueryServiceNodeLocalClientNode> Client = localService.CreateClient<IQueryServiceNodeLocalClientNode>();
        /// <summary>
        /// 用户名称索引节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<int>> UserNameNodeCache = Client.CreateNode(client => client.GetOrCreateNode<IRemoveMarkHashKeyIndexNodeLocalClientNode<int>>(nameof(OperationDataTypeEnum.UserNameNode), (index, key, nodeInfo) => client.ClientNode.CreateRemoveMarkHashKeyIndexNode(index, key, nodeInfo, typeof(int))));
        /// <summary>
        /// 用户备注索引节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<IRemoveMarkHashKeyIndexNodeLocalClientNode<int>> UserRemarkNodeCache = Client.CreateNode(client => client.GetOrCreateNode<IRemoveMarkHashKeyIndexNodeLocalClientNode<int>>(nameof(OperationDataTypeEnum.UserRemarkNode), (index, key, nodeInfo) => client.ClientNode.CreateRemoveMarkHashKeyIndexNode(index, key, nodeInfo, typeof(int))));
    }
}
