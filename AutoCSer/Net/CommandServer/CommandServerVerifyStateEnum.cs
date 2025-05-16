using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令服务验证结果状态
    /// </summary>
    public enum CommandServerVerifyStateEnum : byte
    {
        /// <summary>
        /// 验证失败
        /// </summary>
        Fail,
        /// <summary>
        /// 验证成功
        /// </summary>
        Success,
        /// <summary>
        /// 验证失败，允许客户端重试
        /// </summary>
        Retry,
        /// <summary>
        /// 缺少验证函数逻辑，需要重载实现 AutoCSer.Net.CommandServerAttribute.ClientVerifyMethod
        /// </summary>
        LessVerifyMethod,
    }
}
