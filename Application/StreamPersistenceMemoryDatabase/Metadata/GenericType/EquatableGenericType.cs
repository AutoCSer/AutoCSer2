using AutoCSer.Metadata;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class EquatableGenericType : AutoCSer.Metadata.GenericTypeCache<EquatableGenericType>
    {
        /// <summary>
        /// Create distributed lock nodes IDistributedLockNode{KT}
        /// 创建分布式锁节点 IDistributedLockNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateDistributedLockNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// Create a dictionary node IByteArrayFragmentDictionaryNode{KT}
        /// 创建字典节点 IByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateByteArrayFragmentDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// Create a dictionary node IByteArrayDictionaryNode{KT}
        /// 创建字典节点 IByteArrayDictionaryNode{KT}
        /// </summary>
        /// <param name="node"></param>
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
        internal abstract NodeIndex CreateByteArrayDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);
        /// <summary>
        /// Create a 256 base fragment hash table node IFragmentHashSetNode{KT}
        /// 创建 256 基分片哈希表节点 IFragmentHashSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateFragmentHashSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// Create a hash table node IHashSetNode{KT}
        /// 创建哈希表节点 IHashSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
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
        internal abstract NodeIndex CreateHashSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType);

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static EquatableGenericType create<T>()
            where T : IEquatable<T>
        {
            return new EquatableGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static EquatableGenericType? lastGenericType;
#else
        protected static EquatableGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EquatableGenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class EquatableGenericType<T> : EquatableGenericType
        where T : IEquatable<T>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// Create distributed lock nodes IDistributedLockNode{KT}
        /// 创建分布式锁节点 IDistributedLockNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateDistributedLockNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateSnapshotNode<IDistributedLockNode<T>>(index, key, nodeInfo, () => new DistributedLockNode<T>());
        }
        /// <summary>
        /// Create a dictionary node IByteArrayFragmentDictionaryNode{KT}
        /// 创建字典节点 IByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateByteArrayFragmentDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateSnapshotNode<IByteArrayFragmentDictionaryNode<T>>(index, key, nodeInfo, () => new ByteArrayFragmentDictionaryNode<T>());
        }
        /// <summary>
        /// Create a dictionary node IByteArrayDictionaryNode{KT}
        /// 创建字典节点 IByteArrayDictionaryNode{KT}
        /// </summary>
        /// <param name="node"></param>
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
        internal override NodeIndex CreateByteArrayDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            return node.CreateSnapshotNode<IByteArrayDictionaryNode<T>>(index, key, nodeInfo, () => new ByteArrayDictionaryNode<T>(capacity, groupType));
        }
        /// <summary>
        /// Create a 256 base fragment hash table node IFragmentHashSetNode{KT}
        /// 创建 256 基分片哈希表节点 IFragmentHashSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateFragmentHashSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateSnapshotNode<IFragmentHashSetNode<T>>(index, key, nodeInfo, () => new FragmentHashSetNode<T>());
        }
        /// <summary>
        /// Create a hash table node IHashSetNode{KT}
        /// 创建哈希表节点 IHashSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
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
        internal override NodeIndex CreateHashSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            return node.CreateSnapshotNode<IHashSetNode<T>>(index, key, nodeInfo, () => new HashSetNode<T>(capacity, groupType));
        }
    }
}
