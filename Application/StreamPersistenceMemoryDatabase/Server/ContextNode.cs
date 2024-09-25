using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点上下文
    /// </summary>
    /// <typeparam name="T">服务节点类型</typeparam>
    public abstract class ContextNode<T> : INode<T>
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        protected StreamPersistenceMemoryDatabaseService streamPersistenceMemoryDatabaseService;
        /// <summary>
        /// 服务端节点
        /// </summary>
        protected ServerNode<T> streamPersistenceMemoryDatabaseNode;
        /// <summary>
        /// 服务端节点上下文
        /// </summary>
        /// <param name="node">服务端节点</param>
        public virtual void SetContext(ServerNode<T> node)
        {
            streamPersistenceMemoryDatabaseService = (StreamPersistenceMemoryDatabaseService)node.NodeCreator.Service;
            streamPersistenceMemoryDatabaseNode = node;
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
        public virtual T StreamPersistenceMemoryDatabaseServiceLoaded() { return default(T); }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public virtual void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved() { }
        /// <summary>
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isSnapshotTransaction">默认为 true 表示需要建立快照事务关系，比如在持久化 API 中同步更新其它节点状态</param>
        /// <returns>服务端节点，失败返回 null</returns>
        protected ServerNode getServerNode(string key, bool isSnapshotTransaction = true)
        {
            HashString hashKey = key;
            if (isSnapshotTransaction)
            {
                ServerNode node = streamPersistenceMemoryDatabaseNode.GetSnapshotTransactionNode(ref hashKey);
                if (node != null) return node;
                node = streamPersistenceMemoryDatabaseService.GetServerNode(ref hashKey);
                if (node != null) streamPersistenceMemoryDatabaseNode.AppendSnapshotTransaction(ref hashKey, node);
                return node;
            }
            return streamPersistenceMemoryDatabaseService.GetServerNode(ref hashKey);
        }
    }
}
