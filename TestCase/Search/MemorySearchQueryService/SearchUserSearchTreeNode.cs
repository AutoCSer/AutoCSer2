using AutoCSer.Data;
using AutoCSer.TestCase.SearchDataSource;
using System;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 用户搜索非索引条件数据节点
    /// </summary>
    internal sealed class SearchUserSearchTreeNode : AutoCSer.SearchTree.Node<SearchUserSearchTreeNode, int>
    {
        /// <summary>
        /// 用户搜索非索引条件数据
        /// </summary>
        internal SearchUser User;
        /// <summary>
        /// 快照序列化数据
        /// </summary>
        internal BinarySerializeKeyValue<int, SearchUser> SnapshotValue { get { return new BinarySerializeKeyValue<int, SearchUser>(Key, User); } }
        /// <summary>
        /// 获取最后一次登录操作时间关键字
        /// </summary>
        /// <returns></returns>
        internal CompareKey<DateTime, int> GetLoginTimeKey()
        {
            return new CompareKey<DateTime, int>(User.LoginTime, Key);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal bool Equals(ref BinarySerializeKeyValue<int, SearchUser> user)
        {
            return User.LoginTime == user.Value.LoginTime && User.Gender == user.Value.Gender && Key == user.Key;
        }
        /// <summary>
        /// 用户搜索非索引条件数据节点
        /// </summary>
        /// <param name="user"></param>
        internal SearchUserSearchTreeNode(ref BinarySerializeKeyValue<int, SearchUser> user) : base(user.Key)
        {
            User = user.Value;
        }

        /// <summary>
        /// 最后登录时间降序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int loginTimeDesc(SearchUserSearchTreeNode left, SearchUserSearchTreeNode right)
        {
            return right.User.LoginTime.CompareTo(left.User.LoginTime);
        }
        /// <summary>
        /// 最后登录时间降序
        /// </summary>
        public static readonly Func<SearchUserSearchTreeNode, SearchUserSearchTreeNode, int> LoginTimeDesc = loginTimeDesc;
        /// <summary>
        /// 获取用户标识
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static int getId(SearchUserSearchTreeNode user)
        {
            return user.key;
        }
        /// <summary>
        /// 获取用户标识
        /// </summary>
        public static readonly Func<SearchUserSearchTreeNode, int> GetId = getId;
    }
}
