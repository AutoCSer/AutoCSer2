//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 接口实时调用监视服务接口 客户端接口
        /// </summary>
        public partial interface IInterfaceRealTimeCallMonitorServiceClientController
        {
            /// <summary>
            /// 接口监视服务在线检查
            /// </summary>
            AutoCSer.Net.EnumeratorCommand Check();
            /// <summary>
            /// 调用完成
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="isException">接口是否执行异常</param>
            AutoCSer.Net.SendOnlyCommand Completed(long callIdentity, bool isException);
            /// <summary>
            /// 获取未完成调用数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> GetCount();
            /// <summary>
            /// 获取异常调用数据
            /// </summary>
            /// <returns>实时调用时间戳信息回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetException();
            /// <summary>
            /// 获取超时调用数据
            /// </summary>
            /// <returns>实时调用时间戳信息回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetTimeout();
            /// <summary>
            /// 获取指定数量的超时调用
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>超时调用回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetTimeoutCalls(int count);
            /// <summary>
            /// 获取超时调用数量
            /// </summary>
            /// <returns>超时调用数量</returns>
            AutoCSer.Net.ReturnCommand<int> GetTimeoutCount();
            /// <summary>
            /// 设置自定义调用步骤
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="step">自定义调用步骤</param>
            AutoCSer.Net.SendOnlyCommand SetStep(long callIdentity, int step);
            /// <summary>
            /// 新增一个实时调用信息
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <param name="timeoutMilliseconds">超时毫秒数</param>
            /// <param name="type">调用类型</param>
            AutoCSer.Net.SendOnlyCommand Start(long callIdentity, string callType, string callName, int timeoutMilliseconds, ushort type);
        }
}namespace AutoCSer.CommandService
{
    public enum InterfaceRealTimeCallMonitorServiceMethodEnum
    {
            /// <summary>
            /// [0] 接口监视服务在线检查
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// AutoCSer.Net.CommandServerKeepCallback callback 在线检查回调
            /// </summary>
            Check = 0,
            /// <summary>
            /// [1] 调用完成
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// long callIdentity 调用标识
            /// bool isException 接口是否执行异常
            /// </summary>
            Completed = 1,
            /// <summary>
            /// [2] 获取未完成调用数量
            /// 返回值 int 
            /// </summary>
            GetCount = 2,
            /// <summary>
            /// [3] 获取异常调用数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp} callback 实时调用时间戳信息回调
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp 
            /// </summary>
            GetException = 3,
            /// <summary>
            /// [4] 获取超时调用数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp} callback 实时调用时间戳信息回调
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp 
            /// </summary>
            GetTimeout = 4,
            /// <summary>
            /// [5] 获取指定数量的超时调用
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// int count 获取数量
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp} callback 超时调用回调
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp 
            /// </summary>
            GetTimeoutCalls = 5,
            /// <summary>
            /// [6] 获取超时调用数量
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// 返回值 int 超时调用数量
            /// </summary>
            GetTimeoutCount = 6,
            /// <summary>
            /// [7] 设置自定义调用步骤
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// long callIdentity 调用标识
            /// int step 自定义调用步骤
            /// </summary>
            SetStep = 7,
            /// <summary>
            /// [8] 新增一个实时调用信息
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// long callIdentity 调用标识
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// int timeoutMilliseconds 超时毫秒数
            /// ushort type 调用类型
            /// </summary>
            Start = 8,
    }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 异常调用统计信息节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.IExceptionStatisticsNode))]
        public partial interface IExceptionStatisticsNodeClientNode
        {
            /// <summary>
            /// 添加异常调用时间
            /// </summary>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <param name="callTime">异常调用时间</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Append(string callType, string callName, System.DateTime callTime);
            /// <summary>
            /// 获取异常调用总次数
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> GetCount();
            /// <summary>
            /// 获取指定数量调用异常统计信息
            /// </summary>
            /// <param name="count">获取调用异常统计信息数量</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallExceptionStatistics>> GetManyStatistics(int count);
            /// <summary>
            /// 获取调用异常统计信息
            /// </summary>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <returns>异常统计信息，失败返回 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.ExceptionStatistics> GetStatistics(string callType, string callName);
            /// <summary>
            /// 获取异常统计信息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetStatisticsCount();
            /// <summary>
            /// 移除异常统计信息
            /// </summary>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Remove(string callType, string callName);
            /// <summary>
            /// 移除当前节点
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RemoveNode();
        }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 创建异常调用统计信息节点的自定义基础服务接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.IExceptionStatisticsServiceNode))]
        public partial interface IExceptionStatisticsServiceNodeClientNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode
        {
            /// <summary>
            /// 创建异常调用统计信息节点 IExceptionStatisticsNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="removeTime">节点自动移除时间</param>
            /// <param name="callTimeCount">保存调用时间数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateExceptionStatisticsNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, System.DateTime removeTime, int callTimeCount);
        }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 异常调用统计信息节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IExceptionStatisticsNodeMethodEnum))]
        public partial interface IExceptionStatisticsNode { }
        /// <summary>
        /// 异常调用统计信息节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IExceptionStatisticsNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加异常调用时间
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// System.DateTime callTime 异常调用时间
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 获取异常调用总次数
            /// 返回值 long 
            /// </summary>
            GetCount = 1,
            /// <summary>
            /// [2] 获取指定数量调用异常统计信息
            /// int count 获取调用异常统计信息数量
            /// </summary>
            GetManyStatistics = 2,
            /// <summary>
            /// [3] 获取调用异常统计信息
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.ExceptionStatistics 异常统计信息，失败返回 null
            /// </summary>
            GetStatistics = 3,
            /// <summary>
            /// [4] 获取异常统计信息数量
            /// 返回值 int 
            /// </summary>
            GetStatisticsCount = 4,
            /// <summary>
            /// [5] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{long,AutoCSer.CommandService.InterfaceRealTimeCallMonitor.ExceptionStatistics} value 数据
            /// </summary>
            SnapshotSet = 5,
            /// <summary>
            /// [6] 快照设置数据
            /// AutoCSer.LeftArray{string} stringArray 数据
            /// </summary>
            SnapshotSetStringArray = 6,
            /// <summary>
            /// [7] 移除异常统计信息
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// </summary>
            Remove = 7,
            /// <summary>
            /// [8] 移除当前节点
            /// </summary>
            RemoveNode = 8,
        }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 创建异常调用统计信息节点的自定义基础服务接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IExceptionStatisticsServiceNodeMethodEnum))]
        public partial interface IExceptionStatisticsServiceNode { }
        /// <summary>
        /// 创建异常调用统计信息节点的自定义基础服务接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IExceptionStatisticsServiceNodeMethodEnum
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
            /// [256] 创建异常调用统计信息节点 IExceptionStatisticsNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// System.DateTime removeTime 节点自动移除时间
            /// int callTimeCount 保存调用时间数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateExceptionStatisticsNode = 256,
        }
}
#endif