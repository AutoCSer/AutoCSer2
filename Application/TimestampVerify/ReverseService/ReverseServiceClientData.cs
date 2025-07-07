using System;
using System.Security.Cryptography;

namespace AutoCSer.CommandService.TimestampVerify
{
    /// <summary>
    /// The original data verified by the reverse service client
    /// 反向服务客户端验证原始数据
    /// </summary>
    /// <typeparam name="T">Additional verification data type
    /// 附加验证数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ReverseServiceClientData<T>
    {
        /// <summary>
        /// Timestamp to be verified
        /// 待验证时间戳
        /// </summary>
        internal long Timestamp;
        /// <summary>
        /// Verify random value
        /// 验证随机值
        /// </summary>
        internal ulong Random;
        /// <summary>
        /// Additional verification data
        /// 附加验证数据
        /// </summary>
        internal T Data;
        /// <summary>
        /// The original data verified by the reverse service client
        /// 反向服务客户端验证原始数据
        /// </summary>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <param name="data">Additional verification data
        /// 附加验证数据</param>
        public ReverseServiceClientData(long timestamp, T data)
        {
            this.Timestamp = timestamp;
            this.Data = data;
            Random = AutoCSer.Random.Default.SecureNextULongNotZero();
        }
        /// <summary>
        /// The original data verified by the reverse service client
        /// 反向服务客户端验证原始数据
        /// </summary>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <param name="random">Verify random value
        /// 验证随机值</param>
        /// <param name="data">Additional verification data
        /// 附加验证数据</param>
        internal ReverseServiceClientData(long timestamp, ulong random, ref T data)
        {
            this.Timestamp = timestamp;
            Random = random;
            this.Data = data;
        }
    }
}
