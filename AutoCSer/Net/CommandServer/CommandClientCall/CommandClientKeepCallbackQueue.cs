using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端队列保持回调委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientKeepCallbackQueue
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> Callback;
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientKeepCallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientKeepCallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> value) { return new CommandClientKeepCallbackQueue(value); }
        /// <summary>
        /// 获取客户端回调委托
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
    /// 客户端队列保持回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientKeepCallbackQueue<T>
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> Callback;
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientKeepCallbackQueue(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientKeepCallbackQueue<T>(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> value) { return new CommandClientKeepCallbackQueue<T>(value); }
        /// <summary>
        /// 获取客户端回调委托
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
