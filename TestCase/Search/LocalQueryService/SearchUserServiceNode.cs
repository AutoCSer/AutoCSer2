using AutoCSer.CommandService;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using System.Reflection;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 搜索聚合查询服务服务本地节点的自定义基础服务
    /// </summary>
    public sealed class SearchUserServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, ISearchUserServiceNode
    {
        /// <summary>
        /// 搜索聚合查询服务服务本地节点的自定义基础服务
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        public SearchUserServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }

        /// <summary>
        /// 创建非索引条件查询数据节点 ISearchUserNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSearchUserNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ISearchUserNode>(index, key, nodeInfo, () => new SearchUserNode());
        }
    }
}
