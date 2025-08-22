using AutoCSer.Extensions;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Service basic operation interface
    /// 服务基础操作接口
    /// </summary>
    [ServerNode(IsLocalClient = true, IsReturnValueNode = false)]
    public partial interface IServiceNode
    {
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        bool RemoveNode(NodeIndex index);
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        bool RemoveNodeByKey(string key);
#if !AOT
        /// <summary>
        /// Create a server registration node IServerRegistryNode
        /// 创建服务注册节点 IServerRegistryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="loadTimeoutSeconds">Cold start session timeout seconds
        /// 冷启动会话超时秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateServerRegistryNode(NodeIndex index, string key, NodeInfo nodeInfo, int loadTimeoutSeconds);
        /// <summary>
        /// Create a service process daemon node IProcessGuardNode
        /// 创建服务进程守护节点 IProcessGuardNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateProcessGuardNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// Create a message processing node IMessageNode{ServerByteArrayMessage}
        /// 创建消息处理节点 IMessageNode{ServerByteArrayMessage}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateServerByteArrayMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// Create a message processing node IMessageNode{T}
        /// 创建消息处理节点 IMessageNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="messageType">Message data type
        /// 消息数据类型</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// Create distributed lock nodes IDistributedLockNode{KT}
        /// 创建分布式锁节点 IDistributedLockNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateDistributedLockNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// Create a dictionary node IByteArrayFragmentDictionaryNode{KT}
        /// 创建字典节点 IByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// Create a dictionary node IFragmentDictionaryNode{KT,VT}
        /// 创建字典节点 IFragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
        /// <summary>
        /// Create a dictionary node IByteArrayDictionaryNode{KT}
        /// 创建字典节点 IByteArrayDictionaryNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// Create a dictionary node IDictionaryNode{KT,VT}
        /// 创建字典节点 IDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// Create a binary search tree node ISearchTreeDictionaryNode{KT,VT}
        /// 创建二叉搜索树节点 ISearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
        /// <summary>
        /// Create a sorting dictionary node ISortedDictionaryNode{KT,VT}
        /// 创建排序字典节点 ISortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
        /// <summary>
        /// Create a sorting list node ISortedListNode{KT,VT}
        /// 创建排序列表节点 ISortedListNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
        /// <summary>
        /// Create a 256 base fragment hash table node IFragmentHashSetNode{KT}
        /// 创建 256 基分片哈希表节点 IFragmentHashSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// Create a hash table node IHashSetNode{KT}
        /// 创建哈希表节点 IHashSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// Create a binary search tree collection node ISearchTreeSetNode{KT}
        /// 创建二叉搜索树集合节点 ISearchTreeSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// Create sorted collection node ISortedSetNode{KT}
        /// 创建排序集合节点 ISortedSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// Create a queue node IQueueNode{T} (First in, first Out)
        /// 创建队列节点（先进先出） IQueueNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// Create a stack node IStackNode{T} (Last in, first out)
        /// 创建栈节点（后进先出） IStackNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStackNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// Create a array node ILeftArrayNode{T}
        /// 创建数组节点 ILeftArrayNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// Create a array node IArrayNode{T}
        /// 创建数组节点 IArrayNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="length">Array length</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length);
        /// <summary>
        /// Create a dictionary node IHashBytesFragmentDictionaryNode
        /// 创建字典节点 IHashBytesFragmentDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashBytesFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// Create a dictionary node IHashBytesDictionaryNode
        /// 创建字典节点 IHashBytesDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashBytesDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// Create a queue node IByteArrayQueueNode (First in, first Out)
        /// 创建队列节点（先进先出） IByteArrayQueueNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// Create a stack node IByteArrayStackNode (Last in, first out)
        /// 创建栈节点（后进先出） IByteArrayStackNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// Create an archive node only IOnlyPersistenceNode{T}
        /// 创建仅存档节点 IOnlyPersistenceNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="valueType">Archive data type
        /// 存档数据类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateOnlyPersistenceNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType valueType);
#endif
        /// <summary>
        /// Creat a multi-hash bitmap client synchronization filter node IManyHashBitMapClientFilterNode
        /// 创建多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateManyHashBitMapClientFilterNode(NodeIndex index, string key, NodeInfo nodeInfo, int size);
        /// <summary>
        /// Creat a multi-hash bitmap filter node IManyHashBitMapFilterNode
        /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateManyHashBitMapFilterNode(NodeIndex index, string key, NodeInfo nodeInfo, int size);
        /// <summary>
        /// Create a client synchronization total statistics node based on uniform probability IUniformProbabilityClientStatisticsNode
        /// 创建基于均匀概率的客户端同步总量统计节点 IUniformProbabilityClientStatisticsNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="indexBits">The number of binary bits in the index must be even, with a minimum of 8 and a maximum of 20
        /// 索引二进制位数量，必须为偶数，最小值为 8，最大值为 20</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateUniformProbabilityClientStatisticsNode(NodeIndex index, string key, NodeInfo nodeInfo, byte indexBits);
        /// <summary>
        /// Create a total statistics node based on uniform probability IUniformProbabilityTotalStatisticsNode
        /// 创建基于均匀概率的总量统计节点 IUniformProbabilityTotalStatisticsNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="indexBits">The number of binary bits in the index must be even, with a minimum of 8 and a maximum of 20
        /// 索引二进制位数量，必须为偶数，最小值为 8，最大值为 20</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateUniformProbabilityTotalStatisticsNode(NodeIndex index, string key, NodeInfo nodeInfo, byte indexBits);
        /// <summary>
        /// Create a 64-bit auto-increment identity node IIdentityGeneratorNode
        /// 创建 64 位自增ID 节点 IIdentityGeneratorNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="identity">Initial Allocation identity
        /// 起始分配 ID</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateIdentityGeneratorNode(NodeIndex index, string key, NodeInfo nodeInfo, long identity);
        /// <summary>
        /// Create a bitmap node IBitmapNode
        /// 创建位图节点 IBitmapNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">The number of binary bits
        /// 二进制位数量</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity);
    }
}
