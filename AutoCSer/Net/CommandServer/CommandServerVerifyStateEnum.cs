using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// The command service verifies the result status
    /// 命令服务验证结果状态
    /// </summary>
    public enum CommandServerVerifyStateEnum : byte
    {
        /// <summary>
        /// Verification failed
        /// 验证失败
        /// </summary>
        Fail,
        /// <summary>
        /// Verification successful
        /// 验证成功
        /// </summary>
        Success,
        /// <summary>
        /// Verification failed. Allow the client to try again
        /// 验证失败，允许客户端重试
        /// </summary>
        Retry,
        /// <summary>
        /// Lack of validation logic function, need to reload AutoCSer.Net.CommandServerAttribute.ClientVerifyMethod
        /// 缺少验证函数逻辑，需要重载实现 AutoCSer.Net.CommandServerAttribute.ClientVerifyMethod
        /// </summary>
        LessVerifyMethod,
    }
}
