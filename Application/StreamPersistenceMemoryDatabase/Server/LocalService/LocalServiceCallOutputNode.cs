﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCallOutputNode<T> : LocalServiceQueueNode<ResponseResult<T>>
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
        /// 节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceCallOutputNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        private LocalServiceCallOutputNode(LocalClientNode clientNode, int methodIndex) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.methodIndex = methodIndex;
            callback = new LocalServiceCallOutputNodeCallback<T>(this);
            nodeIndex = clientNode.Index;
            service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.CallOutput(nodeIndex, methodIndex, callback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal bool Callback(ResponseParameter result)
        {
            if (result.State == CallStateEnum.Success) this.result = ((ResponseParameter<T>)result).Value.ReturnValue;
            else this.result = new ResponseResult<T>(result.State);
            completed();
            if (result.State != CallStateEnum.Success) clientNode.CheckState(nodeIndex, result.State);
            return true;
        }
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static LocalServiceQueueNode<ResponseResult<T>> Create(LocalClientNode clientNode, int methodIndex)
        {
            return new LocalServiceCallOutputNode<T>(clientNode, methodIndex);
        }
    }
}