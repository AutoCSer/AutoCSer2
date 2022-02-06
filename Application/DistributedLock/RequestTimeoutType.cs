using System;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 请求超时处理类型
    /// </summary>
    internal enum RequestTimeoutType : byte
    {
        /// <summary>
        /// 释放锁
        /// </summary>
        Release,
        /// <summary>
        /// 等待锁
        /// </summary>
        Wait,
    }
}
