using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.SearchDataSource;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户搜索非索引条件数据查询参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public class SearchUserQueryParameter : PageParameter
    {
        /// <summary>
        /// 查询性别
        /// </summary>
        public GenderEnum Gender;
        /// <summary>
        /// 排序类型，默认为 用户标识降序
        /// </summary>
        public UserOrderEnum Order;
        /// <summary>
        /// 是否存在非索引条件查询参数
        /// </summary>
        public bool IsSearchCondition
        {
            get
            {
                return Gender != default(GenderEnum);
            }
        }
        /// <summary>
        /// 非索引条件
        /// </summary>
        /// <param name="user">用户搜索非索引条件数据</param>
        /// <returns></returns>
        public bool SearchCondition(SearchUser user)
        {
            if (Gender != default(GenderEnum) && user.Gender != Gender) return false;
            return true;
        }
    }
}
