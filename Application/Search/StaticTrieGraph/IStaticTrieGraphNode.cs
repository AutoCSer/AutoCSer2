using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 字符串 Trie 图节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IStaticTrieGraphNode
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSetWord(string value);
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetGraph(GraphData value);
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 3)]
        void SnapshotSetWordIdentity(BinarySerializeKeyValue<SubString, int> value);
        /// <summary>
        /// 添加 Trie 图词语
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        AppendWordStateEnum AppendWord(string word);
        /// <summary>
        /// 是否已经建图
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool IsGraph();
        /// <summary>
        /// 获取 Trie 图词语数量
        /// </summary>
        /// <returns>Trie 图词语数量</returns>
        [ServerMethod(IsPersistence = false)]
        int GetWordCount();
        /// <summary>
        /// 建图
        /// </summary>
        /// <returns>Trie 图词语数量</returns>
        int BuildGraph();
        /// <summary>
        /// 添加文本并返回词语编号集合 持久化前检查
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        ValueResult<int[]> GetAddTextIdentityBeforePersistence(string text);
        /// <summary>
        /// 添加文本并返回词语编号集合
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        int[] GetAddTextIdentity(string text);
        /// <summary>
        /// 获取查询词语编号集合（忽略未匹配词语）
        /// </summary>
        /// <param name="text">搜索文本内容</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        int[] GetWordSegmentIdentity(string text);
        /// <summary>
        /// 获取查询分词结果
        /// </summary>
        /// <param name="text">搜索文本内容</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        WordSegmentResult[] GetWordSegmentResult(string text);
    }
}
