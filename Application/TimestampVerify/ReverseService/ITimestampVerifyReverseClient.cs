using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的反向服务认证客户端接口
    /// </summary>
    /// <typeparam name="T">附加数据类型</typeparam>
    public interface ITimestampVerifyReverseClient<T>
    {
        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns>反向服务验证数据</returns>
        ReturnCommand<ReverseServiceVerifyData<T>> GetVerifyData(long timestamp);
    }
}
