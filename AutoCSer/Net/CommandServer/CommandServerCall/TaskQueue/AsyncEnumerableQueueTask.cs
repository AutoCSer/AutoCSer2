#if NetStandard21
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列保持回调任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AsyncEnumerableQueueTask<T> : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        [AllowNull]
        private CommandServerKeepCallbackCount<T> keepCallback;
        /// <summary>
        /// 回调任务
        /// </summary>
        private Task callTask;
        /// <summary>
        /// 服务端异步调用保持回调队列任务
        /// </summary>
        /// <param name="socket"></param>
        internal AsyncEnumerableQueueTask(CommandServerSocket socket) : base(socket, ServerMethodTypeEnum.AsyncEnumerableTaskQueue)
        {
            autoCancelKeep = true;
#if NetStandard21
            callTask = AutoCSer.Common.CompletedTask;
#endif
        }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception == null) keepCallback.CancelKeep();
            else Socket.RemoveKeepCallback(CallbackIdentity, exception);
        }
        /// <summary>
        /// 任务完成发送数据后调用下一个队列任务
        /// </summary>
        private void queueOnCompleted()
        {
            try
            {
                onCompleted();
            }
            finally { Queue.OnCompleted(this); }
        }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        /// <param name="asyncEnumerator"></param>
        /// <returns></returns>
#if NET8
        [MemberNotNull(nameof(callTask))]
#endif
        private bool checkCallTask(IAsyncEnumerator<T> asyncEnumerator)
        {
            callTask = keepCallback.CallbackAsync(asyncEnumerator);
            //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted)
            {
                onCompleted();
                return true;
            }
            callTask.GetAwaiter().UnsafeOnCompleted(queueOnCompleted);
            return false;
        }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        /// <param name="node"></param>
        /// <param name="asyncEnumerable"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool CheckCallTask(AsyncEnumerableQueueTask<T> node, IAsyncEnumerable<T> asyncEnumerable)
        {
            return node.checkCallTask(asyncEnumerable.GetAsyncEnumerator());
        }

        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(AsyncEnumerableQueueTask<T> task, ServerInterfaceMethod method, out CommandServerCallTaskQueue queue)
        {
            task.keepCallback = CommandServerKeepCallbackCount<T>.CreateServerKeepCallback(task, method);
            return task.GetSocket(out queue);
        }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
#if NetStandard21
        private CommandServerTaskQueueService? getTaskQueue(ServerInterfaceMethod method)
#else
        private CommandServerTaskQueueService getTaskQueue(ServerInterfaceMethod method)
#endif
        {
            keepCallback = CommandServerKeepCallbackCount<T>.CreateServerKeepCallback(this, method);
            return base.getTaskQueue();
        }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandServerTaskQueueService? GetTaskQueue(AsyncEnumerableQueueTask<T> task, ServerInterfaceMethod method)
#else
        internal static CommandServerTaskQueueService GetTaskQueue(AsyncEnumerableQueueTask<T> task, ServerInterfaceMethod method)
#endif
        {
            return task.getTaskQueue(method);
        }

        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        internal delegate CommandServerSocket GetSocketDelegate(AsyncEnumerableQueueTask<T> task, ServerInterfaceMethod method, out CommandServerCallTaskQueue queue);
    }
}
#endif
