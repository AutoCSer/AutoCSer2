using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Create the node of the calling method and parameter information
    /// 创建调用方法与参数信息节点
    /// </summary>
    /// <typeparam name="T">Service node type
    /// 服务节点类型</typeparam>
    public abstract class MethodParameterCreatorNode<T> : ContextNode<T>
    {
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected MethodParameterCreator<T> methodParameterCreator;
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        public T StreamPersistenceMemoryDatabaseMethodParameterCreator { get { return methodParameterCreator.Creator; } }
        /// <summary>
        /// Server-side node context
        /// 服务端节点上下文
        /// </summary>
        /// <param name="node">Server node
        /// 服务端节点</param>
        public override void SetContext(ServerNode<T> node)
        {
            base.SetContext(node);
            methodParameterCreator = node.CreateMethodParameterCreator();
        }
    }
    /// <summary>
    /// Create the node of the calling method and parameter information
    /// 创建调用方法与参数信息节点
    /// </summary>
    /// <typeparam name="T">Service node type
    /// 服务节点类型</typeparam>
    /// <typeparam name="ST">Snapshot data type
    /// 快照数据类型</typeparam>
    public abstract class MethodParameterCreatorNode<T, ST> : ContextNode<T, ST>
    {
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected MethodParameterCreator<T> methodParameterCreator;
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        public T StreamPersistenceMemoryDatabaseMethodParameterCreator { get { return methodParameterCreator.Creator; } }
        /// <summary>
        /// Server-side node context
        /// 服务端节点上下文
        /// </summary>
        /// <param name="node">Server node
        /// 服务端节点</param>
        public override void SetContext(ServerNode<T> node)
        {
            base.SetContext(node);
            methodParameterCreator = node.CreateMethodParameterCreator();
        }
    }
}
