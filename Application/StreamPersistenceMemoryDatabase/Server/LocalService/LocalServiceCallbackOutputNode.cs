﻿using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCallbackOutputNode<T> : QueueTaskNode
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
        private readonly LocalServiceCallbackOutputNodeCallback<T> serverCallback;
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        private readonly Action<LocalResult<T>> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">客户端回调委托</param>
        private LocalServiceCallbackOutputNode(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.methodIndex = methodIndex;
            this.callback = callback;
            serverCallback = new LocalServiceCallbackOutputNodeCallback<T>(this);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            clientNode.Client.Service.CallOutput(nodeIndex, methodIndex, serverCallback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="responseParameter"></param>
        /// <returns></returns>
        internal bool Callback(ResponseParameter responseParameter)
        {
            if (responseParameter.State == CallStateEnum.Success)
            {
                var result = ((ResponseParameter<T>)responseParameter).Value.ReturnValue;
                if (clientNode.IsSynchronousCallback) callback(result);
                else Task.Run(() => callback(result));
            }
            else
            {
                try
                {
                    if (clientNode.IsSynchronousCallback) callback(responseParameter.State);
                    else Task.Run(() => callback(responseParameter.State));
                }
                finally { clientNode.CheckState(nodeIndex, responseParameter.State); }
            }
            return true;
        }
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">客户端回调委托</param>
        internal static void Create(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback)
        {
            bool isCallback = true;
            try
            {
                clientNode.Client.Service.CommandServerCallQueue.AddOnly(new LocalServiceCallbackOutputNode<T>(clientNode, methodIndex, callback));
                isCallback = false;
            }
            finally
            {
                if (isCallback) callback(CallStateEnum.Unknown);
            }
        }
    }
}