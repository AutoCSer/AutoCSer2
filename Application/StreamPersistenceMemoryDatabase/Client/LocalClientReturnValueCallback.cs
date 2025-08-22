using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// A client callback delegate that directly retrieves the return value
    /// 直接获取返回值的客户端回调委托
    /// </summary>
    public sealed class LocalClientReturnValueCallback
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
        internal readonly Action<LocalResult> ErrorCallback;
        /// <summary>
        /// A client callback delegate that directly retrieves the return value
        /// 直接获取返回值的客户端回调委托
        /// </summary>
        /// <param name="callback">Success value callback delegate
        /// 成功值回调委托</param>
        /// <param name="errorCallback">Error return value type callback delegate
        /// 错误成功值类型回调委托</param>
#if NetStandard21
        public LocalClientReturnValueCallback(Action callback, Action<LocalResult>? errorCallback = null)
#else
        public LocalClientReturnValueCallback(Action callback, Action<LocalResult> errorCallback = null)
#endif
        {
            Callback = callback;
            ErrorCallback = errorCallback ?? EmptyCallback;
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="result"></param>
        private void callback(LocalResult result)
        {
            if (result.IsSuccess) Callback();
            else ErrorCallback(result);
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<LocalResult>(LocalClientReturnValueCallback callback) { return callback.callback; }

        /// <summary>
        /// Default empty callback
        /// </summary>
        /// <param name="returnValue"></param>
        private static void emptyCallback(LocalResult returnValue) { }
        /// <summary>
        /// Default empty callback
        /// </summary>
        internal static readonly Action<LocalResult> EmptyCallback = emptyCallback;
    }
    /// <summary>
    /// A client callback delegate that directly retrieves the return value
    /// 直接获取返回值的客户端回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct LocalClientReturnValueCallback<T>
    {
        /// <summary>
        /// Success value callback delegate
        /// 成功值回调委托
        /// </summary>
#if NetStandard21
        internal readonly Action<T?> Callback;
#else
        internal readonly Action<T> Callback;
#endif
        /// <summary>
        /// Error return value type callback delegate
        /// 错误成功值类型回调委托
        /// </summary>
        internal readonly Action<LocalResult> ErrorCallback;
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
        public LocalClientReturnValueCallback(Action<T?> callback, Action<LocalResult>? errorCallback = null)
#else
        public LocalClientReturnValueCallback(Action<T> callback, Action<LocalResult> errorCallback = null)
#endif
        {
            Callback = callback;
            ErrorCallback = errorCallback ?? LocalClientReturnValueCallback.EmptyCallback;
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="result"></param>
        private void callback(LocalResult<T> result)
        {
            if (result.IsSuccess) Callback(result.Value);
            else ErrorCallback(result);
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<LocalResult<T>>(LocalClientReturnValueCallback<T> callback) { return callback.callback; }
    }
}
