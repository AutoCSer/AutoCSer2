using AutoCSer.CommandService;
using System;
using System.Security.Cryptography;

namespace AutoCSer.Document.ServiceAuthentication.CustomVerify
{
    /// <summary>
    /// 自定义服务认证数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CustomVerifyData
    {
        /// <summary>
        /// 验证用户标识
        /// </summary>
        public string UserName;
        /// <summary>
        /// 验证用户密钥，用户密码应该增加前缀并哈希处理
        /// </summary>
        public string Key;
        /// <summary>
        /// 验证时间戳
        /// </summary>
        public long Timestamp;
        /// <summary>
        /// 随机数据
        /// </summary>
        public ulong Random;
        /// <summary>
        /// 自定义服务认证数据
        /// </summary>
        /// <param name="userName">验证用户标识</param>
        /// <param name="key">验证用户密钥，用户密码应该增加前缀并哈希处理</param>
        public CustomVerifyData(string userName, string key)
        {
            UserName = userName;
            Key = key;
            Random = AutoCSer.Random.Default.SecureNextULongNotZero();
            Timestamp = TimestampVerifyChecker.CurrentTimestamp;
        }
        /// <summary>
        /// 客户端获取 MD5 数据并清除验证用户密钥信息
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
        /// 服务端获取 MD5 数据
        /// </summary>
        /// <param name="key">验证用户密钥</param>
        /// <returns></returns>
        public byte[] GetMd5Data(string key)
        {
            Key = key;
            using (MD5 md5 = MD5.Create()) return md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(this));
        }
    }
}
