using System;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// 用户验证信息
    /// </summary>
    internal class UserVerifyInfo
    {
        /// <summary>
        /// 验证用户标识
        /// </summary>
        public required string UserName;
        /// <summary>
        /// 验证用户密钥，用户密码应该增加前缀并哈希处理
        /// </summary>
        public required string Key;
        /// <summary>
        /// 最后一次验证时间戳
        /// </summary>
        public long Timestamp;
    }
}
