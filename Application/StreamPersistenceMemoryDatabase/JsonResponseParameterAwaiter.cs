using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// await ResponseValueResult{T}, which returns JSON serialized data
    /// await ResponseValueResult{T}，返回 JSON 序列化数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class JsonResponseParameterAwaiter<T> : ResponseParameter, INotifyCompletion
    {
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
        /// The return value command
        /// 返回值命令
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private ReturnCommand<ResponseParameter> command;
        /// <summary>
        /// Return data
        /// </summary>
#if NetStandard21
        private T? value;
#else
        private T value;
#endif
        /// <summary>
        /// Return the JSON serialized data
        /// 返回 JSON 序列化数据
        /// </summary>
        internal JsonResponseParameterAwaiter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeJsonBuffer(ref value);
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
        /// Set the return parameters
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
        /// The asynchronous operation has been completed
        /// 异步操作已完成
        /// </summary>
        private void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// Wait for the command call to return the result
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
        /// Get the result of the command call
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
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public JsonResponseParameterAwaiter<T> GetAwaiter()
        {
            return this;
        }
    }
}
