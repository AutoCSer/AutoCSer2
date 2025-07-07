using AutoCSer.CommandService.TimestampVerify;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Reverse service authentication interface based on incremental login timestamp verification (combined with HASH to prevent replay login operations)
    /// 基于递增登录时间戳验证的反向服务认证接口（配合 HASH 防止重放登录操作）
    /// </summary>
    /// <typeparam name="T">Additional verification data type
    /// 附加验证数据类型</typeparam>
    [AutoCSer.Net.CommandServerControllerInterface]
    public partial interface ITimestampVerifyReverseService<T>
    {
        /// <summary>
        /// Get the reverse service verification data
        /// 获取反向服务验证数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns>Reverse service verification data
        /// 反向服务验证数据</returns>
        ReverseServiceVerifyData<T> GetVerifyData(CommandServerSocket socket, long timestamp);
    }
}
