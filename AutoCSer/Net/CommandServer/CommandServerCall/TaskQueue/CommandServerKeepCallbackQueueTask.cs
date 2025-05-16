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
    public abstract class CommandServerKeepCallbackQueueTask : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
        private Task callTask;
        /// <summary>
        /// TCP 服务器端异步保持回调
        /// </summary>
        private CommandServerKeepCallback keepCallback;
        /// <summary>
        /// 服务端异步调用保持回调队列任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="methodType"></param>
        /// <param name="autoCancelKeep"></param>
        internal CommandServerKeepCallbackQueueTask(CommandServerSocket socket, ServerMethodTypeEnum methodType, bool autoCancelKeep) : base(socket, methodType)
        {
            this.autoCancelKeep = autoCancelKeep;
#if NetStandard21
            callTask = AutoCSer.Common.CompletedTask;
            keepCallback = CommandServerKeepCallback.Null;
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
            if (exception == null)
            {
                if (autoCancelKeep) keepCallback.CancelKeep();
                //Socket.Send(CallbackIdentity, CommandClientReturnType.Success);
            }
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
        /// <param name="callTask"></param>
        /// <returns></returns>
        private bool checkCallTask(Task callTask)
        {
            this.callTask = callTask;
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
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool CheckCallTask(CommandServerKeepCallbackQueueTask node, Task callTask)
        {
            return node.checkCallTask(callTask);
        }

        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="keepCallback"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(CommandServerKeepCallbackQueueTask task, CommandServerKeepCallback keepCallback, out CommandServerCallTaskQueue queue)
        {
            task.keepCallback = keepCallback;
            return task.GetSocket(out queue);
        }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private CommandServerTaskQueueService? getTaskQueue(CommandServerKeepCallback keepCallback)
#else
        private CommandServerTaskQueueService getTaskQueue(CommandServerKeepCallback keepCallback)
#endif
        {
            this.keepCallback = keepCallback;
            return base.getTaskQueue();
        }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="keepCallback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandServerTaskQueueService? GetTaskQueue(CommandServerKeepCallbackQueueTask task, CommandServerKeepCallback keepCallback)
#else
        internal static CommandServerTaskQueueService GetTaskQueue(CommandServerKeepCallbackQueueTask task, CommandServerKeepCallback keepCallback)
#endif
        {
            return task.getTaskQueue(keepCallback);
        }
    }
    /// <summary>
    /// 服务端异步调用队列保持回调任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerKeepCallbackQueueTask<T> : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private Task<IEnumerable<T>> callTask;
        /// <summary>
        /// TCP 服务器端异步保持回调
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private CommandServerKeepCallbackCount<T> keepCallback;
        /// <summary>
        /// 回调任务
        /// </summary>
        private Task callbackTask;
        /// <summary>
        /// 服务端异步调用保持回调队列任务
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerKeepCallbackQueueTask(CommandServerSocket socket) : base(socket, ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue)
        {
            autoCancelKeep = true;
#if NetStandard21
            callbackTask = AutoCSer.Common.CompletedTask;
#endif
        }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = callbackTask.Exception;
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
        /// 调用接口完成
        /// </summary>
        private void onCallCompleted()
        {
            bool callQueueOnCompleted = true;
            try
            {
                var exception = callTask.Exception;
                if (exception == null)
                {
                    IEnumerable<T> result = callTask.Result;
                    if (result != null)
                    {
                        callbackTask = keepCallback.EnumerableCallbackAsync(result);
                        //TaskAwaiter callbackAwaiter = callbackTask.GetAwaiter();
                        if (callbackTask.IsCompleted) onCompleted();
                        else
                        {
                            callbackTask.GetAwaiter().UnsafeOnCompleted(queueOnCompleted);
                            callQueueOnCompleted = false;
                        }
                    }
                    else
                    {
                        checkOfflineCount();
                        keepCallback.CancelKeep();
                    }
                }
                else
                {
                    checkOfflineCount();
                    Socket.RemoveKeepCallback(CallbackIdentity, exception);
                }
            }
            finally
            {
                if (callQueueOnCompleted) Queue.OnCompleted(this);
            }
        }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        /// <param name="callTask"></param>
        /// <returns></returns>
        private bool checkCallTask(Task<IEnumerable<T>> callTask)
        {
            this.callTask = callTask;
            //TaskAwaiter<IEnumerable<T>> taskAwaiter = callTask.GetAwaiter();
            if (callTask.IsCompleted)
            {
                var exception = callTask.Exception;
                if (exception == null)
                {
                    IEnumerable<T> result = callTask.Result;
                    if (result != null)
                    {
                        callbackTask = keepCallback.EnumerableCallbackAsync(result);
                        //TaskAwaiter callbackAwaiter = callbackTask.GetAwaiter();
                        if (callbackTask.IsCompleted)
                        {
                            onCompleted();
                            return true;
                        }
                        callbackTask.GetAwaiter().UnsafeOnCompleted(queueOnCompleted);
                        return false;
                    }
                    checkOfflineCount();
                    keepCallback.CancelKeep();
                }
                else
                {
                    checkOfflineCount();
                    Socket.RemoveKeepCallback(CallbackIdentity, exception);
                }
                return true;
            }
            callTask.GetAwaiter().UnsafeOnCompleted(onCallCompleted);
            return false;
        }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callTask"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool CheckCallTask(CommandServerKeepCallbackQueueTask<T> node, Task<IEnumerable<T>> callTask)
        {
            return node.checkCallTask(callTask);
        }

        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(CommandServerKeepCallbackQueueTask<T> task, ServerInterfaceMethod method, out CommandServerCallTaskQueue queue)
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
        internal static CommandServerTaskQueueService? GetTaskQueue(CommandServerKeepCallbackQueueTask<T> task, ServerInterfaceMethod method)
#else
        internal static CommandServerTaskQueueService GetTaskQueue(CommandServerKeepCallbackQueueTask<T> task, ServerInterfaceMethod method)
#endif
        {
            return task.getTaskQueue(method);
        }

        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        internal delegate CommandServerSocket GetSocketDelegate(CommandServerKeepCallbackQueueTask<T> task, ServerInterfaceMethod method, out CommandServerCallTaskQueue queue);
    }
}
