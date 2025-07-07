using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 创建分词结果磁盘块索引信息节点的自定义基础服务接口
    /// </summary>
    [ServerNode]
    public partial interface ISearchUserServiceNode : CommandService.StreamPersistenceMemoryDatabase.IServiceNode
    {
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
        NodeIndex CreateSearchUserNode(NodeIndex index, string key, NodeInfo nodeInfo);
    }
}
