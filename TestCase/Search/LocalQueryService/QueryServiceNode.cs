﻿using AutoCSer.CommandService;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using System.Reflection;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 搜索聚合查询服务服务本地节点的自定义基础服务
    /// </summary>
    public sealed class QueryServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, IQueryServiceNode
    {
        /// <summary>
        /// 搜索聚合查询服务服务本地节点的自定义基础服务
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        public QueryServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }

        /// <summary>
        /// 创建字符串 Trie 图节点 IStaticTrieGraphNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="maxTrieWordSize">Trie 词语最大文字长度</param>
        /// <param name="maxWordSize">未知词语最大文字长度</param>
        /// <param name="wordSegmentFlags">分词选项</param>
        /// <param name="replaceChars">替换文字集合</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateStaticTrieGraphNode(NodeIndex index, string key, NodeInfo nodeInfo, byte maxTrieWordSize, byte maxWordSize, WordSegmentFlags wordSegmentFlags, string replaceChars)
        {
            return CreateSnapshotNode<IStaticTrieGraphNode>(index, key, nodeInfo, () => new StaticTrieGraphNode(maxTrieWordSize, maxWordSize, wordSegmentFlags, replaceChars));
        }

        /// <summary>
        /// 创建用户名称分词结果磁盘块索引信息节点 IWordIdentityBlockIndex.ILocalNode{int}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateUserNameWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNode<int>>(index, key, nodeInfo, () => new UserNameNode());
        }
        /// <summary>
        /// 创建用户备注分词结果磁盘块索引信息节点 IWordIdentityBlockIndex.LocalNode{int}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateUserRemarkWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNode<int>>(index, key, nodeInfo, () => new UserRemarkNode());
        }

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
                return (NodeIndex)typeof(QueryServiceNode).GetMethod(nameof(createRemoveMarkHashKeyIndexNode), BindingFlags.Static | BindingFlags.NonPublic)
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
                return (NodeIndex)typeof(QueryServiceNode).GetMethod(nameof(createRemoveMarkHashIndexNode), BindingFlags.Static | BindingFlags.NonPublic)
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
        /// <param name="node"></param>
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
