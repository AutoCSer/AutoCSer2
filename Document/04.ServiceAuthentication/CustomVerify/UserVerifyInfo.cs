using System;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// User verification information
    /// 用户验证信息
    /// </summary>
    internal class UserVerifyInfo
    {
        /// <summary>
        /// User identification to be verified
        /// 待验证用户标识
        /// </summary>
        public required string UserName;
        /// <summary>
        /// User verification key. The user password should be prefixed and hashed
        /// 用户验证密钥，用户密码应该增加前缀并哈希处理
        /// </summary>
        public required string Key;
        /// <summary>
        /// Verify the timestamp for the last time
        /// 最后一次验证时间戳
        /// </summary>
        public long Timestamp;
    }
}
