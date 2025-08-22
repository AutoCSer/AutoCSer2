using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// A client callback delegate that directly retrieves the return value
    /// 直接获取返回值的客户端回调委托
    /// </summary>
    public sealed class ClientReturnValueCallback
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
        internal readonly Action<ResponseResult> ErrorCallback;
        /// <summary>
        /// A client callback delegate that directly retrieves the return value
        /// 直接获取返回值的客户端回调委托
        /// </summary>
        /// <param name="callback">Success value callback delegate
        /// 成功值回调委托</param>
        /// <param name="errorCallback">Error return value type callback delegate
        /// 错误成功值类型回调委托</param>
#if NetStandard21
        public ClientReturnValueCallback(Action callback, Action<ResponseResult>? errorCallback = null)
#else
        public ClientReturnValueCallback(Action callback, Action<ResponseResult> errorCallback = null)
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
        private void callback(ResponseResult result)
        {
            if (result.IsSuccess) Callback();
            else ErrorCallback(result);
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<ResponseResult>(ClientReturnValueCallback callback) { return callback.callback; }

        /// <summary>
        /// Default empty callback
        /// </summary>
        /// <param name="returnValue"></param>
        private static void emptyCallback(ResponseResult returnValue) { }
        /// <summary>
        /// Default empty callback
        /// </summary>
        internal static readonly Action<ResponseResult> EmptyCallback = emptyCallback;
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
#if NetStandard21
        internal readonly Action<T?> Callback;
#else
        internal readonly Action<T> Callback;
#endif
        /// <summary>
        /// Error return value type callback delegate
        /// 错误成功值类型回调委托
        /// </summary>
        internal readonly Action<ResponseResult> ErrorCallback;
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
        public ClientReturnValueCallback(Action<T?> callback, Action<ResponseResult>? errorCallback = null)
#else
        public ClientReturnValueCallback(Action<T> callback, Action<ResponseResult> errorCallback = null)
#endif
        {
            Callback = callback;
            ErrorCallback = errorCallback ?? ClientReturnValueCallback.EmptyCallback;
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="result"></param>
        private void callback(ResponseResult<T> result)
        {
            if (result.IsSuccess) Callback(result.Value);
            else ErrorCallback(result);
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        private void callback(ResponseResult<T> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess) Callback(result.Value);
            else ErrorCallback(result);
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<ResponseResult<T>>(ClientReturnValueCallback<T> callback) { return callback.callback; }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand>(ClientReturnValueCallback<T> callback) { return callback.callback; }
    }
}
