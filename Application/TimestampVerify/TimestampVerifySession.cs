using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象
    /// </summary>
    public class TimestampVerifySession
    {
        /// <summary>
        /// 服务端分配的时间戳
        /// </summary>
        internal long ServerTimestamp;
    }
}
