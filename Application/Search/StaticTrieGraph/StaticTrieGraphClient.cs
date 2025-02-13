using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 字符串 Trie 图节点客户端
    /// </summary>
    public sealed class StaticTrieGraphClient
    {
        /// <summary>
        /// 字符串 Trie 图节点客户端
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IStaticTrieGraphNodeClientNode> NodeCache;
        /// <summary>
        /// 字符串 Trie 图节点客户端
        /// </summary>
        /// <param name="nodeCache"></param>
        public StaticTrieGraphClient(StreamPersistenceMemoryDatabaseClientNodeCache<IStaticTrieGraphNodeClientNode> nodeCache)
        {
            this.NodeCache = nodeCache;
        }
        /// <summary>
        /// 批量添加 Trie 图词语
        /// </summary>
        /// <param name="words"></param>
        /// <param name="concurrency">并发数量</param>
        /// <returns></returns>
        public async Task<ResponseResult<AppendWordStateEnum>> AppendWord(IEnumerable<string> words, int concurrency = 1 << 13)
        {
            if (words == null) return AppendWordStateEnum.Success;
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await NodeCache.GetSynchronousNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<AppendWordStateEnum>();
            IStaticTrieGraphNodeClientNode node = nodeResult.Value.notNull();
            if (concurrency > 1)
            {
                foreach (string word in words)
                {
                    ResponseResult<AppendWordStateEnum> result = await node.AppendWord(word);
                    if (!result.IsSuccess || result.Value != AppendWordStateEnum.Success) return result;
                }
            }
            else
            {
                int index = 0;
#if NetStandard21
                ResponseParameterAwaiter<AppendWordStateEnum>?[] awaiters = new ResponseParameterAwaiter<AppendWordStateEnum>?[concurrency];
#else
                ResponseParameterAwaiter<AppendWordStateEnum>[] awaiters = new ResponseParameterAwaiter<AppendWordStateEnum>[concurrency];
#endif
                foreach (string word in words)
                {
                    var awaiter = awaiters[index];
                    if (awaiter != null)
                    {
                        ResponseResult<AppendWordStateEnum> result = await awaiter;
                        if (!result.IsSuccess || result.Value != AppendWordStateEnum.Success) return result;
                    }
                    awaiters[index] = node.AppendWord(word);
                    if (++index == concurrency) index = 0;
                }
                foreach (var awaiter in awaiters)
                {
                    if (awaiter != null)
                    {
                        ResponseResult<AppendWordStateEnum> result = await awaiter;
                        if (!result.IsSuccess || result.Value != AppendWordStateEnum.Success) return result;
                    }
                    else break;
                }
            }
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return AppendWordStateEnum.Success;
        }
        /// <summary>
        /// 批量添加 Trie 图词语
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AppendWordStateEnum>> AppendWord(string word)
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<AppendWordStateEnum>();
            return await nodeResult.Value.notNull().AppendWord(word);
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <returns>Trie 图词语数量</returns>
        public async Task<ResponseResult<int>> BuildGraph()
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<int>();
            return await nodeResult.Value.notNull().BuildGraph();
        }
        /// <summary>
        /// 添加文本并返回词语编号集合
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<ResponseResult<int[]>> GetAddTextIdentity(string text)
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<int[]>();
            return await nodeResult.Value.notNull().GetAddTextIdentity(text);
        }
        /// <summary>
        /// 获取查询词语编号集合（忽略未匹配词语）
        /// </summary>
        /// <param name="text">搜索文本内容</param>
        /// <returns></returns>
        public async Task<ResponseResult<int[]>> GetWordSegmentIdentity(string text)
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<int[]>();
            return await nodeResult.Value.notNull().GetWordSegmentIdentity(text);
        }
        /// <summary>
        /// 获取查询分词结果
        /// </summary>
        /// <param name="text">搜索文本内容</param>
        /// <returns></returns>
        public async Task<ResponseResult<WordSegmentResult[]>> GetWordSegmentResult(string text)
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> nodeResult = await NodeCache.GetNode();
            if (!nodeResult.IsSuccess) return nodeResult.Cast<WordSegmentResult[]>();
            return await nodeResult.Value.notNull().GetWordSegmentResult(text);
        }
    }
}
