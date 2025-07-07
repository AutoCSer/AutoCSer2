using System;
using AutoCSer.Net;

namespace AutoCSer.TestCase.TimestampVerifyClient
{
    /// <summary>
    /// Sample interface of service authentication client based on incremental login timestamp verification
    /// 基于递增登录时间戳验证的服务认证客户端示例接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(AutoCSer.TestCase.TimestampVerify.ITimestampVerify))]
#endif
    public partial interface ITimestampVerifyClient : AutoCSer.CommandService.ITimestampVerifyClient
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
