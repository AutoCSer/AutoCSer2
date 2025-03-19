using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.TestCase.SearchQueryService;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 非索引条件查询数据节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsClient = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface ISearchUserNode : IConditionDataNode<int, SearchUser>
    {
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void GetPage(SearchUserQueryParameter queryParameter, MethodCallback<PageResult<int>> callback);
        /// <summary>
        /// 获取非索引条件数据用户数组
        /// </summary>
        /// <param name="userIds">用户标识数组</param>
        /// <param name="users">用户数组</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void GetArray(LeftArray<int> userIds, ArrayBuffer<SearchUser> users, MethodCallback<LeftArray<SearchUser>> callback);
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys">关键字集合</param>
        /// <param name="isValue">条件委托</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void Filter(ArrayBuffer<int> keys, Func<SearchUser, bool> isValue, MethodCallback<ArrayBuffer<int>> callback);
    }
}
