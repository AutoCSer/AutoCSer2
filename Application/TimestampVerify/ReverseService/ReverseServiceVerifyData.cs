using System;
using System.Security.Cryptography;

namespace AutoCSer.CommandService.TimestampVerify
{
    /// <summary>
    /// Reverse service verification data
    /// 反向服务验证数据
    /// </summary>
    /// <typeparam name="T">Additional verification data type
    /// 附加验证数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReverseServiceVerifyData<T>
    {
        /// <summary>
        /// Verify random value
        /// 验证随机值
        /// </summary>
        internal ulong Random;
        /// <summary>
        /// Additional verification data
        /// 附加验证数据
        /// </summary>
        public T Data;
        /// <summary>
        /// MD5 data
        /// MD5 数据
        /// </summary>
        internal byte[] HashData;
        /// <summary>
        /// Reverse service verification data
        /// 反向服务验证数据
        /// </summary>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <param name="data">Additional verification data
        /// 附加验证数据</param>
        public ReverseServiceVerifyData(long timestamp, T data)
        {
            ReverseServiceClientData<T> clientData = new ReverseServiceClientData<T>(timestamp, data);
            Random = clientData.Random;
            Data = data;
            using (MD5 md5 = MD5.Create()) HashData = md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(clientData));
        }
        /// <summary>
        /// Verify the data
        /// 验证数据
        /// </summary>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public bool Verify(long timestamp, MD5 md5)
        {
            if (this.HashData != null)
            {
                ReverseServiceClientData<T> clientData = new ReverseServiceClientData<T>(timestamp, Random, ref Data);
                byte[] hashData = md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(clientData));
                return AutoCSer.Net.TimestampVerify.Md5Equals(this.HashData, hashData) == 0;
            }
            return false;
        }
        /// <summary>
        /// Verify the data
        /// 验证数据
        /// </summary>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns></returns>
        public bool Verify(long timestamp)
        {
            using (MD5 md5 = MD5.Create()) return Verify(timestamp, md5);
        }
    }
}
