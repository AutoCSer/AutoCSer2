using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 非索引条件过滤
    /// </summary>
    internal sealed class SearchUserFilterCallback : ConcurrencyQueueTaskNode
    {
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        private readonly SearchUserNode node;
        /// <summary>
        /// 用户标识数组
        /// </summary>
        private readonly ArrayBuffer<int> userIds;
        /// <summary>
        /// 条件委托
        /// </summary>
        private readonly Func<SearchUser, bool> isValue;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly MethodCallback<ArrayBuffer<int>> callback;
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="userIds"></param>
        /// <param name="isValue"></param>
        /// <param name="callback"></param>
        internal SearchUserFilterCallback(SearchUserNode node, ArrayBuffer<int> userIds, Func<SearchUser, bool> isValue, MethodCallback<ArrayBuffer<int>> callback) : base(node.ConcurrencyQueue)
        {
            this.node = node;
            this.userIds = userIds;
            this.isValue = isValue;
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
                callback.Callback(node.Filter(userIds, isValue));
                isCallback = true;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isCallback) callback.Callback(default(ArrayBuffer<int>));
            }
        }
    }
}
