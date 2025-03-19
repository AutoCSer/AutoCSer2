using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 搜索聚合查询服务
    /// </summary>
    internal sealed class QueryService : IQueryService
    {
        /// <summary>
        /// 搜索聚合查询服务
        /// </summary>
        internal QueryService() { }
        /// <summary>
        /// 用户数据更新消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> UserMessage(OperationData<int> data)
        {
            if ((data.DataType & OperationDataTypeEnum.SearchUserNode) != 0)
            {
                LocalResult<ISearchUserNodeLocalClientNode> node = await MemorySearchUserServiceConfig.SearchUserNodeCache.GetSynchronousNode();
                if (!node.IsSuccess) return false;
                LocalResult<ConditionDataUpdateStateEnum> userResult;
                switch (data.OperationType)
                {
                    case OperationTypeEnum.Insert: userResult = await node.Value.Create(data.Key); break;
                    case OperationTypeEnum.Delete: userResult = await node.Value.Delete(data.Key); break;
                    default: userResult = await node.Value.Update(data.Key); break;
                }
                if (!userResult.IsSuccess || userResult.Value != ConditionDataUpdateStateEnum.Success) return false;
            }
            if ((data.DataType & OperationDataTypeEnum.UserNameNode) != 0)
            {
                if (!await wordIdentity(data, MemoryWordIdentityBlockIndexServiceConfig.UserNameNodeCache)) return false;
            }
            if ((data.DataType & OperationDataTypeEnum.UserRemarkNode) != 0)
            {
                if (!await wordIdentity(data, MemoryWordIdentityBlockIndexServiceConfig.UserRemarkNodeCache)) return false;
            }
            return true;
        }
        /// <summary>
        /// 分词数据更新
        /// </summary>
        /// <param name="data"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task<bool> wordIdentity(OperationData<int> data, StreamPersistenceMemoryDatabaseLocalClientNodeCache<AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNodeLocalClientNode<int>> node)
        {
            LocalResult<AutoCSer.CommandService.Search.WordIdentityBlockIndex.ILocalNodeLocalClientNode<int>> nodeResult = await node.GetSynchronousNode();
            if (!nodeResult.IsSuccess) return false;
            LocalResult<WordIdentityBlockIndexUpdateStateEnum> result;
            switch (data.OperationType)
            {
                case OperationTypeEnum.Insert: result = await nodeResult.Value.Create(data.Key); break;
                case OperationTypeEnum.Delete: result = await nodeResult.Value.Delete(data.Key); break;
                default: result = await nodeResult.Value.Update(data.Key); break;
            }
            if (!result.IsSuccess) return false;
            switch (result.Value)
            {
                case WordIdentityBlockIndexUpdateStateEnum.Success:
                case WordIdentityBlockIndexUpdateStateEnum.DeletedNotFoundKey:
                case WordIdentityBlockIndexUpdateStateEnum.NotSupportDeleteKey:
                    return true;
                default: return false;
            }
        }
        /// <summary>
        /// 获取用户标识分页记录
        /// </summary>
        /// <param name="queryParameter">用户搜索查询参数</param>
        /// <returns></returns>
        public async Task<PageResult<int>> GetUserPage(UserQueryParameter queryParameter)
        {
            int[] userNameWordIdentitys = EmptyArray<int>.Array, userRemarkWordIdentitys = EmptyArray<int>.Array;
            if (!string.IsNullOrEmpty(queryParameter.Name))
            {
                LocalResult<IStaticTrieGraphNodeLocalClientNode> trieGraphNode = await MemoryTrieGraphServiceConfig.StaticTrieGraphNodeCache.GetNode();
                if (!trieGraphNode.IsSuccess) return trieGraphNode.GetPageResult<int>();
                LocalResult<int[]> wordIdentitysResult = await trieGraphNode.Value.notNull().GetWordSegmentIdentity(queryParameter.Name);
                if (!wordIdentitysResult.IsSuccess) return wordIdentitysResult.GetPageResult<int>();
                if (wordIdentitysResult.Value.Length == 0) return queryParameter.GetPageResult<int>();
                userNameWordIdentitys = wordIdentitysResult.Value;
            }
            if (!string.IsNullOrEmpty(queryParameter.Remark))
            {
                LocalResult<IStaticTrieGraphNodeLocalClientNode> trieGraphNode = await MemoryTrieGraphServiceConfig.StaticTrieGraphNodeCache.GetNode();
                if (!trieGraphNode.IsSuccess) return trieGraphNode.GetPageResult<int>();
                LocalResult<int[]> wordIdentitysResult = await trieGraphNode.Value.notNull().GetWordSegmentIdentity(queryParameter.Remark);
                if (!wordIdentitysResult.IsSuccess) return wordIdentitysResult.GetPageResult<int>();
                if (wordIdentitysResult.Value.Length == 0) return queryParameter.GetPageResult<int>();
                userRemarkWordIdentitys = wordIdentitysResult.Value;
            }
            LocalResult<ISearchUserNodeLocalClientNode> searchUserNode = await MemorySearchUserServiceConfig.SearchUserNodeCache.GetSynchronousNode();
            if (!searchUserNode.IsSuccess) return searchUserNode.GetPageResult<int>();
            LocalResult<PageResult<int>> pageResult = await searchUserNode.Value.GetPage(queryParameter, userNameWordIdentitys, userRemarkWordIdentitys);
            if (pageResult.IsSuccess) return pageResult.Value;
            return pageResult.GetPageResult<int>();
        }
    }
}
