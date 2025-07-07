using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// The client queue keep the callback delegate
    /// 客户端队列保持回调委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientKeepCallbackQueue
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> Callback;
        /// <summary>
        /// The client queue keep the callback delegate
        /// 客户端队列保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientKeepCallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientKeepCallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> value) { return new CommandClientKeepCallbackQueue(value); }
        /// <summary>
        /// Get the client queue keep callback delegate
        /// 获取客户端队列保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientKeepCallbackQueue Get(Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback)
        {
            return new CommandClientKeepCallbackQueue(callback);
        }
    }
    /// <summary>
    /// The client queue keep the callback delegate
    /// 客户端队列保持回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientKeepCallbackQueue<T>
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> Callback;
        /// <summary>
        /// The client queue keep the callback delegate
        /// 客户端队列保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientKeepCallbackQueue(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientKeepCallbackQueue<T>(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> value) { return new CommandClientKeepCallbackQueue<T>(value); }
        /// <summary>
        /// Get the client queue keep callback delegate
        /// 获取客户端队列保持回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientKeepCallbackQueue<T> Get(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback)
        {
            return new CommandClientKeepCallbackQueue<T>(callback);
        }
    }
}
