using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [BinarySerialize(IsMixJsonSerialize = true)]
    public sealed class User
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender;
        /// <summary>
        /// 最后一次登录操作时间
        /// </summary>
        public DateTime LoginTime;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 用户备注
        /// </summary>
        public string Remark;
    }
}
