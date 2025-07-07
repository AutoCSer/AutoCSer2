using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence memory database local service
    /// 日志流持久化内存数据库本地服务
    /// </summary>
    public sealed class LocalService : StreamPersistenceMemoryDatabaseService
    {
        /// <summary>
        /// Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        /// <param name="config">Configuration of in-memory database service for log stream persistence
        /// 日志流持久化内存数据库服务配置</param>
        /// <param name="createServiceNode">The delegate that creates the underlying operation node for the service
        /// 创建服务基础操作节点委托</param>
        internal LocalService(LocalServiceConfig config, Func<LocalService, ServerNode> createServiceNode) : base(config, p => createServiceNode((LocalService)p), true)
        {
            if (config.OnlyLocalService)
            {
                CommandServerCallQueue = new AutoCSer.Net.CommandServerCallConcurrencyReadQueue(new AutoCSer.Net.CommandListener(new AutoCSer.Net.CommandServerConfig { QueueTimeoutSeconds = config.QueueTimeoutSeconds }), null);
                CommandServerCallQueue.AppendWriteOnly(new ServiceCallback(this, ServiceCallbackTypeEnum.Load));
            }
        }
        /// <summary>
        /// Log stream persistence memory database local service (supporting concurrent read operations)
        /// 日志流持久化内存数据库本地服务（支持并发读取操作）
        /// </summary>
        /// <param name="config">Configuration of in-memory database service for log stream persistence
        /// 日志流持久化内存数据库服务配置</param>
        /// <param name="createServiceNode">The delegate that creates the underlying operation node for the service
        /// 创建服务基础操作节点委托</param>
        /// <param name="maxConcurrency">The maximum concurrent number of read operations, if less than or equal to 0, indicates the number of processors minus the set value (for example, if the number of processors is 4 and the concurrent number is set to -1, then the concurrent number of reads is 4 -1 = 3)
        /// 最大读取操作并发数量，小于等于 0 表示处理器数量减去设置值（比如处理器数量为 4，并发数量设置为 -1，则读取并发数量为 4 - 1 = 3）</param>
        internal LocalService(LocalServiceConfig config, Func<LocalService, ServerNode> createServiceNode, int maxConcurrency) : base(config, p => createServiceNode((LocalService)p), true)
        {
            if (config.OnlyLocalService)
            {
                CommandServerCallQueue = new AutoCSer.Net.CommandServerCallReadQueue(new AutoCSer.Net.CommandListener(new AutoCSer.Net.CommandServerConfig { QueueTimeoutSeconds = config.QueueTimeoutSeconds }), null, maxConcurrency);
                CommandServerCallQueue.AppendWriteOnly(new ServiceCallback(this, ServiceCallbackTypeEnum.Load));
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (((LocalServiceConfig)Config).OnlyLocalService) CommandServerCallQueue.Server.Dispose();
        }
        /// <summary>
        /// Create a default log stream persistence in-memory database local service client
        /// 创建默认日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalClient<IServiceNodeLocalClientNode> CreateClient()
        {
            return new LocalClient<IServiceNodeLocalClientNode>(this);
        }
        /// <summary>
        /// Create a log stream persistence in-memory database local service client
        /// 创建日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <typeparam name="T">Basic service operation client interface type
        /// 服务基础操作客户端接口类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalClient<T> CreateClient<T>() where T : class, IServiceNodeLocalClientNode
        {
            return new LocalClient<T>(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CallInput(CallInputMethodParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed) state = parameter.CallInput(ref refCallback);
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.Callback(state); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CallInputOutput(CallInputOutputMethodParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed) state = parameter.CallInputOutput(ref refCallback);
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.Callback(ResponseParameter.CallStates[(byte)state]); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        internal void InputKeepCallback(InputKeepCallbackMethodParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed) parameter.InputKeepCallback(ref refCallback);
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state)); }
        }
        /// <summary>
        /// Add non-persistent queue tasks (without modifying the status of in-memory data)
        /// 添加非持久化队列任务（不修改内存数据状态）
        /// </summary>
        /// <typeparam name="T">Return the data type of the result
        /// 返回结果数据类型</typeparam>
        /// <param name="getResult">The delegate to get the result
        /// 获取结果数据委托</param>
        /// <returns>Queue node
        /// 队列节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<T> AppendQueueNode<T>(Func<T> getResult)
        {
            return new LocalServiceCustomQueueNode<T>(this, getResult, AutoCSer.Net.CommandServer.ReadWriteNodeTypeEnum.Read);
        }
    }
}
