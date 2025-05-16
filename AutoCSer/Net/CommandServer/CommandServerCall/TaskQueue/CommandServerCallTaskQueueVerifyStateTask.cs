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
    public abstract class CommandServerCallTaskQueueVerifyStateTask : CommandServerCallTaskQueueNode
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
        private Task<CommandServerVerifyStateEnum> callTask;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerCallTaskQueueVerifyStateTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, ServerMethodTypeEnum.TaskQueue)
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
            if (exception == null)
            {
                if (Socket.SetVerifyState(callTask.Result))
                {
                    Socket.Send(CallbackIdentity, method, new ServerReturnValue<CommandServerVerifyStateEnum>(Socket.VerifyState));
                }
            }
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
        private bool checkCallTask(Task<CommandServerVerifyStateEnum> callTask)
        {
            this.callTask = callTask;
            //TaskAwaiter<CommandServerVerifyStateEnum> taskAwaiter = callTask.GetAwaiter();
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
        internal static bool CheckCallTask(CommandServerCallTaskQueueVerifyStateTask node, Task<CommandServerVerifyStateEnum> callTask)
        {
            return node.checkCallTask(callTask);
        }
    }
}
