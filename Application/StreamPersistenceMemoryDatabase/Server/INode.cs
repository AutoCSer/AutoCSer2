using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点接口
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved();
    }
    /// <summary>
    /// 服务端节点接口
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    public interface INode<T> : INode
    {
        /// <summary>
        /// 设置节点上下文
        /// </summary>
        /// <param name="node"></param>
        void SetContext(ServerNode<T> node);
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
        T StreamPersistenceMemoryDatabaseServiceLoaded();
    }
}
