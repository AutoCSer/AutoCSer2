using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server-side node context
    /// 服务端节点上下文
    /// </summary>
    /// <typeparam name="T">Service node type
    /// 服务节点类型</typeparam>
    public abstract class ContextNode<T> : INode<T>
    {
        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public StreamPersistenceMemoryDatabaseService StreamPersistenceMemoryDatabaseService { get; private set; }
        /// <summary>
        /// Server node
        /// 服务端节点
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public ServerNode<T> StreamPersistenceMemoryDatabaseNode { get; private set; }
        /// <summary>
        /// The server synchronizes the read and write queues
        /// 服务端同步读写队列
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public CommandServerCallReadWriteQueue StreamPersistenceMemoryDatabaseCallQueue { get; private set; }
        /// <summary>
        /// Determine whether the node has been removed or whether the database service has released resources
        /// 判断节点是否已经被移除或者数据库服务是否已经释放资源
        /// </summary>
        public bool StreamPersistenceMemoryDatabaseNodeIsRemoved { get { return StreamPersistenceMemoryDatabaseNode.IsRemoved || StreamPersistenceMemoryDatabaseService.IsDisposed;  } }
        /// <summary>
        /// Server-side node context
        /// 服务端节点上下文
        /// </summary>
        /// <param name="node">Server node
        /// 服务端节点</param>
        public virtual void SetContext(ServerNode<T> node)
        {
            StreamPersistenceMemoryDatabaseService = (StreamPersistenceMemoryDatabaseService)node.NodeCreator.Service;
            StreamPersistenceMemoryDatabaseNode = node;
            StreamPersistenceMemoryDatabaseCallQueue = StreamPersistenceMemoryDatabaseService.CommandServerCallQueue;
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public virtual T? StreamPersistenceMemoryDatabaseServiceLoaded() { return default(T); }
#else
        public virtual T StreamPersistenceMemoryDatabaseServiceLoaded() { return default(T); }
#endif
        /// <summary>
        /// Processing operations after node removal
        /// 节点移除后的处理操作
        /// </summary>
        public virtual void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved() { }
        /// <summary>
        /// Database service shutdown operation
        /// 数据库服务关闭操作
        /// </summary>
        public virtual void StreamPersistenceMemoryDatabaseServiceDisposable() { }
        /// <summary>
        /// Get server node based on node global keywords
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isSnapshotTransaction">The default value of true indicates that a snapshot transaction relationship needs to be established, such as synchronously updating the states of other nodes in the persistence API
        /// 默认为 true 表示需要建立快照事务关系，比如在持久化 API 中同步更新其它节点状态</param>
        /// <returns>Server node, null is returned upon failure
        /// 服务端节点，失败返回 null</returns>
#if NetStandard21
        protected ServerNode? getServerNode(string key, bool isSnapshotTransaction = true)
#else
        protected ServerNode getServerNode(string key, bool isSnapshotTransaction = true)
#endif
        {
            if (isSnapshotTransaction)
            {
                var node = StreamPersistenceMemoryDatabaseNode.GetSnapshotTransactionNode(key);
                if (node != null) return node;
                node = StreamPersistenceMemoryDatabaseService.GetServerNode(key);
                if (node != null) StreamPersistenceMemoryDatabaseNode.AppendSnapshotTransaction(key, node);
                return node;
            }
            return StreamPersistenceMemoryDatabaseService.GetServerNode(key);
        }
    }
    /// <summary>
    /// Server-side node context
    /// 服务端节点上下文
    /// </summary>
    /// <typeparam name="T">Service node type
    /// 服务节点类型</typeparam>
    /// <typeparam name="ST">Snapshot data type
    /// 快照数据类型</typeparam>
    public abstract class ContextNode<T, ST> : ContextNode<T>
    {
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public virtual void SetSnapshotResult(ref LeftArray<ST> array, ref LeftArray<ST> newArray) { }
    }
}
