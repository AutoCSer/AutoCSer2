using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.TestCase.SearchQueryService;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// Non-index condition query data node interface
    /// 非索引条件查询数据节点接口
    /// </summary>
    [ServerNode(IsClient = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface ISearchUserNode : IConditionDataNode<int, SearchUser>
    {
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        PageResult<int> GetPage(SearchUserQueryParameter queryParameter);
        /// <summary>
        /// 获取非索引条件数据用户数组
        /// </summary>
        /// <param name="userIds">用户标识数组</param>
        /// <param name="users">用户数组</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        LeftArray<SearchUser> GetArray(LeftArray<int> userIds, ArrayBuffer<SearchUser> users);
        /// <summary>
        /// 非索引条件过滤
        /// </summary>
        /// <param name="keys">关键字集合</param>
        /// <param name="isValue">条件委托</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        ArrayBuffer<int> Filter(ArrayBuffer<int> keys, Func<SearchUser, bool> isValue);
    }
}
