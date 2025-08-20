using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// A client callback delegate that directly retrieves the return value
    /// 直接获取返回值的客户端回调委托
    /// </summary>
    public struct ClientReturnValueCallback
    {
        /// <summary>
        /// Success value callback delegate
        /// 成功值回调委托
        /// </summary>
        internal readonly Action Callback;
        /// <summary>
        /// Error return value type callback delegate
        /// 错误成功值类型回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue> ErrorCallback;
        ///// <summary>
        ///// A client callback delegate that directly retrieves the return value
        ///// 直接获取返回值的客户端回调委托
        ///// </summary>
        ///// <param name="callback">Success value callback delegate
        ///// 成功值回调委托</param>
        //public ClientReturnValueCallback(Action callback)
        //{
        //    Callback = callback;
        //    ErrorCallback = EmptyCallback;
        //}
        /// <summary>
        /// A client callback delegate that directly retrieves the return value
        /// 直接获取返回值的客户端回调委托
        /// </summary>
        /// <param name="callback">Success value callback delegate
        /// 成功值回调委托</param>
        /// <param name="errorCallback">Error return value type callback delegate
        /// 错误成功值类型回调委托</param>
#if NetStandard21
        public ClientReturnValueCallback(Action callback, Action<CommandClientReturnValue>? errorCallback = null)
#else
        public ClientReturnValueCallback(Action callback, Action<CommandClientReturnValue> errorCallback = null)
#endif
        {
            Callback = callback;
            ErrorCallback = errorCallback ?? EmptyCallback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue>(ClientReturnValueCallback callback) { return new CommandClientCallback.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientCallback(ClientReturnValueCallback callback) { return (Action<CommandClientReturnValue>)callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand>(ClientReturnValueCallback callback) { return new CommandClientCallback.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue, CommandClientCallQueue>(ClientReturnValueCallback callback) { return new CommandClientCallback.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientCallbackQueueNode(ClientReturnValueCallback callback) { return (Action<CommandClientReturnValue, CommandClientCallQueue>)callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>(ClientReturnValueCallback callback) { return new CommandClientCallback.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientKeepCallback(ClientReturnValueCallback callback) { return (Action<CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand>)callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientKeepCallbackQueue(ClientReturnValueCallback callback) { return (Action<CommandClientReturnValue, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>)callback; }

        /// <summary>
        /// Default empty callback
        /// </summary>
        /// <param name="returnValue"></param>
        private static void emptyCallback(CommandClientReturnValue returnValue) { }
        /// <summary>
        /// Default empty callback
        /// </summary>
        internal static readonly Action<CommandClientReturnValue> EmptyCallback = emptyCallback;
    }
    /// <summary>
    /// A client callback delegate that directly retrieves the return value
    /// 直接获取返回值的客户端回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ClientReturnValueCallback<T>
    {
        /// <summary>
        /// Success value callback delegate
        /// 成功值回调委托
        /// </summary>
        internal readonly Action<T> Callback;
        /// <summary>
        /// Error return value type callback delegate
        /// 错误成功值类型回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue> ErrorCallback;
        ///// <summary>
        ///// A client callback delegate that directly retrieves the return value
        ///// 直接获取返回值的客户端回调委托
        ///// </summary>
        ///// <param name="callback">Success value callback delegate
        ///// 成功值回调委托</param>
        //public ClientReturnValueCallback(Action<T> callback)
        //{
        //    Callback = callback;
        //    ErrorCallback = ClientReturnValueCallback.EmptyCallback;
        //}
        /// <summary>
        /// A client callback delegate that directly retrieves the return value
        /// 直接获取返回值的客户端回调委托
        /// </summary>
        /// <param name="callback">Success value callback delegate
        /// 成功值回调委托</param>
        /// <param name="errorCallback">Error return value type callback delegate
        /// 错误成功值类型回调委托</param>
#if NetStandard21
        public ClientReturnValueCallback(Action<T> callback, Action<CommandClientReturnValue>? errorCallback = null)
#else
        public ClientReturnValueCallback(Action<T> callback, Action<CommandClientReturnValue> errorCallback = null)
#endif
        {
            Callback = callback;
            ErrorCallback = errorCallback ?? ClientReturnValueCallback.EmptyCallback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue<T>>(ClientReturnValueCallback<T> callback) { return new CommandClientCallback<T>.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientCallback<T>(ClientReturnValueCallback<T> callback) { return (Action<CommandClientReturnValue<T>>)callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue<T>, AutoCSer.Net.KeepCallbackCommand>(ClientReturnValueCallback<T> callback) { return new CommandClientCallback<T>.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue<T>, CommandClientCallQueue>(ClientReturnValueCallback<T> callback) { return new CommandClientCallback<T>.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientCallbackQueueNode<T>(ClientReturnValueCallback<T> callback) { return (Action<CommandClientReturnValue<T>, CommandClientCallQueue>)callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<CommandClientReturnValue<T>, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>(ClientReturnValueCallback<T> callback) { return new CommandClientCallback<T>.ErrorCallback(callback).Callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientKeepCallback<T>(ClientReturnValueCallback<T> callback) { return (Action<CommandClientReturnValue<T>, AutoCSer.Net.KeepCallbackCommand>)callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator CommandClientKeepCallbackQueue<T>(ClientReturnValueCallback<T> callback) { return (Action<CommandClientReturnValue<T>, CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>)callback; }
    }
}
