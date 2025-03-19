using AutoCSer.CommandService.Search.MemoryIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.SearchQueryService
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
        /// 索引条件
        /// </summary>
        private readonly IIndexCondition<int> indexCondition;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly MethodCallback<PageResult<int>> callback;
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="queryParameter"></param>
        /// <param name="indexCondition"></param>
        /// <param name="callback"></param>
        internal SearchUserGetPageCallback(SearchUserNode node, SearchUserQueryParameter queryParameter, IIndexCondition<int> indexCondition, MethodCallback<PageResult<int>> callback) : base(node.ConcurrencyQueue)
        {
            this.node = node;
            this.queryParameter = queryParameter;
            this.indexCondition = indexCondition;
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
                callback.Callback(node.GetPage(queryParameter, indexCondition));
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
