using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 创建异常调用统计信息节点的自定义基础服务接口
    /// </summary>
    [ServerNode]
    public partial interface IExceptionStatisticsServiceNode : IServiceNode
    {
        /// <summary>
        /// 创建异常调用统计信息节点 IExceptionStatisticsNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="removeTime">节点自动移除时间</param>
        /// <param name="callTimeCount">保存调用时间数量</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateExceptionStatisticsNode(NodeIndex index, string key, NodeInfo nodeInfo, DateTime removeTime, int callTimeCount);
    }
}
