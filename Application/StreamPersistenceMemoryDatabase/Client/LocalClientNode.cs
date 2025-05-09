using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务客户端节点
    /// </summary>
    public abstract class LocalClientNode
    {
        /// <summary>
        /// 节点全局关键字
        /// </summary>
        public readonly string Key;
        /// <summary>
        /// 创建节点操作对象委托
        /// </summary>
        internal readonly Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> Creator;
        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
        internal readonly LocalClient Client;
        /// <summary>
        /// 节点索引信息
        /// </summary>
        internal NodeIndex Index;
        /// <summary>
        /// 是否重建中
        /// </summary>
        private int isRenewing;
        /// <summary>
        /// 是否正在重新获取索引
        /// </summary>
        private int isReindex;
        /// <summary>
        /// 是否 IO 线程同步回调
        /// </summary>
        internal bool IsSynchronousCallback;
        /// <summary>
        /// 服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失
        /// </summary>
        private bool isPersistenceCallbackExceptionRenewNode;
        /// <summary>
        /// 客户端节点
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建节点操作对象委托</param>
        /// <param name="client">日志流持久化内存数据库客户端</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        internal LocalClientNode(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, LocalClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
        {
            this.Creator = creator;
            this.Key = key;
            this.Client = client;
            this.Index = index;
            this.isPersistenceCallbackExceptionRenewNode = isPersistenceCallbackExceptionRenewNode;
        }
        /// <summary>
        /// 检查返回错误状态
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal void CheckState(NodeIndex nodeIndex, CallStateEnum state)
        {
            switch (state)
            {
                case CallStateEnum.PersistenceCallbackException: Renew(nodeIndex).NotWait(); break;
                case CallStateEnum.NodeIndexOutOfRange:
                case CallStateEnum.NodeIdentityNotMatch:
                    Reindex(nodeIndex).NotWait();
                    break;
            }
        }
        ///// <summary>
        ///// 检查返回错误状态
        ///// </summary>
        ///// <param name="nodeIndex"></param>
        ///// <param name="state"></param>
        ///// <returns></returns>
        //internal Task CheckStateAsync(NodeIndex nodeIndex, CallStateEnum state)
        //{
        //    switch (state)
        //    {
        //        case CallStateEnum.PersistenceCallbackException: await Renew(nodeIndex); break;
        //        case CallStateEnum.NodeIndexOutOfRange:
        //        case CallStateEnum.NodeIdentityNotMatch:
        //            await Reindex(nodeIndex);
        //            break;
        //    }
        //}
        /// <summary>
        /// 节点重建
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal async Task Renew(NodeIndex index)
        {
            if (isPersistenceCallbackExceptionRenewNode && Index.Equals(index) && Interlocked.CompareExchange(ref isRenewing, 1, 0) == 0)
            {
                try
                {
                    if (Index.Equals(index)) await renew();
                }
                finally { Interlocked.Exchange(ref isRenewing, 0); }
            }
        }
        /// <summary>
        /// 节点重建
        /// </summary>
        /// <returns></returns>
        protected abstract Task renew();
        /// <summary>
        /// 索引失效重新获取
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal async Task Reindex(NodeIndex index)
        {
            if (Index.Equals(index) && Interlocked.CompareExchange(ref isReindex, 1, 0) == 0)
            {
                try
                {
                    if (Index.Equals(index)) await reindex();
                }
                finally { Interlocked.Exchange(ref isReindex, 0); }
            }
        }
        /// <summary>
        /// 索引失效重新获取
        /// </summary>
        /// <returns></returns>
        protected abstract Task reindex();
    }
    /// <summary>
    /// 客户端节点
    /// </summary>
    /// <typeparam name="T">客户端节点接口类型</typeparam>
    public abstract class LocalClientNode<T> : LocalClientNode
         where T : class
    {
        /// <summary>
        /// IO 线程同步客户端节点
        /// </summary>
#if NetStandard21
        private LocalClientNode<T>? synchronousNode;
#else
        private LocalClientNode<T> synchronousNode;
#endif
        /// <summary>
        /// 客户端节点
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建节点操作对象委托</param>
        /// <param name="client">日志流持久化内存数据库客户端</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        public LocalClientNode(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, LocalClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
        /// <summary>
        /// 节点重建
        /// </summary>
        /// <returns></returns>
        protected override async Task renew()
        {
            LocalResult<bool> isRemove = await Client.RemoveNode(this);
            if (isRemove.IsSuccess) await reindex();
        }
        /// <summary>
        /// 索引失效重新获取
        /// </summary>
        /// <returns></returns>
        protected override async Task reindex()
        {
            LocalResult<NodeIndex> nodeIndex = await Client.GetOrCreateNodeIndex<T>(Key, Creator);
            if (nodeIndex.IsSuccess) Index = nodeIndex.Value;
        }
        /// <summary>
        /// 创建 IO 线程同步回调节点
        /// </summary>
        /// <returns></returns>
        private LocalClientNode<T> createSynchronousCallback()
        {
            LocalClientNode<T> node = (LocalClientNode<T>)this.MemberwiseClone();
            node.IsSynchronousCallback = true;
            synchronousNode = node;
            return node;
        }
        /// <summary>
        /// 获取 IO 线程同步回调节点，节点调用 await 后续操作不允许存在同步阻塞逻辑或者长时间占用 CPU 运算
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T GetSynchronousCallback(T node)
        {
            LocalClientNode<T> clientNode = node.notNullCastType<LocalClientNode<T>>();
            if (!clientNode.IsSynchronousCallback) return (T)(object)clientNode.createSynchronousCallback();
            return node;
        }
    }
}
