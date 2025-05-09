using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口）
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface ICustomServiceNode : IServiceNode
    {
        /// <summary>
        /// 创建回调测试节点 ICallbackNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateCallbackNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建游戏测试节点 GameNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateGameNode(NodeIndex index, string key, NodeInfo nodeInfo);
#if AOT
        /// <summary>
        /// 创建测试数组节点 StringArrayNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length);
        /// <summary>
        /// 创建测试字典节点 StringDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// 创建测试 256 基分片字典节点 StringFragmentDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试哈希表节点 StringHashSetNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// 创建测试 256 基分片哈希表节点 StringFragmentHashSetNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试数组节点 StringLeftArrayNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试队列节点接口 StringQueueNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试栈节点 StringStackNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试二叉搜索树集合节点 LongSearchTreeSetNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试排序集合节点 LongSortedSetNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试排序字典节点 LongStringSortedDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongStringSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建排序列表节点 LongStringSortedListNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongStringSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建测试二叉搜索树字典节点 LongStringSearchTreeDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLongStringSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建测试消息处理节点 TestClassMessageNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateTestClassMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// 创建性能测试字典节点 PerformanceDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreatePerformanceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// 创建性能测试二叉搜索树字典节点 PerformanceSearchTreeDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreatePerformanceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建性能测试消息处理节点 PerformanceMessageNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreatePerformanceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
#endif
    }
}
