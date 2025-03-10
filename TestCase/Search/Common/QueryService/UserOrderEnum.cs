using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户数据排序类型
    /// </summary>
    public enum UserOrderEnum : byte
    {
        /// <summary>
        /// 用户标识降序
        /// </summary>
        IdDesc,
        /// <summary>
        /// 最后登录时间降序
        /// </summary>
        LoginTimeDesc,
    }
}
