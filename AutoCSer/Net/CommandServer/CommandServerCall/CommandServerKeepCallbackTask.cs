using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP server-side asynchronous callback
    /// TCP 服务器端异步回调
    /// </summary>
    public class CommandServerKeepCallbackTask : CommandServerKeepCallback
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task callTask;
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        private CommandServerKeepCallbackTask(CommandServerSocket socket, OfflineCount offlineCount) : base(socket, offlineCount)
        {
#if NetStandard21
            callTask = CommandServerRunTask.NullTask;
#endif
        }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        protected CommandServerKeepCallbackTask(CommandServerSocket socket) : base(socket)
        {
            callTask = CommandServerRunTask.NullTask;
        }
        /// <summary>
        /// 任务完成检查
        /// </summary>
        protected void onCompleted()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception != null) RemoveKeepCallback(exception);
        }
        /// <summary>
        /// 设置接口调用任务
        /// </summary>
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Action setCallTask(Task callTask)
        {
            this.callTask = callTask;
            OfflineCount = Socket.OfflineCount;
            return onCompleted;
        }
        /// <summary>
        /// 任务完成检查
        /// </summary>
        protected void onCompletedAutoCancelKeep()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception == null) CancelKeep();
            else RemoveKeepCallback(exception);
        }
        /// <summary>
        /// 设置接口调用任务
        /// </summary>
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Action setCallTaskAutoCancelKeep(Task callTask)
        {
            this.callTask = callTask;
            OfflineCount = Socket.OfflineCount;
            return onCompletedAutoCancelKeep;
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackTask CreateServerKeepCallbackTask(CommandServerSocket socket)
        {
            return new CommandServerKeepCallbackTask(socket, OfflineCount.Null);
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTask(CommandServerKeepCallbackTask task, Task callTask)
        {
            //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted)
            {
                task.Socket.CheckOfflineCount();
                var exception = callTask.Exception;
                if (exception != null) task.RemoveKeepCallback(exception);
            }
            else callTask.GetAwaiter().UnsafeOnCompleted(task.setCallTask(callTask));
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTaskAutoCancelKeep(CommandServerKeepCallbackTask task, Task callTask)
        {
            //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted)
            {
                task.Socket.CheckOfflineCount();
                var exception = callTask.Exception;
                if (exception == null) task.CancelKeep();
                else task.RemoveKeepCallback(exception);
            }
            else callTask.GetAwaiter().UnsafeOnCompleted(task.setCallTaskAutoCancelKeep(callTask));
        }
    }
    /// <summary>
    /// TCP server-side asynchronous callback
    /// TCP 服务器端异步回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerKeepCallbackTask<T> : CommandServerKeepCallback<T>
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task callTask;
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallbackTask(CommandServerSocket socket, OfflineCount offlineCount, ServerInterfaceMethod method) : base(socket, offlineCount, method)
        {
#if NetStandard21
            callTask = CommandServerRunTask.NullTask;
#endif
        }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerKeepCallbackTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method)
        {
            callTask = CommandServerRunTask.NullTask;
        }
        /// <summary>
        /// 任务完成检查
        /// </summary>
        protected void onCompleted()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception != null) RemoveKeepCallback(exception);
        }
        /// <summary>
        /// 设置接口调用任务
        /// </summary>
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Action setCallTask(Task callTask)
        {
            this.callTask = callTask;
            OfflineCount = Socket.OfflineCount;
            return onCompleted;
        }
        /// <summary>
        /// 任务完成检查
        /// </summary>
        protected void onCompletedAutoCancelKeep()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception == null) CancelKeep();
            else RemoveKeepCallback(exception);
        }
        /// <summary>
        /// 设置接口调用任务
        /// </summary>
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Action setCallTaskAutoCancelKeep(Task callTask)
        {
            this.callTask = callTask;
            OfflineCount = Socket.OfflineCount;
            return onCompletedAutoCancelKeep;
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackTask<T> CreateServerKeepCallbackTask(CommandServerSocket socket, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackTask<T>(socket, OfflineCount.Null, method);
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTask(CommandServerKeepCallbackTask<T> task, Task callTask)
        {
            //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted)
            {
                task.Socket.CheckOfflineCount();
                var exception = callTask.Exception;
                if (exception != null) task.RemoveKeepCallback(exception);
            }
            else callTask.GetAwaiter().UnsafeOnCompleted(task.setCallTask(callTask));
        }
        /// <summary>
        /// Check the completion status of the interface task
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckTaskAutoCancelKeep(CommandServerKeepCallbackTask<T> task, Task callTask)
        {
            //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted)
            {
                task.Socket.CheckOfflineCount();
                var exception = callTask.Exception;
                if (exception == null) task.CancelKeep();
                else task.RemoveKeepCallback(exception);
            }
            else callTask.GetAwaiter().UnsafeOnCompleted(task.setCallTaskAutoCancelKeep(callTask));
        }
    }
}
