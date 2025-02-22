using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 创建异常调用统计信息节点的自定义基础服务
    /// </summary>
    public sealed class ExceptionStatisticsServiceNode : ServiceNode, IExceptionStatisticsServiceNode
    {
        /// <summary>
        /// 创建异常调用统计信息节点的自定义基础服务
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public ExceptionStatisticsServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建异常调用统计信息节点 IExceptionStatisticsNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="removeTime">节点自动移除时间</param>
        /// <param name="callTimeCount">保存调用时间数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateExceptionStatisticsNode(NodeIndex index, string key, NodeInfo nodeInfo, DateTime removeTime, int callTimeCount)
        {
            return CreateSnapshotNode<IExceptionStatisticsNode>(index, key, nodeInfo, () => new ExceptionStatisticsNode(removeTime, callTimeCount));
        }
    }
}
