using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端
    /// </summary>
    public abstract class StreamPersistenceMemoryDatabaseClient
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端套接字事件
        /// </summary>
        public readonly IStreamPersistenceMemoryDatabaseClientSocketEvent Client;
        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
        /// <param name="client">日志流持久化内存数据库客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClient(IStreamPersistenceMemoryDatabaseClientSocketEvent client)
        {
            this.Client = client;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public abstract Task<ResponseResult<bool>> RemoveNode(ClientNode node);
        /// <summary>
        /// 获取节点，不存在则创建节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建客户端节点委托</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点接口对象派生自 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}</returns>
        public async Task<ResponseResult<T>> GetOrCreateNode<T>(string key, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            ResponseResult<NodeIndex> nodeIndex = await GetOrCreateNodeIndex<T>(key, creator);
            if (nodeIndex.IsSuccess) return ClientNodeCreator<T>.Create(key, creator, this, nodeIndex.Value, isPersistenceCallbackExceptionRenewNode);
            return nodeIndex.Cast<T>();
        }
        /// <summary>
        /// 获取节点，不存在则创建节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="PT">附加参数类型</typeparam>
        /// <param name="key">节点全局关键字</param>
        /// <param name="parameter">附加参数</param>
        /// <param name="creator">创建客户端节点委托</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点接口对象派生自 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, Task<ResponseResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            return GetOrCreateNode<T>(key, (nodexIndex, nodeKey, nodeInfo) => creator(nodexIndex, nodeKey, nodeInfo, parameter), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取创建节点标识
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建客户端节点委托</param>
        /// <returns></returns>
        internal async Task<ResponseResult<NodeIndex>> GetOrCreateNodeIndex<T>(string key, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>> creator) where T : class
        {
            var exception = default(Exception);
            NodeInfo nodeInfo = ClientNodeCreator<T>.GetNodeInfo(out exception);
            if (exception == null)
            {
                CommandClientReturnValue<NodeIndex> index = await Client.StreamPersistenceMemoryDatabaseClient.GetNodeIndex(key, nodeInfo, true);
                if (index.IsSuccess)
                {
                    CallStateEnum state = index.Value.GetState();
                    if (state == CallStateEnum.Success)
                    {
                        if (index.Value.GetFree())
                        {
                            ResponseResult<NodeIndex> nodeIndex = await creator(index.Value, key, nodeInfo);
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
            return CallStateEnum.NotFoundClientNodeCreator;
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        public async Task<CommandClientReturnValue<CallStateEnum>> RepairNodeMethod(ClientNode node, MethodInfo method)
        {
            Assembly assembly = method.DeclaringType.notNull().Assembly;
            byte[] rawAssembly = await AutoCSer.Common.ReadFileAllBytes(assembly.Location);
            return await Client.StreamPersistenceMemoryDatabaseClient.RepairNodeMethod(node.Index, rawAssembly, new RepairNodeMethodName(method));
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="node">AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode 实例，所有的客户端节点接口实例都派生自该类型</param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<CommandClientReturnValue<CallStateEnum>> RepairNodeMethod(object node, MethodInfo method)
        {
            return RepairNodeMethod((ClientNode)node, method);
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        public async Task<CommandClientReturnValue<CallStateEnum>> BindNodeMethod(ClientNode node, MethodInfo method)
        {
            Assembly assembly = method.DeclaringType.notNull().Assembly;
            byte[] rawAssembly = await AutoCSer.Common.ReadFileAllBytes(assembly.Location);
            return await Client.StreamPersistenceMemoryDatabaseClient.BindNodeMethod(node.Index, rawAssembly, new RepairNodeMethodName(method));
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="node">AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode 实例，所有的客户端节点接口实例都派生自该类型</param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<CommandClientReturnValue<CallStateEnum>> BindNodeMethod(object node, MethodInfo method)
        {
            return BindNodeMethod((ClientNode)node, method);
        }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        internal static async Task<ResponseResult> Call(ClientNode node, int methodIndex)
        {
            NodeIndex nodeIndex = node.Index;
            CommandClientReturnValue<CallStateEnum> returnValue = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.Call(nodeIndex, methodIndex);
            if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success && returnValue.Value != CallStateEnum.Success) await node.CheckStateAsync(nodeIndex, returnValue.Value);
            return new ResponseResult(returnValue);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<ResponseResult> CallInput<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            NodeIndex nodeIndex = node.Index;
            CommandClientReturnValue<CallStateEnum> returnValue = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInput(new RequestParameter(nodeIndex, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)));
            if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success && returnValue.Value != CallStateEnum.Success) await node.CheckStateAsync(nodeIndex, returnValue.Value);
            return new ResponseResult(returnValue);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<ResponseResult> SimpleSerializeCallInput<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            NodeIndex nodeIndex = node.Index;
            CommandClientReturnValue<CallStateEnum> returnValue = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInput(new RequestParameter(nodeIndex, methodIndex, new RequestParameterSimpleSerializer<T> (ref parameter)));
            if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success && returnValue.Value != CallStateEnum.Success) await node.CheckStateAsync(nodeIndex, returnValue.Value);
            return new ResponseResult(returnValue);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        internal static async Task<ResponseResult<T>> CallOutput<T>(ClientNode node, int methodIndex)
        {
            NodeIndex nodeIndex = node.Index;
            ResponseParameter<T> responseParameter = new BinarySerializeResponseParameter<T>();
            CommandClientReturnValue<ResponseParameter> returnValue = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, nodeIndex, methodIndex);
            if (returnValue.IsSuccess)
            {
                switch (returnValue.Value.State)
                {
                    case CallStateEnum.Success: return new ResponseResult<T>(responseParameter.Value.ReturnValue);
                    case CallStateEnum.PersistenceCallbackException: await node.Renew(nodeIndex); break;
                    case CallStateEnum.NodeIndexOutOfRange:
                    case CallStateEnum.NodeIdentityNotMatch:
                        await node.Reindex(nodeIndex);
                        break;
                }
                return returnValue.Value.State;
            }
            return new ResponseResult<T>(returnValue.ReturnType, returnValue.ErrorMessage);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        internal static async Task<ResponseResult<T>> SimpleDeserializeCallOutput<T>(ClientNode node, int methodIndex)
        {
            NodeIndex nodeIndex = node.Index;
            ResponseParameter<T> responseParameter = new SimpleSerializeResponseParameter<T>();
            CommandClientReturnValue<ResponseParameter> returnValue = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallOutput(responseParameter, nodeIndex, methodIndex);
            if (returnValue.IsSuccess)
            {
                switch (returnValue.Value.State)
                {
                    case CallStateEnum.Success: return new ResponseResult<T>(responseParameter.Value.ReturnValue);
                    case CallStateEnum.PersistenceCallbackException: await node.Renew(nodeIndex); break;
                    case CallStateEnum.NodeIndexOutOfRange:
                    case CallStateEnum.NodeIdentityNotMatch:
                        await node.Reindex(nodeIndex);
                        break;
                }
                return returnValue.Value.State;
            }
            return new ResponseResult<T>(returnValue.ReturnType, returnValue.ErrorMessage);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        /// <param name="parameter">调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<ResponseResult<RT>> CallInputOutput<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            NodeIndex nodeIndex = node.Index;
            ResponseParameter<RT> responseParameter = (flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0 ? (ResponseParameter<RT>)new SimpleSerializeResponseParameter<RT>() : new BinarySerializeResponseParameter<RT>();
            CommandClientReturnValue<ResponseParameter> returnValue = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.CallInputOutput(responseParameter, new RequestParameter(nodeIndex, methodIndex, (flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0 ? (RequestParameterSerializer)new RequestParameterSimpleSerializer<T>(ref parameter) : new RequestParameterBinarySerializer<T>(ref parameter)));
            if (returnValue.IsSuccess)
            {
                switch (returnValue.Value.State)
                {
                    case CallStateEnum.Success: return new ResponseResult<RT>(responseParameter.Value.ReturnValue);
                    case CallStateEnum.PersistenceCallbackException: await node.Renew(nodeIndex); break;
                    case CallStateEnum.NodeIndexOutOfRange:
                    case CallStateEnum.NodeIdentityNotMatch:
                        await node.Reindex(nodeIndex);
                        break;
                }
                return returnValue.Value.State;
            }
            return new ResponseResult<RT>(returnValue.ReturnType, returnValue.ErrorMessage);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用方法请求参数</param>
        /// <returns></returns>
        internal static SendOnlyCommand SendOnly<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.SendOnly(new RequestParameter(node.Index, methodIndex, new RequestParameterBinarySerializer<T>(ref parameter)));
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用方法请求参数</param>
        /// <returns></returns>
        internal static SendOnlyCommand SimpleSerializeSendOnly<T>(ClientNode node, int methodIndex, T parameter) where T : struct
        {
            return node.Client.Client.StreamPersistenceMemoryDatabaseClient.SendOnly(new RequestParameter(node.Index, methodIndex, new RequestParameterSimpleSerializer<T>(ref parameter)));
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<T>> KeepCallback<T>(ClientNode node, int methodIndex)
        {
            var enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<T>.Default, false), node.Index, methodIndex);
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
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<T>> SimpleDeserializeKeepCallback<T>(ClientNode node, int methodIndex)
        {
            var enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.KeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<T>.Default, true), node.Index, methodIndex);
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
        /// 调用节点方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="flags">服务端节点方法标记</param>
        /// <param name="parameter">调用方法请求参数</param>
        /// <returns></returns>
        internal static async Task<KeepCallbackResponse<RT>> InputKeepCallback<T, RT>(ClientNode node, int methodIndex, MethodFlagsEnum flags, T parameter)
            where T : struct
        {
            var enumeratorCommand = default(AutoCSer.Net.EnumeratorCommand<KeepCallbackResponseParameter>);
            if ((flags & MethodFlagsEnum.IsSimpleDeserializeParamter) != 0)
            {
                enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterSimpleSerializer<RT>.Default, true), new RequestParameter(node.Index, methodIndex, (flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0 ? (RequestParameterSerializer)new RequestParameterSimpleSerializer<T>(ref parameter) : new RequestParameterBinarySerializer<T>(ref parameter)));
            }
            else
            {
                enumeratorCommand = await node.Client.Client.StreamPersistenceMemoryDatabaseClient.InputKeepCallback(new KeepCallbackResponseParameter(KeepCallbackResponseParameterBinarySerializer<RT>.Default, false), new RequestParameter(node.Index, methodIndex, (flags & MethodFlagsEnum.IsSimpleSerializeParamter) != 0 ? (RequestParameterSerializer)new RequestParameterSimpleSerializer<T>(ref parameter) : new RequestParameterBinarySerializer<T>(ref parameter)));
            }
            if (enumeratorCommand == null) return KeepCallbackResponse<RT>.NullResponse;
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
    }
    /// <summary>
    /// 日志流持久化内存数据库客户端
    /// </summary>
    /// <typeparam name="CT">服务基础操作客户端接口类型</typeparam>
    public class StreamPersistenceMemoryDatabaseClient<CT> : StreamPersistenceMemoryDatabaseClient
        where CT : class, IServiceNodeClientNode
    {
        /// <summary>
        /// 服务基础操作客户端
        /// </summary>
        public readonly CT ClientNode;
        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
        /// <param name="client">日志流持久化内存数据库客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClient(IStreamPersistenceMemoryDatabaseClientSocketEvent client) : base(client)
        {
            ClientNode = ClientNodeCreator<CT>.Create(string.Empty, null, this, ServiceNode.ServiceNodeIndex, false);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<bool>> RemoveNode(NodeIndex index)
        {
            return ClientNode.RemoveNode(index);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override Task<ResponseResult<bool>> RemoveNode(ClientNode node)
        {
            return ClientNode.RemoveNode(node.Index);
        }
        /// <summary>
        /// 获取字典节点，不存在则创建节点 FragmentHashStringDictionary256{HashString,string}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<IHashStringFragmentDictionaryNodeClientNode<string>>> GetOrCreateFragmentHashStringDictionaryNode(string key, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IHashStringFragmentDictionaryNodeClientNode<string>>(key, ClientNode.CreateFragmentHashStringDictionaryNode, isPersistenceCallbackExceptionRenewNode);
        }
    }
}
