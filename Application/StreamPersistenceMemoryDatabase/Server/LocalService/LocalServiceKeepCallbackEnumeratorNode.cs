﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceKeepCallbackEnumeratorNode<T> : LocalServiceQueueNode<LocalKeepCallback<T>>
    {
        /// <summary>
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 调用方法编号
        /// </summary>
        private readonly int methodIndex;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackEnumeratorNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        private LocalServiceKeepCallbackEnumeratorNode(LocalClientNode clientNode, int methodIndex) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.methodIndex = methodIndex;
            callback = new LocalServiceKeepCallbackEnumeratorNodeCallback<T>(clientNode.IsSynchronousCallback);
            result = callback.Response;
            service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.KeepCallback(clientNode.Index, methodIndex, callback);
            completed();
        }
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static LocalServiceQueueNode<LocalKeepCallback<T>> Create(LocalClientNode clientNode, int methodIndex)
        {
            return new LocalServiceKeepCallbackEnumeratorNode<T>(clientNode, methodIndex);
        }
    }
}