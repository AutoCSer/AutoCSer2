using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 异步任务完成回调
    /// </summary>
    public class CommandServerCallSendOnlyTask : CommandServerCall
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task<CommandServerSendOnly> task;
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="task"></param>
        internal CommandServerCallSendOnlyTask(CommandServerSocket socket, Task<CommandServerSendOnly> task) : base(socket)
        {
            this.task = task;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        internal void OnCompleted()
        {
            checkOfflineCount();
            Socket.CloseShortLink();
            var exception = task.Exception;
            if (exception != null) Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
    }
}
