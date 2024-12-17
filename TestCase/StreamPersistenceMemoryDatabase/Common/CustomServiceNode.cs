using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作自定义扩展（用于添加自定义节点创建接口）
    /// </summary>
    public class CustomServiceNode : ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// 服务基础操作自定义扩展
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public CustomServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建回调测试节点 ICallbackNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateCallbackNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateNode<ICallbackNode, int>(index, key, nodeInfo, () => new CallbackNode());
        }
        /// <summary>
        /// 创建游戏测试节点 GameNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateGameNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createSnapshotCloneNode<IGameNode, GameNode, Monster>(index, key, nodeInfo, () => new GameNode());
        }
    }
}
