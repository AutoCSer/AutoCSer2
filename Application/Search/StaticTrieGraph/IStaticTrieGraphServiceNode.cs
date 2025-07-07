using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 创建字符串 Trie 图节点的自定义基础服务接口
    /// </summary>
    [ServerNode]
    public partial interface IStaticTrieGraphServiceNode : IServiceNode
    {
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
        NodeIndex CreateStaticTrieGraphNode(NodeIndex index, string key, NodeInfo nodeInfo, byte maxTrieWordSize, byte maxWordSize, WordSegmentFlags wordSegmentFlags, string replaceChars);
    }
}
