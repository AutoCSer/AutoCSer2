using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 磁盘块客户端套接字事件
    /// </summary>
    public interface IDiskBlockClientSocketEvent
    {
        /// <summary>
        /// 磁盘块客户端接口
        /// </summary>
        IDiskBlockClient DiskBlockClient { get; }
    }
}
