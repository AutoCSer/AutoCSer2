using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作
    /// </summary>
    public class ServiceNode : IServiceNode
    {
        /// <summary>
        /// 创建服务基础操作节点标识
        /// </summary>
        internal static readonly NodeIndex ServiceNodeIndex = new NodeIndex(0, 1);

        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseService service;
        /// <summary>
        /// 服务基础操作
        /// </summary>
        /// <param name="service"></param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 删除节点持久化参数检查
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public virtual ValueResult<bool> RemoveNodeBeforePersistence(NodeIndex index)
        {
            if (index.Index == 0) return false;
            return service.RemoveNodeBeforePersistence(index);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public virtual bool RemoveNode(NodeIndex index)
        {
            return index.Index != 0 && service.RemoveNode(index);
        }
        /// <summary>
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createNode<T>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode) where T : class
        {
            try
            {
                if (!service.IsLoaded) service.LoadCreateNode(index, key);
                ServerNodeCreator nodeCreator = service.GetNodeCreator<T>();
                if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                NodeIndex nodeIndex = service.CheckCreateNodeIndex(index, key, ref nodeInfo);
                if (nodeIndex.Index < 0 || !nodeIndex.GetFree()) return nodeIndex;
                return new ServerNode<T>(service, nodeIndex, key, getNode()).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照的服务端节点 参数检查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ST"></typeparam>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="nodeIndex"></param>
        /// <returns>返回 true 表示直接返回无需继续操作</returns>
        protected virtual bool checkCreateNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, out NodeIndex nodeIndex)
        {
            if (!service.IsLoaded) service.LoadCreateNode(index, key);
            ServerNodeCreator nodeCreator = service.GetNodeCreator<T>();
            if (nodeCreator == null)
            {
                nodeIndex = new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                return true;
            }
            if (nodeCreator.SnapshotType != typeof(ST))
            {
                nodeIndex = new NodeIndex(CallStateEnum.SnapshotTypeNotMatch);
                return true;
            }
            nodeIndex = service.CheckCreateNodeIndex(index, key, ref nodeInfo);
            return nodeIndex.Index < 0 || !nodeIndex.GetFree();
        }
        /// <summary>
        /// 创建支持快照的服务端节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="NT">节点接口操作对象类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createNode<T, NT, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<NT> getNode)
            where T : class
            where NT : T, ISnapshot<ST>
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
            where T : class
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照克隆的服务端节点
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="NT">节点接口操作对象类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createSnapshotCloneNode<T, NT, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<NT> getNode)
            where T : class
            where NT : T, ISnapshot<ST>
            where ST : SnapshotCloneObject<ST>
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerSnapshotCloneNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照克隆的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <typeparam name="ST">快照数据类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        protected virtual NodeIndex createSnapshotCloneNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
            where T : class
            where ST : SnapshotCloneObject<ST>
        {
            try
            {
                NodeIndex nodeIndex;
                if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
                return new ServerSnapshotCloneNode<T, ST>(service, nodeIndex, key, getNode(), service.CurrentCallIsPersistence).Index;
            }
            finally { service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建字典节点 FragmentHashStringDictionary256{HashString,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateFragmentHashStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IHashStringFragmentDictionaryNode<string>, HashStringFragmentDictionaryNode<string>, KeyValue<string, string>>(index, key, nodeInfo, () => new HashStringFragmentDictionaryNode<string>());
        }
        /// <summary>
        /// 创建字典节点 FragmentHashStringDictionary256{HashString,byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateFragmentHashStringByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IHashStringFragmentDictionaryNode<byte[]>, HashStringFragmentDictionaryNode<byte[]>, KeyValue<string, byte[]>>(index, key, nodeInfo, () => new HashStringFragmentDictionaryNode<byte[]>());
        }
        /// <summary>
        /// 创建分布式锁节点节点 DistributedLockNode{HashString}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashStringDistributedLockNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IDistributedLockNode<HashString>, DistributedLockIdentity<HashString>>(index, key, nodeInfo, () => new DistributedLockNode<HashString>());
        }
        /// <summary>
        /// 创建数组节点 ArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateStringArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length)
        {
            return createNode<IArrayNode<string>, ArrayNode<string>, KeyValue<int, string>>(index, key, nodeInfo, () => new ArrayNode<string>(length));
        }
        /// <summary>
        /// 创建数组节点 ArrayNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int length)
        {
            return createNode<IArrayNode<byte[]>, ArrayNode<byte[]>, KeyValue<int, byte[]>>(index, key, nodeInfo, () => new ArrayNode<byte[]>(length));
        }
        /// <summary>
        /// 创建位图节点 BitmapNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity)
        {
            return createNode<IBitmapNode, BitmapNode, byte[]>(index, key, nodeInfo, () => new BitmapNode(capacity));
        }
        /// <summary>
        /// 创建字典节点 DictionaryNode{HashString,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IDictionaryNode<HashString, string>, DictionaryNode<HashString, string>, KeyValue<HashString, string>>(index, key, nodeInfo, () => new DictionaryNode<HashString, string>(capacity));
        }
        /// <summary>
        /// 创建字典节点 DictionaryNode{HashString,byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashStringByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IDictionaryNode<HashString, byte[]>, DictionaryNode<HashString, byte[]>, KeyValue<HashString, byte[]>>(index, key, nodeInfo, () => new DictionaryNode<HashString, byte[]>(capacity));
        }
        /// <summary>
        /// 创建 256 基分片哈希表节点 FragmentHashSetNode{HashString}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateFragmentHashStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IFragmentHashSetNode<HashString>, FragmentHashSetNode<HashString>, HashString>(index, key, nodeInfo, () => new FragmentHashSetNode<HashString>());
        }
        /// <summary>
        /// 创建哈希表节点 HashSetNode{HashString}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashStringHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return createNode<IHashSetNode<HashString>, HashSetNode<HashString>, HashString>(index, key, nodeInfo, () => new HashSetNode<HashString>());
        }
        /// <summary>
        /// 创建数组节点 LeftArrayNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateStringLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<ILeftArrayNode<string>, LeftArrayNode<string>, string>(index, key, nodeInfo, () => new LeftArrayNode<string>(capacity));
        }
        /// <summary>
        /// 创建数组节点 LeftArrayNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<ILeftArrayNode<byte[]>, LeftArrayNode<byte[]>, byte[]>(index, key, nodeInfo, () => new LeftArrayNode<byte[]>(capacity));
        }
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateStringQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IQueueNode<string>, QueueNode<string>, string>(index, key, nodeInfo, () => new QueueNode<string>(capacity));
        }
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IQueueNode<byte[]>, QueueNode<byte[]>, byte[]>(index, key, nodeInfo, () => new QueueNode<byte[]>(capacity));
        }
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateStringStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IStackNode<string>, StackNode<string>, string>(index, key, nodeInfo, () => new StackNode<string>(capacity));
        }
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{byte[]}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return createNode<IStackNode<byte[]>, StackNode<byte[]>, byte[]>(index, key, nodeInfo, () => new StackNode<byte[]>(capacity));
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
        public virtual NodeIndex CreateStringMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<StringMessage>, StringMessage>(index, key, nodeInfo, () => MessageNode<StringMessage>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
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
        public virtual NodeIndex CreateByteArrayMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return createNode<IMessageNode<ByteArrayMessage>, ByteArrayMessage>(index, key, nodeInfo, () => MessageNode<ByteArrayMessage>.Create(service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }

        /// <summary>
        /// 创建服务基础操作节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ServerNode<T> CreateServiceNode<T>(StreamPersistenceMemoryDatabaseService service, T target)
             where T : class, IServiceNode
        {
            return new ServerNode<T>(service, ServiceNodeIndex, string.Empty, target);
        }
    }
}
