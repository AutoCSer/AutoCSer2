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
        /// 用户标识
        /// </summary>
        public readonly int Id;
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
            Id = user.Id;
            LoginTime = user.LoginTime;
            Gender = user.Gender;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SearchUser other)
        {
            return LoginTime == other.LoginTime && Gender == other.Gender && Id == other.Id;
        }
        /// <summary>
        /// 获取最后一次登录操作时间关键字
        /// </summary>
        /// <returns></returns>
        public CompareKey<DateTime, int> GetLoginTimeKey()
        {
            return new CompareKey<DateTime, int>(LoginTime, Id);
        }

        /// <summary>
        /// 最后登录时间降序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int loginTimeDesc(SearchUser left, SearchUser right)
        {
            return right.LoginTime.CompareTo(left.LoginTime);
        }
        /// <summary>
        /// 最后登录时间降序
        /// </summary>
        public static readonly Func<SearchUser, SearchUser, int> LoginTimeDesc = loginTimeDesc;
        /// <summary>
        /// 获取用户标识
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static int getId(SearchUser user)
        {
            return user.Id;
        }
        /// <summary>
        /// 获取用户标识
        /// </summary>
        public static readonly Func<SearchUser, int> GetId = getId;
    }
}
