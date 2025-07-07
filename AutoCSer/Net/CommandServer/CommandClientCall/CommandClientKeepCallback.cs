using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// The client keep the callback delegate
    /// 客户端保持回调委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientKeepCallback
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
#if NetStandard21
        private Action<CommandClientReturnValue, KeepCallbackCommand>? callback;
#else
        private Action<CommandClientReturnValue, KeepCallbackCommand> callback;
#endif
        /// <summary>
        /// The client keep the callback delegate
        /// 客户端保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientKeepCallback(Action<CommandClientReturnValue, KeepCallbackCommand> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientKeepCallback(Action<CommandClientReturnValue, KeepCallbackCommand> value) { return new CommandClientKeepCallback(value); }
        /// <summary>
        /// Get the client keep callback delegate
        /// 获取客户端保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientKeepCallback Get(Action<CommandClientReturnValue, KeepCallbackCommand> callback)
        {
            return new CommandClientKeepCallback(callback);
        }
        /// <summary>
        /// Failure callback
        /// 失败回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="keepCallbackCommand"></param>
#if NetStandard21
        internal void Error(CommandClientReturnTypeEnum returnType, string? errorMessage, KeepCallbackCommand keepCallbackCommand)
#else
        internal void Error(CommandClientReturnTypeEnum returnType, string errorMessage, KeepCallbackCommand keepCallbackCommand)
#endif
        {
            if (callback != null)
            {
                Action<CommandClientReturnValue, KeepCallbackCommand> callback = this.callback;
                this.callback = null;
                callback(new CommandClientReturnValue(returnType, errorMessage), keepCallbackCommand);
            }
        }
        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="keepCallbackCommand"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(CommandClientReturnTypeEnum returnType, string? errorMessage, KeepCallbackCommand keepCallbackCommand)
#else
        internal void Callback(CommandClientReturnTypeEnum returnType, string errorMessage, KeepCallbackCommand keepCallbackCommand)
#endif
        {
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                if (callback != null) callback(returnType, keepCallbackCommand);
            }
            else Error(returnType, errorMessage, keepCallbackCommand);
        }

        /// <summary>
        /// Empty callback, such as heartbeats
        /// 空回调，比如心跳
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
        private static void emptyCallback(CommandClientReturnValue returnValue, KeepCallbackCommand keepCallbackCommand) { }
        /// <summary>
        /// Empty callback, such as heartbeats
        /// 空回调，比如心跳
        /// </summary>
        public static readonly Action<CommandClientReturnValue, KeepCallbackCommand> EmptyCallback = emptyCallback;
    }
    /// <summary>
    /// The client keep the callback delegate
    /// 客户端保持回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientKeepCallback<T>
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
#if NetStandard21
        internal Action<CommandClientReturnValue<T>, KeepCallbackCommand>? Callback;
#else
        internal Action<CommandClientReturnValue<T>, KeepCallbackCommand> Callback;
#endif
        /// <summary>
        /// The client keep the callback delegate
        /// 客户端保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientKeepCallback(Action<CommandClientReturnValue<T>, KeepCallbackCommand> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientKeepCallback<T>(Action<CommandClientReturnValue<T>, KeepCallbackCommand> value) { return new CommandClientKeepCallback<T>(value); }
        /// <summary>
        /// Get the client keep callback delegate
        /// 获取客户端保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientKeepCallback<T> Get(Action<CommandClientReturnValue<T>, KeepCallbackCommand> callback)
        {
            return new CommandClientKeepCallback<T>(callback);
        }
        /// <summary>
        /// Failure callback
        /// 失败回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="keepCallbackCommand"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Error(CommandClientReturnTypeEnum returnType, string? errorMessage, KeepCallbackCommand keepCallbackCommand)
#else
        internal void Error(CommandClientReturnTypeEnum returnType, string errorMessage, KeepCallbackCommand keepCallbackCommand)
#endif
        {
            if (Callback != null)
            {
                Action<CommandClientReturnValue<T>, KeepCallbackCommand> callback = Callback;
                Callback = null;
                callback(new CommandClientReturnValue<T>(returnType, errorMessage), keepCallbackCommand);
            }
        }
    }
}
