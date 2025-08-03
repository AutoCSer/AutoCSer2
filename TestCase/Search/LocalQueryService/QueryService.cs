using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
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
    internal sealed class QueryService : AutoCSer.Threading.SecondTimerTaskArrayNode, IQueryService, IQueryContext<int, SearchUser>, IDisposable
    {
        /// <summary>
        /// 关键字数组缓冲区池
        /// </summary>
        internal readonly ArrayBufferPoolArray<int> IntBufferPool;
        /// <summary>
        /// 用户搜索非索引条件数据数组缓冲区池
        /// </summary>
        internal readonly ArrayBufferPoolArray<SearchUser> UserBufferPool;
        /// <summary>
        /// 空数组缓冲区
        /// </summary>
        public Task<ArrayBuffer<int>> NullBufferTask { get; private set; }
        /// <summary>
        /// 关键字可重用哈希表缓冲区池
        /// </summary>
        public HashSetPool<int>[] HashSetPool { get; private set; }
        /// <summary>
        /// 用户名称分词索引缓存
        /// </summary>
        private readonly SingleDiskBlockUIntKeyIntValueLocalCache userNameIndexCache;
        /// <summary>
        /// 用户备注分词索引缓存
        /// </summary>
        private readonly SingleDiskBlockUIntKeyIntValueLocalCache userRemarkIndexCache;
        /// <summary>
        /// Whether resources have been released
        /// 是否已释放资源
        /// </summary>
        internal bool IsDispose;
        /// <summary>
        /// 搜索聚合查询服务
        /// </summary>
        internal QueryService() : base(AutoCSer.Threading.SecondTimer.TaskArray, SecondTimerTaskThreadModeEnum.Synchronous, SecondTimerKeepModeEnum.After)
        {
            IntBufferPool = new ArrayBufferPoolArray<int>(8);
            UserBufferPool = new ArrayBufferPoolArray<SearchUser>(8);
            HashSetPool = HashSetPool<int>.GetArray(256);
            NullBufferTask = Task.FromResult(IntBufferPool.GetNull());
            DiskBlockCommandClientSocketEvent diskBlockClient = DiskBlockCommandClientSocketEvent.CommandClient.SocketEvent;
            userNameIndexCache = new SingleDiskBlockUIntKeyIntValueLocalCache(diskBlockClient, LocalDiskBlockIndexServiceConfig.UserNameNodeCache, 1 << 26);
            userRemarkIndexCache = new SingleDiskBlockUIntKeyIntValueLocalCache(diskBlockClient, LocalDiskBlockIndexServiceConfig.UserRemarkNodeCache, 1 << 26);
            DiskBlockCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().AutoCSerNotWait();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            IsDispose = true;
            KeepSeconds = 0;
            userNameIndexCache.Dispose();
            userRemarkIndexCache.Dispose();
        }
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected override void OnTimer()
        {
            IntBufferPool.FreeCache();
            UserBufferPool.FreeCache();
            HashSetPool<int>.FreeCache(HashSetPool);
        }
        /// <summary>
        /// 用户数据更新消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> UserMessage(OperationData<int> data)
        {
            if ((data.DataType & OperationDataTypeEnum.SearchUserNode) != 0)
            {
                LocalResult<ISearchUserNodeLocalClientNode> node = await LocalSearchUserServiceConfig.SearchUserNodeCache.GetSynchronousNode();
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
                if (!await wordIdentity(data, LocalWordIdentityBlockIndexServiceConfig.UserNameNodeCache)) return false;
            }
            if ((data.DataType & OperationDataTypeEnum.UserRemarkNode) != 0)
            {
                if (!await wordIdentity(data, LocalWordIdentityBlockIndexServiceConfig.UserRemarkNodeCache)) return false;
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
            LeftArray<IIndexCondition<int>> conditions = new LeftArray<IIndexCondition<int>>(0);
            if (!string.IsNullOrEmpty(queryParameter.Name))
            {
                ResponseResult<IIndexCondition<int>> condition = await userNameIndexCache.GetCondition(LocalTrieGraphServiceConfig.StaticTrieGraphNodeCache, queryParameter.Name);
                if(!condition.IsSuccess) return condition.GetPageResult<int>();
                if (condition.Value == null) return queryParameter.GetPageResult<int>();
                conditions.Add(condition.Value);
            }
            if (!string.IsNullOrEmpty(queryParameter.Remark))
            {
                ResponseResult<IIndexCondition<int>> condition = await userRemarkIndexCache.GetCondition(LocalTrieGraphServiceConfig.StaticTrieGraphNodeCache, queryParameter.Remark);
                if (!condition.IsSuccess) return condition.GetPageResult<int>();
                if (condition.Value == null) return queryParameter.GetPageResult<int>();
                conditions.Add(condition.Value);
            }
            if (conditions.Count != 0)
            {
                ArrayBuffer<SearchUser> users = UserBufferPool.GetNull();
                ArrayBuffer<int> userIds = await new QueryCondition<int, SearchUser>(this, conditions, queryParameter.SearchCondition).Query(queryParameter);
                try
                {
                    switch (queryParameter.Order)
                    {
                        case UserOrderEnum.LoginTimeDesc:
                            if (queryParameter.GetPageSize(userIds.Count) > 0)
                            {
                                LocalResult<ISearchUserNodeLocalClientNode> node = await LocalSearchUserServiceConfig.SearchUserNodeCache.GetSynchronousNode();
                                if (!node.IsSuccess) return node.GetPageResult<int>();
                                LocalResult<LeftArray<SearchUser>> userArray = await node.Value.GetArray(userIds.GetLeftArray(), users = UserBufferPool.GetBuffer(userIds.Count));
                                if (!userArray.IsSuccess) return userArray.GetPageResult<int>();
                                userIds.Free();
                                if (userArray.Value.Count != 0) return queryParameter.GetPageResult(userArray.Value, SearchUser.LoginTimeDesc, SearchUser.GetId);
                                return new PageResult<int>(Net.CommandClientReturnTypeEnum.ServerException);
                            }
                            return new PageResult<int>(EmptyArray<int>.Array, userIds.Count, queryParameter.PageIndex, queryParameter.PageSize);
                        default: return queryParameter.GetDescPageResult(userIds.GetLeftArray());
                    }
                }
                finally
                {
                    userIds.Free();
                    users.Free();
                }
            }
            LocalResult<ISearchUserNodeLocalClientNode> searchUserNode = await LocalSearchUserServiceConfig.SearchUserNodeCache.GetSynchronousNode();
            if (!searchUserNode.IsSuccess) return searchUserNode.GetPageResult<int>();
            LocalResult<PageResult<int>> pageResult = await searchUserNode.Value.GetPage(queryParameter);
            if (pageResult.IsSuccess) return pageResult.Value; 
            return pageResult.GetPageResult<int>();
        }
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys">关键字集合</param>
        /// <param name="isValue">条件委托</param>
        /// <returns></returns>
        async Task<ArrayBuffer<int>> IQueryContext<int, SearchUser>.Filter(ArrayBuffer<int> keys, Func<SearchUser, bool> isValue)
        {
            LocalResult<ISearchUserNodeLocalClientNode> node = await LocalSearchUserServiceConfig.SearchUserNodeCache.GetSynchronousNode();
            if (node.IsSuccess)
            {
                LocalResult<ArrayBuffer<int>> userIds = await node.Value.Filter(keys, isValue);
                if (userIds.IsSuccess && userIds.Value.Count > 0) return userIds.Value;
            }
            keys.Free();
            return IntBufferPool.GetNull();
        }
        /// <summary>
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="size">数组最小容量</param>
        /// <returns></returns>
        ArrayBuffer<int> IQueryContext<int, SearchUser>.GetBuffer(int size)
        {
            return IntBufferPool.GetBuffer(size);
        }
    }
}
