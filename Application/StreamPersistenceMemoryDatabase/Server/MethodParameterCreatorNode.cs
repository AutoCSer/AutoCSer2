﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 创建调用方法与参数信息节点
    /// </summary>
    /// <typeparam name="T">服务节点类型</typeparam>
    public abstract class MethodParameterCreatorNode<T> : ContextNode<T>
    {
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        protected MethodParameterCreator<T> methodParameterCreator;
        /// <summary>
        /// 服务端节点上下文
        /// </summary>
        /// <param name="node">服务端节点</param>
        public override void SetContext(ServerNode<T> node)
        {
            base.SetContext(node);
            methodParameterCreator = node.CreateMethodParameterCreator();
        }
    }
}