using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 异步任务完成回调
    /// </summary>
    public class CommandServerCallTask : CommandServerCall
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task task;
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="task"></param>
        internal CommandServerCallTask(CommandServerSocket socket, Task task) : base(socket)
        {
            this.task = task;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        internal void OnCompleted()
        {
            checkOfflineCount();
            var exception = task.Exception;
            if (exception == null) Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success, exception);
            else Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
        }
    }
    /// <summary>
    /// 异步任务完成回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerCallTask<T> : CommandServerCall
    {
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        internal readonly ServerInterfaceMethod Method;
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task<T> task;
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="task"></param>
        internal CommandServerCallTask(CommandServerSocket socket, ServerInterfaceMethod method, Task<T> task) : base(socket)
        {
            this.Method = method;
            this.task = task;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        internal void OnCompleted()
        {
            checkOfflineCount();
            var exception = task.Exception;
            if (exception == null) Socket.Send(CallbackIdentity, Method, new ServerReturnValue<T>(task.Result));
            else Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
        }
    }
}
