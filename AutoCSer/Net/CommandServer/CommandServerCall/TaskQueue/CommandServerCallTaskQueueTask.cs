using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    public abstract class CommandServerCallTaskQueueTask : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
        private Task callTask;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerCallTaskQueueTask(CommandServerSocket socket) : base(socket, ServerMethodTypeEnum.TaskQueue)
        {
#if NetStandard21
            callTask = AutoCSer.Common.CompletedTask;
#endif
        }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception == null) Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success);
            else Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
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
        internal static bool CheckCallTask(CommandServerCallTaskQueueTask node, Task callTask)
        {
            return node.checkCallTask(callTask);
        }
    }
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerCallTaskQueueTask<T> : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 服务端接口方法信息
        /// </summary>
        private readonly ServerInterfaceMethod method;
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private Task<T> callTask;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerCallTaskQueueTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, ServerMethodTypeEnum.TaskQueue)
        {
            this.method = method;
        }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = callTask.Exception;
            if (exception == null) Socket.Send(CallbackIdentity, method, new ServerReturnValue<T>(callTask.Result));
            else Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
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
        private bool checkCallTask(Task<T> callTask)
        {
            this.callTask = callTask;
            //TaskAwaiter<T> taskAwaiter = callTask.GetAwaiter();
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
        internal static bool CheckCallTask(CommandServerCallTaskQueueTask<T> node, Task<T> callTask)
        {
           return node.checkCallTask(callTask);
        }
    }
}
