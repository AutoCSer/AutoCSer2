using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户搜索数据更新消息节点的自定义基础服务接口
    /// </summary>
    [ServerNode(IsClient = false, IsLocalClient = true)]
    public partial interface ITimeoutMessageServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode
    {
        /// <summary>
        /// 创建用户搜索数据更新消息节点 ITimeoutMessageNode{OperationData{int}}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="timeoutSeconds">触发任务执行超时秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchUserMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int timeoutSeconds);
    }
}
