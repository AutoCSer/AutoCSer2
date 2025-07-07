using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using AutoCSer.TestCase.SearchQueryService;
using System;

namespace AutoCSer.TestCase.SearchCommon
{
    /// <summary>
    /// Non-index condition query data node interface
    /// 非索引条件查询数据节点接口
    /// </summary>
    [ServerNode(IsMethodParameterCreator = true)]
    public partial interface ISearchUserNode : IConditionDataNode<int, SearchUser>
    {
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        PageResult<int> GetPage(SearchUserQueryParameter queryParameter);
        /// <summary>
        /// 获取非索引条件数据用户数组
        /// </summary>
        /// <param name="userIds">用户标识数组</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        LeftArray<SearchUser> GetArray(LeftArray<int> userIds);
    }
}
