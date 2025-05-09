using AutoCSer.CommandService.Search;
using AutoCSer.Data;
using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户搜索非索引条件数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SearchUser
    {
        /// <summary>
        /// 最后一次登录操作时间
        /// </summary>
        public readonly DateTime LoginTime;
        /// <summary>
        /// 性别
        /// </summary>
        public readonly GenderEnum Gender;
        /// <summary>
        /// 用户搜索非索引条件数据
        /// </summary>
        /// <param name="user"></param>
        public SearchUser(User user)
        {
            LoginTime = user.LoginTime;
            Gender = user.Gender;
        }
    }
}
