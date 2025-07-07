using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.ConditionData;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchCommon;
using AutoCSer.TestCase.SearchDataSource;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户搜索非索引条件数据缓存
    /// </summary>
    internal sealed class SearchUserCache : ConditionDataCache<SearchUser>
    {
        /// <summary>
        /// 搜索聚合查询服务
        /// </summary>
        private readonly QueryService queryService;
        /// <summary>
        /// 用户搜索数据更新消息保持回调
        /// </summary>
        private AutoCSer.Net.CommandKeepCallback getMessageKeepCallback;
        /// <summary>
        /// 批处理数量
        /// </summary>
        private readonly int batchCount;
        /// <summary>
        /// 用户搜索非索引条件数据缓存
        /// </summary>
        /// <param name="queryService">搜索聚合查询服务</param>
        /// <param name="maxCount">最大缓存数据数量</param>
        /// <param name="batchCount">获取缓存数据批处理数量</param>
        internal SearchUserCache(QueryService queryService, int maxCount, int batchCount = 1 << 10) : base(maxCount)
        {
            this.queryService = queryService;
            this.batchCount = Math.Max(batchCount, 1);
            getUserMessage().NotWait();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close()
        {
            getMessageKeepCallback?.Dispose();
        }
        /// <summary>
        /// 获取用户搜索数据更新消息
        /// </summary>
        /// <returns></returns>
        private async Task getUserMessage()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                ResponseResult<ITimeoutMessageNodeClientNode<OperationData<int>>> node = await UserMessageCommandClientSocketEvent.UserMessageNodeCache.GetSynchronousNode();
                if (node.IsSuccess)
                {
                    getMessageKeepCallback = await node.Value.GetRunTask(getUserMessage);
                    if (getMessageKeepCallback != null) return;
                }
                await Task.Delay(1000);
            }
            while (queryService.IsDispose);
        }
        /// <summary>
        /// 获取用户搜索数据更新消息
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        private void getUserMessage(ResponseResult<OperationData<int>> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                if ((result.Value.DataType & OperationDataTypeEnum.SearchUserNode) != 0)
                {
                    switch (result.Value.OperationType)
                    {
                        case OperationTypeEnum.Update:
                        case OperationTypeEnum.Delete:
                            Remove(result.Value.Key);
                            return;
                    }
                }
                return;
            }
            command.Dispose();
            if (!queryService.IsDispose) getUserMessage().NotWait();
        }
        /// <summary>
        /// 获取用户搜索非索引条件数据集合
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        internal async Task<ArrayBuffer<SearchUser>> Get(ArrayBuffer<int> userIds)
        {
            bool isReturn = false;
            ArrayBuffer<SearchUser> buffer = queryService.UserBufferPool.GetBuffer(userIds.Count);
            try
            {
                Monitor.Enter(cacheLock);
                try
                {
                    foreach (int userId in userIds.GetLeftArray())
                    {
                        if (cache.TryGetValue(userId, out SearchUser user, true)) buffer.Add(user);
                        else userIds.Add(userId);
                    }
                }
                finally { Monitor.Exit(cacheLock); }
                if (userIds.Count != 0)
                {
                    ResponseResult<ISearchUserNodeClientNode> searchUserNode = await WordIdentityBlockIndexCommandClientSocketEvent.SearchUserNodeCache.GetSynchronousNode();
                    if (searchUserNode.IsSuccess)
                    {
                        foreach (LeftArray<int> userIdArray in userIds.GetLeftArray(batchCount))
                        {
                            ResponseResult<LeftArray<SearchUser>> users = await searchUserNode.Value.GetArray(userIdArray);
                            if (users.IsSuccess && users.Value.Count != 0)
                            {
                                Monitor.Enter(cacheLock);
                                try
                                {
                                    foreach (SearchUser user in users.Value)
                                    {
                                        if (cache.Set(user.Id, user, true) && cache.Count > maxCount) cache.RemoveRoll();
                                        buffer.Add(user);
                                    }
                                }
                                finally { Monitor.Exit(cacheLock); }
                            }
                        }
                    }
                }
                isReturn = true;
                return buffer;
            }
            finally 
            {
                if (!isReturn) buffer.Free();
            }
        }
        /// <summary>
        /// 获取用户搜索非索引条件数据集合
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="isValue"></param>
        /// <returns></returns>
        internal async Task<ArrayBuffer<int>> Get(ArrayBuffer<int> userIds, Func<SearchUser, bool> isValue)
        {
            bool isReturn = false;
            ArrayBuffer<int> buffer = queryService.IntBufferPool.GetBuffer(userIds.Count);
            try
            {
                Monitor.Enter(cacheLock);
                try
                {
                    foreach (int userId in userIds.GetLeftArray())
                    {
                        if (cache.TryGetValue(userId, out SearchUser user, true))
                        {
                            if (isValue(user)) buffer.Add(userId);
                        }
                        else userIds.Add(userId);
                    }
                }
                finally { Monitor.Exit(cacheLock); }
                if (userIds.Count != 0)
                {
                    ResponseResult<ISearchUserNodeClientNode> searchUserNode = await WordIdentityBlockIndexCommandClientSocketEvent.SearchUserNodeCache.GetSynchronousNode();
                    if (searchUserNode.IsSuccess)
                    {
                        foreach (LeftArray<int> userIdArray in userIds.GetLeftArray(batchCount))
                        {
                            ResponseResult<LeftArray<SearchUser>> users = await searchUserNode.Value.GetArray(userIdArray);
                            if (users.IsSuccess && users.Value.Count != 0)
                            {
                                Monitor.Enter(cacheLock);
                                try
                                {
                                    foreach (SearchUser user in users.Value)
                                    {
                                        if (cache.Set(user.Id, user, true) && cache.Count > maxCount) cache.RemoveRoll();
                                        if (isValue(user)) buffer.Add(user.Id);
                                    }
                                }
                                finally { Monitor.Exit(cacheLock); }
                            }
                        }
                    }
                }
                isReturn = true;
                return buffer;
            }
            finally
            {
                userIds.Free();
                if (!isReturn) buffer.Free();
            }
        }
    }
}
