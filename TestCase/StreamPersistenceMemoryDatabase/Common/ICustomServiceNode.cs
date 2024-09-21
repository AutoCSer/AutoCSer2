using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口）
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(CustomServiceNodeMethodEnum), IsAutoMethodIndex = false, IsClientCodeGeneratorOnlyDeclaringMethod = true, IsLocalClient = true)]
    public interface ICustomServiceNode : IServiceNode
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
        /// 创建字符串消息节点 IMessageNode{StringMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1);
        /// <summary>
        /// 创建服务端 JSON 字符串二进制序列化数据 / 客户端对象 消息节点 IMessageNode{ServerJsonBinaryMessage{TestClass}}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateServerJsonBinaryMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1);
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
        NodeIndex CreateServerJsonMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1);
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
        NodeIndex CreateServerBinaryMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1);
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
        NodeIndex CreateBinaryMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1);
        /// <summary>
        /// 创建数组节点 ArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length);
        /// <summary>
        /// 创建位图节点 BitmapNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity);
        /// <summary>
        /// 创建字典节点 DictionaryNode{string,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity = 0);
        /// <summary>
        /// 创建 256 基分片字典节点 FragmentDictionaryNode{string,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建 256 基分片哈希表节点 FragmentHashSetNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建哈希表节点 HashSetNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建数组节点 LeftArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity = 0);
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity = 0);
        /// <summary>
        /// 创建二叉搜索树字典节点 SearchTreeDictionaryNode{long,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建二叉搜索树集合节点 SearchTreeSetNode{long}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建排序字典节点 SortedDictionaryNode{long,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建排序列表节点 SortedListNode{long,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity = 0);
        /// <summary>
        /// 创建排序集合节点 SortedSetNode{long}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity = 0);

        #region 吞吐性能测试
        /// <summary>
        /// 创建吞吐性能测试字典节点（持久化模式） DictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreatePerformancePersistenceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建吞吐性能测试字典节点（非持久化纯内存模式） DictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [ServerMethod(IsPersistence = false)]
        NodeIndex CreatePerformanceDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建吞吐性能测试二叉搜索树字典节点（持久化模式） SearchTreeDictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreatePerformancePersistenceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建吞吐性能测试二叉搜索树字典节点（非持久化纯内存模式） SearchTreeDictionaryNode{int,int}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        [ServerMethod(IsPersistence = false)]
        NodeIndex CreatePerformanceSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
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
        NodeIndex CreatePerformancePersistenceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
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
        [ServerMethod(IsPersistence = false)]
        NodeIndex CreatePerformanceMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        #endregion
    }
}
