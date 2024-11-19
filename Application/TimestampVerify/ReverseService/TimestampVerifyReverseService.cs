using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的反向服务认证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class TimestampVerifyReverseService<T> : ITimestampVerifyReverseService<T>
    {
        /// <summary>
        /// 附加数据
        /// </summary>
        private readonly T data;
        /// <summary>
        /// 基于递增登录时间戳验证的反向服务认证
        /// </summary>
        /// <param name="data">附加数据</param>
        public TimestampVerifyReverseService(T data)
        {
            this.data = data;
        }
        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns>反向服务验证数据</returns>
        public ReverseServiceVerifyData<T> GetVerifyData(CommandServerSocket socket, long timestamp)
        {
            return new ReverseServiceVerifyData<T>(timestamp, data);
        }
    }
}
