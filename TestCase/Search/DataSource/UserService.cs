using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchQueryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户信息服务
    /// </summary>
    internal sealed class UserService : IUserService
    {
        /// <summary>
        /// 模拟数据源
        /// </summary>
        private readonly Dictionary<int, User> users;
        /// <summary>
        /// 用户信息服务
        /// </summary>
        internal UserService()
        {
            users = DictionaryCreator.CreateInt<User>();
            users.Add(1, new User { Id = 1, Gender = GenderEnum.Male, LoginTime = DateTime.UtcNow, Name = @"AutoCSer", Remark = @"现在的努力是为了曾经吹过的牛B" });
            users.Add(2, new User { Id = 2, Gender = GenderEnum.Unknown, LoginTime = DateTime.UtcNow.AddDays(-1), Name = @"张三丰", Remark = @"现在的努力是为了将来能够吹牛B" });
            users.Add(3, new User { Id = 3, Gender = GenderEnum.Female, LoginTime = DateTime.UtcNow.AddMonths(-1), Name = @"张三", Remark = @"现在吹下的牛B是将来努力的动力" });
        }
        /// <summary>
        /// 获取所有用户标识
        /// </summary>
        /// <param name="callback">用户标识回调</param>
        /// <returns></returns>
        public async Task GetAllUserId(CommandServerKeepCallbackCount<int> callback)
        {
            foreach (int id in users.Keys)
            {
                if (!await callback.CallbackAsync(id)) return;
            }
        }
        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>null 表示不存在</returns>
        public Task<string> GetName(int id)
        {
            return Task.FromResult(users.TryGetValue(id, out User user) ? user.Name : null);
        }
        /// <summary>
        /// 获取用户备注
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>null 表示不存在</returns>
        public Task<string> GetRemark(int id)
        {
            return Task.FromResult(users.TryGetValue(id, out User user) ? user.Remark : null);
        }
        /// <summary>
        /// 获取用户搜索非索引条件数据
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns>Id 为 0 表示不存在</returns>
        public Task<SearchUser> GetSearchUser(int id)
        {
            return Task.FromResult(users.TryGetValue(id, out User user) ? new SearchUser(user) : default);
        }
        /// <summary>
        /// 获取用户标识分页记录
        /// </summary>
        /// <param name="queryParameter">用户搜索查询参数</param>
        /// <returns></returns>
        public async Task<PageResult<User>> GetPage(UserQueryParameter queryParameter)
        {
            SearchQueryCommandClientSocketEvent client = (SearchQueryCommandClientSocketEvent)await SearchQueryCommandClientSocketEvent.CommandClient.Client.GetSocketEvent();
            CommandClientReturnValue<PageResult<int>> ids = await client.SearchUserClient.GetUserPage(queryParameter);
            if (!ids.IsSuccess) return new PageResult<User>(ids.ReturnType);
            if (!ids.Value.IsSuccess) return ids.Value.Cast<User>();
            return ids.Value.Cast(users.getValueArray(ids.Value.Values));
        }
    }
}
