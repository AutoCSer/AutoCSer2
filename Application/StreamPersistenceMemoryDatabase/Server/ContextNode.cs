using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
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
#if NetStandard21
        [AllowNull]
#endif
        public StreamPersistenceMemoryDatabaseService StreamPersistenceMemoryDatabaseService { get; private set; }
        /// <summary>
        /// 服务端节点
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public ServerNode<T> StreamPersistenceMemoryDatabaseNode { get; private set; }
        /// <summary>
        /// 服务端执行队列
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public CommandServerCallQueue StreamPersistenceMemoryDatabaseCallQueue { get; private set; }
        /// <summary>
        /// 服务端节点上下文
        /// </summary>
        /// <param name="node">服务端节点</param>
        public virtual void SetContext(ServerNode<T> node)
        {
            StreamPersistenceMemoryDatabaseService = (StreamPersistenceMemoryDatabaseService)node.NodeCreator.Service;
            StreamPersistenceMemoryDatabaseNode = node;
            StreamPersistenceMemoryDatabaseCallQueue = StreamPersistenceMemoryDatabaseService.CommandServerCallQueue;
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public virtual T? StreamPersistenceMemoryDatabaseServiceLoaded() { return default(T); }
#else
        public virtual T StreamPersistenceMemoryDatabaseServiceLoaded() { return default(T); }
#endif
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public virtual void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved() { }
        /// <summary>
        /// 数据库服务关闭操作
        /// </summary>
        public virtual void StreamPersistenceMemoryDatabaseServiceDisposable() { }
        /// <summary>
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isSnapshotTransaction">默认为 true 表示需要建立快照事务关系，比如在持久化 API 中同步更新其它节点状态</param>
        /// <returns>服务端节点，失败返回 null</returns>
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
    /// 服务端节点上下文
    /// </summary>
    /// <typeparam name="T">服务节点类型</typeparam>
    /// <typeparam name="ST">快照数据类型</typeparam>
    public abstract class ContextNode<T, ST> : ContextNode<T>
    {
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public virtual void SetSnapshotResult(ref LeftArray<ST> array, ref LeftArray<ST> newArray) { }
    }
}
