using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP 服务器端异步保持回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerEnumerableKeepCallbackCountTask<T> : CommandServerKeepCallbackCount<T>
    {
        /// <summary>
        /// 接口调用任务
        /// </summary>
        protected Task<IEnumerable<T>> callTask;
        /// <summary>
        /// 回调任务
        /// </summary>
        private Task callbackTask;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="callTask"></param>
        private CommandServerEnumerableKeepCallbackCountTask(CommandServerSocket socket, ServerInterfaceMethod method, Task<IEnumerable<T>> callTask) : base(socket, method)
        {
            this.callTask = callTask;
#if NetStandard21
            callbackTask = CommandServerRunTask.NullTask;
#endif
            //TaskAwaiter<IEnumerable<T>> taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted) onCallCompleted();
            else callTask.GetAwaiter().UnsafeOnCompleted(onCallCompleted);
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerEnumerableKeepCallbackCountTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method)
        {
#if NetStandard21
            callTask = CommandServerRunTask<IEnumerable<T>>.NullTask;
            callbackTask = CommandServerRunTask.NullTask;
#endif
        }
        /// <summary>
        /// 接口调用完成
        /// </summary>
        protected void onCallCompleted()
        {
            var exception = callTask.Exception;
            if (exception == null)
            {
                IEnumerable<T> result = callTask.Result;
                if (result != null)
                {
                    callbackTask = EnumerableCallbackAsync(result);
                    //TaskAwaiter taskAwaiter = callbackTask.GetAwaiter();
                    if (callbackTask.IsCompleted) onCompleted();
                    else callbackTask.GetAwaiter().UnsafeOnCompleted(onCompleted);
                }
                else
                {
                    checkOfflineCount();
                    CancelKeep();
                }
            }
            else
            {
                checkOfflineCount();
                RemoveKeepCallback(exception);
            }
        }
        /// <summary>
        /// 任务完成检查
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = callbackTask.Exception;
            if (exception != null) RemoveKeepCallback(exception);
        }

        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CreateServerKeepCallbackTask(CommandServerSocket socket, ServerInterfaceMethod method, Task<IEnumerable<T>> callTask)
        {
            new CommandServerEnumerableKeepCallbackCountTask<T>(socket, method, callTask);
        }
    }
}
