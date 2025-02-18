//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 关键字索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IIndexNode<,>))]
        public partial interface IIndexNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Append(KT key, VT value);
            /// <summary>
            /// 移除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Remove(KT key, VT value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendArray(KT[] keys, VT value);
            /// <summary>
            /// 移除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RemoveArray(KT[] keys, VT value);
            /// <summary>
            /// 获取索引数据
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.IndexData<VT>> GetData(KT key);
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IWordIdentityBlockIndexNode<>))]
        public partial interface IWordIdentityBlockIndexNodeClientNode<T>
        {
            /// <summary>
            /// 创建分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum> Create(T key);
            /// <summary>
            /// 删除分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum> Delete(T key);
            /// <summary>
            /// 更新分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum> Update(T key);
        }
}namespace AutoCSer.CommandService.Search.StaticTrieGraph
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
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 关键字索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IIndexNode<,>))]
        public partial interface IIndexNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Append(KT key, VT value);
            /// <summary>
            /// 移除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Remove(KT key, VT value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendArray(KT[] keys, VT value);
            /// <summary>
            /// 移除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveArray(KT[] keys, VT value);
            /// <summary>
            /// 获取索引数据
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.IndexData<VT>>> GetData(KT key);
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IWordIdentityBlockIndexNode<>))]
        public partial interface IWordIdentityBlockIndexNodeLocalClientNode<T>
        {
            /// <summary>
            /// 创建分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> Create(T key);
            /// <summary>
            /// 删除分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> Delete(T key);
            /// <summary>
            /// 更新分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> Update(T key);
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
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 关键字索引节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IIndexNodeMethodEnum))]
        public partial interface IIndexNode<KT,VT> { }
        /// <summary>
        /// 关键字索引节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IIndexNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            AppendLoadPersistence = 1,
            /// <summary>
            /// [2] 磁盘块索引信息写入完成
            /// KT key 索引关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 
            /// int valueCount 
            /// int version 
            /// </summary>
            Completed = 2,
            /// <summary>
            /// [3] 移除匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            Remove = 3,
            /// <summary>
            /// [4] 移除匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveLoadPersistence = 4,
            /// <summary>
            /// [5] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{KT,AutoCSer.CommandService.Search.IndexData{VT}} value 数据
            /// </summary>
            SnapshotSet = 5,
            /// <summary>
            /// [6] 磁盘块索引信息写入完成
            /// KT key 索引关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 
            /// int valueCount 
            /// int version 
            /// </summary>
            CompletedLoadPersistence = 6,
            /// <summary>
            /// [7] 添加匹配数据关键字
            /// KT[] keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendArray = 7,
            /// <summary>
            /// [8] 添加匹配数据关键字
            /// KT[] keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendArrayLoadPersistence = 8,
            /// <summary>
            /// [9] 移除匹配数据关键字
            /// KT[] keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveArray = 9,
            /// <summary>
            /// [10] 移除匹配数据关键字
            /// KT[] keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveArrayLoadPersistence = 10,
            /// <summary>
            /// [11] 获取索引数据
            /// KT key 索引关键字
            /// 返回值 AutoCSer.CommandService.Search.IndexData{VT} 
            /// </summary>
            GetData = 11,
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IWordIdentityBlockIndexNodeMethodEnum))]
        public partial interface IWordIdentityBlockIndexNode<T> { }
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IWordIdentityBlockIndexNodeMethodEnum
        {
            /// <summary>
            /// [0] 分词结果磁盘块索引信息完成更新操作
            /// T key 分词数据关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 磁盘块索引信息
            /// int version 操作版本号
            /// </summary>
            Completed = 0,
            /// <summary>
            /// [1] 创建分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Create = 1,
            /// <summary>
            /// [2] 创建分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            CreateLoadPersistence = 2,
            /// <summary>
            /// [3] 删除分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Delete = 3,
            /// <summary>
            /// [4] 删除分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            DeleteLoadPersistence = 4,
            /// <summary>
            /// [5] 删除分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// </summary>
            Deleted = 5,
            /// <summary>
            /// [6] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{T,AutoCSer.CommandService.DiskBlock.BlockIndex} value 数据
            /// </summary>
            SnapshotSet = 6,
            /// <summary>
            /// [7] 更新分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Update = 7,
            /// <summary>
            /// [8] 更新分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            UpdateLoadPersistence = 8,
            /// <summary>
            /// [9] 分词结果磁盘块索引信息完成更新操作
            /// T key 分词数据关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 磁盘块索引信息
            /// int version 操作版本号
            /// </summary>
            CompletedLoadPersistence = 9,
            /// <summary>
            /// [10] 删除分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// </summary>
            DeletedLoadPersistence = 10,
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
            /// AutoCSer.BinarySerializeKeyValue{AutoCSer.SubString,int} value 数据
            /// </summary>
            SnapshotSetWordIdentity = 8,
        }
}
#endif