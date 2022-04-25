using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    public interface ITimestampVerifyService
    {
        /// <summary>
        /// 验证函数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="hashData">验证 Hash 数据</param>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns></returns>
        CommandServerVerifyState Verify(CommandServerSocket socket, CommandServerCallQueue queue, ulong randomPrefix, byte[] hashData, ref long timestamp);
    }
}
