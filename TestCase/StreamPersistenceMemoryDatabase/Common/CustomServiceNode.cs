using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作自定义扩展（用于添加自定义节点创建接口）
    /// </summary>
    public class CustomServiceNode : ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// 服务基础操作自定义扩展
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public CustomServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建回调测试节点 ICallbackNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateCallbackNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ICallbackNode, int>(index, key, nodeInfo, () => new CallbackNode());
        }
        /// <summary>
        /// 创建字符串消息节点 IMessageNode{StringMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateStringMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<StringMessage>, StringMessage>(index, key, nodeInfo, () => MessageNode<StringMessage>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建服务端 JSON 字符串 / 客户端对象 消息节点 IMessageNode{ServerJsonBinaryMessage{TestClass}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateServerJsonBinaryMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<ServerJsonBinaryMessage<TestClass>>, ServerJsonBinaryMessage<TestClass>>(index, key, nodeInfo, () => MessageNode<ServerJsonBinaryMessage<TestClass>>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建服务端 JSON 字符串 / 客户端对象 消息节点 IMessageNode{ServerJsonMessage{TestClass}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateServerJsonMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<ServerJsonMessage<TestClass>>, ServerJsonMessage<TestClass>>(index, key, nodeInfo, () => MessageNode<ServerJsonMessage<TestClass>>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建服务端二进制序列化数据 / 客户端对象 消息节点 IMessageNode{ServerBinaryMessage{TestClass}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateServerBinaryMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<ServerBinaryMessage<TestClass>>, ServerBinaryMessage<TestClass>>(index, key, nodeInfo, () => MessageNode<ServerBinaryMessage<TestClass>>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建二进制序列化消息节点 IMessageNode{BinaryMessage{TestClass}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateBinaryMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<BinaryMessage<TestClass>>, BinaryMessage<TestClass>>(index, key, nodeInfo, () => MessageNode<BinaryMessage<TestClass>>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建分布式锁节点节点 DistributedLockNode{int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateDistributedLockNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IDistributedLockNode<int>, DistributedLockIdentity<int>>(index, key, nodeInfo, () => new DistributedLockNode<int>());
        }
        /// <summary>
        /// 创建数组节点 ArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length)
        {
            return createNode<IArrayNode<string>, ArrayNode<string>, KeyValue<int, string>>(index, key, nodeInfo, () => new ArrayNode<string>(length));
        }
        /// <summary>
        /// 创建位图节点 BitmapNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity)
        {
            return createNode<IBitmapNode, BitmapNode, byte[]>(index, key, nodeInfo, () => new BitmapNode(capacity));
        }
        /// <summary>
        /// 创建字典节点 DictionaryNode{string,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IDictionaryNode<string, string>, DictionaryNode<string, string>, KeyValue<string, string>>(index, key, nodeInfo, () => new DictionaryNode<string, string>(capacity));
        }
        /// <summary>
        /// 创建 256 基分片字典节点 FragmentDictionaryNode{string,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IFragmentDictionaryNode<string, string>, FragmentDictionaryNode<string, string>, KeyValue<string, string>>(index, key, nodeInfo, () => new FragmentDictionaryNode<string, string>());
        }
        /// <summary>
        /// 创建 256 基分片哈希表节点 FragmentHashSetNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IFragmentHashSetNode<string>, FragmentHashSetNode<string>, string>(index, key, nodeInfo, () => new FragmentHashSetNode<string>());
        }
        /// <summary>
        /// 创建哈希表节点 HashSetNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IHashSetNode<string>, HashSetNode<string>, string>(index, key, nodeInfo, () => new HashSetNode<string>());
        }
        /// <summary>
        /// 创建数组节点 LeftArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<ILeftArrayNode<string>, LeftArrayNode<string>, string>(index, key, nodeInfo, () => new LeftArrayNode<string>(capacity));
        }
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IQueueNode<string>, QueueNode<string>, string>(index, key, nodeInfo, () => new QueueNode<string>(capacity));
        }
        /// <summary>
        /// 创建二叉搜索树字典节点 SearchTreeDictionaryNode{long,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ISearchTreeDictionaryNode<long, string>, SearchTreeDictionaryNode<long, string>, KeyValue<long, string>>(index, key, nodeInfo, () => new SearchTreeDictionaryNode<long, string>());
        }
        /// <summary>
        /// 创建二叉搜索树集合节点 SearchTreeSetNode{long}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ISearchTreeSetNode<long>, SearchTreeSetNode<long>, long>(index, key, nodeInfo, () => new SearchTreeSetNode<long>());
        }
        /// <summary>
        /// 创建排序字典节点 SortedDictionaryNode{long,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ISortedDictionaryNode<long, string>, SortedDictionaryNode<long, string>, KeyValue<long, string>>(index, key, nodeInfo, () => new SortedDictionaryNode<long, string>());
        }
        /// <summary>
        /// 创建排序列表节点 SortedListNode{long,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<ISortedListNode<long, string>, SortedListNode<long, string>, KeyValue<long, string>>(index, key, nodeInfo, () => new SortedListNode<long, string>(capacity));
        }
        /// <summary>
        /// 创建排序集合节点 SortedSetNode{long}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ISortedSetNode<long>, SortedSetNode<long>, long>(index, key, nodeInfo, () => new SortedSetNode<long>());
        }
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IStackNode<string>, StackNode<string>, string>(index, key, nodeInfo, () => new StackNode<string>(capacity));
        }

        #region 吞吐性能测试
        /// <summary>
        /// 创建吞吐性能测试字典节点（持久化模式） DictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreatePerformancePersistenceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IDictionaryNode<int, int>, DictionaryNode<int, int>, KeyValue<int, int>>(index, key, nodeInfo, () => new DictionaryNode<int, int>(capacity));
        }
        /// <summary>
        /// 创建吞吐性能测试字典节点（非持久化纯内存模式） DictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreatePerformanceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IDictionaryNode<int, int>>(index, key, nodeInfo, () => new DictionaryNode<int, int>(capacity));
        }
        /// <summary>
        /// 创建吞吐性能测试二叉搜索树字典节点（持久化模式） SearchTreeDictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreatePerformancePersistenceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ISearchTreeDictionaryNode<int, int>, SearchTreeDictionaryNode<int, int>, KeyValue<int, int>>(index, key, nodeInfo, () => new SearchTreeDictionaryNode<int, int>());
        }
        /// <summary>
        /// 创建吞吐性能测试二叉搜索树字典节点（非持久化纯内存模式） SearchTreeDictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreatePerformanceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<ISearchTreeDictionaryNode<int, int>>(index, key, nodeInfo, () => new SearchTreeDictionaryNode<int, int>());
        }
        /// <summary>
        /// 创建吞吐性能测试消息节点（持久化模式） IMessageNode{PerformanceMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreatePerformancePersistenceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<PerformanceMessage>, PerformanceMessage>(index, key, nodeInfo, () => MessageNode<PerformanceMessage>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建吞吐性能测试消息节点（非持久化纯内存模式） IMessageNode{PerformanceMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreatePerformanceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<PerformanceMessage>>(index, key, nodeInfo, () => MessageNode<PerformanceMessage>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        #endregion

        /// <summary>
        /// 创建游戏测试节点 GameNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateGameNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createSnapshotCloneNode<IGameNode, GameNode, Monster>(index, key, nodeInfo, () => new GameNode());
        }
    }
}
