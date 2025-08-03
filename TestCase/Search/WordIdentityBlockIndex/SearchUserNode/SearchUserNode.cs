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
    internal sealed class SearchUserNode : ConditionDataNode<ISearchUserNode, int, SearchUser, ISearchUserNodeClientNode>, ISearchUserNode, IEnumerableSnapshot<bool>
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
        /// 异步操作队列访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim queueLock;
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        protected override int snapshotCapacity { get { return users.Count; } }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        protected override IEnumerable<SearchUser> snapshotValues { get { return users.Values; } }
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        internal SearchUserNode() : base(CommandClientSocketEvent.SearchUserNodeCache)
        {
            users = new SearchTree.Dictionary<int, SearchUser>();
            loginTimes = new SearchTree.Set<CompareKey<DateTime, int>>();
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
            DataSourceCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().AutoCSerNotWait();
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
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
        protected override EnumeratorCommand<SearchUser> getLoadCommand()
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient?.GetAllSearchUser();
        }
        /// <summary>
        /// 创建用户搜索非索引条件数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override ResponseResultAwaiter loadCreate(ISearchUserNodeClientNode client, SearchUser value)
        {
            return client.LoadCreate(value);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public override void SetSnapshotResult(ref LeftArray<SearchUser> array, ref LeftArray<SearchUser> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
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
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Contains(SearchUser value)
        {
            return users.ContainsKey(value.Id);
        }
        /// <summary>
        /// Create non-indexed conditional query data (Check the input parameters before the persistence operation)
        /// 创建非索引条件查询数据（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        public override bool LoadCreateBeforePersistence(SearchUser value)
        {
            return !users.ContainsKey(value.Id);
        }
        /// <summary>
        /// Create non-indexed conditional query data (Initialize and load the persistent data)
        /// 创建非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        public override void LoadCreateLoadPersistence(SearchUser value)
        {
            users.TryAdd(value.Id, value);
        }
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        public override void LoadCreate(SearchUser value)
        {
            if (users.TryAdd(value.Id, value)) loginTimes.Add(value.GetLoginTimeKey());
        }
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        protected override void create(int key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                if (!users.ContainsKey(key))
                {
                    createTask(key, callback).AutoCSerNotWait();
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
        /// Create non-indexed conditional query data
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
                        StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(user.Value, callback);
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
        /// Update non-indexed condition query data
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        protected override void update(int key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                if (users.ContainsKey(key)) updateTask(key, callback).AutoCSerNotWait();
                else createTask(key, callback).AutoCSerNotWait();
                state = ConditionDataUpdateStateEnum.Callbacked;
            }
            finally
            {
                if (state != ConditionDataUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// Update non-indexed condition query data
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
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
                    if (user.Value.Id != 0) StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(user.Value, callback);
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
        /// Delete non-indexed condition query data (Initialize and load the persistent data)
        /// 删除非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Data keyword</param>
        protected override void deleteLoadPersistence(int key)
        {
            users.Remove(key);
        }
        /// <summary>
        /// Delete non-indexed condition query data
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <returns></returns>
        protected override ConditionDataUpdateStateEnum delete(int key)
        {
            SearchUser user;
            if (users.Remove(key, out user)) loginTimes.Remove(user.GetLoginTimeKey());
            return ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Check the input parameters before the persistence operation)
        /// 非索引条件查询数据完成更新操作（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns></returns>
        public override ValueResult<ConditionDataUpdateStateEnum> CompletedBeforePersistence(SearchUser value)
        {
            SearchUser historyUser;
            if (users.TryGetValue(value.Id, out historyUser) && historyUser.Equals(value)) return ConditionDataUpdateStateEnum.Success;
            return default;
        }
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Initialize and load the persistent data)
        /// 非索引条件查询数据完成更新操作（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        protected override void completedLoadPersistence(SearchUser value)
        {
            users.Set(value.Id, value);
        }
        /// <summary>
        /// The non-indexed condition query data has completed the update operation
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns></returns>
        protected override ConditionDataUpdateStateEnum completed(SearchUser value)
        {
            SearchUser historyUser;
            if (users.Set(value.Id, value, out historyUser)) loginTimes.Add(value.GetLoginTimeKey());
            else if (value.LoginTime != historyUser.LoginTime)
            {
                loginTimes.Add(value.GetLoginTimeKey());
                loginTimes.Remove(historyUser.GetLoginTimeKey());
            }
            return ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public PageResult<int> GetPage(SearchUserQueryParameter queryParameter)
        {
            if (queryParameter.IsSearchCondition)
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
            switch (queryParameter.Order)
            {
                case UserOrderEnum.LoginTimeDesc: return queryParameter.GetDescPageResult(loginTimes, getLoginTimeUserId);
                default: return queryParameter.GetDescKeyPageResult(users);
            }
        }
        /// <summary>
        /// 获取非索引条件数据用户数组
        /// </summary>
        /// <param name="userIds">用户标识数组</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public LeftArray<SearchUser> GetArray(LeftArray<int> userIds)
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
