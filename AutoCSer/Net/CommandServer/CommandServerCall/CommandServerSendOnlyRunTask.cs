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
    public abstract class CommandServerSendOnlyRunTask : CommandServerCallSendOnlyTask
    {
        /// <summary>
        /// 服务端接口方法信息
        /// </summary>
        private readonly ServerInterfaceMethod method;
        /// <summary>
        /// 参数是否反序列化成功
        /// </summary>
        internal bool IsDeserialize;
        /// <summary>
        /// Task.Run 异步任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerSendOnlyRunTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, CommandServerRunTask<CommandServerSendOnly>.NullTask)
        {
            this.method = method;
            IsDeserialize = method.InputParameterType == null;
        }
        /// <summary>
        /// 获取 Task
        /// </summary>
        /// <returns></returns>
        public abstract Task<CommandServerSendOnly> GetTask();
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
                if (method.CheckGetTaskTimestamp(timestamp) == 0) Socket.Server.FreeTaskRunConcurrent();
                if (task.IsCompleted) OnCompleted();
                else task.GetAwaiter().UnsafeOnCompleted(OnCompleted);
            }
            catch (Exception exception)
            {
                runTaskException = exception;
                Socket.CloseShortLink();
            }
            finally
            {
                if (object.ReferenceEquals(task, CommandServerRunTask<CommandServerSendOnly>.NullTask))
                {
                    if (method.CheckGetTaskException() == 0) Socket.Server.FreeTaskRunConcurrent();
                    RunTaskException(ServerMethodTypeEnum.SendOnlyTask, runTaskException.notNull());
                }
            }
        }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isDeserialize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetIsDeserialize(CommandServerSendOnlyRunTask task, bool isDeserialize)
        {
            task.IsDeserialize = isDeserialize;
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum RunTaskIsDeserialize(CommandServerSendOnlyRunTask task)
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
