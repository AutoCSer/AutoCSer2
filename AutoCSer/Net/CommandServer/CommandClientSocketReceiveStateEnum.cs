using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端套接字接收命令执行状态
    /// </summary>
    internal enum CommandClientSocketReceiveStateEnum : byte
    {
        /// <summary>
        /// 等待异步完成
        /// </summary>
        Asynchronous,
    }
}
