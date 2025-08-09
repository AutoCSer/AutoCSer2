using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 位图客户端节点返回值类型转换 await ResponseResult{bool}
    /// </summary>
    public sealed class BitmapNodeResponseAwaiter : INotifyCompletion
    {
        /// <summary>
        /// 返回参数
        /// </summary>
        private readonly ResponseParameterAwaiter<ValueResult<int>> awaiter;
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
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 位图客户端节点返回值类型转换 await ResponseResult{bool}
        /// </summary>
        /// <param name="awaiter"></param>
        private BitmapNodeResponseAwaiter(ResponseParameterAwaiter<ValueResult<int>> awaiter)
        {
            this.awaiter = awaiter;
            awaiter.OnCompleted(onCompleted);
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the call, return an error message before the result is returned (Only for supporting await)
        /// 获取调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        public ResponseResult<bool> GetResult()
        {
            ResponseResult<ValueResult<int>> result = awaiter.GetResult();
            if (result.IsSuccess)
            {
                if (result.Value.IsValue) return result.Value.Value != 0;
                return CallStateEnum.NoReturnValue;
            }
            return result.Cast<bool>();
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BitmapNodeResponseAwaiter GetAwaiter()
        {
            return this;
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
        /// 设置错误结果并尝试回调操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
        }

        /// <summary>
        /// 隐式类型转换
        /// </summary>
        /// <param name="response"></param>
        public static implicit operator BitmapNodeResponseAwaiter(ResponseParameterAwaiter<ValueResult<int>> response) { return new BitmapNodeResponseAwaiter(response); }
    }
}
