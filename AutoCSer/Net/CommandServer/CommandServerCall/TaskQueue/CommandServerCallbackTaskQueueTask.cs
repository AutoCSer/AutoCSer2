using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    public abstract class CommandServerCallbackTaskQueueTask : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
        private Task callTask;
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerCallbackTaskQueueTask(CommandServerSocket socket) : base(socket, ServerMethodTypeEnum.CallbackTaskQueue)
        {
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
            //checkOfflineCount();
            var exception = callTask.Exception;
            if (exception != null)
            {
                //Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
            }
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
        internal static bool CheckCallTask(CommandServerCallbackTaskQueueTask node, Task callTask)
        {
            return node.checkCallTask(callTask);
        }
    }
}
