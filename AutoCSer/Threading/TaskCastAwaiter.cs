using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Task 返回值类型转换 await T
    /// </summary>
    /// <typeparam name="T">await 返回值类型</typeparam>
    public abstract class TaskCastAwaiter<T> : INotifyCompletion
    {
        /// <summary>
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// 任务执行结果
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T result;
        /// <summary>
        /// 是否存在执行结果
        /// </summary>
        public bool IsResult { get; protected set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 执行异常信息
        /// </summary>
        public abstract Exception Exception { get; }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<T> Wait()
        {
            return await this;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            if (IsResult) return result;
            throw Exception;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TaskCastAwaiter<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 设置返回结果
        /// </summary>
        /// <param name="result"></param>
        protected void setResult(T result)
        {
            this.result = result;
            IsResult = true;
            setCompleted();
        }
        /// <summary>
        /// 设置完成状态
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void setCompleted()
        {
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// 设置返回结果并尝试回调操作
        /// </summary>
        /// <param name="result"></param>
        protected void onCompleted(T result)
        {
            this.result = result;
            IsResult = true;
            onCompleted();
        }
        /// <summary>
        /// 设置错误结果并尝试回调操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
        }
    }
    /// <summary>
    /// Task 返回值类型转换 Awaiter
    /// </summary>
    /// <typeparam name="T">await 返回值类型</typeparam>
    /// <typeparam name="TT">Task 返回值类型</typeparam>
    public abstract class TaskCastAwaiter<T, TT> : TaskCastAwaiter<T>
    {
        /// <summary>
        /// 执行 Task
        /// </summary>
        private readonly Task<TT> task;
        /// <summary>
        /// 数据转换异常信息
        /// </summary>
#if NetStandard21
        private Exception? exception;
#else
        private Exception exception;
#endif
        /// <summary>
        /// 执行异常信息
        /// </summary>
        public override Exception Exception
        {
            get { return exception ?? task.Exception.notNull(); }
        }
        /// <summary>
        /// Task 返回值类型转换 Awaiter
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isCheck"></param>
        protected TaskCastAwaiter(Task<TT> task, bool isCheck)
        {
            this.task = task;
            if (isCheck) check();
        }
        /// <summary>
        /// Task 返回值类型转换 Awaiter
        /// </summary>
        /// <param name="task"></param>
        public TaskCastAwaiter(Task<TT> task) : this(task, true) { }
        /// <summary>
        /// 检查 Task 是否完成
        /// </summary>
        protected void check()
        {
            if (task.IsCompleted)
            {
                if (task.Exception == null) setResult(GetResult(task.Result));
                else setCompleted();
            }
            else task.GetAwaiter().UnsafeOnCompleted(onCompleted);
        }
        /// <summary>
        /// Task 执行完成
        /// </summary>
        private new void onCompleted()
        {
            if (task.Exception == null)
            {
                bool isResult = false;
                try
                {
                    T result = GetResult(task.Result);
                    isResult = true;
                    onCompleted(result);
                }
                catch (Exception exception)
                {
                    if (isResult)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                        return;
                    }
                    this.exception = exception;
                }
            }
            base.onCompleted();
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public abstract T GetResult(TT result);
    }
}
