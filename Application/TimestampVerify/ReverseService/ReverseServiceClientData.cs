using System;
using System.Security.Cryptography;

namespace AutoCSer.CommandService.TimestampVerify
{
    /// <summary>
    /// 反向服务客户端验证原始数据
    /// </summary>
    /// <typeparam name="T">附加数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ReverseServiceClientData<T>
    {
        /// <summary>
        /// 待验证时间戳
        /// </summary>
        internal long Timestamp;
        /// <summary>
        /// 随机值
        /// </summary>
        internal ulong Random;
        /// <summary>
        /// 附加数据
        /// </summary>
        internal T Data;
        /// <summary>
        /// 反向服务客户端验证原始数据
        /// </summary>
        /// <param name="timestamp">待验证时间戳</param>
        /// <param name="data">附加数据</param>
        public ReverseServiceClientData(long timestamp, T data)
        {
            this.Timestamp = timestamp;
            this.Data = data;
            Random = AutoCSer.Random.Default.SecureNextULongNotZero();
        }
        /// <summary>
        /// 反向服务客户端验证原始数据
        /// </summary>
        /// <param name="timestamp">待验证时间戳</param>
        /// <param name="random">随机值</param>
        /// <param name="data">附加数据</param>
        internal ReverseServiceClientData(long timestamp, ulong random, ref T data)
        {
            this.Timestamp = timestamp;
            Random = random;
            this.Data = data;
        }
    }
}
