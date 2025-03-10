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
    /// 返回参数
    /// </summary>
    public abstract class ResponseParameterAwaiter : ResponseParameter, INotifyCompletion
    {
        /// <summary>
        /// 客户端节点
        /// </summary>
        private readonly ClientNode node;
        /// <summary>
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// 请求传参的节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">客户端节点</param>
        internal ResponseParameterAwaiter(ClientNode node)
        {
            this.node = node;
            nodeIndex = node.Index;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 返回值完成
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 触发节点重建
        /// </summary>
        protected void renew()
        {
            try
            {
                node.Renew(nodeIndex).NotWait();
            }
            finally { onCompleted(); }
        }
        /// <summary>
        /// 索引失效重新获取
        /// </summary>
        protected void reindex()
        {
            try
            {
                node.Reindex(nodeIndex).NotWait();
            }
            finally { onCompleted(); }
        }
    }
    /// <summary>
    /// 返回参数 await ResponseResult{T}
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    public class ResponseParameterAwaiter<T> : ResponseParameterAwaiter
    {
        /// <summary>
        /// 返回值命令
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal ReturnCommand<ResponseParameter> Command;
        /// <summary>
        /// 返回值
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">客户端节点</param>
        protected ResponseParameterAwaiter(ClientNode node) : base(node) { }
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node">客户端节点</param>
        /// <param name="value">返回值</param>
        internal ResponseParameterAwaiter(ClientNode node, T value) : base(node)
        {
            Value.ReturnValue = value;
        }

        /// <summary>
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<ResponseResult<T>> Wait()
        {
            return await this;
        }
        /// <summary>
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
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ResponseParameterAwaiter<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
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
