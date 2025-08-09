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
        private readonly ReturnCommand<CallStateEnum> command;
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
            this.command = command;
            command.OnCompleted(onCommandCompleted);
        }

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
            if (command.ReturnType == CommandClientReturnTypeEnum.Success) return command.ReturnValue;
            return new ResponseResult(command.ReturnType, command.ErrorMessage);
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
            if (command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                switch (command.ReturnValue)
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
    }
}
