using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Task.Run 异步任务
    /// </summary>
    public abstract class CommandServerVerifyStateRunTask : CommandServerRunTask<CommandServerVerifyStateEnum>
    {
        /// <summary>
        /// Task.Run 异步任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerVerifyStateRunTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method)
        {
            IsDeserialize = method.InputParameterType == null;
        }
        /// <summary>
        /// 异步任务完成回调
        /// </summary>
        private void onCompleted()
        {
            checkOfflineCount();
            var exception = task.Exception;
            if (exception == null)
            {
                if (Socket.SetVerifyState(task.Result)) Socket.Send(CallbackIdentity, Method, new ServerReturnValue<CommandServerVerifyStateEnum>(Socket.VerifyState));
            }
            else Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        private void runTask()
        {
            var runTaskException = default(Exception);
            try
            {
                long timestamp = Stopwatch.GetTimestamp();
                task = GetTask();
                if (Method.CheckGetTaskTimestamp(timestamp) == 0) Socket.Server.FreeTaskRunConcurrent();
                if (task.IsCompleted) onCompleted();
                else task.GetAwaiter().UnsafeOnCompleted(onCompleted);
            }
            catch (Exception exception)
            {
                runTaskException = exception;
            }
            finally
            {
                if (object.ReferenceEquals(task, NullTask))
                {
                    if (Method.CheckGetTaskException() == 0) Socket.Server.FreeTaskRunConcurrent();
                    RunTaskException(ServerMethodTypeEnum.Task, runTaskException.notNull());
                }
            }
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum RunTaskIsDeserialize(CommandServerVerifyStateRunTask task)
        {
            if (task.IsDeserialize)
            {
                Task.Run((Action)task.runTask);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
    }
}
