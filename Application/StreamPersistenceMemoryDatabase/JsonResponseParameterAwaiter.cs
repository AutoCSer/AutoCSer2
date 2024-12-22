using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// JSON 返回参数 await ResponseValueResult{T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class JsonResponseParameterAwaiter<T> : ResponseParameter, INotifyCompletion
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
        /// 返回值命令
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private ReturnCommand<ResponseParameter> command;
        /// <summary>
        /// 返回对象
        /// </summary>
#if NetStandard21
        private T? value;
#else
        private T value;
#endif
        /// <summary>
        /// JSON 返回参数
        /// </summary>
        internal JsonResponseParameterAwaiter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.CommandClientDeserializeJsonBuffer(ref value);
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
        /// 设置返回参数
        /// </summary>
        /// <param name="responseParameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ResponseParameterAwaiter<ResponseParameter> responseParameter)
        {
            command = responseParameter.Command;
            responseParameter.OnCompleted(onCompleted);
        }
        /// <summary>
        /// 返回值完成
        /// </summary>
        private void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<ResponseValueResult<T?>> Wait()
#else
        public async Task<ResponseValueResult<T>> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public ResponseValueResult<T?> GetResult()
        {
            if (command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                CallStateEnum state = command.ReturnValue.State;
                switch (state)
                {
                    case CallStateEnum.Success: return new ResponseValueResult<T?>(value);
                    case CallStateEnum.NullResponseParameter: return new ResponseValueResult<T?>(false);
                    default: return new ResponseValueResult<T?>(state);
                }
            }
            return new ResponseValueResult<T?>(command.ReturnType, command.ErrorMessage);
        }
#else
        public ResponseValueResult<T> GetResult()
        {
            if (command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                CallStateEnum state = command.ReturnValue.State;
                switch (state)
                {
                    case CallStateEnum.Success: return new ResponseValueResult<T>(value);
                    case CallStateEnum.NullResponseParameter: return new ResponseValueResult<T>(false);
                    default: return new ResponseValueResult<T>(state);
                }
            }
            return new ResponseValueResult<T>(command.ReturnType, command.ErrorMessage);
        }
#endif
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public JsonResponseParameterAwaiter<T> GetAwaiter()
        {
            return this;
        }
    }
}
