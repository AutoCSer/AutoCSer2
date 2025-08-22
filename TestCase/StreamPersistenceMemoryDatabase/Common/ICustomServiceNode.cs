using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Customize the basic service node interface
    /// 自定义基础服务节点接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode(IsLocalClient = true, IsReturnValueNode = false)]
    public partial interface ICustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode
    {
        /// <summary>
        /// 创建回调测试节点 ICallbackNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateCallbackNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建游戏测试节点 GameNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateGameNode(NodeIndex index, string key, NodeInfo nodeInfo);
#if AOT
        /// <summary>
        /// 创建测试数组节点 StringArrayNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="length">Array length</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length);
        /// <summary>
        /// Create dictionary generics to expand custom node IStringDictionaryNode
        /// 创建字典泛型展开自定义节点 IStringDictionaryNode
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
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateStringDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// 创建测试 256 基分片字典节点 StringFragmentDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试哈希表节点 StringHashSetNode
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
        NodeIndex CreateStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// 创建测试 256 基分片哈希表节点 StringFragmentHashSetNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试数组节点 StringLeftArrayNode
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
        NodeIndex CreateStringLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试队列节点接口 StringQueueNode
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
        NodeIndex CreateStringQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试栈节点 StringStackNode
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
        NodeIndex CreateStringStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试二叉搜索树集合节点 LongSearchTreeSetNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试排序集合节点 LongSortedSetNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试排序字典节点 LongStringSortedDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongStringSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// Create a sorting list node LongStringSortedListNode
        /// 创建排序列表节点 LongStringSortedListNode
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
        NodeIndex CreateLongStringSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试二叉搜索树字典节点 LongStringSearchTreeDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongStringSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试消息处理节点 TestClassMessageNode
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
        NodeIndex CreateTestClassMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// 创建性能测试字典节点 PerformanceDictionaryNode
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
        NodeIndex CreatePerformanceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// 创建性能测试二叉搜索树字典节点 PerformanceSearchTreeDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreatePerformanceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建性能测试消息处理节点 PerformanceMessageNode
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
        NodeIndex CreatePerformanceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// 创建测试仅存档节点 TestClassOnlyPersistenceNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateTestClassOnlyPersistenceNode(NodeIndex index, string key, NodeInfo nodeInfo);
#endif
    }
}
