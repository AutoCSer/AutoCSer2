using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回字符串参数 await ResponseValueResult{string}
    /// </summary>
    public sealed class StringResponseParameterAwaiter : ResponseParameter, INotifyCompletion
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
        /// 字符串
        /// </summary>
#if NetStandard21
        private string? value;
#else
        private string value;
#endif
        /// <summary>
        /// 返回字符串参数
        /// </summary>
        internal StringResponseParameterAwaiter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref value);
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
        public async Task<ResponseValueResult<string?>> Wait()
#else
        public async Task<ResponseValueResult<string>> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// 获取命令调用结果
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
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StringResponseParameterAwaiter GetAwaiter()
        {
            return this;
        }
    }
}
