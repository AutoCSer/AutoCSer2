using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端心跳检测定时
    /// </summary>
    internal sealed class ClientCheckTimer : AutoCSer.Threading.SecondTimerArrayNode
    {
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
#if NetStandard21
        private CommandClientSocket? socket;
#else
        private CommandClientSocket socket;
#endif
        /// <summary>
        /// 客户端心跳检测定时
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="seconds">超时秒数</param>
        internal ClientCheckTimer(CommandClientSocket socket, int seconds)
            : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, Threading.SecondTimerKeepModeEnum.After)
        {
            this.socket = socket;
            AppendTaskArray(seconds);
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            var socket = this.socket;
            if (socket != null && !socket.Check()) KeepSeconds = 0;
        }
        /// <summary>
        /// 取消心跳检测
        /// </summary>
        internal void Cancel()
        {
            KeepSeconds = 0;
            socket = null;
        }
    }
}
