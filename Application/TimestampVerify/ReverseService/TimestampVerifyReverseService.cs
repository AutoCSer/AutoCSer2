using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Reverse service authentication based on incremental login timestamp verification
    /// 基于递增登录时间戳验证的反向服务认证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class TimestampVerifyReverseService<T> : ITimestampVerifyReverseService<T>
    {
        /// <summary>
        /// Additional verification data
        /// 附加验证数据
        /// </summary>
        private readonly T data;
        /// <summary>
        /// Reverse service authentication based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的反向服务认证
        /// </summary>
        /// <param name="data">Additional verification data
        /// 附加验证数据</param>
        public TimestampVerifyReverseService(T data)
        {
            this.data = data;
        }
        /// <summary>
        /// Get the reverse service verification data
        /// 获取反向服务验证数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns>Reverse service verification data
        /// 反向服务验证数据</returns>
        public ReverseServiceVerifyData<T> GetVerifyData(CommandServerSocket socket, long timestamp)
        {
            return new ReverseServiceVerifyData<T>(timestamp, data);
        }
    }
}
