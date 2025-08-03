using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Return parameters that support await
    /// 支持 await 的返回参数
    /// </summary>
    public abstract class ResponseParameterAwaiter : ResponseParameter, INotifyCompletion
    {
        /// <summary>
        /// Client node
        /// 客户端节点
        /// </summary>
        private readonly ClientNode node;
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
        /// Request the index information of the node for passing parameters
        /// 请求传参的节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// Return parameters that support await
        /// 支持 await 的返回参数
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        internal ResponseParameterAwaiter(ClientNode node)
        {
            this.node = node;
            nodeIndex = node.Index;
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
        /// The asynchronous operation has been completed
        /// 异步操作已完成
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// Trigger node reconstruction
        /// 触发节点重建
        /// </summary>
        protected void renew()
        {
            try
            {
                node.Renew(nodeIndex).AutoCSerNotWait();
            }
            finally { onCompleted(); }
        }
        /// <summary>
        /// The index is invalid. Get the index again
        /// 索引已失效，重新获取获取索引
        /// </summary>
        protected void reindex()
        {
            try
            {
                node.Reindex(nodeIndex).AutoCSerNotWait();
            }
            finally { onCompleted(); }
        }
    }
    /// <summary>
    /// await ResponseResult{T}, and return the parameter
    /// await ResponseResult{T}，返回参数
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public class ResponseParameterAwaiter<T> : ResponseParameterAwaiter
    {
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal ReturnCommand<ResponseParameter> Command;
        /// <summary>
        /// Return value
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// Return parameters that support await
        /// 支持 await 的返回参数
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        protected ResponseParameterAwaiter(ClientNode node) : base(node) { }
        /// <summary>
        /// Return parameters that support await
        /// 支持 await 的返回参数
        /// </summary>
        /// <param name="node">Client node
        /// 客户端节点</param>
        /// <param name="value">Return value</param>
        internal ResponseParameterAwaiter(ClientNode node, T value) : base(node)
        {
            Value.ReturnValue = value;
        }

        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<ResponseResult<T>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
        public ResponseResult<T> GetResult()
        {
            if (Command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                CallStateEnum state = Command.ReturnValue.State;
                if (state == CallStateEnum.Success) return new ResponseResult<T>(Value.ReturnValue);
                return state;
            }
            return new ResponseResult<T>(Command.ReturnType, Command.ErrorMessage);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseParameterAwaiter<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Set the return value command
        /// 设置返回值命令
        /// </summary>
        /// <param name="command"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ReturnCommand<ResponseParameter> command)
        {
            this.Command = command;
            command.OnCompleted(onCommandCompleted);
        }
        /// <summary>
        /// The return value command completes the callback
        /// 返回值命令完成回调
        /// </summary>
        private void onCommandCompleted()
        {
            if (Command.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                switch (Command.ReturnValue.State)
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
