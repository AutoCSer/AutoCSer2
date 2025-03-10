using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 创建字符串 Trie 图节点的自定义基础服务
    /// </summary>
    public sealed class StaticTrieGraphServiceNode : ServiceNode, IStaticTrieGraphServiceNode
    {
        /// <summary>
        /// 创建字符串 Trie 图节点的自定义基础服务
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        public StaticTrieGraphServiceNode(StreamPersistenceMemoryDatabaseService service) : base(service) { }
        /// <summary>
        /// 创建字符串 Trie 图节点 IStaticTrieGraphNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="maxWordSize">词语最大文字长度</param>
        /// <param name="isSingleCharacter">是否支持单字符搜索结果</param>
        /// <param name="replaceChars">替换文字集合</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public NodeIndex CreateStaticTrieGraphNode(NodeIndex index, string key, NodeInfo nodeInfo, int maxWordSize, bool isSingleCharacter, string replaceChars)
        {
            return CreateSnapshotNode<IStaticTrieGraphNode>(index, key, nodeInfo, () => new StaticTrieGraphNode(maxWordSize, isSingleCharacter, replaceChars));
        }
    }
}
