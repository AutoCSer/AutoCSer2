using AutoCSer.CommandService;
using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.TestCase.DeployTask
{
    /// <summary>
    /// 自定义基础服务，用于添加自定义节点创建 API 方法
    /// </summary>
    public sealed class CustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// 自定义基础服务
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public CustomServiceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建发布任务节点 IDeployTaskNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDeployTaskNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IDeployTaskNode>(index, key, nodeInfo, () => new DeployTaskNode());
        }
    }
}
