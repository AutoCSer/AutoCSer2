﻿using AutoCSer.Extensions;
using AutoCSer.Net.Packet;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence in-memory database service local client
    /// 日志流持久化内存数据库本地服务客户端
    /// </summary>
    public abstract class LocalClient
    {
        /// <summary>
        /// Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        public readonly LocalService Service;
        /// <summary>
        /// The written location of the persistent stream
        /// 持久化流已写入位置
        /// </summary>
        public long PersistencePosition { get { return Service.PersistencePosition; } }
        /// <summary>
        /// Rebuild the end position of the snapshot
        /// 重建快照结束位置
        /// </summary>
        public long RebuildSnapshotPosition { get { return Service.RebuildSnapshotPosition; } }
        /// <summary>
        /// Log stream persistence in-memory database service local client
        /// 日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <param name="service">Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务</param>
        protected LocalClient(LocalService service)
        {
            Service = service;
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public abstract LocalServiceQueueNode<LocalResult<bool>> RemoveNode(LocalClientNode node);
        /// <summary>
        /// Get the client node. If the server does not exist, create the node
        /// 获取客户端本地客户端节点，服务端不存在则创建节点
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
        public async Task<LocalResult<T>> GetOrCreateNode<T>(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            LocalResult<NodeIndex> nodeIndex = await GetOrCreateNodeIndex<T>(key, creator);
            if (nodeIndex.IsSuccess) return LocalClientNodeCreator<T>.Create(key, creator, this, nodeIndex.Value, isPersistenceCallbackExceptionRenewNode);
            return nodeIndex.Cast<T>();
        }
        /// <summary>
        /// Get the client node. If the server does not exist, create the node
        /// 获取客户端本地客户端节点，服务端不存在则创建节点
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
        public Task<LocalResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
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
        internal async Task<LocalResult<NodeIndex>> GetOrCreateNodeIndex<T>(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator) where T : class
        {
            var exception = default(Exception);
            var nodeInfo = LocalClientNodeCreator<T>.GetNodeInfo(out exception);
            if (exception == null)
            {
                NodeIndex index = await new LocalServiceGetNodeIndex(Service, key, nodeInfo.notNull()).AppendWrite();
                CallStateEnum state = index.GetState();
                if (state == CallStateEnum.Success)
                {
                    if (index.GetFree())
                    {
                        LocalResult<NodeIndex> nodeIndex = await creator(index, key, nodeInfo.notNull());
                        if (nodeIndex.IsSuccess)
                        {
                            state = nodeIndex.Value.GetState();
                            if (state == CallStateEnum.Success) return nodeIndex.Value;
                            return state;
                        }
                        return nodeIndex.Cast<NodeIndex>();
                    }
                    return index;
                }
                return state;
            }
            return new LocalResult<NodeIndex>(CallStateEnum.NotFoundClientNodeCreator, exception);
        }
        /// <summary>
        /// Rebuild the persistent file (clear invalid data), and note that nodes that do not support snapshots will be discarded
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<RebuildResult> Rebuild()
        {
            return new LocalServiceRebuild(Service).AppendWrite();
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
            return Service.AppendQueueNode(getResult);
        }
    }
    /// <summary>
    /// Log stream persistence in-memory database service local client
    /// 日志流持久化内存数据库本地服务客户端
    /// </summary>
    /// <typeparam name="CT">Service basic operation client interface type
    /// 服务基础操作客户端接口类型</typeparam>
    public class LocalClient<CT> : LocalClient
        where CT : class, IServiceNodeLocalClientNode
    {
        /// <summary>
        /// Service basic operation client
        /// 服务基础操作客户端
        /// </summary>
        public readonly CT ClientNode;
        /// <summary>
        /// Log stream persistence in-memory database service local client
        /// 日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <param name="service">Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务</param>
        internal LocalClient(LocalService service) : base(service)
        {
            ClientNode = LocalClientNodeCreator<CT>.Create(string.Empty, null, this, ServiceNode.ServiceNodeIndex, false);
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
        public LocalServiceQueueNode<LocalResult<bool>> RemoveNode(NodeIndex index)
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
        public override LocalServiceQueueNode<LocalResult<bool>> RemoveNode(LocalClientNode node)
        {
            return ClientNode.RemoveNode(node.Index);
        }
#if !AOT
        /// <summary>
        /// Get the local client node for message processing. If the server does not exist, create node MessageNode{BinaryMessage{T}}
        /// 获取消息处理本地客户端节点，服务端不存在则创建节点 MessageNode{BinaryMessage{T}}
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
        public Task<LocalResult<IMessageNodeLocalClientNode<BinaryMessage<T>>>> GetOrCreateBinaryMessageNode<T>(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IMessageNodeLocalClientNode<BinaryMessage<T>>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateMessageNode(index, nodeKey, nodeInfo, typeof(BinaryMessage<T>), arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node for message processing. If the server does not exist, create node MessageNode{T}
        /// 获取消息处理本地客户端节点，服务端不存在则创建节点 MessageNode{T}
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
        public Task<LocalResult<IMessageNodeLocalClientNode<T>>> GetOrCreateMessageNode<T>(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
            where T : Message<T>
        {
            return GetOrCreateNode<IMessageNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateMessageNode(index, nodeKey, nodeInfo, typeof(T), arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the distributed lock local client node. If the server does not exist, create node DistributedLockNode{KT}
        /// 获取分布式锁本地客户端节点，服务端不存在则创建节点 DistributedLockNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IDistributedLockNodeLocalClientNode<KT>>> GetOrCreateDistributedLockNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IDistributedLockNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateDistributedLockNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node of the dictionary. If the server does not exist, create node FragmentDictionaryNode{KT,VT}
        /// 获取字典本地客户端节点，服务端不存在则创建节点 FragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IFragmentDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateFragmentDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IFragmentDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateFragmentDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node of the dictionary. If the server does not exist, create node DictionaryNode{KT,VT}
        /// 获取字典本地客户端节点，服务端不存在则创建节点 DictionaryNode{KT,VT}
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
        public Task<LocalResult<IDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateDictionaryNode<KT, VT>(string key, int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT), capacity, groupType), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node of the binary search tree. If the server does not exist, create node SearchTreeDictionaryNode{KT,VT}
        /// 获取二叉搜索树本地客户端节点，服务端不存在则创建节点 SearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISearchTreeDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateSearchTreeDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISearchTreeDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSearchTreeDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node of the sorting dictionary. If the server does not exist, create node SortedDictionaryNode{KT,VT}
        /// 获取排序字典本地客户端节点，服务端不存在则创建节点 SortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISortedDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateSortedDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the sorting list of local client nodes. If the server does not exist, create node SortedListNode{KT,VT}
        /// 获取排序列表本地客户端节点，服务端不存在则创建节点 SortedListNode{KT,VT}
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
        public Task<LocalResult<ISortedListNodeLocalClientNode<KT, VT>>> GetOrCreateSortedListNode<KT, VT>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedListNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedListNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get 256 base fragment hash table local client nodes. If the server does not exist, create node FragmentHashSetNode{KT}
        /// 获取 256 基分片哈希表本地客户端节点，服务端不存在则创建节点 FragmentHashSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IFragmentHashSetNodeLocalClientNode<KT>>> GetOrCreateFragmentHashSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IFragmentHashSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateFragmentHashSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node of the hash table. If the server does not exist, create node HashSetNode{KT}
        /// 获取哈希表本地客户端节点，服务端不存在则创建节点 HashSetNode{KT}
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
        public Task<LocalResult<IHashSetNodeLocalClientNode<KT>>> GetOrCreateHashSetNode<KT>(string key, int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IHashSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateHashSetNode(index, nodeKey, nodeInfo, typeof(KT), capacity, groupType), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the binary search tree set of local client nodes. If the server does not exist, create node SearchTreeSetNode{KT}
        /// 获取二叉搜索树集合本地客户端节点，服务端不存在则创建节点 SearchTreeSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISearchTreeSetNodeLocalClientNode<KT>>> GetOrCreateSearchTreeSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISearchTreeSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSearchTreeSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client nodes of the sorted collection. If the server does not exist, create node SortedSetNode{KT}
        /// 获取排序集合本地客户端节点，服务端不存在则创建节点 SortedSetNode{KT}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISortedSetNodeLocalClientNode<KT>>> GetOrCreateSortedSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the queue node (first-in-first-out). If the server does not exist, create node QueueNode{T}
        /// 获取队列节点（先进先出），服务端不存在则创建节点 QueueNode{T}
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
        public Task<LocalResult<IQueueNodeLocalClientNode<T>>> GetOrCreateQueueNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IQueueNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateQueueNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the stack node (last in, first out). If the server does not exist, create node StackNode{T}
        /// 获取栈节点（后进先出），服务端不存在则创建节点 StackNode{T}
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
        public Task<LocalResult<IStackNodeLocalClientNode<T>>> GetOrCreateStackNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IStackNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateStackNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client nodes of the array. If the server does not exist, create node LeftArrayNode{T}
        /// 获取数组本地客户端节点，服务端不存在则创建节点 LeftArrayNode{T}
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
        public Task<LocalResult<ILeftArrayNodeLocalClientNode<T>>> GetOrCreateLeftArrayNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<ILeftArrayNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateLeftArrayNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client nodes of the array. If the server does not exist, create node ArrayNode{T}
        /// 获取数组本地客户端节点，服务端不存在则创建节点 ArrayNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="length">Array length</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IArrayNodeLocalClientNode<T>>> GetOrCreateArrayNode<T>(string key, int length, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IArrayNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateArrayNode(index, nodeKey, nodeInfo, typeof(T), length), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get archive-only data local client nodes. If the server does not exist, create node OnlyPersistenceNode{T}
        /// 获取仅存档本地客户端节点，服务端不存在则创建节点 OnlyPersistenceNode{T}
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Client node
        /// 客户端节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IOnlyPersistenceNodeLocalClientNode<T>>> GetOrCreateOnlyPersistenceNode<T>(string key)
        {
            return GetOrCreateNode<IOnlyPersistenceNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateOnlyPersistenceNode(index, nodeKey, nodeInfo, typeof(T)), false);
        }
#endif
        /// <summary>
        /// Get a 64-bit auto-increment identity local client node. If the server does not exist, create node IdentityGeneratorNode
        /// 获取 64 位自增ID 本地客户端节点，服务端不存在则创建节点 IdentityGeneratorNode
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
        public Task<LocalResult<IIdentityGeneratorNodeLocalClientNode>> GetOrCreateIdentityGeneratorNode(string key, long identity = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IIdentityGeneratorNodeLocalClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateIdentityGeneratorNode(index, nodeKey, nodeInfo, identity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// Get the local client node of the bitmap. If the server does not exist, create node BitmapNode
        /// 获取位图本地客户端节点，服务端不存在则创建节点 BitmapNode
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
        public Task<LocalResult<IBitmapNodeLocalClientNode>> GetOrCreateBitmapNode(string key, uint capacity, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IBitmapNodeLocalClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateBitmapNode(index, nodeKey, nodeInfo, capacity), isPersistenceCallbackExceptionRenewNode);
        }

        /// <summary>
        /// Get the log stream persistent in-memory database client node
        /// 获取日志流持久化内存数据库客户端节点
        /// </summary>
        /// <typeparam name="NT">Client node type
        /// 客户端节点类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseLocalClientNodeCache<NT> CreateNode<NT>(Func<LocalClient<CT>, Task<LocalResult<NT>>> getNodeTask)
            where NT : class
        {
            return new StreamPersistenceMemoryDatabaseLocalClientNodeCache<NT, CT>(this, getNodeTask);
        }
    }
}
