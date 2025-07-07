using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Service authentication interface based on incremental login timestamp verification (in conjunction with HASH to prevent replay login operations)
    /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    public interface ITimestampVerifyService
    {
        /// <summary>
        /// Verification method
        /// 验证方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="randomPrefix">Random prefix
        /// 随机前缀</param>
        /// <param name="hashData">Hash data to be verified
        /// 待验证 Hash 数据</param>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns></returns>
        CommandServerVerifyStateEnum Verify(CommandServerSocket socket, CommandServerCallQueue queue, ulong randomPrefix, byte[] hashData, ref long timestamp);
    }
}
