//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
        /// <summary>
        /// 计数器节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.Document.MemoryDatabaseLocalService.CustomNode.ICounterNode))]
        public partial interface ICounterNodeLocalClientNode
        {
            /// <summary>
            /// 获取当前计数
            /// </summary>
            /// <returns>当前计数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> GetCount();
            /// <summary>
            /// 计数 +1
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Increment();
        }
}namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
        /// <summary>
        /// 自定义基础服务接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.Document.MemoryDatabaseLocalService.CustomNode.ICustomServiceNode))]
        public partial interface ICustomServiceNodeLocalClientNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 创建计数器节点 ICounterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
        /// <summary>
        /// 计数器节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ICounterNodeMethodEnum))]
        public partial interface ICounterNode { }
        /// <summary>
        /// 计数器节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ICounterNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取当前计数
            /// 返回值 long 当前计数
            /// </summary>
            GetCount = 0,
            /// <summary>
            /// [1] 计数 +1
            /// </summary>
            Increment = 1,
            /// <summary>
            /// [2] 快照设置数据，从快照数据恢复内存数据
            /// long value 数据
            /// </summary>
            SnapshotSet = 2,
        }
}namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
        /// <summary>
        /// 自定义基础服务接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ICustomServiceNodeMethodEnum))]
        public partial interface ICustomServiceNode { }
        /// <summary>
        /// 自定义基础服务接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ICustomServiceNodeMethodEnum
        {
            /// <summary>
            /// [0] 创建数组节点 ArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int length 数组长度
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateArrayNode = 0,
            /// <summary>
            /// [1] 创建位图节点 BitmapNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// uint capacity 二进制位数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateBitmapNode = 1,
            /// <summary>
            /// [2] 创建字典节点 ByteArrayDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayDictionaryNode = 2,
            /// <summary>
            /// [3] 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayFragmentDictionaryNode = 3,
            /// <summary>
            /// [4] 创建队列节点（先进先出） ByteArrayQueueNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayQueueNode = 4,
            /// <summary>
            /// [5] 创建栈节点（后进先出） ByteArrayStackNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayStackNode = 5,
            /// <summary>
            /// [6] 创建字典节点 DictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDictionaryNode = 6,
            /// <summary>
            /// [7] 创建分布式锁节点 DistributedLockNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDistributedLockNode = 7,
            /// <summary>
            /// [8] 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentDictionaryNode = 8,
            /// <summary>
            /// [9] 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashSetNode = 9,
            /// <summary>
            /// [10] 创建字典节点 HashBytesDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesDictionaryNode = 10,
            /// <summary>
            /// [11] 创建字典节点 HashBytesFragmentDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesFragmentDictionaryNode = 11,
            /// <summary>
            /// [12] 创建哈希表节点 HashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashSetNode = 12,
            /// <summary>
            /// [13] 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// long identity 起始分配 ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateIdentityGeneratorNode = 13,
            /// <summary>
            /// [14] 创建数组节点 LeftArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateLeftArrayNode = 14,
            /// <summary>
            /// [15] 创建消息处理节点 MessageNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType messageType 消息数据类型
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateMessageNode = 15,
            /// <summary>
            /// [16] 创建队列节点（先进先出） QueueNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateQueueNode = 16,
            /// <summary>
            /// [17] 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeDictionaryNode = 17,
            /// <summary>
            /// [18] 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeSetNode = 18,
            /// <summary>
            /// [19] 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerByteArrayMessageNode = 19,
            /// <summary>
            /// [20] 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedDictionaryNode = 20,
            /// <summary>
            /// [21] 创建排序列表节点 SortedListNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedListNode = 21,
            /// <summary>
            /// [22] 创建排序集合节点 SortedSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedSetNode = 22,
            /// <summary>
            /// [23] 创建栈节点（后进先出） StackNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateStackNode = 23,
            /// <summary>
            /// [24] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 24,
            /// <summary>
            /// [25] 创建服务注册节点 IServerRegistryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int loadTimeoutSeconds 冷启动会话超时秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerRegistryNode = 25,
            /// <summary>
            /// [26] 创建服务进程守护节点 IProcessGuardNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateProcessGuardNode = 26,
            /// <summary>
            /// [27] 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapClientFilterNode = 27,
            /// <summary>
            /// [28] 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapFilterNode = 28,
            /// <summary>
            /// [29] 删除节点
            /// string key 节点全局关键字
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNodeByKey = 29,
            /// <summary>
            /// [256] 创建计数器节点 ICounterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateCounterNode = 256,
        }
}
#endif