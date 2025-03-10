using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Data;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchCommon;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.TestCase.SearchQueryService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 用户搜索非索引条件数据节点
    /// </summary>
    internal sealed class SearchUserNode : ConditionDataNode<ISearchUserNode, int, SearchUser, ISearchUserNodeClientNode>, ISearchUserNode, ISnapshot<bool>
    {
        /// <summary>
        /// 用户搜索非索引条件数据
        /// </summary>
        private readonly AutoCSer.SearchTree.Dictionary<int, SearchUser> users;
        /// <summary>
        /// 最后一次登录操作时间数据
        /// </summary>
        private readonly AutoCSer.SearchTree.Set<CompareKey<DateTime, int>> loginTimes;
        /// <summary>
        /// 并发队列
        /// </summary>
        internal readonly ConcurrencyQueue ConcurrencyQueue;
        /// <summary>
        /// 异步操作队列访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim queueLock;
        /// <summary>
        /// 初始化加载数据获取非索引条件查询数据节点单例
        /// </summary>
        protected override StreamPersistenceMemoryDatabaseClientNodeCache<ISearchUserNodeClientNode> loadClientNode { get { return CommandClientSocketEvent.SearchUserNodeCache; } }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        protected override int snapshotCapacity { get { return users.Count; } }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        protected override IEnumerable<SearchUser> snapshotValues { get { return users.Values; } }
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        internal SearchUserNode()
        {
            ConcurrencyQueue = new ConcurrencyQueue(AutoCSer.Common.ProcessorCount);
            users = new SearchTree.Dictionary<int, SearchUser>();
            loginTimes = new SearchTree.Set<CompareKey<DateTime, int>>();
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
            DataSourceCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().NotWait();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override ISearchUserNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override ISearchUserNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (SearchUser user in users.Values) loginTimes.Add(user.GetLoginTimeKey());
            return base.StreamPersistenceMemoryDatabaseServiceLoaded();
        }
        /// <summary>
        /// 获取初始化加载所有关键字数据命令
        /// </summary>
        /// <returns></returns>
        protected override EnumeratorCommand<int> getLoadCommand()
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetAllUserId();
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override ResponseParameterAwaiter<ConditionDataUpdateStateEnum> loadCreate(ISearchUserNodeClientNode client, int key)
        {
            return client.Create(key);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public override void SetSnapshotResult(ref LeftArray<SearchUser> array, ref LeftArray<SearchUser> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<bool>.GetSnapshotCapacity(ref object customObject)
        {
            return isLoaded ? 1 : 0;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public override void SnapshotSet(SearchUser value)
        {
            users.Set(value.Id, value);
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool ContainsKey(int key)
        {
            return users.ContainsKey(key);
        }
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        protected override void create(int key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                if (!users.ContainsKey(key))
                {
                    createTask(key, callback).NotWait();
                    state = ConditionDataUpdateStateEnum.Callbacked;
                }
                else state = ConditionDataUpdateStateEnum.Success;
            }
            finally
            {
                if (state != ConditionDataUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private async Task createTask(int key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                CommandClientReturnValue<SearchUser> user = await DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetSearchUser(key);
                if (user.IsSuccess)
                {
                    if (user.Value.Id != 0)
                    {
                        StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, user.Value, callback);
                        state = ConditionDataUpdateStateEnum.Callbacked;
                    }
                    else state = ConditionDataUpdateStateEnum.Success;
                }
                else state = ConditionDataUpdateStateEnum.GetDataFailed;
            }
            finally
            {
                queueLock.Release();
                if (state != ConditionDataUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        protected override void update(int key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                if (users.ContainsKey(key)) updateTask(key, callback).NotWait();
                else createTask(key, callback).NotWait();
                state = ConditionDataUpdateStateEnum.Callbacked;
            }
            finally
            {
                if (state != ConditionDataUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private async Task updateTask(int key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                CommandClientReturnValue<SearchUser> user = await DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetSearchUser(key);
                if (user.IsSuccess)
                {
                    if (user.Value.Id != 0) StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, user.Value, callback);
                    else StreamPersistenceMemoryDatabaseMethodParameterCreator.Delete(key, callback);
                    state = ConditionDataUpdateStateEnum.Callbacked;
                }
                else state = ConditionDataUpdateStateEnum.GetDataFailed;
            }
            finally
            {
                queueLock.Release();
                if (state != ConditionDataUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        protected override void deleteLoadPersistence(int key)
        {
            users.Remove(key);
        }
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <returns></returns>
        protected override ConditionDataUpdateStateEnum delete(int key)
        {
            SearchUser user;
            ConcurrencyQueue.WaitQueue();
            try
            {
                if (users.Remove(key, out user)) loginTimes.Remove(user.GetLoginTimeKey());
            }
            finally { ConcurrencyQueue.ReleaseQueue(); }
            return ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        protected override ValueResult<ConditionDataUpdateStateEnum> completedBeforePersistence(int key, SearchUser value)
        {
            SearchUser historyUser;
            if (users.TryGetValue(key, out historyUser) && historyUser.Equals(value)) return ConditionDataUpdateStateEnum.Success;
            return default;
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        protected override void completedLoadPersistence(int key, SearchUser value)
        {
            users.Set(key, value);
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        protected override ConditionDataUpdateStateEnum completed(int key, SearchUser value)
        {
            SearchUser historyUser;
            ConcurrencyQueue.WaitQueue();
            try
            {
                if (users.Set(key, value, out historyUser)) loginTimes.Add(value.GetLoginTimeKey());
                else if (value.LoginTime != historyUser.LoginTime)
                {
                    loginTimes.Add(value.GetLoginTimeKey());
                    loginTimes.Remove(historyUser.GetLoginTimeKey());
                }
            }
            finally { ConcurrencyQueue.ReleaseQueue(); }
            return ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <param name="callback"></param>
        public void GetPage(SearchUserQueryParameter queryParameter, MethodCallback<PageResult<int>> callback)
        {
            bool isCallback = false;
            try
            {
                if (queryParameter.IsSearchCondition) ConcurrencyQueue.Push(new SearchUserGetPageCallback(this, queryParameter, callback));
                else
                {
                    switch (queryParameter.Order)
                    {
                        case UserOrderEnum.LoginTimeDesc: callback.Callback(queryParameter.GetDescPageResult(loginTimes, getLoginTimeUserId)); break;
                        default: callback.Callback(queryParameter.GetDescKeyPageResult(users)); break;
                    }
                }
                isCallback = true;
            }
            finally
            {
                if (!isCallback) callback.Callback(new PageResult<int>(CommandClientReturnTypeEnum.ServerException));
            }
        }
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <returns></returns>
        internal PageResult<int> GetPage(SearchUserQueryParameter queryParameter)
        {
            PageArray<int> pageArray = queryParameter.GetPageArray<int>();
            switch (queryParameter.Order)
            {
                case UserOrderEnum.LoginTimeDesc:
                    SearchUser searchUser;
                    foreach (CompareKey<DateTime, int> loginTime in loginTimes.Values)
                    {
                        if (users.TryGetValue(loginTime.Value2, out searchUser) && queryParameter.SearchCondition(searchUser) && pageArray.Add()) pageArray.Add(searchUser.Id);
                    }
                    break;
                default:
                    foreach (SearchUser user in users.Values)
                    {
                        if (queryParameter.SearchCondition(user) && pageArray.Add()) pageArray.Add(user.Id);
                    }
                    break;
            }
            return pageArray.GetPageResult();
        }
        /// <summary>
        /// 获取非索引条件数据用户数组
        /// </summary>
        /// <param name="userIds">用户标识数组</param>
        /// <param name="callback"></param>
        public void GetArray(LeftArray<int> userIds, MethodCallback<LeftArray<SearchUser>> callback)
        {
            ConcurrencyQueue.Push(new SearchUserGetArrayCallback(this, userIds, callback));
        }
        /// <summary>
        /// 获取非索引条件数据用户数组
        /// </summary>
        /// <param name="userIds">用户标识数组</param>
        /// <returns></returns>
        internal LeftArray<SearchUser> GetArray(LeftArray<int> userIds)
        {
            SearchUser user;
            LeftArray<SearchUser> users = new LeftArray<SearchUser>(userIds.Count);
            foreach (int userId in userIds)
            {
                if (this.users.TryGetValue(userId, out user)) users.Add(user);
            }
            return users;
        }

        /// <summary>
        /// 获取用户标识
        /// </summary>
        /// <param name="loginTime"></param>
        /// <returns></returns>
        private static int getUserId(CompareKey<DateTime, int> loginTime)
        {
            return loginTime.Value2;
        }
        /// <summary>
        /// 获取用户标识
        /// </summary>
        private static readonly Func<CompareKey<DateTime, int>, int> getLoginTimeUserId = getUserId;
    }
}
