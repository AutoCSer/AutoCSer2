using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.TestCase.SearchCommon;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
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
        /// 用户搜索非索引条件数据缓存
        /// </summary>
        private readonly SearchUserCache userCache;
        /// <summary>
        /// 用户名称分词索引缓存
        /// </summary>
        private readonly SingleDiskBlockUIntKeyIntValueCache userNameIndexCache;
        /// <summary>
        /// 用户备注分词索引缓存
        /// </summary>
        private readonly SingleDiskBlockUIntKeyIntValueCache userRemarkIndexCache;
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
            userCache = new SearchUserCache(this, 1 << 16);
            NullBufferTask = Task.FromResult(IntBufferPool.GetNull());
            DiskBlockCommandClientSocketEvent diskBlockClient = DiskBlockCommandClientSocketEvent.CommandClient.SocketEvent;
            userNameIndexCache = new SingleDiskBlockUIntKeyIntValueCache(diskBlockClient, DiskBlockIndexCommandClientSocketEvent.UserNameDiskBlockIndexNodeCache, 1 << 26);
            userRemarkIndexCache = new SingleDiskBlockUIntKeyIntValueCache(diskBlockClient, DiskBlockIndexCommandClientSocketEvent.UserRemarkDiskBlockIndexNodeCache, 1 << 26);
            DiskBlockCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().NotWait();
            TryAppendTaskArrayAsync(60 * 60).NotWait();
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
            userCache.Close();
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
        /// 获取用户标识分页记录
        /// </summary>
        /// <param name="queryParameter">用户搜索查询参数</param>
        /// <returns></returns>
        public async Task<PageResult<int>> GetUserPage(UserQueryParameter queryParameter)
        {
            LeftArray<IIndexCondition<int>> conditions = new LeftArray<IIndexCondition<int>>(0);
            if (!string.IsNullOrEmpty(queryParameter.Name))
            {
                ResponseResult<IIndexCondition<int>> condition = await userNameIndexCache.GetCondition(TrieGraphCommandClientSocketEvent.StaticTrieGraphNodeCache, queryParameter.Name);
                if(!condition.IsSuccess) return condition.GetPageResult<int>();
                if (condition.Value == null) return queryParameter.GetPageResult<int>();
                conditions.Add(condition.Value);
            }
            if (!string.IsNullOrEmpty(queryParameter.Remark))
            {
                ResponseResult<IIndexCondition<int>> condition = await userRemarkIndexCache.GetCondition(TrieGraphCommandClientSocketEvent.StaticTrieGraphNodeCache, queryParameter.Remark);
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
                                users = await userCache.Get(userIds);
                                userIds.Free();
                                return queryParameter.GetPageResult(users.GetLeftArray(), SearchUser.LoginTimeDesc, SearchUser.GetId);
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
            ResponseResult<ISearchUserNodeClientNode> searchUserNode = await WordIdentityBlockIndexCommandClientSocketEvent.SearchUserNodeCache.GetSynchronousNode();
            if (!searchUserNode.IsSuccess) return searchUserNode.GetPageResult<int>();
            ResponseResult<PageResult<int>> pageResult = await searchUserNode.Value.GetPage(queryParameter);
            if (pageResult.IsSuccess) return pageResult.Value; 
            return pageResult.GetPageResult<int>();
        }
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys">关键字集合</param>
        /// <param name="isValue">条件委托</param>
        /// <returns></returns>
        Task<ArrayBuffer<int>> IQueryContext<int, SearchUser>.Filter(ArrayBuffer<int> keys, Func<SearchUser, bool> isValue)
        {
            return userCache.Get(keys, isValue);
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
