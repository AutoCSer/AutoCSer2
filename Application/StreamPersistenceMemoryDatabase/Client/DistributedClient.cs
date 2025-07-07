using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Distributed client
    /// 分布式客户端
    /// </summary>
    public abstract class DistributedClient
    {
        /// <summary>
        /// Client collection
        /// 客户端集合
        /// </summary>
        protected readonly Dictionary<string, Task<StreamPersistenceMemoryDatabaseClient>> clients;
        /// <summary>
        /// The access lock of the client collection
        /// 客户端集合访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim clientLock;
        /// <summary>
        /// Distributed client
        /// 分布式客户端
        /// </summary>
        private DistributedClient()
        {
            clientLock = new System.Threading.SemaphoreSlim(1, 1);
            clients = DictionaryCreator<string>.Create<Task<StreamPersistenceMemoryDatabaseClient>>();
        }
        /// <summary>
        /// Create the client based on the global keywords of the node
        /// 根据节点全局关键字创建客户端
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Log stream persistence in-memory database client
        /// 日志流持久化内存数据库客户端</returns>
        protected abstract Task<StreamPersistenceMemoryDatabaseClient> createClient(string key);
        /// <summary>
        /// Get the client
        /// 获取客户端
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns></returns>
        private async Task<StreamPersistenceMemoryDatabaseClient> getClient(string key)
        {
            var client = default(StreamPersistenceMemoryDatabaseClient);
            var task = default(Task<StreamPersistenceMemoryDatabaseClient>);
            await clientLock.WaitAsync();
            try
            {
                if (clients.TryGetValue(key, out task)) return task.Result;
                clients.Add(key, Task.FromResult(client = await createClient(key)));
            }
            finally { clientLock.Release(); }
            return client;
        }
        /// <summary>
        /// Get the client based on the global key of the node
        /// 根据节点全局关键字获取客户端
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Log stream persistence in-memory database client
        /// 日志流持久化内存数据库客户端</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<StreamPersistenceMemoryDatabaseClient> GetClient(string key)
        {
            var client = default(Task<StreamPersistenceMemoryDatabaseClient>);
            return clients.TryGetValue(key, out client) ? client : getClient(key);
        }

        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public async Task<ResponseResult<bool>> RemoveNode(ClientNode node)
        {
            string key = node.Key;
            var client = default(Task<StreamPersistenceMemoryDatabaseClient>);
            if (clients.TryGetValue(key, out client)) return await client.Result.RemoveNode(node);
            return await (await getClient(key)).RemoveNode(node);
        }
        /// <summary>
        /// Get the client node. If the server does not exist, create the node
        /// 获取客户端节点，服务端不存在则创建节点
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
            var client = default(Task<StreamPersistenceMemoryDatabaseClient>);
            if (clients.TryGetValue(key, out client)) return await client.Result.GetOrCreateNode<T>(key, creator, isPersistenceCallbackExceptionRenewNode);
            return await (await getClient(key)).GetOrCreateNode<T>(key, creator, isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the client node. If the server does not exist, create the node
        /// 获取客户端节点，服务端不存在则创建节点
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
        public async Task<ResponseResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, ResponseParameterAwaiter<NodeIndex>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            var client = default(Task<StreamPersistenceMemoryDatabaseClient>);
            if (clients.TryGetValue(key, out client)) return await client.Result.GetOrCreateNode<T, PT>(key, parameter, creator, isPersistenceCallbackExceptionRenewNode);
            return await (await getClient(key)).GetOrCreateNode<T, PT>(key, parameter, creator, isPersistenceCallbackExceptionRenewNode);
        }

        /// <summary>
        /// Get the collection of all clients
        /// 获取所有客户端集合
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<StreamPersistenceMemoryDatabaseClient> getAllClients()
        {
            HashSet<HashObject<StreamPersistenceMemoryDatabaseClient>> clientHash = HashSetCreator.CreateHashObject<StreamPersistenceMemoryDatabaseClient>();
            foreach (Task<StreamPersistenceMemoryDatabaseClient> client in clients.Values)
            {
                if (clientHash.Add(client.Result)) yield return client.Result;
            }
        }
        /// <summary>
        /// All client fixes interface method errors and forcibly overwrites the original interface method calls. Except for the first parameter being the operation node object, the method definitions must be consistent
        /// 所有客户端 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="method">It must be a static method. The first parameter must be the interface type of the operation node, and the method number must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        public async Task<DistributedClientRepairNodeMethodState> RepairNodeMethod<T>(ClientNode<T> node, MethodInfo method)
            where T : class
        {
            Assembly assembly = method.DeclaringType.notNull().Assembly;
            byte[] rawAssembly = await AutoCSer.Common.ReadFileAllBytes(assembly.Location);
            RepairNodeMethodName nodeMethodName = new RepairNodeMethodName(method);
            foreach (StreamPersistenceMemoryDatabaseClient client in getAllClients())
            {
                CommandClientReturnValue<NodeIndex> index = await client.Client.StreamPersistenceMemoryDatabaseClient.GetNodeIndex(node.Key, ClientNodeCreator<T>.NodeInfo.notNull(), false);
                if (!index.IsSuccess) return new DistributedClientRepairNodeMethodState(index.ReturnType, client);
                CallStateEnum state = index.Value.GetState();
                if (state != CallStateEnum.Success) return new DistributedClientRepairNodeMethodState(state, client);
                CommandClientReturnValue<CallStateEnum> result = await client.Client.StreamPersistenceMemoryDatabaseClient.RepairNodeMethod(index.Value, rawAssembly, nodeMethodName);
                if (!result.IsSuccess) return new DistributedClientRepairNodeMethodState(result.ReturnType, client);
                if (result.Value != CallStateEnum.Success) return new DistributedClientRepairNodeMethodState(result.Value, client);
            }
            return new DistributedClientRepairNodeMethodState(CallStateEnum.Success, null);
        }
        /// <summary>
        /// All clients bind new methods to dynamically add interface functionality. The initial state of the new method number must be idle
        /// 所有客户端 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="method">It must be a static method. The first parameter must be the interface type of the operation node. The method number and other necessary configuration information must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        public async Task<DistributedClientRepairNodeMethodState> BindNodeMethod<T>(ClientNode<T> node, MethodInfo method)
            where T : class
        {
            Assembly assembly = method.DeclaringType.notNull().Assembly;
            byte[] rawAssembly = await AutoCSer.Common.ReadFileAllBytes(assembly.Location);
            RepairNodeMethodName nodeMethodName = new RepairNodeMethodName(method);
            foreach (StreamPersistenceMemoryDatabaseClient client in getAllClients())
            {
                CommandClientReturnValue<NodeIndex> index = await client.Client.StreamPersistenceMemoryDatabaseClient.GetNodeIndex(node.Key, ClientNodeCreator<T>.NodeInfo.notNull(), false);
                if (!index.IsSuccess) return new DistributedClientRepairNodeMethodState(index.ReturnType, client);
                CallStateEnum state = index.Value.GetState();
                if (state != CallStateEnum.Success) return new DistributedClientRepairNodeMethodState(state, client);
                CommandClientReturnValue<CallStateEnum> result = await client.Client.StreamPersistenceMemoryDatabaseClient.BindNodeMethod(index.Value, rawAssembly, nodeMethodName);
                if (!result.IsSuccess) return new DistributedClientRepairNodeMethodState(result.ReturnType, client);
                if (result.Value != CallStateEnum.Success) return new DistributedClientRepairNodeMethodState(result.Value, client);
            }
            return new DistributedClientRepairNodeMethodState(CallStateEnum.Success, null);
        }
    }
}
