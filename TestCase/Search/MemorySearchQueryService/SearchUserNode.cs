using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.MemoryIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Data;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户搜索非索引条件数据节点
    /// </summary>
    internal sealed class SearchUserNode : ConditionDataLocalNode<ISearchUserNode, int, BinarySerializeKeyValue<int, SearchUser>, ISearchUserNodeLocalClientNode>, ISearchUserNode, IEnumerableSnapshot<bool>, IQueryContext<int, SearchUser>
    {
        /// <summary>
        /// 用户搜索非索引条件数据
        /// </summary>
        private readonly AutoCSer.SearchTree.NodeDictionary<int, SearchUserSearchTreeNode> users;
        /// <summary>
        /// 最后一次登录操作时间数据
        /// </summary>
        private readonly AutoCSer.SearchTree.Set<CompareKey<DateTime, int>> loginTimes;
        /// <summary>
        /// 定时释放缓冲区
        /// </summary>
        private readonly SearchUserNodeTimer searchUserNodeTimer;
        /// <summary>
        /// 关键字数组缓冲区池
        /// </summary>
        private readonly AutoCSer.CommandService.Search.IndexQuery.ArrayBufferPoolArray<int> intBufferPool;
        /// <summary>
        /// 用户搜索非索引条件数据数组缓冲区池
        /// </summary>
        private readonly AutoCSer.CommandService.Search.IndexQuery.ArrayBufferPoolArray<SearchUserSearchTreeNode> userBufferPool;
        /// <summary>
        /// 关键字可重用哈希表缓冲区池
        /// </summary>
        public AutoCSer.CommandService.Search.IndexQuery.HashSetPool<int>[] HashSetPool { get; private set; }
        /// <summary>
        /// 用户名称分词结果磁盘块索引信息节点
        /// </summary>
        private readonly HashCodeKeyIndexNode<int> userNameNode;
        /// <summary>
        /// 用户备注分词结果磁盘块索引信息节点
        /// </summary>
        private readonly HashCodeKeyIndexNode<int> userRemarkNode;
        /// <summary>
        /// 异步操作队列访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim queueLock;
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        protected override int snapshotCapacity { get { return users.Count; } }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        protected override IEnumerable<BinarySerializeKeyValue<int, SearchUser>> snapshotValues
        {
            get
            {
                foreach (SearchUserSearchTreeNode node in users.Values) yield return node.SnapshotValue;
            }
        }
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        /// <param name="service"></param>
        internal SearchUserNode(StreamPersistenceMemoryDatabaseService service) : base(MemorySearchUserServiceConfig.SearchUserNodeCache)
        {
            userNameNode = (HashCodeKeyIndexNode<int>)service.GetNode(nameof(OperationDataTypeEnum.UserNameNode)).castType<ServerNode<IHashCodeKeyIndexNode<int>>>().Target;
            userRemarkNode = (HashCodeKeyIndexNode<int>)service.GetNode(nameof(OperationDataTypeEnum.UserRemarkNode)).castType<ServerNode<IHashCodeKeyIndexNode<int>>>().Target;
            intBufferPool = new AutoCSer.CommandService.Search.IndexQuery.ArrayBufferPoolArray<int>(8);
            userBufferPool = new AutoCSer.CommandService.Search.IndexQuery.ArrayBufferPoolArray<SearchUserSearchTreeNode>(8);
            HashSetPool = AutoCSer.CommandService.Search.IndexQuery.HashSetPool<int>.GetArray(256);
            searchUserNodeTimer = new SearchUserNodeTimer(60 * 60);
            users = new AutoCSer.SearchTree.NodeDictionary<int, SearchUserSearchTreeNode>();
            loginTimes = new AutoCSer.SearchTree.Set<CompareKey<DateTime, int>>();
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
            DataSourceCommandClientSocketEvent.CommandClient.Client.GetSocketEvent().NotWait();
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        internal void FreeCache()
        {
            intBufferPool.FreeCache();
            userBufferPool.FreeCache();
            AutoCSer.CommandService.Search.IndexQuery.HashSetPool<int>.FreeCache(HashSetPool);
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
            foreach (SearchUserSearchTreeNode node in users.Values) loginTimes.Add(node.GetLoginTimeKey());
            searchUserNodeTimer.Set(this);
            return base.StreamPersistenceMemoryDatabaseServiceLoaded();
        }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            searchUserNodeTimer.Cancel();
        }
        /// <summary>
        /// 数据库服务关闭操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceDisposable()
        {
            searchUserNodeTimer.Cancel();
        }
        /// <summary>
        /// 获取初始化加载所有关键字数据命令
        /// </summary>
        /// <returns></returns>
        protected override EnumeratorCommand<BinarySerializeKeyValue<int, SearchUser>> getLoadCommand()
        {
            return DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient?.GetAllSearchUser();
        }
        /// <summary>
        /// 创建用户搜索非索引条件数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override LocalServiceQueueNode<LocalResult> loadCreate(ISearchUserNodeLocalClientNode client, BinarySerializeKeyValue<int, SearchUser> value)
        {
            return client.LoadCreate(value);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public override void SetSnapshotResult(ref LeftArray<BinarySerializeKeyValue<int, SearchUser>> array, ref LeftArray<BinarySerializeKeyValue<int, SearchUser>> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public override void SnapshotSet(BinarySerializeKeyValue<int, SearchUser> value)
        {
            users.Set(new SearchUserSearchTreeNode(ref value));
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
        public override bool Contains(BinarySerializeKeyValue<int, SearchUser> value)
        {
            return users.ContainsKey(value.Key);
        }
        /// <summary>
        /// 创建非索引条件查询数据 持久化前检查
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns>是否继续持久化操作</returns>
        public override bool LoadCreateBeforePersistence(BinarySerializeKeyValue<int, SearchUser> value)
        {
            return !users.ContainsKey(value.Key);
        }
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        public override void LoadCreateLoadPersistence(BinarySerializeKeyValue<int, SearchUser> value)
        {
            users.TryAdd(new SearchUserSearchTreeNode(ref value));
        }
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        public override void LoadCreate(BinarySerializeKeyValue<int, SearchUser> value)
        {
            SearchUserSearchTreeNode node = new SearchUserSearchTreeNode(ref value);
            if (users.TryAdd(node)) loginTimes.Add(node.GetLoginTimeKey());
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
                CommandClientReturnValue<BinarySerializeKeyValue<int, SearchUser>> user = await DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetSearchUser(key);
                if (user.IsSuccess)
                {
                    if (user.Value.Key != 0)
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
                CommandClientReturnValue<BinarySerializeKeyValue<int, SearchUser>> user = await DataSourceCommandClientSocketEvent.CommandClient.SocketEvent.UserClient.GetSearchUser(key);
                if (user.IsSuccess)
                {
                    if (user.Value.Key != 0) StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(user.Value, callback);
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
            SearchUserSearchTreeNode user;
            if (users.Remove(key, out user)) loginTimes.Remove(user.GetLoginTimeKey());
            return ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        public override ValueResult<ConditionDataUpdateStateEnum> CompletedBeforePersistence(BinarySerializeKeyValue<int, SearchUser> value)
        {
            SearchUserSearchTreeNode historyUser;
            if (users.TryGetValue(value.Key, out historyUser) && historyUser.Equals(value)) return ConditionDataUpdateStateEnum.Success;
            return default;
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        protected override void completedLoadPersistence(BinarySerializeKeyValue<int, SearchUser> value)
        {
            users.Set(new SearchUserSearchTreeNode(ref value));
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        protected override ConditionDataUpdateStateEnum completed(BinarySerializeKeyValue<int, SearchUser> value)
        {
            SearchUserSearchTreeNode node = new SearchUserSearchTreeNode(ref value), historyUser;
            if (users.Set(node, out historyUser)) loginTimes.Add(node.GetLoginTimeKey());
            else if (value.Value.LoginTime != historyUser.User.LoginTime)
            {
                loginTimes.Add(node.GetLoginTimeKey());
                loginTimes.Remove(historyUser.GetLoginTimeKey());
            }
            return ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <param name="userNameWordIdentitys">用户名称查询分词编号集合</param>
        /// <param name="userRemarkWordIdentitys">用户备注查询分词编号集合</param>
        /// <returns></returns>
        public PageResult<int> GetPage(SearchUserQueryParameter queryParameter, int[] userNameWordIdentitys, int[] userRemarkWordIdentitys)
        {
            LeftArray<IIndexCondition<int>> conditions = new LeftArray<IIndexCondition<int>>(0);
            if (userNameWordIdentitys.Length != 0)
            {
                var indexCondition = userNameNode.GetIntIndexCondition(userNameWordIdentitys);
                if (indexCondition == null) return queryParameter.GetPageResult<int>();
                conditions.Add(indexCondition);
            }
            if (userRemarkWordIdentitys.Length != 0)
            {
                var indexCondition = userRemarkNode.GetIntIndexCondition(userRemarkWordIdentitys);
                if (indexCondition == null) return queryParameter.GetPageResult<int>();
                conditions.Add(indexCondition);
            }
            if (queryParameter.IsSearchCondition || conditions.Count != 0)
            {
                var indexCondition = IndexConditionArray<int>.GetIndexCondition(conditions);
                if (indexCondition != null)
                {
                    AutoCSer.CommandService.Search.IndexQuery.ArrayBuffer<SearchUserSearchTreeNode> users = userBufferPool.GetNull();
                    AutoCSer.CommandService.Search.IndexQuery.ArrayBuffer<int> userIds = new QueryCondition<int, SearchUser>(this, indexCondition, queryParameter.SearchCondition).Query(queryParameter);
                    try
                    {
                        switch (queryParameter.Order)
                        {
                            case UserOrderEnum.LoginTimeDesc:
                                int count = userIds.Count;
                                if (queryParameter.GetPageSize(count) > 0)
                                {
                                    users = userBufferPool.GetBuffer(count);
                                    foreach (int userId in userIds.GetLeftArray())
                                    {
                                        if (this.users.TryGetValue(userId, out SearchUserSearchTreeNode node)) users.Add(node);
                                    }
                                    userIds.Free();
                                    if (users.Count != 0) return queryParameter.GetPageResult(users.GetLeftArray(), SearchUserSearchTreeNode.LoginTimeDesc, SearchUserSearchTreeNode.GetId);
                                    return new PageResult<int>(Net.CommandClientReturnTypeEnum.ServerException);
                                }
                                return new PageResult<int>(EmptyArray<int>.Array, count, queryParameter.PageIndex, queryParameter.PageSize);
                            default: return queryParameter.GetDescPageResult(userIds.GetLeftArray());
                        }
                    }
                    finally
                    {
                        userIds.Free();
                        users.Free();
                    }
                }
                PageArray<int> pageArray = queryParameter.GetPageArray<int>();
                switch (queryParameter.Order)
                {
                    case UserOrderEnum.LoginTimeDesc:
                        SearchUserSearchTreeNode node;
                        foreach (CompareKey<DateTime, int> loginTime in loginTimes.Values)
                        {
                            if (users.TryGetValue(loginTime.Value2, out node) && queryParameter.SearchCondition(node.User) && pageArray.Add()) pageArray.Add(node.Key);
                        }
                        break;
                    default:
                        foreach (SearchUserSearchTreeNode user in users.Values)
                        {
                            if (queryParameter.SearchCondition(user.User) && pageArray.Add()) pageArray.Add(user.Key);
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
        /// 获取数组缓冲区
        /// </summary>
        /// <param name="size">数组最小容量</param>
        /// <returns></returns>
        AutoCSer.CommandService.Search.IndexQuery.ArrayBuffer<int> IQueryContext<int, SearchUser>.GetBuffer(int size)
        {
            return intBufferPool.GetBuffer(size);
        }
        /// <summary>
        /// 空数组缓冲区
        /// </summary>
        AutoCSer.CommandService.Search.IndexQuery.ArrayBuffer<int> IQueryContext<int, SearchUser>.NullBuffer { get { return intBufferPool.GetNull(); } }
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys">关键字集合</param>
        /// <param name="isValue">条件委托</param>
        /// <param name="buffer">关键字集合</param>
        /// <returns></returns>
        AutoCSer.CommandService.Search.IndexQuery.ArrayBuffer<int> IQueryContext<int, SearchUser>.Filter(IEnumerable<int> keys, Func<SearchUser, bool> isValue, AutoCSer.CommandService.Search.IndexQuery.ArrayBuffer<int> buffer)
        {
            foreach (int userId in keys)
            {
                if (users.TryGetValue(userId, out SearchUserSearchTreeNode node) && isValue(node.User)) buffer.Add(userId);
            }
            if (buffer.Count != 0) return buffer;
            buffer.Free();
            return intBufferPool.GetNull();
        }
        /// <summary>
        /// 关键字可重用哈希表缓冲区池
        /// </summary>
        AutoCSer.CommandService.Search.IndexQuery.HashSetPool<int>[] IQueryContext<int, SearchUser>.HashSetPool { get { return HashSetPool; } }

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
