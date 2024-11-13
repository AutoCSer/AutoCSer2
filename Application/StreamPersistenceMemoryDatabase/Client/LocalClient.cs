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
        public abstract LocalServiceQueueNode<ResponseResult<bool>> RemoveNode(LocalClientNode node);
        /// <summary>
        /// 获取节点，不存在则创建节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建客户端节点委托</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns>节点接口对象派生自 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode{T}</returns>
        public async Task<ResponseResult<T>> GetOrCreateNode<T>(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<ResponseResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
        {
            ResponseResult<NodeIndex> nodeIndex = await GetOrCreateNodeIndex<T>(key, creator);
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
        public Task<ResponseResult<T>> GetOrCreateNode<T, PT>(string key, PT parameter, Func<NodeIndex, string, NodeInfo, PT, LocalServiceQueueNode<ResponseResult<NodeIndex>>> creator, bool isPersistenceCallbackExceptionRenewNode = false) where T : class
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
        internal async Task<ResponseResult<NodeIndex>> GetOrCreateNodeIndex<T>(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<ResponseResult<NodeIndex>>> creator) where T : class
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
                        ResponseResult<NodeIndex> nodeIndex = await creator(index, key, nodeInfo);
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
            return CallStateEnum.NotFoundClientNodeCreator;
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
        public LocalServiceQueueNode<ResponseResult<bool>> RemoveNode(NodeIndex index)
        {
            return ClientNode.RemoveNode(index);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override LocalServiceQueueNode<ResponseResult<bool>> RemoveNode(LocalClientNode node)
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
        public Task<ResponseResult<IHashStringFragmentDictionaryNodeLocalClientNode<string>>> GetOrCreateFragmentHashStringDictionaryNode(string key, bool isPersistenceCallbackExceptionRenewNode = false)
        {
            return GetOrCreateNode<IHashStringFragmentDictionaryNodeLocalClientNode<string>>(key, ClientNode.CreateFragmentHashStringDictionaryNode, isPersistenceCallbackExceptionRenewNode);
        }
    }
}
