#if NetStandard21
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP 服务器端异步保持回调任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CommandServerAsyncEnumerableTask<T> : CommandServerKeepCallbackCount<T>
    {
        /// <summary>
        /// 回调任务
        /// </summary>
        private readonly Task callTask;
        /// <summary>
        /// TCP 服务器端异步保持回调任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="asyncEnumerator"></param>
        private CommandServerAsyncEnumerableTask(CommandServerSocket socket, ServerInterfaceMethod method, IAsyncEnumerator<T> asyncEnumerator) : base(socket, method)
        {
            callTask = CallbackAsync(asyncEnumerator);

            //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted) onCompleted();
            else callTask.GetAwaiter().UnsafeOnCompleted(onCompleted);
        }
        /// <summary>
        /// 任务完成检查
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception != null) RemoveKeepCallback(exception);
        }

        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <param name="asyncEnumerable"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CreateServerKeepCallbackTask(CommandServerSocket socket, ServerInterfaceMethod method, IAsyncEnumerable<T> asyncEnumerable)
        {
            new CommandServerAsyncEnumerableTask<T>(socket, method, asyncEnumerable.GetAsyncEnumerator());
        }
    }
}
#endif
