using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.IO;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 搜索 Trie 图分词节点服务端配置
    /// </summary>
    internal sealed class LocalTrieGraphServiceConfig : LocalServiceConfig
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
        private static readonly LocalService localService = new LocalTrieGraphServiceConfig
        {
            PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(LocalTrieGraphServiceConfig)),
            PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(LocalTrieGraphServiceConfig) + nameof(PersistenceSwitchPath))
        }.Create<IQueryServiceNode>(p => new QueryServiceNode(p));
        /// <summary>
        /// 日志流持久化内存数据库本地客户端
        /// </summary>
        public static readonly LocalClient<IQueryServiceNodeLocalClientNode> Client = localService.CreateClient<IQueryServiceNodeLocalClientNode>();
        /// <summary>
        /// 搜索 Trie 图分词节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<IStaticTrieGraphNodeLocalClientNode> StaticTrieGraphNodeCache = Client.CreateNode(client => client.GetOrCreateNode<IStaticTrieGraphNodeLocalClientNode>(nameof(IStaticTrieGraphNode), (index, key, nodeInfo) => client.ClientNode.CreateStaticTrieGraphNode(index, key, nodeInfo, 256, false, null)));
    }
}
