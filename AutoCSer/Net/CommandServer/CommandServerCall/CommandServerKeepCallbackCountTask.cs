using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP 服务器端异步保持回调
    /// </summary>
    public class CommandServerKeepCallbackCountTask : CommandServerKeepCallbackCount
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task callTask;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        /// <param name="outputCount"></param>
        private CommandServerKeepCallbackCountTask(CommandServerSocket socket, OfflineCount offlineCount, int outputCount) : base(socket, offlineCount, outputCount)
        {
#if NetStandard21
            callTask = CommandServerRunTask.NullTask;
#endif
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCountTask(CommandServerSocket socket, int outputCount) : base(socket, outputCount)
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
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCountTask CreateServerKeepCallbackTask(CommandServerSocket socket, int outputCount)
        {
            return new CommandServerKeepCallbackCountTask(socket, OfflineCount.Null, outputCount);
        }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTask(CommandServerKeepCallbackCountTask task, Task callTask)
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
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTaskAutoCancelKeep(CommandServerKeepCallbackCountTask task, Task callTask)
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
    /// TCP 服务器端异步保持回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerKeepCallbackCountTask<T> : CommandServerKeepCallbackCount<T>
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task callTask;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallbackCountTask(CommandServerSocket socket, OfflineCount offlineCount, ServerInterfaceMethod method) : base(socket, offlineCount, method)
        {
#if NetStandard21
            callTask = CommandServerRunTask.NullTask;
#endif
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerKeepCallbackCountTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method)
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
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCountTask<T> CreateServerKeepCallbackTask(CommandServerSocket socket, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackCountTask<T>(socket, OfflineCount.Null, method);
        }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTask(CommandServerKeepCallbackCountTask<T> task, Task callTask)
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
        /// 检查接口任务完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callTask"></param>
        internal static void CheckTaskAutoCancelKeep(CommandServerKeepCallbackCountTask<T> task, Task callTask)
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
