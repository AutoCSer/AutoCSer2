using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数 await ResponseResult
    /// </summary>
    public sealed class ResponseResultAwaiter : ResponseParameterAwaiter
    {
        /// <summary>
        /// 返回值命令
        /// </summary>
        private readonly ReturnCommand<CallStateEnum> command;
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <param name="command">返回值命令</param>
        internal ResponseResultAwaiter(ClientNode node, ReturnCommand<CallStateEnum> command) : base(node)
        {
            this.command = command;
            command.OnCompleted(onCommandCompleted);
        }

        /// <summary>
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<ResponseResult> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseResult GetResult()
        {
            if (command.ReturnType == CommandClientReturnTypeEnum.Success) return command.ReturnValue;
            return new ResponseResult(command.ReturnType, command.ErrorMessage);
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseResultAwaiter GetAwaiter()
        {
            return this;
        }
        /// <summary>
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
