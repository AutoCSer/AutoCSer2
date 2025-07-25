﻿using AutoCSer.CommandService.Search;
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
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建用户名称分词结果磁盘块索引信息节点 IWordIdentityBlockIndex
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateUserNameWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IWordIdentityBlockIndexNode<int>>(index, key, nodeInfo, () => new UserNameNode());
        }
        /// <summary>
        /// 创建用户备注分词结果磁盘块索引信息节点 IWordIdentityBlockIndex
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateUserRemarkWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IWordIdentityBlockIndexNode<int>>(index, key, nodeInfo, () => new UserRemarkNode());
        }
    }
}
