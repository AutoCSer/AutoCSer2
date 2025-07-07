using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    public abstract class CommandServerCallTaskQueueCustomTaskNode : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 获取执行任务
        /// </summary>
        private readonly Func<CommandServerCallTaskQueue, Task> getTask;
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
        private Task callTask;
        /// <summary>
        /// 任务执行异常
        /// </summary>
#if NetStandard21
        protected Exception? exception;
#else
        protected Exception exception;
#endif
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 是否同步执行回调
        /// </summary>
        private bool isSynchronous;
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        protected CommandServerCallTaskQueueCustomTaskNode(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous)
        {
            this.getTask = getTask;
            this.isSynchronous = isSynchronous;
#if NetStandard21
            callTask = AutoCSer.Common.CompletedTask;
#endif
        }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        protected CommandServerCallTaskQueueCustomTaskNode(Func<Task> getTask, bool isSynchronous)
        {
            this.getTask = new CommandServer.CommandServerCallTaskQueueFunc(getTask).GetTask;
            this.isSynchronous = isSynchronous;
#if NetStandard21
            callTask = AutoCSer.Common.CompletedTask;
#endif

        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public override bool RunTask()
        {
            try
            {
                callTask = getTask(Queue);
                //TaskAwaiter taskAwaiter = callTask.GetAwaiter();
                if (callTask.IsCompleted)
                {
                    exception = callTask.Exception;
                    onCompleted();
                    return true;
                }
                callTask.GetAwaiter().UnsafeOnCompleted(queueOnCompleted);
                return false;
            }
            catch (Exception exception)
            {
                this.exception = exception;
                onCompleted();
                return true;
            }
        }
        /// <summary>
        /// 任务完成发送数据后调用下一个队列任务
        /// </summary>
        private void queueOnCompleted()
        {
            try
            {
                exception = callTask.Exception;
                onCompleted();
            }
            finally { Queue.OnCompleted(this); }
        }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        private void onCompleted()
        {
            //checkOfflineCount();
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (isSynchronous)
                {
                    try
                    {
                        continuation();
                    }
                    catch(Exception exception)
                    {
                        Server.Log.ExceptionIgnoreException(exception);
                    }
                }
                else Task.Run(continuation);
            }
        }
    }
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerCallTaskQueueCustomTaskNode<T> : CommandServerCallTaskQueueNode
    {
        /// <summary>
        /// 获取执行任务
        /// </summary>
        private readonly Func<CommandServerCallTaskQueue, Task<T>> getTask;
        /// <summary>
        /// 接口返回返回任务
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private Task<T> callTask;
        /// <summary>
        /// 任务返回值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T value;
        /// <summary>
        /// 任务执行异常
        /// </summary>
#if NetStandard21
        protected Exception? exception;
#else
        protected Exception exception;
#endif
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 是否同步执行回调
        /// </summary>
        private bool isSynchronous;
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        protected CommandServerCallTaskQueueCustomTaskNode(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous)
        {
            this.getTask = getTask;
            this.isSynchronous = isSynchronous;
        }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        protected CommandServerCallTaskQueueCustomTaskNode(Func<Task<T>> getTask, bool isSynchronous)
        {
            this.getTask = new CommandServer.CommandServerCallTaskQueueFunc<T>(getTask).GetTask;
            this.isSynchronous = isSynchronous;
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public override bool RunTask()
        {
            try
            {
                callTask = getTask(Queue);
                //TaskAwaiter<T> taskAwaiter = callTask.GetAwaiter();
                if (callTask.IsCompleted)
                {
                    exception = callTask.Exception;
                    if (exception == null) value = callTask.Result;
                    onCompleted();
                    return true;
                }
                callTask.GetAwaiter().UnsafeOnCompleted(queueOnCompleted);
                return false;
            }
            catch (Exception exception)
            {
                this.exception = exception;
                onCompleted();
                return true;
            }
        }
        /// <summary>
        /// 任务完成发送数据后调用下一个队列任务
        /// </summary>
        private void queueOnCompleted()
        {
            try
            {
                exception = callTask.Exception;
                if (exception == null) value = callTask.Result;
                onCompleted();
            }
            finally { Queue.OnCompleted(this); }
        }
        /// <summary>
        /// 任务完成发送数据
        /// </summary>
        private void onCompleted()
        {
            //checkOfflineCount();
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (isSynchronous)
                {
                    try
                    {
                        continuation();
                    }
                    catch { }
                }
                else Task.Run(continuation);
            }
        }
    }
}
