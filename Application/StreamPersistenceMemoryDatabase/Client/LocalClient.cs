using AutoCSer.Net.Packet;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库本地服务客户端
    /// </summary>
    public abstract class LocalClient
    {
        /// <summary>
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        public readonly LocalService Service;
        /// <summary>
        /// 持久化流已写入位置
        /// </summary>
        public long PersistencePosition { get { return Service.PersistencePosition; } }
        /// <summary>
        /// 重建快照结束位置
        /// </summary>
        public long RebuildSnapshotPosition { get { return Service.RebuildSnapshotPosition; } }
        /// <summary>
        /// 日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <param name="service">日志流持久化内存数据库本地服务</param>
        protected LocalClient(LocalService service)
        {
            Service = service;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public abstract LocalServiceQueueNode<LocalResult<bool>> RemoveNode(LocalClientNode node);
        /// <summary>
        /// 获取节点，不存在则创建节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建客户端节点委托</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点接口对象派生自 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}</returns>
        public async Task<LocalResult<T>> GetOrCreateNode<T>(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            LocalResult<NodeIndex> nodeIndex = await GetOrCreateNodeIndex<T>(key, creator);
            if (nodeIndex.IsSuccess) return LocalClientNodeCreator<T>.Create(key, creator, this, nodeIndex.Value, isPersistenceCallbackExceptionRenewNode);
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
        public Task<LocalResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
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
        internal async Task<LocalResult<NodeIndex>> GetOrCreateNodeIndex<T>(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator) where T : class
        {
            var exception = default(Exception);
            NodeInfo nodeInfo = LocalClientNodeCreator<T>.GetNodeInfo(out exception);
            if (exception == null)
            {
                NodeIndex index = await new LocalServiceGetNodeIndex(Service, key, nodeInfo).AppendQueue();
                CallStateEnum state = index.GetState();
                if (state == CallStateEnum.Success)
                {
                    if (index.GetFree())
                    {
                        LocalResult<NodeIndex> nodeIndex = await creator(index, key, nodeInfo);
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
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<RebuildResult> Rebuild()
        {
            return new LocalServiceRebuild(Service).AppendQueue();
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
            return Service.AppendQueueNode(getResult);
        }
    }
    /// <summary>
    /// 日志流持久化内存数据库本地服务客户端
    /// </summary>
    /// <typeparam name="CT">服务基础操作客户端接口类型</typeparam>
    public class LocalClient<CT> : LocalClient
        where CT : class, IServiceNodeLocalClientNode
    {
        /// <summary>
        /// 服务基础操作客户端
        /// </summary>
        public readonly CT ClientNode;
        /// <summary>
        /// 日志流持久化内存数据库本地服务客户端
        /// </summary>
        /// <param name="service">日志流持久化内存数据库本地服务</param>
        internal LocalClient(LocalService service) : base(service)
        {
            ClientNode = LocalClientNodeCreator<CT>.Create(string.Empty, null, this, ServiceNode.ServiceNodeIndex, false);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<LocalResult<bool>> RemoveNode(NodeIndex index)
        {
            return ClientNode.RemoveNode(index);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override LocalServiceQueueNode<LocalResult<bool>> RemoveNode(LocalClientNode node)
        {
            return ClientNode.RemoveNode(node.Index);
        }
        /// <summary>
        /// 获取消息处理节点，不存在则创建节点 MessageNode{BinaryMessage{T}}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IMessageNodeLocalClientNode<BinaryMessage<T>>>> GetOrCreateBinaryMessageNode<T>(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IMessageNodeLocalClientNode<BinaryMessage<T>>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateMessageNode(index, nodeKey, nodeInfo, typeof(BinaryMessage<T>), arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取消息处理节点，不存在则创建节点 MessageNode{T}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IMessageNodeLocalClientNode<T>>> GetOrCreateMessageNode<T>(string key, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1, bool isPersistenceCallbackExceptionRenewNode = false)
            where T : Message<T>
        {
            return GetOrCreateNode<IMessageNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateMessageNode(index, nodeKey, nodeInfo, typeof(T), arraySize, timeoutSeconds, checkTimeoutSeconds), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取分布式锁节点，不存在则创建节点 DistributedLockNodeKT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IDistributedLockNodeLocalClientNode<KT>>> GetOrCreateDistributedLockNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IDistributedLockNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateDistributedLockNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取字典节点，不存在则创建节点 FragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IFragmentDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateFragmentDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IFragmentDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateFragmentDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取字典节点，不存在则创建节点 DictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateDictionaryNode<KT, VT>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取二叉搜索树节点，不存在则创建节点 SearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISearchTreeDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateSearchTreeDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISearchTreeDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSearchTreeDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取排序字典节点，不存在则创建节点 SortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISortedDictionaryNodeLocalClientNode<KT, VT>>> GetOrCreateSortedDictionaryNode<KT, VT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedDictionaryNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedDictionaryNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取排序列表节点，不存在则创建节点 SortedListNode{KT,VT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISortedListNodeLocalClientNode<KT, VT>>> GetOrCreateSortedListNode<KT, VT>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedListNodeLocalClientNode<KT, VT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedListNode(index, nodeKey, nodeInfo, typeof(KT), typeof(VT), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取 256 基分片哈希表节点，不存在则创建节点 FragmentHashSetNode{KT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IFragmentHashSetNodeLocalClientNode<KT>>> GetOrCreateFragmentHashSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IFragmentHashSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateFragmentHashSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取哈希表节点，不存在则创建节点 HashSetNode{KT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IHashSetNodeLocalClientNode<KT>>> GetOrCreateHashSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IEquatable<KT>
        {
            return GetOrCreateNode<IHashSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateHashSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取二叉搜索树集合节点，不存在则创建节点 SearchTreeSetNode{KT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISearchTreeSetNodeLocalClientNode<KT>>> GetOrCreateSearchTreeSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISearchTreeSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSearchTreeSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取排序集合节点，不存在则创建节点 SortedSetNode{KT}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ISortedSetNodeLocalClientNode<KT>>> GetOrCreateSortedSetNode<KT>(string key, bool isPersistenceCallbackExceptionRenewNode = false)
            where KT : IComparable<KT>
        {
            return GetOrCreateNode<ISortedSetNodeLocalClientNode<KT>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateSortedSetNode(index, nodeKey, nodeInfo, typeof(KT)), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取队列节点（先进先出），不存在则创建节点 QueueNode{T}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IQueueNodeLocalClientNode<T>>> GetOrCreateQueueNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IQueueNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateQueueNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取栈节点（后进先出），不存在则创建节点 StackNode{T}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IStackNodeLocalClientNode<T>>> GetOrCreateStackNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IStackNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateStackNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取数组节点，不存在则创建节点 LeftArrayNode{T}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<ILeftArrayNodeLocalClientNode<T>>> GetOrCreateLeftArrayNode<T>(string key, int capacity = 0, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<ILeftArrayNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateLeftArrayNode(index, nodeKey, nodeInfo, typeof(T), capacity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取数组节点，不存在则创建节点 ArrayNode{T}
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="length">数组长度</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IArrayNodeLocalClientNode<T>>> GetOrCreateArrayNode<T>(string key, int length, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IArrayNodeLocalClientNode<T>>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateArrayNode(index, nodeKey, nodeInfo, typeof(T), length), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取 64 位自增ID 节点，不存在则创建节点 IdentityGeneratorNode
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="identity">起始分配 ID</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IIdentityGeneratorNodeLocalClientNode>> GetOrCreateIdentityGeneratorNode(string key, long identity = 1, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IIdentityGeneratorNodeLocalClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateIdentityGeneratorNode(index, nodeKey, nodeInfo, identity), isPersistenceCallbackExceptionRenewNode);
        }
        /// <summary>
        /// 获取位图节点，不存在则创建节点 BitmapNode
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<LocalResult<IBitmapNodeLocalClientNode>> GetOrCreateBitmapNode(string key, uint capacity, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IBitmapNodeLocalClientNode>(key, (index, nodeKey, nodeInfo) => ClientNode.CreateBitmapNode(index, nodeKey, nodeInfo, capacity), isPersistenceCallbackExceptionRenewNode);
        }

        /// <summary>
        /// 获取日志流持久化内存数据库客户端节点
        /// </summary>
        /// <typeparam name="NT">客户端节点类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseLocalClientNodeCache<NT> CreateNode<NT>(Func<LocalClient<CT>, Task<LocalResult<NT>>> getNodeTask)
            where NT : class
        {
            return new StreamPersistenceMemoryDatabaseLocalClientNodeCache<NT, CT>(this, getNodeTask);
        }
    }
}
