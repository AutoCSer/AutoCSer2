using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// TCP 服务端发送数据错误类型
    /// </summary>
    public enum ServerSendErrorTypeEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 异常
        /// </summary>
        Exception,
        /// <summary>
        /// 发送数据不完整时连续两次发送数据不足
        /// </summary>
        SendSizeLess,
    }
}
