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
    public abstract class CommandServerRunTask : CommandServerCallTask
    {
        /// <summary>
        /// 默认 null 任务
        /// </summary>
        internal static readonly Task NullTask = new Task(AutoCSer.Common.EmptyAction);

        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        private readonly ServerInterfaceMethod method;
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
        internal CommandServerRunTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, NullTask)
        {
            this.method = method;
            IsDeserialize = method.InputParameterType == null;
        }
        /// <summary>
        /// Get the Task object that executes the command
        /// 获取执行命令的 Task 对象
        /// </summary>
        /// <returns></returns>
        public abstract Task GetTask();
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
            }
            finally
            {
                if (object.ReferenceEquals(task, NullTask))
                {
                    if (method.CheckGetTaskException() == 0) Socket.Server.FreeTaskRunConcurrent();
                    RunTaskException(ServerMethodTypeEnum.Task, runTaskException.notNull());
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
        internal static void SetIsDeserialize(CommandServerRunTask task, bool isDeserialize)
        {
            task.IsDeserialize = isDeserialize;
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum RunTaskIsDeserialize(CommandServerRunTask task)
        {
            if (task.IsDeserialize)
            {
                Task.Run((Action)task.runTask);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
    }
    /// <summary>
    /// Task.Run 异步任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerRunTask<T> : CommandServerCallTask<T>
    {
        /// <summary>
        /// 默认 null 任务
        /// </summary>
#pragma warning disable CS8621
        internal static readonly Task<T> NullTask = new Task<T>(AutoCSer.Common.GetDefault<T>);
#pragma warning restore CS8621

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
        internal CommandServerRunTask(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method, NullTask)
        {
            IsDeserialize = method.InputParameterType == null;
        }
        /// <summary>
        /// Get the Task object that executes the command
        /// 获取执行命令的 Task 对象
        /// </summary>
        /// <returns></returns>
        public abstract Task<T> GetTask();
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
                if (task.IsCompleted) OnCompleted();
                else task.GetAwaiter().UnsafeOnCompleted(OnCompleted);
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
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isDeserialize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetIsDeserialize(CommandServerRunTask<T> task, bool isDeserialize)
        {
            task.IsDeserialize = isDeserialize;
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum RunTaskIsDeserialize(CommandServerRunTask<T> task)
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
