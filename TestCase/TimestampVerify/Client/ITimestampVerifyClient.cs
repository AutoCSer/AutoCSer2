using System;
using AutoCSer.Net;

namespace AutoCSer.TestCase.TimestampVerifyClient
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证客户端示例接口
    /// </summary>
    public interface ITimestampVerifyClient : AutoCSer.CommandService.ITimestampVerifyClient
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        CommandClientReturnValue<int> Add(int left, int right);
    }
}
