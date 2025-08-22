using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// await ResponseResult, which returns the result of the call status
    /// await ResponseResult，返回调用状态结果
    /// </summary>
    public sealed class ResponseResultAwaiter : ResponseParameterAwaiter
    {
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        internal readonly ReturnCommand<CallStateEnum> Command;
        /// <summary>
        /// Return the result of the call status
        /// 返回调用状态结果
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="command">The return value command
        /// 返回值命令</param>
        internal ResponseResultAwaiter(ClientNode node, ReturnCommand<CallStateEnum> command) : base(node)
        {
            this.Command = command;
            command.OnCompleted(onCommandCompleted);
        }
        ///// <summary>
        ///// Return the result of the call status
        ///// 返回调用状态结果
        ///// </summary>
        ///// <param name="command">The return value command
        ///// 返回值命令</param>
        //internal ResponseResultAwaiter(ReturnCommand<CallStateEnum> command) : base()
        //{
        //    this.Command = command;
        //    command.OnCompleted(onCommandCompleted);
        //}

        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<ResponseResult> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the call, return an error message before the result is returned (Only for supporting await)
        /// 获取调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseResult GetResult()
        {
            if (Command.ReturnType == CommandClientReturnTypeEnum.Success) return Command.ReturnValue;
            return new ResponseResult(Command.ReturnType, Command.ErrorMessage);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseResultAwaiter GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// The return value command completes the callback
        /// 返回值命令完成回调
        /// </summary>
        private void onCommandCompleted()
        {
            if (Command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                switch (Command.ReturnValue)
                {
                    case CallStateEnum.PersistenceCallbackException: renew(); return;
                    case CallStateEnum.NodeIndexOutOfRange:
                    case CallStateEnum.NodeIdentityNotMatch:
                        reindex();
                        return;
                }
            }
            onCompleted();
        }
        /// <summary>
        /// Get the command return value (suitable for scenarios where the server does not return default and does not care about the specific error)
        /// 获取命令返回值（适合服务端不会返回 default 并且不关心具体错误的场景）
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseReturnValue GetValue(bool isIgnoreError = false)
        {
            return new ResponseReturnValue(this, isIgnoreError);
        }
    }
}
