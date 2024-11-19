using System;
using System.Net;

namespace AutoCSer.CommandService.ReverseLogCollection
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    public abstract class CommandReverseListenerConfig : AutoCSer.Net.CommandReverseListenerConfig
    {
        /// <summary>
        /// 待发送日志队列数量默认为 1023 条，超出限制则抛弃日志
        /// </summary>
        public int LogQueueCapacity = (1 << 10) - 1;
    }
}
