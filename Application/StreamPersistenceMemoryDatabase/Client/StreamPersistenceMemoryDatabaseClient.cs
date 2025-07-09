using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persistence in-memory database client
    /// 日志流持久化内存数据库客户端
    /// </summary>
    public abstract class StreamPersistenceMemoryDatabaseClient
    {
        /// <summary>
        /// Log stream persists in-memory database client socket events
        /// 日志流持久化内存数据库客户端套接字事件
        /// </summary>
        public readonly IStreamPersistenceMemoryDatabaseClientSocketEvent Client;
        /// <summary>
        /// Access lock of the create client node
        /// 创建客户端节点访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim createNodeLock = new System.Threading.SemaphoreSlim(1, 1);
        /// <summary>
        /// Log stream persistence in-memory database client
        /// 日志流持久化内存数据库客户端
        /// </summary>
        /// <param name="client">Log stream persists in-memory database client socket events
        /// 日志流持久化内存数据库客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClient(IStreamPersistenceMemoryDatabaseClientSocketEvent client)
        {
            this.Client = client;
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public abstract ResponseParameterAwaiter<bool> RemoveNode(ClientNode node);
        /// <summary>
        ///  Get the client node. If the server does not exist, create the node
        ///  获取客户端客户端节点，服务端不存在则创建节点
        /// </summary>
        /// <typeparam name="T">Client node interface type
        /// 客户端节点接口类型</typeparam>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="creator">The delegate for creating the client node
        /// 创建客户端节点委托</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>The client node interface object is derived from AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}
        /// 客户端节点接口对象派生自 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}</returns>
        public async Task<ResponseResult<T>> GetOrCreateNode<T>(string key, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            ResponseResult<NodeIndex> nodeIndex = await GetOrCreateNodeIndex<T>(key, creator);
            if (nodeIndex.IsSuccess) return ClientNodeCreator<T>.Create(key, creator, this, nodeIndex.Value, isPersistenceCallbackExceptionRenewNode);
            return nodeIndex.Cast<T>();
        }
        /// <summary>
        ///  Get the client node. If the server does not exist, create the node
        ///  获取客户端客户端节点，服务端不存在则创建节点
        /// </summary>
        /// <typeparam name="T">Client node interface type
        /// 客户端节点接口类型</typeparam>
        /// <typeparam name="PT">Additional parameter type
        /// 附加参数类型</typeparam>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="parameter">Additional parameters
        /// 附加参数</param>
        /// <param name="creator">The delegate for creating the client node
        /// 创建客户端节点委托</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>The client node interface object is derived from AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}
        /// 客户端节点接口对象派生自 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, ResponseParameterAwaiter<NodeIndex>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            return GetOrCreateNode<T>(key, (nodexIndex, nodeKey, nodeInfo) => creator(nodexIndex, nodeKey, nodeInfo, parameter), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the node identifier and create a node when it does not exist
        /// 获取节点标识，不存在节点时创建节点
        /// </summary>
        /// <typeparam name="T">Client node interface type
        /// 客户端节点接口类型</typeparam>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="creator">The delegate for creating the client node
        /// 创建客户端节点委托</param>
        /// <returns></returns>
        internal async Task<ResponseResult<NodeIndex>> GetOrCreateNodeIndex<T>(string key, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>> creator) where T : class
        {
            var exception = default(Exception);
            var nodeInfo = ClientNodeCreator<T>.GetNodeInfo(out exception);
            if (exception == null)
            {
                await createNodeLock.WaitAsync();
                try
                {
                    CommandClientReturnValue<NodeIndex> index = await Client.StreamPersistenceMemoryDatabaseClient.GetNodeIndex(key, nodeInfo.notNull(), true);
                    if (index.IsSuccess)
                    {
                        CallStateEnum state = index.Value.GetState();
                        if (state == CallStateEnum.Success)
                        {
                            if (index.Value.GetFree())
                            {
                                ResponseResult<NodeIndex> nodeIndex = await creator(index.Value, key, nodeInfo.notNull());
                                if (nodeIndex.IsSuccess)
                                {
                                    state = nodeIndex.Value.GetState();
                                    if (state == CallStateEnum.Success) return nodeIndex.Value;
                                    return state;
                                }
                                return nodeIndex.Cast<NodeIndex>();
                            }
                            return index.Value;
                        }
                        return state;
                    }
                    return new ResponseResult<NodeIndex>(index.ReturnType, index.ErrorMessage);
                }
                finally { createNodeLock.Release(); }
            }
            return new ResponseResult<NodeIndex>(CallStateEnum.NotFoundClientNodeCreator, exception.Message);
        }
        /// <summary>
        /// Fix the interface method error and force overwriting the original interface method call. Except for the first parameter being the operation node object, the method definition must be consistent
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method">It must be a static method. The first parameter must be the interface type of the operation node, and the method number must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        public async Task<CommandClientReturnValue<CallStateEnum>> RepairNodeMethod(ClientNode node, MethodInfo method)
        {
            Assembly assembly = method.DeclaringType.notNull().Assembly;
            byte[] rawAssembly = await AutoCSer.Common.ReadFileAllBytes(assembly.Location);
            return await Client.StreamPersistenceMemoryDatabaseClient.RepairNodeMethod(node.Index, rawAssembly, new RepairNodeMethodName(method));
        }
        /// <summary>
        /// Fix the interface method error and force overwriting the original interface method call. Except for the first parameter being the operation node object, the method definition must be consistent
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="node">For instance AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode, all client node interface instances are derived from this type
        /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode 实例，所有的客户端节点接口实例都派生自该类型</param>
        /// <param name="method">It must be a static method. The first parameter must be the interface type of the operation node, and the method number must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<CommandClientReturnValue<CallStateEnum>> RepairNodeMethod(object node, MethodInfo method)
        {
            return RepairNodeMethod((ClientNode)node, method);
        }
        /// <summary>
        /// Bind a new method to dynamically add interface functionality. The initial state of the new method number must be free
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method">It must be a static method. The first parameter must be the interface type of the operation node. The method number and other necessary configuration information must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        public async Task<CommandClientReturnValue<CallStateEnum>> BindNodeMethod(ClientNode node, MethodInfo method)
        {
            Assembly assembly = method.DeclaringType.notNull().Assembly;
            byte[] rawAssembly = await AutoCSer.Common.ReadFileAllBytes(assembly.Location);
            return await Client.StreamPersistenceMemoryDatabaseClient.BindNodeMethod(node.Index, rawAssembly, new RepairNodeMethodName(method));
        }
        /// <summary>
        /// Bind a new method to dynamically add interface functionality. The initial state of the new method number must be free
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="node">For instance AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode, all client node interface instances are derived from this type
        /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode 实例，所有的客户端节点接口实例都派生自该类型</param>
        /// <param name="method">It must be a static method. The first parameter must be the interface type of the operation node. The method number and other necessary configuration information must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<CommandClientReturnValue<CallStateEnum>> BindNodeMethod(object node, MethodInfo method)
        {
            return BindNodeMethod((ClientNode)node, method);
        }

        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseResultAwaiter Call(ClientNode node, int methodIndex)
        {
            if (node.IsSynchronousCallback) return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCall(node.Index, methodIndex));
            return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.Call(node.Index, methodIndex));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseResultAwaiter CallWrite(ClientNode node, int methodIndex)
        {
            if (node.IsSynchronousCallback) return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallWrite(node.Index, methodIndex));
            return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallWrite(node.Index, methodIndex));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallCommand(ClientNode node, int methodIndex, Action<ResponseResult> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCall(node.Index, methodIndex, new CallbackCommandResponseParameter(node, callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.Call(node.Index, methodIndex, new CallbackCommandResponseParameter(node, callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallWriteCommand(ClientNode node, int methodIndex, Action<ResponseResult> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallWrite(node.Index, methodIndex, new CallbackCommandResponseParameter(node, callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallWrite(node.Index, methodIndex, new CallbackCommandResponseParameter(node, callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseResultAwaiter CallInput<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            if (node.IsSynchronousCallback) return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter))));
            return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter))));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseResultAwaiter CallInputWrite<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            if (node.IsSynchronousCallback) return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter))));
            return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter))));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseResultAwaiter SimpleSerializeCallInput<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            if (node.IsSynchronousCallback) return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter))));
            return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter))));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ResponseResultAwaiter SimpleSerializeCallInputWrite<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            if (node.IsSynchronousCallback) return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter))));
            return new ResponseResultAwaiter(node, node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter))));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallInputCommand<T>(ClientNode node, int methodIndex, T parameter, Action<ResponseResult> callback) where T : struct
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand CallInputWriteCommand<T>(ClientNode node, int methodIndex, T parameter, Action<ResponseResult> callback) where T : struct
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand SimpleSerializeCallInputCommand<T>(ClientNode node, int methodIndex, T parameter, Action<ResponseResult> callback) where T : struct
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInput(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.CallbackCommand SimpleSerializeCallInputWriteCommand<T>(ClientNode node, int methodIndex, T parameter, Action<ResponseResult> callback) where T : struct
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)), new CallbackCommandResponseParameter(node, callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<ResponseParameter> CallOutputResponseParameter(ClientNode node, int methodIndex, ResponseParameter responseParameter)
        {
            ResponseParameterAwaiter<ResponseParameter> responseParameterAwaiter = new ResponseParameterAwaiter<ResponseParameter>(node, responseParameter);
            if (node.IsSynchronousCallback) responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutput(responseParameter, node.Index, methodIndex));
            else responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, node.Index, methodIndex));
            return responseParameterAwaiter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<ResponseParameter> CallOutputWriteResponseParameter(ClientNode node, int methodIndex, ResponseParameter responseParameter)
        {
            ResponseParameterAwaiter<ResponseParameter> responseParameterAwaiter = new ResponseParameterAwaiter<ResponseParameter>(node, responseParameter);
            if (node.IsSynchronousCallback) responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutputWrite(responseParameter, node.Index, methodIndex));
            else responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutputWrite(responseParameter, node.Index, methodIndex));
            return responseParameterAwaiter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<T> CallOutput<T>(ClientNode node, int methodIndex)
        {
            ResponseParameterAwaiter<T> responseParameter = new BinarySerializeResponseParameterAwaiter<T>(node);
            if (node.IsSynchronousCallback) responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutput(responseParameter, node.Index, methodIndex));
            else responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, node.Index, methodIndex));
            return responseParameter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<T> CallOutputWrite<T>(ClientNode node, int methodIndex)
        {
            ResponseParameterAwaiter<T> responseParameter = new BinarySerializeResponseParameterAwaiter<T>(node);
            if (node.IsSynchronousCallback) responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutputWrite(responseParameter, node.Index, methodIndex));
            else responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutputWrite(responseParameter, node.Index, methodIndex));
            return responseParameter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<T> SimpleDeserializeCallOutput<T>(ClientNode node, int methodIndex)
        {
            ResponseParameterAwaiter<T> responseParameter = new SimpleSerializeResponseParameterAwaiter<T>(node);
            if (node.IsSynchronousCallback) responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutput(responseParameter, node.Index, methodIndex));
            else responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, node.Index, methodIndex));
            return responseParameter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<T> SimpleDeserializeCallOutputWrite<T>(ClientNode node, int methodIndex)
        {
            ResponseParameterAwaiter<T> responseParameter = new SimpleSerializeResponseParameterAwaiter<T>(node);
            if (node.IsSynchronousCallback) responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutputWrite(responseParameter, node.Index, methodIndex));
            else responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutputWrite(responseParameter, node.Index, methodIndex));
            return responseParameter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallOutputCommandResponseParameter(ClientNode node, int methodIndex, ResponseParameter responseParameter, Action<ResponseResult<ResponseParameter>> callback)
        {
            CallbackCommandResponseParameter<ResponseParameter> callbackCommandResponseParameter = new CallbackCommandResponseParameter<ResponseParameter>(node, responseParameter, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutput(responseParameter, node.Index, methodIndex, callbackCommandResponseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, node.Index, methodIndex, callbackCommandResponseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallOutputWriteCommandResponseParameter(ClientNode node, int methodIndex, ResponseParameter responseParameter, Action<ResponseResult<ResponseParameter>> callback)
        {
            CallbackCommandResponseParameter<ResponseParameter> callbackCommandResponseParameter = new CallbackCommandResponseParameter<ResponseParameter>(node, responseParameter, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutputWrite(responseParameter, node.Index, methodIndex, callbackCommandResponseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutputWrite(responseParameter, node.Index, methodIndex, callbackCommandResponseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallOutputCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>> callback)
        {
            CallbackCommandResponseParameter<T> responseParameter = new CallbackCommandBinarySerializeResponseParameter<T>(node, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutput(responseParameter, node.Index, methodIndex, responseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, node.Index, methodIndex, responseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallOutputWriteCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>> callback)
        {
            CallbackCommandResponseParameter<T> responseParameter = new CallbackCommandBinarySerializeResponseParameter<T>(node, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutputWrite(responseParameter, node.Index, methodIndex, responseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutputWrite(responseParameter, node.Index, methodIndex, responseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand SimpleDeserializeCallOutputCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>> callback)
        {
            CallbackCommandResponseParameter<T> responseParameter = new CallbackCommandSimpleSerializeResponseParameter<T>(node, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutput(responseParameter, node.Index, methodIndex, responseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, node.Index, methodIndex, responseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand SimpleDeserializeCallOutputWriteCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>> callback)
        {
            CallbackCommandResponseParameter<T> responseParameter = new CallbackCommandSimpleSerializeResponseParameter<T>(node, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallOutputWrite(responseParameter, node.Index, methodIndex, responseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutputWrite(responseParameter, node.Index, methodIndex, responseParameter.Callback);
        }
        /// <summary>
        /// Get the serialization of the request parameters
        /// 获取请求参数序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static RequestParameterSerializer getRequestParameterSerializer<T>(MethodFlagsEnum flags, ref T parameter)
            where T : struct
        {
            if ((flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0) return new RequestParameterSimpleSerializer<T>(ref parameter);
            return new RequestParameterBinarySerializer<T>(ref parameter);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<ResponseParameter> CallInputOutputResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameter responseParameter, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            ResponseParameterAwaiter<ResponseParameter> responseParameterAwaiter = new ResponseParameterAwaiter<ResponseParameter>(node, responseParameter);
            if (node.IsSynchronousCallback) responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            else responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            return responseParameterAwaiter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<ResponseParameter> CallInputOutputWriteResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameter responseParameter, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            ResponseParameterAwaiter<ResponseParameter> responseParameterAwaiter = new ResponseParameterAwaiter<ResponseParameter>(node, responseParameter);
            if (node.IsSynchronousCallback) responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            else responseParameterAwaiter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            return responseParameterAwaiter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<RT> CallInputOutput<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            ResponseParameterAwaiter<RT> responseParameter;
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0) responseParameter = new SimpleSerializeResponseParameterAwaiter<RT>(node);
            else responseParameter = new BinarySerializeResponseParameterAwaiter<RT>(node);
            if (node.IsSynchronousCallback) responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            else responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            return responseParameter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static ResponseParameterAwaiter<RT> CallInputOutputWrite<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            ResponseParameterAwaiter<RT> responseParameter;
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0) responseParameter = new SimpleSerializeResponseParameterAwaiter<RT>(node);
            else responseParameter = new BinarySerializeResponseParameterAwaiter<RT>(node);
            if (node.IsSynchronousCallback) responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            else responseParameter.Set(node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter))));
            return responseParameter;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallInputOutputCommandResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameter responseParameter, MethodFlagsEnum flags, T parameter, Action<ResponseResult<ResponseParameter>> callback)
            where T : struct
        {
            CallbackCommandResponseParameter<ResponseParameter> callbackCommandResponseParameter = new CallbackCommandResponseParameter<ResponseParameter>(node, responseParameter, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), callbackCommandResponseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), callbackCommandResponseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallInputOutputWriteCommandResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameter responseParameter, MethodFlagsEnum flags, T parameter, Action<ResponseResult<ResponseParameter>> callback)
            where T : struct
        {
            CallbackCommandResponseParameter<ResponseParameter> callbackCommandResponseParameter = new CallbackCommandResponseParameter<ResponseParameter>(node, responseParameter, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), callbackCommandResponseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), callbackCommandResponseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallInputOutputCommand<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter, Action<ResponseResult<RT>> callback)
            where T : struct
        {
            CallbackCommandResponseParameter<RT> responseParameter;
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0) responseParameter = new CallbackCommandSimpleSerializeResponseParameter<RT>(node, callback);
            else responseParameter = new CallbackCommandBinarySerializeResponseParameter<RT>(node, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), responseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutput(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), responseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.CallbackCommand CallInputOutputWriteCommand<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter, Action<ResponseResult<RT>> callback)
            where T : struct
        {
            CallbackCommandResponseParameter<RT> responseParameter;
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0) responseParameter = new CallbackCommandSimpleSerializeResponseParameter<RT>(node, callback);
            else responseParameter = new CallbackCommandBinarySerializeResponseParameter<RT>(node, callback);
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousCallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), responseParameter.Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutputWrite(responseParameter, new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), responseParameter.Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static SendOnlyCommand SendOnly<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.SendOnly(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static SendOnlyCommand SendOnlyWrite<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.SendOnlyWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static SendOnlyCommand SimpleSerializeSendOnly<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.SendOnly(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static SendOnlyCommand SimpleSerializeSendOnlyWrite<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.SendOnlyWrite(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)));
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<ResponseParameterSerializer>> KeepCallbackResponseParameter(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter)
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex);
            else enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex);
            if (enumeratorCommand == null) return KeepCallbackResponse<ResponseParameterSerializer>.NullResponse;
            try
            {
                KeepCallbackResponse<ResponseParameterSerializer> response = new KeepCallbackResponse<ResponseParameterSerializer>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<ResponseParameterSerializer>> KeepCallbackWriteResponseParameter(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter)
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex);
            else enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex);
            if (enumeratorCommand == null) return KeepCallbackResponse<ResponseParameterSerializer>.NullResponse;
            try
            {
                KeepCallbackResponse<ResponseParameterSerializer> response = new KeepCallbackResponse<ResponseParameterSerializer>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<T>> KeepCallback<T>(ClientNode node, int methodIndex)
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex);
            else enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex);
            if (enumeratorCommand == null) return KeepCallbackResponse<T>.NullResponse;
            try
            {
                KeepCallbackResponse<T> response = new KeepCallbackResponse<T>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<T>> KeepCallbackWrite<T>(ClientNode node, int methodIndex)
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex);
            else enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex);
            if (enumeratorCommand == null) return KeepCallbackResponse<T>.NullResponse;
            try
            {
                KeepCallbackResponse<T> response = new KeepCallbackResponse<T>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<T>> SimpleDeserializeKeepCallback<T>(ClientNode node, int methodIndex)
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex);
            else enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex);
            if (enumeratorCommand == null) return KeepCallbackResponse<T>.NullResponse;
            try
            {
                KeepCallbackResponse<T> response = new KeepCallbackResponse<T>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<T>> SimpleDeserializeKeepCallbackWrite<T>(ClientNode node, int methodIndex)
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex);
            else enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex);
            if (enumeratorCommand == null) return KeepCallbackResponse<T>.NullResponse;
            try
            {
                KeepCallbackResponse<T> response = new KeepCallbackResponse<T>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackCommandResponseParameter(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackWriteCommandResponseParameter(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand KeepCallbackWriteCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, 0), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand SimpleDeserializeKeepCallbackCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand SimpleDeserializeKeepCallbackWriteCommand<T>(ClientNode node, int methodIndex, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand> callback)
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), node.Index, methodIndex, new KeepCallbackCommandResponse<T>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<ResponseParameterSerializer>> InputKeepCallbackResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)));
            else enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)));
            if (await enumeratorCommand == null) return KeepCallbackResponse<ResponseParameterSerializer>.NullResponse;
            try
            {
                KeepCallbackResponse<ResponseParameterSerializer> response = new KeepCallbackResponse<ResponseParameterSerializer>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<ResponseParameterSerializer>> InputKeepCallbackWriteResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if (node.IsSynchronousCallback) enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)));
            else enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)));
            if (await enumeratorCommand == null) return KeepCallbackResponse<ResponseParameterSerializer>.NullResponse;
            try
            {
                KeepCallbackResponse<ResponseParameterSerializer> response = new KeepCallbackResponse<ResponseParameterSerializer>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<RT>> InputKeepCallback<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0)
            {
                if (node.IsSynchronousCallback) enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
                else enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
            }
            else
            {
                if (node.IsSynchronousCallback) enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
                else enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
            }
            if (await enumeratorCommand == null) return KeepCallbackResponse<RT>.NullResponse;
            try
            {
                KeepCallbackResponse<RT> response = new KeepCallbackResponse<RT>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally 
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<RT>> InputKeepCallbackWrite<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0)
            {
                if (node.IsSynchronousCallback) enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
                else enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
            }
            else
            {
                if (node.IsSynchronousCallback) enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
                else enumeratorCommand = node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)));
            }
            if (await enumeratorCommand == null) return KeepCallbackResponse<RT>.NullResponse;
            try
            {
                KeepCallbackResponse<RT> response = new KeepCallbackResponse<RT>(node, enumeratorCommand);
                enumeratorCommand = null;
                return response;
            }
            finally
            {
                if (enumeratorCommand != null) ((IDisposable)enumeratorCommand).Dispose();
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand InputKeepCallbackCommandResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter, MethodFlagsEnum flags, T parameter, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand> callback)
            where T : struct
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="responseParameter">Return parameter
        /// 返回参数</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static AutoCSer.Net.KeepCallbackCommand InputKeepCallbackWriteCommandResponseParameter<T>(ClientNode node, int methodIndex, ResponseParameterSerializer responseParameter, MethodFlagsEnum flags, T parameter, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand> callback)
            where T : struct
        {
            if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite(new KeepCallbackResponseParameter(responseParameter, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer<T>(flags, ref parameter)), new KeepCallbackCommandResponse<ResponseParameterSerializer>(callback).Callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.KeepCallbackCommand InputKeepCallbackCommand<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter, Action<ResponseResult<RT>, AutoCSer.Net.KeepCallbackCommand> callback)
            where T : struct
        {
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0)
            {
                if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
                return node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
            }
            else
            {
                if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
                return node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="flags">Server-side node method flags
        /// 服务端节点方法标记</param>
        /// <param name="parameter">Call the method request parameters
        /// 调用方法请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static AutoCSer.Net.KeepCallbackCommand InputKeepCallbackWriteCommand<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter, Action<ResponseResult<RT>, AutoCSer.Net.KeepCallbackCommand> callback)
            where T : struct
        {
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0)
            {
                if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
                return node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, MethodFlagsEnum.IsSimpleSerializeParamter), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
            }
            else
            {
                if (node.IsSynchronousCallback) return node.Client.Client.StreamPersistenceMemoryDatabaseClient.ClientSynchronousInputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
                return node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, 0), new RequestParameter(node.Index, methodIndex, getRequestParameterSerializer(flags, ref parameter)), new KeepCallbackCommandResponse<RT>(callback).Callback);
            }
        }
    }
    /// <summary>
    /// Log stream persistence in-memory database client
    /// 日志流持久化内存数据库客户端
    /// </summary>
    /// <typeparam name="CT">Service basic operation client interface type
    /// 服务基础操作客户端接口类型</typeparam>
    public class StreamPersistenceMemoryDatabaseClient<CT> : StreamPersistenceMemoryDatabaseClient
        where CT : class, IServiceNodeClientNode
    {
        /// <summary>
        /// Service basic operation client
        /// 服务基础操作客户端
        /// </summary>
        public readonly CT ClientNode;
        /// <summary>
        /// Log stream persistence in-memory database client
        /// 日志流持久化内存数据库客户端
        /// </summary>
        /// <param name="client">Log stream persists in-memory database client socket events
        /// 日志流持久化内存数据库客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClient(IStreamPersistenceMemoryDatabaseClientSocketEvent client) : base(client)
        {
            ClientNode = ClientNodeCreator<CT>.Create(string.Empty, null, this, ServiceNode.ServiceNodeIndex, false);
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseParameterAwaiter<bool> RemoveNode(NodeIndex index)
        {
            return ClientNode.RemoveNode(index);
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ResponseParameterAwaiter<bool> RemoveNode(ClientNode node)
        {
            return ClientNode.RemoveNode(node.Index);
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseParameterAwaiter<bool> RemoveNode(string key)
        {
            return ClientNode.RemoveNodeByKey(key);
        }
        /// <summary>
        /// Get the server registration client node. If the server does not exist, create the node IServerRegistryNode
        /// 获取服务注册客户端节点，服务端不存在则创建节点 IServerRegistryNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="loadTimeoutSeconds">Cold start session timeout seconds
        /// 冷启动会话超时秒数</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IServerRegistryNodeClientNode>> GetOrCreateServerRegistryNode(string key = nameof(ServerRegistryNode), int loadTimeoutSeconds = ServerRegistryNode.DefaultLoadTimeoutSeconds, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IServerRegistryNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateServerRegistryNode(index, nodeKey, nodeInfo, loadTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the process guardian client node. If the server does not exist, create the node IProcessGuardNode
        /// 获取进程守护客户端节点，服务端不存在则创建节点 IProcessGuardNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IProcessGuardNodeClientNode>> GetOrCreateProcessGuardNode(string key = nameof(ProcessGuardNode), bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IProcessGuardNodeClientNode>(key, ClientNode.CreateProcessGuardNode, isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the message processing client node. If the server does not exist, create a node MessageNode{ServerByteArrayMessage}
        /// 获取消息处理客户端节点，服务端不存在则创建节点 MessageNode{ServerByteArrayMessage}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IMessageNodeClientNode<ServerByteArrayMessage>>> GetOrCreateServerByteArrayMessageNode(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IMessageNodeClientNode<ServerByteArrayMessage>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateServerByteArrayMessageNode(index, nodeKey, nodeInfo, arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the message processing client node. If the server does not exist, create the node MessageNode{BinaryMessage{T}}
        /// 获取消息处理客户端节点，服务端不存在则创建节点 MessageNode{BinaryMessage{T}}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IMessageNodeClientNode<BinaryMessage<T>>>> GetOrCreateBinaryMessageNode<T>(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IMessageNodeClientNode<BinaryMessage<T>>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateMessageNode(index, nodeKey, nodeInfo, typeof(BinaryMessage<T>), arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the message processing client node. If the server does not exist, create the node MessageNode{T}
        /// 获取消息处理客户端节点，服务端不存在则创建节点 MessageNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IMessageNodeClientNode<T>>> GetOrCreateMessageNode<T>(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
            where T : Message<T>
        {
            return GetOrCreateNode<IMessageNodeClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateMessageNode(index, nodeKey, nodeInfo, typeof(T), arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the distributed lock client node. If the server does not exist, create the node DistributedLockNode{KT}
        /// 获取分布式锁客户端节点，服务端不存在则创建节点 DistributedLockNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IDistributedLockNodeClientNode<KT>>> GetOrCreateDistributedLockNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IDistributedLockNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateDistributedLockNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the multi-hash bitmap client-side synchronous filter node (similar to a Bloom filter, suitable for small containers). If the server does not exist, create node ManyHashBitMapClientFilterNode
        /// 获取多哈希位图客户端同步过滤节点（类似布隆过滤器，适合小容器），服务端不存在则创建节点 ManyHashBitMapClientFilterNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IManyHashBitMapClientFilterNodeClientNode>> GetOrCreateManyHashBitMapClientFilterNode(string key, int size, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IManyHashBitMapClientFilterNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateManyHashBitMapClientFilterNode(index, nodeKey, nodeInfo, size), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the multi-hash bitmap filter node (similar to a Bloom filter). If the server does not exist, create node ManyHashBitMapFilterNode
        /// 获取多哈希位图过滤节点（类似布隆过滤器），服务端不存在则创建节点 ManyHashBitMapFilterNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IManyHashBitMapFilterNodeClientNode>> GetOrCreateManyHashBitMapFilterNode(string key, int size, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IManyHashBitMapFilterNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateManyHashBitMapFilterNode(index, nodeKey, nodeInfo, size), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the dictionary client node. If the server does not exist, create node HashBytesFragmentDictionaryNode
        /// 获取字典客户端节点，服务端不存在则创建节点 HashBytesFragmentDictionaryNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IHashBytesFragmentDictionaryNodeClientNode>> GetOrCreateHashBytesFragmentDictionaryNode(string key, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IHashBytesFragmentDictionaryNodeClientNode>(key, ClientNode.CreateHashBytesFragmentDictionaryNode, isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the dictionary client node. If the server does not exist, create node ByteArrayFragmentDictionaryNode{KT}
        /// 获取字典客户端节点，服务端不存在则创建节点 ByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IByteArrayFragmentDictionaryNodeClientNode<KT>>> GetOrCreateByteArrayFragmentDictionaryNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IByteArrayFragmentDictionaryNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateByteArrayFragmentDictionaryNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the dictionary client node. If the server does not exist, create node FragmentDictionaryNode{KT,VT}
        /// 获取字典客户端节点，服务端不存在则创建节点 FragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IFragmentDictionaryNodeClientNode<KT, VT>>> GetOrCreateFragmentDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IFragmentDictionaryNodeClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateFragmentDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the dictionary client node. If the server does not exist, create node HashBytesDictionaryNode
        /// 获取字典客户端节点，服务端不存在则创建节点 HashBytesDictionaryNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IHashBytesDictionaryNodeClientNode>> GetOrCreateHashBytesDictionaryNode(string key, int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IHashBytesDictionaryNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateHashBytesDictionaryNode(index, nodeKey, nodeInfo, capacity, groupType), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the dictionary client node. If the server does not exist, create node ByteArrayDictionaryNode{KT}
        /// 获取字典客户端节点，服务端不存在则创建节点 ByteArrayDictionaryNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IByteArrayDictionaryNodeClientNode<KT>>> GetOrCreateByteArrayDictionaryNode<KT>(string key, int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IByteArrayDictionaryNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateByteArrayDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), capacity, groupType), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the dictionary client node. If the server does not exist, create node DictionaryNode{KT,VT}
        /// 获取字典客户端节点，服务端不存在则创建节点 DictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IDictionaryNodeClientNode<KT, VT>>> GetOrCreateDictionaryNode<KT, VT>(string key, int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IDictionaryNodeClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT), capacity, groupType), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get a binary search tree client node. If the server does not exist, create node SearchTreeDictionaryNode{KT,VT}
        /// 获取二叉搜索树客户端节点，服务端不存在则创建节点 SearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<ISearchTreeDictionaryNodeClientNode<KT, VT>>> GetOrCreateSearchTreeDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISearchTreeDictionaryNodeClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSearchTreeDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the client node of the sorting dictionary. If the server does not exist, create node SortedDictionaryNode{KT,VT}
        /// 获取排序字典客户端节点，服务端不存在则创建节点 SortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<ISortedDictionaryNodeClientNode<KT, VT>>> GetOrCreateSortedDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedDictionaryNodeClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the client nodes of the sorting list. If the server does not exist, create node SortedListNode{KT,VT}
        /// 获取排序列表客户端节点，服务端不存在则创建节点 SortedListNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<ISortedListNodeClientNode<KT, VT>>> GetOrCreateSortedListNode<KT, VT>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedListNodeClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedListNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the client nodes of the 256 base fragment hash table. If the server does not exist, create node FragmentHashSetNode{KT}
        /// 获取 256 基分片哈希表客户端节点，服务端不存在则创建节点 FragmentHashSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IFragmentHashSetNodeClientNode<KT>>> GetOrCreateFragmentHashSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IFragmentHashSetNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateFragmentHashSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the client node of the hash table. If the server does not exist, create node HashSetNode{KT}
        /// 获取哈希表客户端节点，服务端不存在则创建节点 HashSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IHashSetNodeClientNode<KT>>> GetOrCreateHashSetNode<KT>(string key, int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IHashSetNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateHashSetNode(index, nodeKey, nodeInfo, typeof(KT), capacity, groupType), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the binary search tree set of client nodes. If the server does not exist, create node SearchTreeSetNode{KT}
        /// 获取二叉搜索树集合客户端节点，服务端不存在则创建节点 SearchTreeSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<ISearchTreeSetNodeClientNode<KT>>> GetOrCreateSearchTreeSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISearchTreeSetNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSearchTreeSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the client nodes of the sorted collection. If the server does not exist, create node SortedSetNode{KT}
        /// 获取排序集合客户端节点，服务端不存在则创建节点 SortedSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<ISortedSetNodeClientNode<KT>>> GetOrCreateSortedSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedSetNodeClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the queue client node (first-in-first-out). If the server does not exist, create node ByteArrayQueueNodeClientNode
        /// 获取队列客户端节点（先进先出），服务端不存在则创建节点 ByteArrayQueueNodeClientNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IByteArrayQueueNodeClientNode>> GetOrCreateByteArrayQueueNode(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IByteArrayQueueNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateByteArrayQueueNode(index, nodeKey, nodeInfo, capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the queue client node (first-in-first-out). If the server does not exist, create node QueueNode{T}
        /// 获取队列客户端节点（先进先出），服务端不存在则创建节点 QueueNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IQueueNodeClientNode<T>>> GetOrCreateQueueNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IQueueNodeClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateQueueNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the stack client node (last in, first out). If the server does not exist, create node ByteArrayStackNodeClientNode
        /// 获取栈客户端节点（后进先出），服务端不存在则创建节点 ByteArrayStackNodeClientNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IByteArrayStackNodeClientNode>> GetOrCreateByteArrayStackNode(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IByteArrayStackNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateByteArrayStackNode(index, nodeKey, nodeInfo, capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the stack client node (last in, first out). If the server does not exist, create node StackNode{T}
        /// 获取栈客户端节点（后进先出），服务端不存在则创建节点 StackNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IStackNodeClientNode<T>>> GetOrCreateStackNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IStackNodeClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateStackNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the array client nodes. If the server does not exist, create node LeftArrayNode{T}
        /// 获取数组客户端节点，服务端不存在则创建节点 LeftArrayNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<ILeftArrayNodeClientNode<T>>> GetOrCreateLeftArrayNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<ILeftArrayNodeClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateLeftArrayNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the array client nodes. If the server does not exist, create node ArrayNode{T}
        /// 获取数组客户端节点，服务端不存在则创建节点 ArrayNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="length">Array length</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IArrayNodeClientNode<T>>> GetOrCreateArrayNode<T>(string key, int length, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IArrayNodeClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateArrayNode(index, nodeKey, nodeInfo, typeof(T), length), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get archive-only data client nodes. If the server does not exist, create node OnlyPersistenceNode{T}
        /// 获取仅存档数据客户端节点，服务端不存在则创建节点 OnlyPersistenceNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IOnlyPersistenceNodeClientNode<T>>> GetOrCreateOnlyPersistenceNode<T>(string key)
        {
            return GetOrCreateNode<IOnlyPersistenceNodeClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateOnlyPersistenceNode(index, nodeKey, nodeInfo, typeof(T)), false);
        }
        /// <summary>
        /// Get a 64-bit auto-increment identity client node. If the server does not exist, create node IdentityGeneratorNode
        /// 获取 64 位自增ID 客户端节点，服务端不存在则创建节点 IdentityGeneratorNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="identity">Initial Allocation identity
        /// 起始分配 ID</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IIdentityGeneratorNodeClientNode>> GetOrCreateIdentityGeneratorNode(string key, long identity = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IIdentityGeneratorNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateIdentityGeneratorNode(index, nodeKey, nodeInfo, identity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the bitmap client node. If the server does not exist, create node BitmapNode
        /// 获取位图客户端节点，服务端不存在则创建节点 BitmapNode
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IBitmapNodeClientNode>> GetOrCreateBitmapNode(string key, uint capacity, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IBitmapNodeClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateBitmapNode(index, nodeKey, nodeInfo, capacity), isPersistenceCallbackExceptionRenewNode);
        }
    }
}
