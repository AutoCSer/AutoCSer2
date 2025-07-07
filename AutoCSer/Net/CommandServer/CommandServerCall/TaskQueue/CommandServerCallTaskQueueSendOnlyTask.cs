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
    public abstract class CommandServerCallTaskQueueSendOnlyTask : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private Task<CommandServerSendOnly> callTask;
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerCallTaskQueueSendOnlyTask(CommandServerSocket socket) : base(socket, ServerMethodTypeEnum.SendOnlyTaskQueue) { }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            checkOfflineCount();
            Socket.CloseShortLink();
            var exception = callTask.Exception;
            if (exception != null) Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
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
        private bool checkCallTask(Task<CommandServerSendOnly> callTask)
        {
            this.callTask = callTask;
            //TaskAwaiter<CommandServerSendOnly> taskAwaiter = callTask.GetAwaiter();
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
        internal static bool CheckCallTask(CommandServerCallTaskQueueSendOnlyTask node, Task<CommandServerSendOnly> callTask)
        {
            return node.checkCallTask(callTask);
        }
    }
}
