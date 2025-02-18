using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.WordIdentityBlockIndex
{
    /// <summary>
    /// 分词结果磁盘块索引信息本地节点
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    public abstract class LocalNode<T> : WordIdentityBlockIndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 字符串 Trie 图客户端节点
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseLocalClientNodeCache<IStaticTrieGraphNodeLocalClientNode> trieGraphNode;
        /// <summary>
        /// 分词结果磁盘块索引信息客户端
        /// </summary>
        private readonly IDiskBlockClientSocketEvent diskBlockClient;
        /// <summary>
        /// 词语匹配数据关键字索引节点
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseLocalClientNodeCache<IIndexNodeLocalClientNode<int, T>> indexNode;
        /// <summary>
        /// 分词结果磁盘块索引信息本地节点
        /// </summary>
        /// <param name="trieGraphNode">字符串 Trie 图客户端节点</param>
        /// <param name="diskBlockClient">分词结果磁盘块索引信息客户端</param>
        /// <param name="indexNode">词语匹配数据关键字索引节点</param>
        protected LocalNode(StreamPersistenceMemoryDatabaseLocalClientNodeCache<IStaticTrieGraphNodeLocalClientNode> trieGraphNode, IDiskBlockClientSocketEvent diskBlockClient, StreamPersistenceMemoryDatabaseLocalClientNodeCache<IIndexNodeLocalClientNode<int, T>> indexNode)
        {
            this.trieGraphNode = trieGraphNode;
            this.diskBlockClient = diskBlockClient;
            this.indexNode = indexNode;
        }
        /// <summary>
        /// 获取分词词语编号标识集合
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <returns>词语编号标识集合</returns>
        public override async Task<ResponseResult<int[]>> GetWordIdentitys(string text)
        {
            LocalResult<IStaticTrieGraphNodeLocalClientNode> nodeResult = await trieGraphNode.GetSynchronousNode();
            if (!nodeResult.IsSuccess) return nodeResult.CallState;
            LocalResult<int[]> identitys = await nodeResult.Value.notNull().GetAddTextIdentity(text);
            if (!identitys.IsSuccess) return identitys.CallState;
            return identitys.Value;
        }
        /// <summary>
        /// 根据分词数据关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(T key)
        {
            return diskBlockClient.DiskBlockClient;
        }
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex)
        {
            return diskBlockClient.DiskBlockClient;
        }
        /// <summary>
        /// 添加分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public override async Task<ResponseResult> AppendIndex(int[] wordIdentitys, T key)
        {
            LocalResult<IIndexNodeLocalClientNode<int, T>> indexNodeResult = await indexNode.GetSynchronousNode();
            if (!indexNodeResult.IsSuccess) return new ResponseResult(indexNodeResult.CallState);
            LocalResult result = await indexNodeResult.Value.notNull().AppendArray(wordIdentitys, key);
            if (!result.IsSuccess) return new ResponseResult(result.CallState);
            return new ResponseResult(CallStateEnum.Success);
        }
        /// <summary>
        /// 更新分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="historyWordIdentitys">更新之前的词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public override async Task<ResponseResult> AppendIndex(int[] wordIdentitys, int[] historyWordIdentitys, T key)
        {
            LocalResult<IIndexNodeLocalClientNode<int, T>> indexNodeResult = await this.indexNode.GetSynchronousNode();
            if (!indexNodeResult.IsSuccess) return new ResponseResult(indexNodeResult.CallState);
            IIndexNodeLocalClientNode<int, T> indexNode = indexNodeResult.Value.notNull();
            if (historyWordIdentitys.Length != 0)
            {
                LocalResult result = await indexNode.RemoveArray(historyWordIdentitys, key);
                if (!result.IsSuccess) return new ResponseResult(result.CallState);
            }
            if (wordIdentitys.Length != 0)
            {
                LocalResult result = await indexNode.AppendArray(historyWordIdentitys, key);
                if (!result.IsSuccess) return new ResponseResult(result.CallState);
            }
            return new ResponseResult(CallStateEnum.Success);

            //int wordIdentityIndex = 0, isAppend;
            //foreach (int wordIdentity in wordIdentitys)
            //{
            //    isAppend = wordIdentityIndex - historyWordIdentitys.Length;
            //    while (isAppend != 0)
            //    {
            //        int historyWordIdentity = historyWordIdentitys[wordIdentityIndex];
            //        if (wordIdentity == historyWordIdentity)
            //        {
            //            ++wordIdentityIndex;
            //            break;
            //        }
            //        if (wordIdentity < historyWordIdentity) isAppend = 0;
            //        else
            //        {
            //            ResponseResult result = await indexNode.Remove(historyWordIdentity, key);
            //            if (!result.IsSuccess)
            //            {
            //                state = WordIdentityBlockIndexUpdateStateEnum.SetWordIndexFailed;
            //                return;
            //            }
            //            isAppend = ++wordIdentityIndex - historyWordIdentitys.Length;
            //        }
            //    }
            //    if (isAppend == 0)
            //    {
            //        ResponseResult result = await indexNode.Append(wordIdentity, key);
            //        if (!result.IsSuccess)
            //        {
            //            state = WordIdentityBlockIndexUpdateStateEnum.SetWordIndexFailed;
            //            return;
            //        }
            //    }
            //}
        }
    }
}
