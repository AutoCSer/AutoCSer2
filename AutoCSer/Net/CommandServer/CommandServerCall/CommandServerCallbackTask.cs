using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP 服务器端异步回调
    /// </summary>
    public class CommandServerCallbackTask : CommandServerCallback
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task task;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerCallbackTask(CommandServerSocket socket) : base(socket)
        {
            task = CommandServerRunTask.NullTask;
        }
        /// <summary>
        /// 设置接口调用任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private Action setTask(Task task)
        {
            this.task = task;
            return onCompleted;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onCompleted()
        {
            var exception = task.Exception;
            if (exception != null) Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// 创建 TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallbackTask CreateServerCallbackTask(CommandServerSocket socket)
        {
            return new CommandServerCallbackTask(socket);
        }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="callbackTask"></param>
        /// <param name="task"></param>
        internal static void CheckTask(CommandServerCallbackTask callbackTask, Task task)
        {
            //TaskAwaiter taskAwaiter = task.GetAwaiter();
            if (task.IsCompleted)
            {
                var exception = task.Exception;
                if (exception != null) callbackTask.Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
            }
            else task.GetAwaiter().UnsafeOnCompleted(callbackTask.setTask(task));
        }
    }
    /// <summary>
    /// TCP 服务器端异步回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerCallbackTask<T> : CommandServerCallback<T>
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task task;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerCallbackTask(CommandServerSocket socket) : base(socket)
        {
            task = CommandServerRunTask.NullTask;
        }
        /// <summary>
        /// 设置接口调用任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private Action setTask(Task task)
        {
            this.task = task;
            return onCompleted;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onCompleted()
        {
            var exception = task.Exception;
            if (exception != null) Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="callbackTask"></param>
        /// <param name="task"></param>
        internal static void CheckTask(CommandServerCallbackTask<T> callbackTask, Task task)
        {
            //TaskAwaiter taskAwaiter = task.GetAwaiter();
            if (task.IsCompleted)
            {
                var exception = task.Exception;
                if (exception != null) callbackTask.Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
            }
            else task.GetAwaiter().UnsafeOnCompleted(callbackTask.setTask(task));
        }
    }
}
