using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchQueryService;
using System;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 获取非索引条件数据用户分页数据
    /// </summary>
    internal sealed class SearchUserGetPageCallback : ConcurrencyQueueTaskNode
    {
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        private readonly SearchUserNode node;
        /// <summary>
        /// 用户搜索非索引条件数据查询参数
        /// </summary>
        private readonly SearchUserQueryParameter queryParameter;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly MethodCallback<PageResult<int>> callback;
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="queryParameter"></param>
        /// <param name="callback"></param>
        internal SearchUserGetPageCallback(SearchUserNode node, SearchUserQueryParameter queryParameter, MethodCallback<PageResult<int>> callback) : base(node.ConcurrencyQueue)
        {
            this.node = node;
            this.queryParameter = queryParameter;
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
                callback.Callback(node.GetPage(queryParameter));
                isCallback = true;
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (!isCallback) callback.Callback(new PageResult<int>(CommandClientReturnTypeEnum.ServerException));
            }
        }
    }
}
