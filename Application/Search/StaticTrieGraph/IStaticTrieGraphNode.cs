using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// String trie graph node interface
    /// 字符串 Trie 图节点接口
    /// </summary>
    [ServerNode(IsLocalClient = true)]
    public partial interface IStaticTrieGraphNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSetWord(string value);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetGraph(GraphData value);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 3)]
        void SnapshotSetWordIdentity(BinarySerializeKeyValue<SubString, int> value);
        /// <summary>
        /// Add trie graph word
        /// 添加 Trie 图词语
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        AppendWordStateEnum AppendWord(string word);
        /// <summary>
        /// Has the trie graph been created
        /// 是否已经创建 Trie 图
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool IsGraph();
        /// <summary>
        /// Get the number of words in the trie graph
        /// 获取 Trie 图词语数量
        /// </summary>
        /// <returns>The number of words in the trie graph
        /// Trie 图词语数量</returns>
        [ServerMethod(IsPersistence = false)]
        int GetWordCount();
        /// <summary>
        /// Create the trie graph
        /// 创建 Trie 图
        /// </summary>
        /// <returns>The number of words in the trie graph
        /// Trie 图词语数量</returns>
        int BuildGraph();
        /// <summary>
        /// Adds text and returns a collection of word numbers (Check the input parameters before the persistence operation)
        /// 添加文本并返回词语编号集合（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        ValueResult<int[]> GetAddTextIdentityBeforePersistence(string text);
        /// <summary>
        /// Adds text and returns a collection of word numbers
        /// 添加文本并返回词语编号集合
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        int[] GetAddTextIdentity(string text);
        /// <summary>
        /// Get the collection of query word numbers (ignore unmatched words)
        /// 获取查询词语编号集合（忽略未匹配词语）
        /// </summary>
        /// <param name="text">The text content of the search
        /// 搜索文本内容</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        int[] GetWordSegmentIdentity(string text);
        /// <summary>
        /// Get the query word segmentation result
        /// 获取查询分词结果
        /// </summary>
        /// <param name="text">The text content of the search
        /// 搜索文本内容</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        WordSegmentResult[] GetWordSegmentResult(string text);
    }
}
