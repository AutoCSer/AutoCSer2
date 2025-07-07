using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 接口任务队列节点
    /// </summary>
    public abstract class InterfaceControllerTaskQueueNodeBase : AutoCSer.Threading.Link<InterfaceControllerTaskQueueNodeBase>, INotifyCompletion
    {
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
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        protected CommandClientReturnTypeEnum returnType;
        /// <summary>
        /// 客户端 await 等待返回值回调线程模式
        /// </summary>
        private readonly ClientCallbackTypeEnum callbackType;
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueNodeBase(ClientCallbackTypeEnum callbackType)
        {
            this.callbackType = callbackType;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 执行任务（需要主动调用 SetReturn）
        /// </summary>
        public abstract void RunTask();
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void RunTask(ref InterfaceControllerTaskQueueNodeBase? next)
#else
        internal void RunTask(ref InterfaceControllerTaskQueueNodeBase next)
#endif
        {
            next = LinkNext;
            LinkNext = null;
            RunTask();
        }
#if DEBUG
        /// <summary>
        /// 设置返回值类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="isSynchronousThread"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
#if NetStandard21
        internal void SetReturnType(CommandClientReturnTypeEnum returnType, bool isSynchronousThread, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        internal void SetReturnType(CommandClientReturnTypeEnum returnType, bool isSynchronousThread, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
#else
        /// <summary>
        /// 设置返回值类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="isSynchronousThread"></param>
        internal void SetReturnType(CommandClientReturnTypeEnum returnType, bool isSynchronousThread)
#endif
        {
            this.returnType = returnType;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                try
                {
#if DEBUG
                    Command.Callback(continuation, callbackType, isSynchronousThread, callerMemberName, callerFilePath, callerLineNumber);
#else
                    Command.Callback(continuation, callbackType, isSynchronousThread);
#endif
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
    }
}
