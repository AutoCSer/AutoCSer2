using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchQueryService;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户信息服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public interface IUserService
    {
        /// <summary>
        /// 获取所有用户标识
        /// </summary>
        /// <param name="callback">用户标识回调</param>
        /// <returns></returns>
        [CommandServerMethod(IsSynchronousCallTask = true, KeepCallbackOutputCount = 1 << 10)]
        Task GetAllUserId(CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>null 表示不存在</returns>
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<string> GetName(int id);
        /// <summary>
        /// 获取用户备注
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>null 表示不存在</returns>
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<string> GetRemark(int id);
        /// <summary>
        /// 获取用户搜索非索引条件数据
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>Id 为 0 表示不存在</returns>
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<SearchUser> GetSearchUser(int id);
        /// <summary>
        /// 获取用户标识分页记录
        /// </summary>
        /// <param name="queryParameter">用户搜索查询参数</param>
        /// <returns></returns>
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<PageResult<User>> GetPage(UserQueryParameter queryParameter);
    }
}
