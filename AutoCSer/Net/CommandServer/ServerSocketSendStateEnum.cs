using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 发送数据状态
    /// </summary>
    internal enum ServerSocketSendStateEnum : byte
    {
        /// <summary>
        /// 异步
        /// </summary>
        Asynchronous,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 同步
        /// </summary>
        Synchronize,
    }
}
