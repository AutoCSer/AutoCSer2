using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的反向服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    /// <typeparam name="T">附加数据类型</typeparam>
    public interface ITimestampVerifyReverseService<T>
    {
        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns>反向服务验证数据</returns>
        ReverseServiceVerifyData<T> GetVerifyData(CommandServerSocket socket, long timestamp);
    }
}
