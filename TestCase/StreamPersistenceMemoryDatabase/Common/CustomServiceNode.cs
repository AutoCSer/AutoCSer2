﻿using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Customize the basic service node for adding custom nodes to create API methods
    /// 自定义基础服务节点，用于添加自定义节点创建 API 方法
    /// </summary>
    public class CustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// Customize the basic service node
        /// 自定义基础服务节点
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        public CustomServiceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service) : base(service) { }
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
        public NodeIndex CreateCallbackNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ICallbackNode>(index, key, nodeInfo, () => new CallbackNode());
        }
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
        public NodeIndex CreateGameNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IGameNode>(index, key, nodeInfo, () => new GameNode());
        }
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
        public NodeIndex CreateStringArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length)
        {
            return CreateSnapshotNode<IStringArrayNode>(index, key, nodeInfo, () => new StringArrayNode(length));
        }
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
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateStringDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType)
        {
            return CreateSnapshotNode<IStringDictionaryNode>(index, key, nodeInfo, () => new StringDictionaryNode(capacity, groupType));
        }
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
        public NodeIndex CreateStringFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IStringFragmentDictionaryNode>(index, key, nodeInfo, () => new StringFragmentDictionaryNode());
        }
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
        public NodeIndex CreateStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            return CreateSnapshotNode<IStringHashSetNode>(index, key, nodeInfo, () => new StringHashSetNode(capacity, groupType));
        }
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
        public NodeIndex CreateStringFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IStringFragmentHashSetNode>(index, key, nodeInfo, () => new StringFragmentHashSetNode());
        }
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
        public NodeIndex CreateStringLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IStringLeftArrayNode>(index, key, nodeInfo, () => new StringLeftArrayNode(capacity));
        }
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
        public NodeIndex CreateStringQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IStringQueueNode>(index, key, nodeInfo, () => new StringQueueNode(capacity));
        }
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
        public NodeIndex CreateStringStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IStringStackNode>(index, key, nodeInfo, () => new StringStackNode(capacity));
        }
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
        public NodeIndex CreateLongSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ILongSearchTreeSetNode>(index, key, nodeInfo, () => new LongSearchTreeSetNode());
        }
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
        public NodeIndex CreateLongSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ILongSortedSetNode>(index, key, nodeInfo, () => new LongSortedSetNode());
        }
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
        public NodeIndex CreateLongStringSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ILongStringSortedDictionaryNode>(index, key, nodeInfo, () => new LongStringSortedDictionaryNode());
        }
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
        public NodeIndex CreateLongStringSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<ILongStringSortedListNode>(index, key, nodeInfo, () => new LongStringSortedListNode(capacity));
        }
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
        public NodeIndex CreateLongStringSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ILongStringSearchTreeDictionaryNode>(index, key, nodeInfo, () => new LongStringSearchTreeDictionaryNode());
        }
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
        public NodeIndex CreateTestClassMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return CreateSnapshotNode<ITestClassMessageNode>(index, key, nodeInfo, () => new TestClassMessageNode(arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
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
        public NodeIndex CreatePerformanceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            return CreateSnapshotNode<IPerformanceDictionaryNode>(index, key, nodeInfo, () => new PerformanceDictionaryNode(capacity, groupType));
        }
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
        public NodeIndex CreatePerformanceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IPerformanceSearchTreeDictionaryNode>(index, key, nodeInfo, () => new PerformanceSearchTreeDictionaryNode());
        }
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
        public NodeIndex CreatePerformanceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return CreateSnapshotNode<IPerformanceMessageNode>(index, key, nodeInfo, () => new PerformanceMessageNode(arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
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
        public NodeIndex CreateTestClassOnlyPersistenceNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateNode<ITestClassOnlyPersistenceNode>(index, key, nodeInfo, () => new TestClassOnlyPersistenceNode(), true);
        }
#endif
    }
}
