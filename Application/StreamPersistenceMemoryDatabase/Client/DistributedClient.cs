﻿using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式客户端
    /// </summary>
    public abstract class DistributedClient
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        protected readonly Dictionary<HashString, StreamPersistenceMemoryDatabaseClient> clients;
        /// <summary>
        /// 分布式客户端
        /// </summary>
        private DistributedClient()
        {
            clients = DictionaryCreator.CreateHashString<StreamPersistenceMemoryDatabaseClient>();
        }
        /// <summary>
        /// 根据节点全局关键字创建客户端
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <returns>日志流持久化内存数据库客户端</returns>
        protected abstract Task<StreamPersistenceMemoryDatabaseClient> createClient(string key);
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <returns></returns>
        private async Task<StreamPersistenceMemoryDatabaseClient> getClient(HashString key)
        {
            StreamPersistenceMemoryDatabaseClient client = await createClient(key.String);
            clients.Add(key, client);
            return client;
        }
        /// <summary>
        /// 根据节点全局关键字获取客户端
        /// </summary>
        /// <param name="key">日志流持久化内存数据库客户端</param>
        /// <returns>磁盘块客户端</returns>
        public async Task<StreamPersistenceMemoryDatabaseClient> GetClient(string key)
        {
            StreamPersistenceMemoryDatabaseClient client;
            HashString hashKey = key;
            return clients.TryGetValue(hashKey, out client) ? client : await getClient(hashKey);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public async Task<ResponseResult<bool>> RemoveNode(ClientNode node)
        {
            HashString key = node.Key;
            StreamPersistenceMemoryDatabaseClient client;
            if (!clients.TryGetValue(key, out client)) client = await getClient(key);
            return await client.RemoveNode(node);
        }
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
            HashString hashKey = key;
            StreamPersistenceMemoryDatabaseClient client;
            if (!clients.TryGetValue(hashKey, out client)) client = await getClient(hashKey);
            return await client.GetOrCreateNode<T>(key, creator, isPersistenceCallbackExceptionRenewNode);
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
        public async Task<ResponseResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, Task<ResponseResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            HashString hashKey = key;
            StreamPersistenceMemoryDatabaseClient client;
            if (!clients.TryGetValue(hashKey, out client)) client = await getClient(hashKey);
            return await client.GetOrCreateNode<T, PT>(key, parameter, creator, isPersistenceCallbackExceptionRenewNode);
        }

        /// <summary>
        /// 获取所有客户端集合
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<StreamPersistenceMemoryDatabaseClient> getAllClients()
        {
            HashSet<HashObject<StreamPersistenceMemoryDatabaseClient>> clientHash = HashSetCreator.CreateHashObject<StreamPersistenceMemoryDatabaseClient>();
            foreach (StreamPersistenceMemoryDatabaseClient client in clients.Values)
            {
                if (clientHash.Add(client)) yield return client;
            }
        }
        /// <summary>
        /// 所有客户端 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">客户端节点</param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <returns></returns>
        public async Task<DistributedClientRepairNodeMethodState> RepairNodeMethod<T>(ClientNode<T> node, MethodInfo method)
            where T : class
        {
            Assembly assembly = method.DeclaringType.Assembly;
            byte[] rawAssembly = await AutoCSer.Common.Config.ReadFileAllBytes(assembly.Location);
            RepairNodeMethodName nodeMethodName = new RepairNodeMethodName(method);
            foreach (StreamPersistenceMemoryDatabaseClient client in getAllClients())
            {
                CommandClientReturnValue<NodeIndex> index = await client.Client.StreamPersistenceMemoryDatabaseClient.GetNodeIndex(node.Key, ClientNodeCreator<T>.NodeInfo, false);
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
        /// 所有客户端 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <returns></returns>
        public async Task<DistributedClientRepairNodeMethodState> BindNodeMethod<T>(ClientNode<T> node, MethodInfo method)
            where T : class
        {
            Assembly assembly = method.DeclaringType.Assembly;
            byte[] rawAssembly = await AutoCSer.Common.Config.ReadFileAllBytes(assembly.Location);
            RepairNodeMethodName nodeMethodName = new RepairNodeMethodName(method);
            foreach (StreamPersistenceMemoryDatabaseClient client in getAllClients())
            {
                CommandClientReturnValue<NodeIndex> index = await client.Client.StreamPersistenceMemoryDatabaseClient.GetNodeIndex(node.Key, ClientNodeCreator<T>.NodeInfo, false);
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