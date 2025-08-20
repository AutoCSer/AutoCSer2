using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回值 await T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ResponseReturnValue<T> : INotifyCompletion
    {
        /// <summary>
        /// 返回参数
        /// </summary>
        public readonly ResponseParameterAwaiter<T> Response;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return Response.IsCompleted; } }
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType { get { return Response.Command.ReturnType; } }
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage { get { return Response.Command.ErrorMessage; } }
#else
        public string ErrorMessage{ get { return Response.Command.ErrorMessage; } }
#endif
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum State { get { return Response.Command.ReturnValue.State;  } }
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        public T? ReturnValue { get { return Response.Value.ReturnValue; } }
#else
        public T ReturnValue { get { return Response.Value.ReturnValue; } }
#endif
        /// <summary>
        /// Whether errors and exceptions are ignored
        /// 是否忽略错误与异常
        /// </summary>
        private readonly bool isIgnoreError;
        /// <summary>
        /// 命令返回值
        /// </summary>
        /// <param name="response">The return value command
        /// 返回值命令</param>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        internal ResponseReturnValue(ResponseParameterAwaiter<T> response, bool isIgnoreError)
        {
            Response = response;
            this.isIgnoreError = isIgnoreError;
            if (!response.IsCompleted) response.OnCompleted(onCompleted);
            else continuation = Common.EmptyAction;
        }
        /// <summary>
        /// 完成回调操作
        /// </summary>
        private void onCompleted()
        {
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<T?> Wait()
#else
        public async Task<T> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public T? GetResult()
#else
        public T GetResult()
#endif
        {
            if (Response.Command.ReturnValue.State == CallStateEnum.Success)
            {
                if (Response.Command.ReturnType == CommandClientReturnTypeEnum.Success) return Response.Value.ReturnValue;
                if (!isIgnoreError) throw new Exception($"调用返回类型错误 {Response.Command.ReturnType} {ErrorMessage}");
            }
            else if (!isIgnoreError) throw new Exception($"调用返回状态错误 {Response.Command.ReturnValue.State} {ErrorMessage}");
            return default(T);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseReturnValue<T> GetAwaiter()
        {
            return this;
        }
    }
}
