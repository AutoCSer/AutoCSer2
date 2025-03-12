using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 搜索聚合查询服务服务本地节点的自定义基础服务接口
    /// </summary>
    [ServerNode(IsClient = false, IsLocalClient = true)]
    public partial interface IQueryServiceNode : CommandService.StreamPersistenceMemoryDatabase.IServiceNode
    {
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
        NodeIndex CreateStaticTrieGraphNode(NodeIndex index, string key, NodeInfo nodeInfo, int maxWordSize, bool isSingleCharacter, string replaceChars);

        /// <summary>
        /// 创建用户名称分词结果磁盘块索引信息节点 IWordIdentityBlockIndex
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateUserNameWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建用户备注分词结果磁盘块索引信息节点 IWordIdentityBlockIndex
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateUserRemarkWordIdentityBlockIndexNode(NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建非索引条件查询数据节点 ISearchUserNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateSearchUserNode(NodeIndex index, string key, NodeInfo nodeInfo);

        /// <summary>
        /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashIndexNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">索引关键字类型</param>
        /// <param name="valueType">数据关键字类型</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateRemoveMarkHashIndexNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
        /// <summary>
        /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashKeyIndexNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">索引关键字类型</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateRemoveMarkHashKeyIndexNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
    }
}
