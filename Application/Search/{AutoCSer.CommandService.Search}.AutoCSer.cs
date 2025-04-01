//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.DiskBlockIndex.IRemoveMarkHashIndexNode<,>))]
        public partial interface IRemoveMarkHashIndexNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Append(KT key, VT value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendArray(KT[] keys, VT value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<VT>> GetBlockIndexData(KT key);
            /// <summary>
            /// 获取更新关键字集合
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand GetChangeKeys(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<KT>,AutoCSer.Net.KeepCallbackCommand> callback);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RemoveArray(KT[] keys, VT value);
            /// <summary>
            /// 磁盘块索引信息写入完成操作
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="blockIndex">磁盘块索引信息</param>
            /// <param name="valueCount">新增数据数量</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter WriteCompleted(KT key, AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex, int valueCount);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendLeftArray(AutoCSer.LeftArray<KT> keys, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RemoveLeftArray(AutoCSer.LeftArray<KT> keys, VT value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<VT>[]> GetBlockIndexDataArray(KT[] keys);
        }
}namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.DiskBlockIndex.IRemoveMarkHashKeyIndexNode<>))]
        public partial interface IRemoveMarkHashKeyIndexNodeClientNode<T>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Append(T key, uint value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendArray(T[] keys, uint value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<uint>> GetBlockIndexData(T key);
            /// <summary>
            /// 获取更新关键字集合
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand GetChangeKeys(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<T>,AutoCSer.Net.KeepCallbackCommand> callback);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T key, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RemoveArray(T[] keys, uint value);
            /// <summary>
            /// 磁盘块索引信息写入完成操作
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="blockIndex">磁盘块索引信息</param>
            /// <param name="valueCount">新增数据数量</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter WriteCompleted(T key, AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex, int valueCount);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendLeftArray(AutoCSer.LeftArray<T> keys, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RemoveLeftArray(AutoCSer.LeftArray<T> keys, uint value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<uint>[]> GetBlockIndexDataArray(T[] keys);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<int>> GetIntBlockIndexData(T key);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<int>[]> GetIntBlockIndexDataArray(T[] keys);
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 非索引条件查询数据节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IConditionDataNode<,>))]
        public partial interface IConditionDataNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 创建非索引条件查询数据
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum> Create(KT key);
            /// <summary>
            /// 更新非索引条件查询数据
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum> Update(KT key);
            /// <summary>
            /// 删除非索引条件查询数据
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum> Delete(KT key);
            /// <summary>
            /// 创建非索引条件查询数据
            /// </summary>
            /// <param name="value">非索引条件查询数据</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter LoadCreate(VT value);
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 创建哈希索引节点的自定义基础服务接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IDiskBlockIndexServiceNode))]
        public partial interface IDiskBlockIndexServiceNodeClientNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode
        {
            /// <summary>
            /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashIndexNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">索引关键字类型</param>
            /// <param name="valueType">数据关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateRemoveMarkHashIndexNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashKeyIndexNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">索引关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateRemoveMarkHashKeyIndexNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 创建字符串 Trie 图节点的自定义基础服务接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IStaticTrieGraphServiceNode))]
        public partial interface IStaticTrieGraphServiceNodeClientNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode
        {
            /// <summary>
            /// 创建字符串 Trie 图节点 IStaticTrieGraphNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="maxTrieWordSize">Trie 词语最大文字长度</param>
            /// <param name="maxWordSize">未知词语最大文字长度</param>
            /// <param name="wordSegmentFlags">分词选项</param>
            /// <param name="replaceChars">替换文字集合</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateStaticTrieGraphNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, byte maxTrieWordSize, byte maxWordSize, AutoCSer.CommandService.Search.StaticTrieGraph.WordSegmentFlags wordSegmentFlags, string replaceChars);
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
            /// <summary>
            /// 创建分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <param name="text">分词文本数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum> LoadCreate(T key, string text);
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
            /// <summary>
            /// 是否已经建图
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> IsGraph();
            /// <summary>
            /// 获取 Trie 图词语数量
            /// </summary>
            /// <returns>Trie 图词语数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetWordCount();
        }
}namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.DiskBlockIndex.IRemoveMarkHashIndexNode<,>))]
        public partial interface IRemoveMarkHashIndexNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Append(KT key, VT value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendArray(KT[] keys, VT value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<VT>>> GetBlockIndexData(KT key);
            /// <summary>
            /// 获取更新关键字集合
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<System.IDisposable> GetChangeKeys(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<KT>> callback);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveArray(KT[] keys, VT value);
            /// <summary>
            /// 磁盘块索引信息写入完成操作
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="blockIndex">磁盘块索引信息</param>
            /// <param name="valueCount">新增数据数量</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> WriteCompleted(KT key, AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex, int valueCount);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendLeftArray(AutoCSer.LeftArray<KT> keys, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveLeftArray(AutoCSer.LeftArray<KT> keys, VT value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<VT>[]>> GetBlockIndexDataArray(KT[] keys);
        }
}namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.DiskBlockIndex.IRemoveMarkHashKeyIndexNode<>))]
        public partial interface IRemoveMarkHashKeyIndexNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Append(T key, uint value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendArray(T[] keys, uint value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<uint>>> GetBlockIndexData(T key);
            /// <summary>
            /// 获取更新关键字集合
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<System.IDisposable> GetChangeKeys(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<T>> callback);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T key, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveArray(T[] keys, uint value);
            /// <summary>
            /// 磁盘块索引信息写入完成操作
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="blockIndex">磁盘块索引信息</param>
            /// <param name="valueCount">新增数据数量</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> WriteCompleted(T key, AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex, int valueCount);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendLeftArray(AutoCSer.LeftArray<T> keys, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveLeftArray(AutoCSer.LeftArray<T> keys, uint value);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<uint>[]>> GetBlockIndexDataArray(T[] keys);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<int>>> GetIntBlockIndexData(T key);
            /// <summary>
            /// 获取索引数据磁盘块索引信息
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData<int>[]>> GetIntBlockIndexDataArray(T[] keys);
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 非索引条件查询数据节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.IConditionDataNode<,>))]
        public partial interface IConditionDataNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 创建非索引条件查询数据
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum>> Create(KT key);
            /// <summary>
            /// 更新非索引条件查询数据
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum>> Update(KT key);
            /// <summary>
            /// 删除非索引条件查询数据
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum>> Delete(KT key);
            /// <summary>
            /// 创建非索引条件查询数据
            /// </summary>
            /// <param name="value">非索引条件查询数据</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> LoadCreate(VT value);
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
            /// <summary>
            /// 创建分词结果磁盘块索引信息
            /// </summary>
            /// <param name="key">分词数据关键字</param>
            /// <param name="text">分词文本数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> LoadCreate(T key, string text);
        }
}namespace AutoCSer.CommandService.Search.MemoryIndex
{
        /// <summary>
        /// 哈希索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.MemoryIndex.IHashCodeKeyIndexNode<>))]
        public partial interface IHashCodeKeyIndexNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Append(T key, uint value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendArray(T[] keys, uint value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendLeftArray(AutoCSer.LeftArray<T> keys, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T key, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveArray(T[] keys, uint value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveLeftArray(AutoCSer.LeftArray<T> keys, uint value);
        }
}namespace AutoCSer.CommandService.Search.MemoryIndex
{
        /// <summary>
        /// 哈希索引节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.MemoryIndex.IHashIndexNode<,>))]
        public partial interface IHashIndexNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Append(KT key, VT value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendArray(KT[] keys, VT value);
            /// <summary>
            /// 添加匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字集合</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendLeftArray(AutoCSer.LeftArray<KT> keys, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="key">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveArray(KT[] keys, VT value);
            /// <summary>
            /// 删除匹配数据关键字
            /// </summary>
            /// <param name="keys">索引关键字</param>
            /// <param name="value">匹配数据关键字</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RemoveLeftArray(AutoCSer.LeftArray<KT> keys, VT value);
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
            /// <summary>
            /// 是否已经建图
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> IsGraph();
            /// <summary>
            /// 获取 Trie 图词语数量
            /// </summary>
            /// <returns>Trie 图词语数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetWordCount();
        }
}namespace AutoCSer.CommandService.Search.WordIdentityBlockIndex
{
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNode<>))]
        public partial interface ILocalNodeLocalClientNode<T>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> Create(T key);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> Delete(T key);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="text"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> LoadCreate(T key, string text);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum>> Update(T key);
        }
}namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IRemoveMarkHashIndexNodeMethodEnum))]
        public partial interface IRemoveMarkHashIndexNode<KT,VT> { }
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IRemoveMarkHashIndexNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加匹配数据关键字
            /// KT[] keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendArray = 1,
            /// <summary>
            /// [2] 添加匹配数据关键字
            /// KT[] keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendArrayLoadPersistence = 2,
            /// <summary>
            /// [3] 添加匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null
            /// </summary>
            AppendLoadPersistence = 3,
            /// <summary>
            /// [4] 获取索引数据磁盘块索引信息
            /// KT key 索引关键字
            /// 返回值 AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{VT} 
            /// </summary>
            GetBlockIndexData = 4,
            /// <summary>
            /// [5] 获取更新关键字集合
            /// 返回值 KT 
            /// </summary>
            GetChangeKeys = 5,
            /// <summary>
            /// [6] 删除匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null 或者没有找到索引关键字
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 删除匹配数据关键字
            /// KT[] keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveArray = 7,
            /// <summary>
            /// [8] 删除匹配数据关键字
            /// KT[] keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveArrayLoadPersistence = 8,
            /// <summary>
            /// [9] 删除匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null 或者没有找到索引关键字
            /// </summary>
            RemoveLoadPersistence = 9,
            /// <summary>
            /// [10] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{KT,AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{VT}} value 数据
            /// </summary>
            SnapshotSet = 10,
            /// <summary>
            /// [11] 磁盘块索引信息写入完成操作
            /// KT key 索引关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 磁盘块索引信息
            /// int valueCount 新增数据数量
            /// </summary>
            WriteCompleted = 11,
            /// <summary>
            /// [12] 磁盘块索引信息写入完成操作
            /// KT key 索引关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 磁盘块索引信息
            /// int valueCount 新增数据数量
            /// </summary>
            WriteCompletedLoadPersistence = 12,
            /// <summary>
            /// [13] 添加匹配数据关键字
            /// AutoCSer.LeftArray{KT} keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendLeftArray = 13,
            /// <summary>
            /// [14] 添加匹配数据关键字
            /// AutoCSer.LeftArray{KT} keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendLeftArrayLoadPersistence = 14,
            /// <summary>
            /// [15] 删除匹配数据关键字
            /// AutoCSer.LeftArray{KT} keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveLeftArray = 15,
            /// <summary>
            /// [16] 删除匹配数据关键字
            /// AutoCSer.LeftArray{KT} keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveLeftArrayLoadPersistence = 16,
            /// <summary>
            /// [17] 获取索引数据磁盘块索引信息
            /// KT[] keys 索引关键字集合
            /// 返回值 AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{VT}[] 
            /// </summary>
            GetBlockIndexDataArray = 17,
        }
}namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IRemoveMarkHashKeyIndexNodeMethodEnum))]
        public partial interface IRemoveMarkHashKeyIndexNode<T> { }
        /// <summary>
        /// 带移除标记的可重用哈希索引节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IRemoveMarkHashKeyIndexNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加匹配数据关键字
            /// T[] keys 索引关键字集合
            /// uint value 匹配数据关键字
            /// </summary>
            AppendArray = 1,
            /// <summary>
            /// [2] 添加匹配数据关键字
            /// T[] keys 索引关键字集合
            /// uint value 匹配数据关键字
            /// </summary>
            AppendArrayLoadPersistence = 2,
            /// <summary>
            /// [3] 添加匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null
            /// </summary>
            AppendLoadPersistence = 3,
            /// <summary>
            /// [4] 获取索引数据磁盘块索引信息
            /// T key 索引关键字
            /// 返回值 AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{uint} 
            /// </summary>
            GetBlockIndexData = 4,
            /// <summary>
            /// [5] 获取更新关键字集合
            /// 返回值 T 
            /// </summary>
            GetChangeKeys = 5,
            /// <summary>
            /// [6] 删除匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null 或者没有找到索引关键字
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 删除匹配数据关键字
            /// T[] keys 索引关键字
            /// uint value 匹配数据关键字
            /// </summary>
            RemoveArray = 7,
            /// <summary>
            /// [8] 删除匹配数据关键字
            /// T[] keys 索引关键字
            /// uint value 匹配数据关键字
            /// </summary>
            RemoveArrayLoadPersistence = 8,
            /// <summary>
            /// [9] 删除匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null 或者没有找到索引关键字
            /// </summary>
            RemoveLoadPersistence = 9,
            /// <summary>
            /// [10] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{T,AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{uint}} value 数据
            /// </summary>
            SnapshotSet = 10,
            /// <summary>
            /// [11] 磁盘块索引信息写入完成操作
            /// T key 索引关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 磁盘块索引信息
            /// int valueCount 新增数据数量
            /// </summary>
            WriteCompleted = 11,
            /// <summary>
            /// [12] 磁盘块索引信息写入完成操作
            /// T key 索引关键字
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 磁盘块索引信息
            /// int valueCount 新增数据数量
            /// </summary>
            WriteCompletedLoadPersistence = 12,
            /// <summary>
            /// [13] 添加匹配数据关键字
            /// AutoCSer.LeftArray{T} keys 索引关键字集合
            /// uint value 匹配数据关键字
            /// </summary>
            AppendLeftArray = 13,
            /// <summary>
            /// [14] 添加匹配数据关键字
            /// AutoCSer.LeftArray{T} keys 索引关键字集合
            /// uint value 匹配数据关键字
            /// </summary>
            AppendLeftArrayLoadPersistence = 14,
            /// <summary>
            /// [15] 删除匹配数据关键字
            /// AutoCSer.LeftArray{T} keys 索引关键字
            /// uint value 匹配数据关键字
            /// </summary>
            RemoveLeftArray = 15,
            /// <summary>
            /// [16] 删除匹配数据关键字
            /// AutoCSer.LeftArray{T} keys 索引关键字
            /// uint value 匹配数据关键字
            /// </summary>
            RemoveLeftArrayLoadPersistence = 16,
            /// <summary>
            /// [17] 获取索引数据磁盘块索引信息
            /// T[] keys 索引关键字集合
            /// 返回值 AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{uint}[] 
            /// </summary>
            GetBlockIndexDataArray = 17,
            /// <summary>
            /// [18] 获取索引数据磁盘块索引信息
            /// T key 索引关键字
            /// 返回值 AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{int} 
            /// </summary>
            GetIntBlockIndexData = 18,
            /// <summary>
            /// [19] 获取索引数据磁盘块索引信息
            /// T[] keys 索引关键字集合
            /// 返回值 AutoCSer.CommandService.Search.DiskBlockIndex.BlockIndexData{int}[] 
            /// </summary>
            GetIntBlockIndexDataArray = 19,
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 非索引条件查询数据节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IConditionDataNodeMethodEnum))]
        public partial interface IConditionDataNode<KT,VT> { }
        /// <summary>
        /// 非索引条件查询数据节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IConditionDataNodeMethodEnum
        {
            /// <summary>
            /// [0] 快照设置数据
            /// VT value 数据
            /// </summary>
            SnapshotSet = 0,
            /// <summary>
            /// [1] 创建非索引条件查询数据
            /// KT key 数据关键字
            /// 返回值 AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum 
            /// </summary>
            Create = 1,
            /// <summary>
            /// [2] 更新非索引条件查询数据
            /// KT key 数据关键字
            /// 返回值 AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum 
            /// </summary>
            Update = 2,
            /// <summary>
            /// [3] 初始化数据加载完成
            /// </summary>
            Loaded = 3,
            /// <summary>
            /// [4] 快照设置数据
            /// bool value 数据
            /// </summary>
            SnapshotSetLoaded = 4,
            /// <summary>
            /// [5] 非索引条件查询数据完成更新操作
            /// VT value 非索引条件查询数据
            /// 返回值 AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum 
            /// </summary>
            Completed = 5,
            /// <summary>
            /// [6] 非索引条件查询数据完成更新操作
            /// VT value 非索引条件查询数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum} 
            /// </summary>
            CompletedBeforePersistence = 6,
            /// <summary>
            /// [7] 删除非索引条件查询数据
            /// KT key 数据关键字
            /// 返回值 AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum 
            /// </summary>
            Delete = 7,
            /// <summary>
            /// [8] 非索引条件查询数据完成更新操作
            /// VT value 非索引条件查询数据
            /// 返回值 AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum 
            /// </summary>
            CompletedLoadPersistence = 8,
            /// <summary>
            /// [9] 删除非索引条件查询数据
            /// KT key 数据关键字
            /// 返回值 AutoCSer.CommandService.Search.ConditionDataUpdateStateEnum 
            /// </summary>
            DeleteLoadPersistence = 9,
            /// <summary>
            /// [10] 创建非索引条件查询数据
            /// VT value 非索引条件查询数据
            /// </summary>
            LoadCreate = 10,
            /// <summary>
            /// [11] 创建非索引条件查询数据 持久化前检查
            /// VT value 非索引条件查询数据
            /// 返回值 bool 是否继续持久化操作
            /// </summary>
            LoadCreateBeforePersistence = 11,
            /// <summary>
            /// [12] 创建非索引条件查询数据
            /// VT value 非索引条件查询数据
            /// </summary>
            LoadCreateLoadPersistence = 12,
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 创建哈希索引节点的自定义基础服务接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IDiskBlockIndexServiceNodeMethodEnum))]
        public partial interface IDiskBlockIndexServiceNode { }
        /// <summary>
        /// 创建哈希索引节点的自定义基础服务接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IDiskBlockIndexServiceNodeMethodEnum
        {
            /// <summary>
            /// [0] 创建数组节点 ArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int length 数组长度
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateArrayNode = 0,
            /// <summary>
            /// [1] 创建位图节点 BitmapNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// uint capacity 二进制位数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateBitmapNode = 1,
            /// <summary>
            /// [2] 创建字典节点 ByteArrayDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayDictionaryNode = 2,
            /// <summary>
            /// [3] 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayFragmentDictionaryNode = 3,
            /// <summary>
            /// [4] 创建队列节点（先进先出） ByteArrayQueueNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayQueueNode = 4,
            /// <summary>
            /// [5] 创建栈节点（后进先出） ByteArrayStackNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayStackNode = 5,
            /// <summary>
            /// [6] 创建字典节点 DictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDictionaryNode = 6,
            /// <summary>
            /// [7] 创建分布式锁节点 DistributedLockNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDistributedLockNode = 7,
            /// <summary>
            /// [8] 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentDictionaryNode = 8,
            /// <summary>
            /// [9] 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashSetNode = 9,
            /// <summary>
            /// [10] 创建字典节点 HashBytesDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesDictionaryNode = 10,
            /// <summary>
            /// [11] 创建字典节点 HashBytesFragmentDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesFragmentDictionaryNode = 11,
            /// <summary>
            /// [12] 创建哈希表节点 HashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashSetNode = 12,
            /// <summary>
            /// [13] 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// long identity 起始分配 ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateIdentityGeneratorNode = 13,
            /// <summary>
            /// [14] 创建数组节点 LeftArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateLeftArrayNode = 14,
            /// <summary>
            /// [15] 创建消息处理节点 MessageNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType messageType 消息数据类型
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateMessageNode = 15,
            /// <summary>
            /// [16] 创建队列节点（先进先出） QueueNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateQueueNode = 16,
            /// <summary>
            /// [17] 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeDictionaryNode = 17,
            /// <summary>
            /// [18] 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeSetNode = 18,
            /// <summary>
            /// [19] 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerByteArrayMessageNode = 19,
            /// <summary>
            /// [20] 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedDictionaryNode = 20,
            /// <summary>
            /// [21] 创建排序列表节点 SortedListNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedListNode = 21,
            /// <summary>
            /// [22] 创建排序集合节点 SortedSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedSetNode = 22,
            /// <summary>
            /// [23] 创建栈节点（后进先出） StackNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateStackNode = 23,
            /// <summary>
            /// [24] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 24,
            /// <summary>
            /// [25] 创建服务注册节点 IServerRegistryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int loadTimeoutSeconds 冷启动会话超时秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerRegistryNode = 25,
            /// <summary>
            /// [26] 创建服务进程守护节点 IProcessGuardNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateProcessGuardNode = 26,
            /// <summary>
            /// [27] 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapClientFilterNode = 27,
            /// <summary>
            /// [28] 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapFilterNode = 28,
            /// <summary>
            /// [29] 删除节点
            /// string key 节点全局关键字
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNodeByKey = 29,
            /// <summary>
            /// [256] 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashIndexNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 索引关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateRemoveMarkHashIndexNode = 256,
            /// <summary>
            /// [257] 创建带移除标记的可重用哈希索引节点 IRemoveMarkHashKeyIndexNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 索引关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateRemoveMarkHashKeyIndexNode = 257,
        }
}namespace AutoCSer.CommandService.Search
{
        /// <summary>
        /// 创建字符串 Trie 图节点的自定义基础服务接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IStaticTrieGraphServiceNodeMethodEnum))]
        public partial interface IStaticTrieGraphServiceNode { }
        /// <summary>
        /// 创建字符串 Trie 图节点的自定义基础服务接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IStaticTrieGraphServiceNodeMethodEnum
        {
            /// <summary>
            /// [0] 创建数组节点 ArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int length 数组长度
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateArrayNode = 0,
            /// <summary>
            /// [1] 创建位图节点 BitmapNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// uint capacity 二进制位数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateBitmapNode = 1,
            /// <summary>
            /// [2] 创建字典节点 ByteArrayDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayDictionaryNode = 2,
            /// <summary>
            /// [3] 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayFragmentDictionaryNode = 3,
            /// <summary>
            /// [4] 创建队列节点（先进先出） ByteArrayQueueNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayQueueNode = 4,
            /// <summary>
            /// [5] 创建栈节点（后进先出） ByteArrayStackNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayStackNode = 5,
            /// <summary>
            /// [6] 创建字典节点 DictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDictionaryNode = 6,
            /// <summary>
            /// [7] 创建分布式锁节点 DistributedLockNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDistributedLockNode = 7,
            /// <summary>
            /// [8] 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentDictionaryNode = 8,
            /// <summary>
            /// [9] 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashSetNode = 9,
            /// <summary>
            /// [10] 创建字典节点 HashBytesDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesDictionaryNode = 10,
            /// <summary>
            /// [11] 创建字典节点 HashBytesFragmentDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesFragmentDictionaryNode = 11,
            /// <summary>
            /// [12] 创建哈希表节点 HashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashSetNode = 12,
            /// <summary>
            /// [13] 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// long identity 起始分配 ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateIdentityGeneratorNode = 13,
            /// <summary>
            /// [14] 创建数组节点 LeftArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateLeftArrayNode = 14,
            /// <summary>
            /// [15] 创建消息处理节点 MessageNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType messageType 消息数据类型
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateMessageNode = 15,
            /// <summary>
            /// [16] 创建队列节点（先进先出） QueueNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateQueueNode = 16,
            /// <summary>
            /// [17] 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeDictionaryNode = 17,
            /// <summary>
            /// [18] 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeSetNode = 18,
            /// <summary>
            /// [19] 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerByteArrayMessageNode = 19,
            /// <summary>
            /// [20] 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedDictionaryNode = 20,
            /// <summary>
            /// [21] 创建排序列表节点 SortedListNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedListNode = 21,
            /// <summary>
            /// [22] 创建排序集合节点 SortedSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedSetNode = 22,
            /// <summary>
            /// [23] 创建栈节点（后进先出） StackNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateStackNode = 23,
            /// <summary>
            /// [24] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 24,
            /// <summary>
            /// [25] 创建服务注册节点 IServerRegistryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int loadTimeoutSeconds 冷启动会话超时秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerRegistryNode = 25,
            /// <summary>
            /// [26] 创建服务进程守护节点 IProcessGuardNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateProcessGuardNode = 26,
            /// <summary>
            /// [27] 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapClientFilterNode = 27,
            /// <summary>
            /// [28] 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapFilterNode = 28,
            /// <summary>
            /// [29] 删除节点
            /// string key 节点全局关键字
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNodeByKey = 29,
            /// <summary>
            /// [256] 创建字符串 Trie 图节点 IStaticTrieGraphNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// byte maxTrieWordSize Trie 词语最大文字长度
            /// byte maxWordSize 未知词语最大文字长度
            /// AutoCSer.CommandService.Search.StaticTrieGraph.WordSegmentFlags wordSegmentFlags 分词选项
            /// string replaceChars 替换文字集合
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateStaticTrieGraphNode = 256,
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
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
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
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
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
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            CompletedLoadPersistence = 9,
            /// <summary>
            /// [10] 删除分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            DeletedLoadPersistence = 10,
            /// <summary>
            /// [11] 初始化数据加载完成
            /// </summary>
            Loaded = 11,
            /// <summary>
            /// [12] 快照设置数据
            /// bool value 数据
            /// </summary>
            SnapshotSetLoaded = 12,
            /// <summary>
            /// [13] 创建分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// string text 分词文本数据
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            LoadCreate = 13,
            /// <summary>
            /// [14] 创建分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// string text 分词文本数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum} 
            /// </summary>
            LoadCreateBeforePersistence = 14,
            /// <summary>
            /// [15] 创建分词结果磁盘块索引信息
            /// T key 分词数据关键字
            /// string text 分词文本数据
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            LoadCreateLoadPersistence = 15,
        }
}namespace AutoCSer.CommandService.Search.MemoryIndex
{
        /// <summary>
        /// 哈希索引节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashCodeKeyIndexNodeMethodEnum))]
        public partial interface IHashCodeKeyIndexNode<T> { }
        /// <summary>
        /// 哈希索引节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashCodeKeyIndexNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加匹配数据关键字
            /// T[] keys 索引关键字集合
            /// uint value 匹配数据关键字
            /// </summary>
            AppendArray = 1,
            /// <summary>
            /// [2] 添加匹配数据关键字 持久化前检查
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 
            /// </summary>
            AppendBeforePersistence = 2,
            /// <summary>
            /// [3] 添加匹配数据关键字
            /// AutoCSer.LeftArray{T} keys 索引关键字集合
            /// uint value 匹配数据关键字
            /// </summary>
            AppendLeftArray = 3,
            /// <summary>
            /// [4] 删除匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null 或者没有找到索引关键字
            /// </summary>
            Remove = 4,
            /// <summary>
            /// [5] 删除匹配数据关键字
            /// T[] keys 索引关键字
            /// uint value 匹配数据关键字
            /// </summary>
            RemoveArray = 5,
            /// <summary>
            /// [6] 删除匹配数据关键字
            /// T key 索引关键字
            /// uint value 匹配数据关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 
            /// </summary>
            RemoveBeforePersistence = 6,
            /// <summary>
            /// [7] 删除匹配数据关键字
            /// AutoCSer.LeftArray{T} keys 索引关键字
            /// uint value 匹配数据关键字
            /// </summary>
            RemoveLeftArray = 7,
            /// <summary>
            /// [8] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{T,uint[]} value 数据
            /// </summary>
            SnapshotSet = 8,
        }
}namespace AutoCSer.CommandService.Search.MemoryIndex
{
        /// <summary>
        /// 哈希索引节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashIndexNodeMethodEnum))]
        public partial interface IHashIndexNode<KT,VT> { }
        /// <summary>
        /// 哈希索引节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashIndexNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加匹配数据关键字
            /// KT[] keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendArray = 1,
            /// <summary>
            /// [2] 添加匹配数据关键字 持久化前检查
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 
            /// </summary>
            AppendBeforePersistence = 2,
            /// <summary>
            /// [3] 添加匹配数据关键字
            /// AutoCSer.LeftArray{KT} keys 索引关键字集合
            /// VT value 匹配数据关键字
            /// </summary>
            AppendLeftArray = 3,
            /// <summary>
            /// [4] 删除匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 bool 返回 false 表示关键字数据为 null 或者没有找到索引关键字
            /// </summary>
            Remove = 4,
            /// <summary>
            /// [5] 删除匹配数据关键字
            /// KT[] keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveArray = 5,
            /// <summary>
            /// [6] 删除匹配数据关键字
            /// KT key 索引关键字
            /// VT value 匹配数据关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 
            /// </summary>
            RemoveBeforePersistence = 6,
            /// <summary>
            /// [7] 删除匹配数据关键字
            /// AutoCSer.LeftArray{KT} keys 索引关键字
            /// VT value 匹配数据关键字
            /// </summary>
            RemoveLeftArray = 7,
            /// <summary>
            /// [8] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{KT,VT[]} value 数据
            /// </summary>
            SnapshotSet = 8,
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
            /// <summary>
            /// [9] 是否已经建图
            /// 返回值 bool 
            /// </summary>
            IsGraph = 9,
            /// <summary>
            /// [10] 获取 Trie 图词语数量
            /// 返回值 int Trie 图词语数量
            /// </summary>
            GetWordCount = 10,
        }
}namespace AutoCSer.CommandService.Search.WordIdentityBlockIndex
{
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ILocalNodeMethodEnum))]
        public partial interface ILocalNode<T> { }
        /// <summary>
        /// 分词结果磁盘块索引信息节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ILocalNodeMethodEnum
        {
            /// <summary>
            /// [0] 
            /// T key 
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Completed = 0,
            /// <summary>
            /// [1] 
            /// T key 
            /// AutoCSer.CommandService.DiskBlock.BlockIndex blockIndex 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            CompletedLoadPersistence = 1,
            /// <summary>
            /// [2] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Create = 2,
            /// <summary>
            /// [3] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            CreateLoadPersistence = 3,
            /// <summary>
            /// [4] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Delete = 4,
            /// <summary>
            /// [5] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            DeleteLoadPersistence = 5,
            /// <summary>
            /// [6] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Deleted = 6,
            /// <summary>
            /// [7] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            DeletedLoadPersistence = 7,
            /// <summary>
            /// [8] 
            /// T key 
            /// string text 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            LoadCreate = 8,
            /// <summary>
            /// [9] 
            /// T key 
            /// string text 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum} 
            /// </summary>
            LoadCreateBeforePersistence = 9,
            /// <summary>
            /// [10] 
            /// T key 
            /// string text 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            LoadCreateLoadPersistence = 10,
            /// <summary>
            /// [11] 
            /// </summary>
            Loaded = 11,
            /// <summary>
            /// [12] 
            /// AutoCSer.BinarySerializeKeyValue{T,AutoCSer.CommandService.DiskBlock.BlockIndex} value 
            /// </summary>
            SnapshotSet = 12,
            /// <summary>
            /// [13] 
            /// bool value 
            /// </summary>
            SnapshotSetLoaded = 13,
            /// <summary>
            /// [14] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            Update = 14,
            /// <summary>
            /// [15] 
            /// T key 
            /// 返回值 AutoCSer.CommandService.Search.WordIdentityBlockIndexUpdateStateEnum 
            /// </summary>
            UpdateLoadPersistence = 15,
        }
}
#endif