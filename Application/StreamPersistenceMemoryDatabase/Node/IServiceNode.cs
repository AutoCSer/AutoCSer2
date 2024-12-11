using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作接口方法映射枚举
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(ServiceNodeMethodEnum), IsAutoMethodIndex = false, IsLocalClient = true)]
    public interface IServiceNode
    {
        /// <summary>
        /// 删除节点持久化参数检查
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> RemoveNodeBeforePersistence(NodeIndex index);
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        bool RemoveNode(NodeIndex index);
        /// <summary>
        /// 创建字典节点 FragmentHashStringDictionary256{HashString,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建字典节点 FragmentHashStringDictionary256{HashString,byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashStringByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建分布式锁节点节点 DistributedLockNode{HashString}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashStringDistributedLockNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建数组节点 ArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length);
        /// <summary>
        /// 创建数组节点 ArrayNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length);
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
        /// 创建字典节点 DictionaryNode{HashString,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建字典节点 DictionaryNode{HashString,byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashStringByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建 256 基分片哈希表节点 FragmentHashSetNode{HashString}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建哈希表节点 HashSetNode{HashString}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建数组节点 LeftArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建数组节点 LeftArrayNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStringStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
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
        NodeIndex CreateStringMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// 创建字符串消息节点 IMessageNode{ByteArrayMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
    }
}
