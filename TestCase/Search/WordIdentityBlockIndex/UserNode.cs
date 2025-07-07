using AutoCSer.CommandService;
using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 用户信息分词结果磁盘块索引信息节点
    /// </summary>
    internal abstract class UserNode : WordIdentityBlockIndexNode<int>
    {
        /// <summary>
        /// 数据关键字磁盘块索引信息客户端
        /// </summary>
        private readonly DiskBlockCommandClientSocketEvent diskBlockClient;
        /// <summary>
        /// 带移除标记的可重用哈希索引节点
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> diskBlockIndexNode;
        /// <summary>
        /// 用户信息分词结果磁盘块索引信息节点
        /// </summary>
        /// <param name="loadClientNode"></param>
        /// <param name="diskBlockIndexNode"></param>
        internal UserNode(StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> loadClientNode, StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> diskBlockIndexNode)
            : base(TrieGraphCommandClientSocketEvent.StaticTrieGraphNodeCache, loadClientNode)
        {
            this.diskBlockIndexNode = diskBlockIndexNode;
            diskBlockClient = DiskBlockCommandClientSocketEvent.CommandClient.SocketEvent;
            DataSourceCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().NotWait();
            DiskBlockCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().NotWait();
        }
        /// <summary>
        /// 根据分词数据关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <returns></returns>
        public override IDiskBlockClient GetDiskBlockClient(int key)
        {
            return diskBlockClient.DiskBlockClient;
        }
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
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
        public override async Task<ResponseResult> AppendIndex(int[] wordIdentitys, int key)
        {
            ResponseResult<IRemoveMarkHashKeyIndexNodeClientNode<int>> node = await diskBlockIndexNode.GetSynchronousNode();
            if (!node.IsSuccess) return node;
            if (wordIdentitys.Length != 1) return await node.Value.AppendArray(wordIdentitys, (uint)key);
            return await node.Value.Append(wordIdentitys[0], (uint)key);
        }
        /// <summary>
        /// 更新分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="historyWordIdentitys">更新之前的词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public override async Task<ResponseResult> AppendIndex(int[] wordIdentitys, int[] historyWordIdentitys, int key)
        {
            ResponseResult<IRemoveMarkHashKeyIndexNodeClientNode<int>> node = await diskBlockIndexNode.GetSynchronousNode();
            if (!node.IsSuccess) return node;
            if (historyWordIdentitys.Length == 0)
            {
                if(wordIdentitys.Length != 1) return await node.Value.AppendArray(wordIdentitys, (uint)key);
                return await node.Value.Append(wordIdentitys[0], (uint)key);
            }
            if (wordIdentitys.Length == 0)
            {
                if (historyWordIdentitys.Length != 1) return await node.Value.RemoveArray(historyWordIdentitys, (uint)key);
                return await node.Value.Remove(historyWordIdentitys[0], (uint)key);
            }
            LeftArray<int> removeWordIdentitys, newWordIdentitys = StaticTrieGraphClient.GetWordIdentitys(wordIdentitys, historyWordIdentitys, out removeWordIdentitys);
            switch (removeWordIdentitys.Count)
            {
                case 0: break;
                case 1:
                    ResponseResult result = await node.Value.Remove(removeWordIdentitys[0], (uint)key);
                    if (!result.IsSuccess) return result;
                    break;
                default:
                    result = await node.Value.RemoveLeftArray(removeWordIdentitys, (uint)key);
                    if (!result.IsSuccess) return result;
                    break;
            }
            switch (newWordIdentitys.Count)
            {
                case 0: break;
                case 1:
                    ResponseResult result = await node.Value.Append(newWordIdentitys[0], (uint)key);
                    if (!result.IsSuccess) return result;
                    break;
                default:
                    result = await node.Value.AppendLeftArray(newWordIdentitys, (uint)key);
                    if (!result.IsSuccess) return result;
                    break;
            }
            return CallStateEnum.Success;
        }
        /// <summary>
        /// 删除分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public override async Task<ResponseResult> RemoveIndex(int[] wordIdentitys, int key)
        {
            ResponseResult<IRemoveMarkHashKeyIndexNodeClientNode<int>> node = await diskBlockIndexNode.GetSynchronousNode();
            if (!node.IsSuccess) return node;
            if (wordIdentitys.Length != 1) return await node.Value.RemoveArray(wordIdentitys, (uint)key);
            return await node.Value.Remove(wordIdentitys[0], (uint)key);
        }
    }
}
