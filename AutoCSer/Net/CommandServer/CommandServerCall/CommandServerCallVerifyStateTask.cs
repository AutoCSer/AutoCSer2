using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 验证函数异步任务完成回调
    /// </summary>
    internal sealed class CommandServerCallVerifyStateTask : CommandServerCall
    {
        /// <summary>
        /// 服务端接口方法信息
        /// </summary>
        private readonly ServerInterfaceMethod method;
        /// <summary>
        /// 接口调用任务
        /// </summary>
        private readonly Task<CommandServerVerifyStateEnum> task;
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="task"></param>
        internal CommandServerCallVerifyStateTask(CommandServerSocket socket, ServerInterfaceMethod method, Task<CommandServerVerifyStateEnum> task) : base(socket)
        {
            this.method = method;
            this.task = task;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        internal void OnCompleted()
        {
            checkOfflineCount();
            var exception = task.Exception;
            if (exception == null)
            {
                if (Socket.SetVerifyState(task.Result))
                {
                    Socket.Send(CallbackIdentity, method, new ServerReturnValue<CommandServerVerifyStateEnum>(Socket.VerifyState));
                }
            }
            else Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
        }
    }
}
