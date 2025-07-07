using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 超时计数
    /// </summary>
    internal sealed class CommandPoolTimeoutCount : AutoCSer.Threading.TimeoutCount
    {
        /// <summary>
        /// Client command pool
        /// 客户端命令池
        /// </summary>
        private readonly CommandPool commandPool;
        /// <summary>
        /// 超时计数
        /// </summary>
        /// <param name="commandPool"></param>
        /// <param name="maxSeconds">最大超时秒数，必须大于 0</param>
        internal CommandPoolTimeoutCount(CommandPool commandPool, ushort maxSeconds) : base(maxSeconds)
        {
            this.commandPool = commandPool;
        }
        /// <summary>
        /// 超时事件（不允许阻塞）
        /// </summary>
        /// <param name="seconds">超时秒计数</param>
        internal override void OnTimeout(uint seconds)
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(new CommandPoolTimeout(commandPool, seconds).OnTimeout);
        }
    }
}
