using System;
using System.Security.Cryptography;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// Customize user identity authentication data
    /// 自定义用户身份鉴权数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CustomVerifyData
    {
        /// <summary>
        /// User identification to be verified
        /// 待验证用户标识
        /// </summary>
        public string UserName;
        /// <summary>
        /// Verify the user key. The user password should be prefixed and hashed
        /// 用户验证密钥，用户密码应该增加前缀并哈希处理
        /// </summary>
        public string Key;
        /// <summary>
        /// Verify the timestamp
        /// 验证时间戳
        /// </summary>
        public long Timestamp;
        /// <summary>
        /// Random data
        /// 随机数据
        /// </summary>
        public ulong Random;
        /// <summary>
        /// Customize user identity authentication data
        /// 自定义用户身份鉴权数据
        /// </summary>
        /// <param name="userName">User identification to be verified
        /// 待验证用户标识</param>
        /// <param name="key">User verification key. The user password should be prefixed and hashed
        /// 用户验证密钥，用户密码应该增加前缀并哈希处理</param>
        public CustomVerifyData(string userName, string key)
        {
            UserName = userName;
            Key = key;
            Random = AutoCSer.Random.Default.SecureNextULongNotZero();
            Timestamp = AutoCSer.CommandService.TimestampVerifyChecker.CurrentTimestamp;
        }
        /// <summary>
        /// The client acquires the MD5 data and clears the verification user key information
        /// 客户端获取 MD5 数据并清除用户验证密钥信息
        /// </summary>
        /// <returns></returns>
        public byte[] GetMd5Data()
        {
            byte[] data;
            using (MD5 md5 = MD5.Create()) data = md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(this));
            Key = string.Empty;
            return data;
        }
        /// <summary>
        /// The server acquires the MD5 data
        /// 服务端获取 MD5 数据
        /// </summary>
        /// <param name="key">User verification key
        /// 用户验证密钥</param>
        /// <returns></returns>
        public byte[] GetMd5Data(string key)
        {
            Key = key;
            using (MD5 md5 = MD5.Create()) return md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(this));
        }
    }
}
