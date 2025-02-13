//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
        /// <summary>
        /// 字符串 Trie 图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.StaticTrieGraph.IStaticTrieGraphNode))]
        public partial interface IStaticTrieGraphNodeClientNode
        {
            /// <summary>
            /// 添加 Trie 图词语
            /// </summary>
            /// <param name="word"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.StaticTrieGraph.AppendWordStateEnum> AppendWord(string word);
            /// <summary>
            /// 建图
            /// </summary>
            /// <returns>Trie 图词语数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> BuildGraph();
            /// <summary>
            /// 添加文本并返回词语编号集合
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int[]> GetAddTextIdentity(string text);
            /// <summary>
            /// 获取查询词语编号集合（忽略未匹配词语）
            /// </summary>
            /// <param name="text">搜索文本内容</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int[]> GetWordSegmentIdentity(string text);
            /// <summary>
            /// 获取查询分词结果
            /// </summary>
            /// <param name="text">搜索文本内容</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.StaticTrieGraph.WordSegmentResult[]> GetWordSegmentResult(string text);
        }
}namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
        /// <summary>
        /// 字符串 Trie 图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.StaticTrieGraph.IStaticTrieGraphNode))]
        public partial interface IStaticTrieGraphNodeLocalClientNode
        {
            /// <summary>
            /// 添加 Trie 图词语
            /// </summary>
            /// <param name="word"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.StaticTrieGraph.AppendWordStateEnum>> AppendWord(string word);
            /// <summary>
            /// 建图
            /// </summary>
            /// <returns>Trie 图词语数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> BuildGraph();
            /// <summary>
            /// 添加文本并返回词语编号集合
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int[]>> GetAddTextIdentity(string text);
            /// <summary>
            /// 获取查询词语编号集合（忽略未匹配词语）
            /// </summary>
            /// <param name="text">搜索文本内容</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int[]>> GetWordSegmentIdentity(string text);
            /// <summary>
            /// 获取查询分词结果
            /// </summary>
            /// <param name="text">搜索文本内容</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.StaticTrieGraph.WordSegmentResult[]>> GetWordSegmentResult(string text);
        }
}namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
        /// <summary>
        /// 字符串 Trie 图节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IStaticTrieGraphNodeMethodEnum))]
        public partial interface IStaticTrieGraphNode { }
        /// <summary>
        /// 字符串 Trie 图节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IStaticTrieGraphNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加 Trie 图词语
            /// string word 
            /// 返回值 AutoCSer.CommandService.Search.StaticTrieGraph.AppendWordStateEnum 
            /// </summary>
            AppendWord = 0,
            /// <summary>
            /// [1] 建图
            /// 返回值 int Trie 图词语数量
            /// </summary>
            BuildGraph = 1,
            /// <summary>
            /// [2] 添加文本并返回词语编号集合
            /// string text 
            /// 返回值 int[] 
            /// </summary>
            GetAddTextIdentity = 2,
            /// <summary>
            /// [3] 添加文本并返回词语编号集合 持久化前检查
            /// string text 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int[]} 
            /// </summary>
            GetAddTextIdentityBeforePersistence = 3,
            /// <summary>
            /// [4] 获取查询词语编号集合（忽略未匹配词语）
            /// string text 搜索文本内容
            /// 返回值 int[] 
            /// </summary>
            GetWordSegmentIdentity = 4,
            /// <summary>
            /// [5] 获取查询分词结果
            /// string text 搜索文本内容
            /// 返回值 AutoCSer.CommandService.Search.StaticTrieGraph.WordSegmentResult[] 
            /// </summary>
            GetWordSegmentResult = 5,
            /// <summary>
            /// [6] 快照设置数据
            /// AutoCSer.CommandService.Search.StaticTrieGraph.GraphData value 数据
            /// </summary>
            SnapshotSetGraph = 6,
            /// <summary>
            /// [7] 快照设置数据
            /// string value 数据
            /// </summary>
            SnapshotSetWord = 7,
            /// <summary>
            /// [8] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{string,int} value 数据
            /// </summary>
            SnapshotSetWordIdentity = 8,
        }
}
#endif