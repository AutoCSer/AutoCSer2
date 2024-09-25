using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库本地服务
    /// </summary>
    public sealed class LocalService : StreamPersistenceMemoryDatabaseService
    {
        /// <summary>
        /// 默认为 false 表示回调采用 Task.Run，如果确认调用客户端 await 无阻塞可以设置为 true 可以大幅降低 CPU 资源占用
        /// </summary>
        internal bool IsSynchronousCallback;
        /// <summary>
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        /// <param name="config">日志流持久化内存数据库服务端配置</param>
        /// <param name="createServiceNode">创建服务基础操作节点委托</param>
        internal LocalService(LocalServiceConfig config, Func<StreamPersistenceMemoryDatabaseService, ServerNode> createServiceNode) : base(config, createServiceNode, true)
        {
            IsSynchronousCallback = config.IsSynchronousCallback;
            if (config.OnlyLocalService) CommandServerCallQueue = new AutoCSer.Net.CommandServerCallQueue(new AutoCSer.Net.CommandListener(new AutoCSer.Net.CommandServerConfig { QueueTimeoutSeconds = config.QueueTimeoutSeconds }), null, 0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (((LocalServiceConfig)Config).OnlyLocalService) CommandServerCallQueue.Server.Dispose();
        }
        /// <summary>
        /// 创建日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <typeparam name="T">服务基础操作客户端接口类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalClient<T> CreateClient<T>() where T : class, IServiceNodeLocalClientNode
        {
            return new LocalClient<T>(this);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CallInput(CallInputMethodParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed) state = parameter.CallInput(ref callback);
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.Callback(state); }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CallInputOutput(CallInputOutputMethodParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed) state = parameter.CallInputOutput(ref callback);
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.Callback(new ResponseParameter(state)); }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        internal void InputKeepCallback(InputKeepCallbackMethodParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed) parameter.InputKeepCallback(ref callback);
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state)); }
        }
        /// <summary>
        /// 添加非持久化队列任务（不修改内存数据状态）
        /// </summary>
        /// <typeparam name="T">获取结果数据类型</typeparam>
        /// <param name="getResult">获取结果数据委托</param>
        /// <returns>队列节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<T> AppendQueueNode<T>(Func<T> getResult)
        {
            return new LocalServiceCustomQueueNode<T>(this, getResult);
        }
    }
}
