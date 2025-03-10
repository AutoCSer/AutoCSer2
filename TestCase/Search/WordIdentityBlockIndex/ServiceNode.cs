using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService;
using System;
using AutoCSer.TestCase.SearchCommon;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 创建分词结果磁盘块索引信息节点的自定义基础服务
    /// </summary>
    public sealed class ServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, IServiceNode
    {
        /// <summary>
        /// 创建分词结果磁盘块索引信息节点的自定义基础服务
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建用户名称分词结果磁盘块索引信息节点 IWordIdentityBlockIndex
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateUserNameWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IWordIdentityBlockIndexNode<int>>(index, key, nodeInfo, () => new UserNameNode());
        }
        /// <summary>
        /// 创建用户备注分词结果磁盘块索引信息节点 IWordIdentityBlockIndex
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateUserRemarkWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IWordIdentityBlockIndexNode<int>>(index, key, nodeInfo, () => new UserRemarkNode());
        }
        /// <summary>
        /// 创建非索引条件查询数据节点 ISearchUserNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSearchUserNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ISearchUserNode>(index, key, nodeInfo, () => new SearchUserNode());
        }
    }
}
