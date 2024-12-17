using AutoCSer.Extensions;
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
        /// 创建消息处理节点 MessageNode{ServerByteArrayMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateServerByteArrayMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// 创建消息处理节点 MessageNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="messageType">消息数据类型</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
        /// <summary>
        /// 创建分布式锁节点 DistributedLockNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateDistributedLockNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// 创建字典节点 HashBytesFragmentDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashBytesFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// 创建字典节点 FragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
        /// <summary>
        /// 创建字典节点 HashBytesDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashBytesDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建字典节点 ByteArrayDictionaryNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// 创建字典节点 DictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
        /// <summary>
        /// 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
        /// <summary>
        /// 创建排序字典节点 SortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
        /// <summary>
        /// 创建排序列表节点 SortedListNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
        /// <summary>
        /// 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// 创建哈希表节点 HashSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// 创建排序集合节点 SortedSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        /// <summary>
        /// 创建队列节点（先进先出） ByteArrayQueueNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// 创建栈节点（后进先出） ByteArrayStackNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateByteArrayStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateStackNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// 创建数组节点 LeftArrayNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// 创建数组节点 ArrayNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length);
        /// <summary>
        /// 创建 64 位自增ID 节点 IdentityGeneratorNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="identity">起始分配 ID</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateIdentityGeneratorNode(NodeIndex index, string key, NodeInfo nodeInfo, long identity);
        /// <summary>
        /// 创建位图节点 BitmapNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity);
    }
}
