using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Service authentication client interface based on incremental login timestamp verification
    /// 基于递增登录时间戳验证的服务认证客户端接口
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ITimestampVerifyService))]
    public partial interface ITimestampVerifyClient
    {
        /// <summary>
        /// Verification method
        /// 验证方法
        /// </summary>
        /// <param name="randomPrefix">Random prefix
        /// 随机前缀</param>
        /// <param name="hashData">Hash data to be verified
        /// 待验证 Hash 数据</param>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns></returns>
        CommandClientReturnValue<CommandServerVerifyStateEnum> Verify(ulong randomPrefix, byte[] hashData, ref long timestamp);
    }
}
