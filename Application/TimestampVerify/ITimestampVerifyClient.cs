using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证客户端接口
    /// </summary>
    public interface ITimestampVerifyClient
    {
        /// <summary>
        /// 验证函数
        /// </summary>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="hashData">验证 Hash 数据</param>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns></returns>
        CommandClientReturnValue<CommandServerVerifyStateEnum> Verify(ulong randomPrefix, byte[] hashData, ref long timestamp);
    }
}
