﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令返回值 await T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandResult<T> : INotifyCompletion
    {
        /// <summary>
        /// The return value command
        /// 返回值命令
        /// </summary>
        public readonly ReturnCommand<T> Command;
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
        public bool IsCompleted { get { return Command.IsCompleted; } }
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType { get { return Command.ReturnType; } }
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage { get { return Command.ErrorMessage; } }
#else
        public string ErrorMessage{ get { return Command.ErrorMessage; } }
#endif
        /// <summary>
        /// Return value
        /// </summary>
        public T ReturnValue { get { return Command.ReturnValue; } }
        /// <summary>
        /// 命令返回值
        /// </summary>
        /// <param name="command">The return value command
        /// 返回值命令</param>
        internal CommandResult(ReturnCommand<T> command)
        {
            Command = command;
            if (!command.IsCompleted) command.OnCompleted(onCompleted);
            else continuation = Common.EmptyAction;
        }
        /// <summary>
        /// 完成回调操作
        /// </summary>
        private void onCompleted()
        {
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
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
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<T?> Wait()
#else
        public async Task<T> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? GetResult()
#else
        public T GetResult()
#endif
        {
            return Command.ReturnType == CommandClientReturnTypeEnum.Success ? Command.ReturnValue : default(T);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandResult<T> GetAwaiter()
        {
            return this;
        }
    }
}
