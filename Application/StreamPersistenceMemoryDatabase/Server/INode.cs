using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server node interface
    /// 服务端节点接口
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Processing operations after node removal
        /// 节点移除后的处理操作
        /// </summary>
        void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved();
        /// <summary>
        /// Database service shutdown operation
        /// 数据库服务关闭操作
        /// </summary>
        void StreamPersistenceMemoryDatabaseServiceDisposable();
    }
    /// <summary>
    /// Server node interface
    /// 服务端节点接口
    /// </summary>
    /// <typeparam name="T">Node interface type
    /// 节点接口类型</typeparam>
    public interface INode<T> : INode
    {
        /// <summary>
        /// Set the node context
        /// 设置节点上下文
        /// </summary>
        /// <param name="node"></param>
        void SetContext(ServerNode<T> node);
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        T? StreamPersistenceMemoryDatabaseServiceLoaded();
#else
        T StreamPersistenceMemoryDatabaseServiceLoaded();
#endif
    }
}
