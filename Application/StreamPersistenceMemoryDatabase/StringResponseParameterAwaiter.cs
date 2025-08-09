using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// await ResponseValueResult{string}, which returns string data
    /// await ResponseValueResult{string}，返回字符串数据
    /// </summary>
    public sealed class StringResponseParameterAwaiter : ResponseParameter, INotifyCompletion
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
        /// Return string
        /// </summary>
#if NetStandard21
        private string? value;
#else
        private string value;
#endif
        /// <summary>
        /// Return string data
        /// 返回字符串数据
        /// </summary>
        internal StringResponseParameterAwaiter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref value);
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
        public async Task<ResponseValueResult<string?>> Wait()
#else
        public async Task<ResponseValueResult<string>> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the call, return an error message before the result is returned (Only for supporting await)
        /// 获取调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public ResponseValueResult<string?> GetResult()
        {
            if (command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                CallStateEnum state = command.ReturnValue.State;
                switch (state)
                {
                    case CallStateEnum.Success: return new ResponseValueResult<string?>(value);
                    case CallStateEnum.NullResponseParameter: return new ResponseValueResult<string?>(false);
                    default: return new ResponseValueResult<string?>(state);
                }
            }
            return new ResponseValueResult<string?>(command.ReturnType, command.ErrorMessage);
        }
#else
        public ResponseValueResult<string> GetResult()
        {
            if (command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                CallStateEnum state = command.ReturnValue.State;
                switch (state)
                {
                    case CallStateEnum.Success: return new ResponseValueResult<string>(value);
                    case CallStateEnum.NullResponseParameter: return new ResponseValueResult<string>(false);
                    default: return new ResponseValueResult<string>(state);
                }
            }
            return new ResponseValueResult<string>(command.ReturnType, command.ErrorMessage);
        }
#endif
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StringResponseParameterAwaiter GetAwaiter()
        {
            return this;
        }
    }
}
