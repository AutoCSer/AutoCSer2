using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 获取非索引条件数据用户数组
    /// </summary>
    internal sealed class SearchUserGetArrayCallback : ConcurrencyQueueTaskNode
    {
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        private readonly SearchUserNode node;
        /// <summary>
        /// 用户标识数组
        /// </summary>
        private readonly LeftArray<int> userIds;
        /// <summary>
        /// 用户数组
        /// </summary>
        private readonly ArrayBuffer<SearchUser> users;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly MethodCallback<LeftArray<SearchUser>> callback;
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="userIds"></param>
        /// <param name="users"></param>
        /// <param name="callback"></param>
        internal SearchUserGetArrayCallback(SearchUserNode node, LeftArray<int> userIds, ArrayBuffer<SearchUser> users, MethodCallback<LeftArray<SearchUser>> callback) : base(node.ConcurrencyQueue)
        {
            this.node = node;
            this.userIds = userIds;
            this.users = users;
            this.callback = callback;
        }
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        protected override void runTask()
        {
            bool isCallback = false;
            try
            {
                callback.Callback(node.GetArray(userIds, users));
                isCallback = true;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isCallback) callback.Callback(default(LeftArray<SearchUser>));
            }
        }
    }
}
