using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 搜索聚合查询服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface IQueryService
    {
        /// <summary>
        /// 获取用户标识分页记录
        /// </summary>
        /// <param name="queryParameter">用户搜索查询参数</param>
        /// <returns></returns>
        Task<PageResult<int>> GetUserPage(UserQueryParameter queryParameter);
    }
}
