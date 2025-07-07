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
    /// Task.Run 异步任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerEnumerableKeepCallbackCountRunTask<T> : CommandServerEnumerableKeepCallbackCountTask<T>
    {
        /// <summary>
        /// Whether the parameters have been deserialized successfully
        /// 参数是否反序列化成功
        /// </summary>
        internal bool IsDeserialize;
        /// <summary>
        /// Task.Run 异步任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerEnumerableKeepCallbackCountRunTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method)
        {
            IsDeserialize = method.InputParameterType == null;
        }
        /// <summary>
        /// Get the Task object that executes the command
        /// 获取执行命令的 Task 对象
        /// </summary>
        /// <returns></returns>
        public abstract Task<IEnumerable<T>> GetTask();
        /// <summary>
        /// 任务调用
        /// </summary>
        private void runTask()
        {
            var runTaskException = default(Exception);
            try
            {
                long timestamp = Stopwatch.GetTimestamp();
                callTask = GetTask();
                if (Method.CheckGetTaskTimestamp(timestamp) == 0) Socket.Server.FreeTaskRunConcurrent();
                if (callTask.IsCompleted) onCallCompleted();
                else callTask.GetAwaiter().UnsafeOnCompleted(onCallCompleted);
            }
            catch (Exception exception)
            {
                runTaskException = exception;
            }
            finally
            {
                if (object.ReferenceEquals(callTask, CommandServerRunTask<IEnumerable<T>>.NullTask))
                {
                    if (Method.CheckGetTaskException() == 0) Socket.Server.FreeTaskRunConcurrent();
                    RunTaskException(ServerMethodTypeEnum.EnumerableKeepCallbackCountTask, runTaskException.notNull());
                }
            }
        }
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isDeserialize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetIsDeserialize(CommandServerEnumerableKeepCallbackCountRunTask<T> task, bool isDeserialize)
        {
            task.IsDeserialize = isDeserialize;
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum RunTaskIsDeserialize(CommandServerEnumerableKeepCallbackCountRunTask<T> task)
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
