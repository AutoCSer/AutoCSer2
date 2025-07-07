using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService;
using System;
using System.Reflection;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search;

namespace AutoCSer.TestCase.SearchDiskBlockIndex
{
    /// <summary>
    /// 创建索引节点的自定义基础服务
    /// </summary>
    public sealed class ServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, IDiskBlockIndexServiceNode
    {
        /// <summary>
        /// 创建索引节点的自定义基础服务
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashKeyIndexNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">索引关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateRemoveMarkHashKeyIndexNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return (NodeIndex)typeof(ServiceNode).GetMethod(nameof(createRemoveMarkHashKeyIndexNode), BindingFlags.Static | BindingFlags.NonPublic)
                    .MakeGenericMethod(type)
                    .Invoke(null, new object[] { this, index, key, nodeInfo });
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashKeyIndexNode{T}
        /// </summary>
        /// <typeparam name="T">Index keyword type
        /// 索引关键字类型</typeparam>
        /// <param name="node"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        private static NodeIndex createRemoveMarkHashKeyIndexNode<T>(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
            where T : notnull, IEquatable<T>
        {
            return node.CreateSnapshotNode<IRemoveMarkHashKeyIndexNode<T>>(index, key, nodeInfo, () => new RemoveMarkHashKeyIndexNode<T>());
        }
        /// <summary>
        /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashIndexNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">索引关键字类型</param>
        /// <param name="valueType">数据关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateRemoveMarkHashIndexNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getEquatableType2(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return (NodeIndex)typeof(ServiceNode).GetMethod(nameof(createRemoveMarkHashIndexNode), BindingFlags.Static | BindingFlags.NonPublic)
                    .MakeGenericMethod(type, type2)
                    .Invoke(null, new object[] { this, index, key, nodeInfo });
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashIndexNode{KT,VT}
        /// </summary>
        /// <typeparam name="KT">Index keyword type
        /// 索引关键字类型</typeparam>
        /// <typeparam name="VT">Data keyword type
        /// 数据关键字类型</typeparam>
        /// <param name="node">索引关键字类型</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        private static NodeIndex createRemoveMarkHashIndexNode<KT, VT>(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
            where KT : notnull, IEquatable<KT>
            where VT : notnull, IEquatable<VT>
        {
            return node.CreateSnapshotNode<IRemoveMarkHashIndexNode<KT, VT>>(index, key, nodeInfo, () => new RemoveMarkHashIndexNode<KT, VT>());
        }
    }
}
