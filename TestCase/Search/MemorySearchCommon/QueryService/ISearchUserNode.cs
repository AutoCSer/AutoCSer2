using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 非索引条件查询数据节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsClient = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface ISearchUserNode : IConditionDataNode<int, BinarySerializeKeyValue<int, SearchUser>>
    {
        /// <summary>
        /// 获取非索引条件数据用户分页数据
        /// </summary>
        /// <param name="queryParameter">用户搜索非索引条件数据查询参数</param>
        /// <param name="userNameWordIdentitys">用户名称查询分词编号集合</param>
        /// <param name="userRemarkWordIdentitys">用户备注查询分词编号集合</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void GetPage(SearchUserQueryParameter queryParameter, int[] userNameWordIdentitys, int[] userRemarkWordIdentitys, MethodCallback<PageResult<int>> callback);
    }
}
