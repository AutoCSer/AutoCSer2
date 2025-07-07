using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 超时事件
    /// </summary>
    internal sealed class CommandPoolTimeout
    {
        /// <summary>
        /// Client command pool
        /// 客户端命令池
        /// </summary>
        private readonly CommandPool commandPool;
        /// <summary>
        /// 超时秒计数
        /// </summary>
        private readonly uint seconds;
        /// <summary>
        /// 超时事件
        /// </summary>
        /// <param name="commandPool"></param>
        /// <param name="seconds"></param>
        internal CommandPoolTimeout(CommandPool commandPool, uint seconds)
        {
            this.commandPool = commandPool;
            this.seconds = seconds;
        }
        /// <summary>
        /// 超时事件
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void OnTimeout()
        {
            commandPool.OnTimeout(seconds);
        }
    }
}
