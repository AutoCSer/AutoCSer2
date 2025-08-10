using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 保持回调超时信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct MethodKeepCallbackTimeout<T>
    {
        /// <summary>
        /// 回调包装
        /// </summary>
        private readonly MethodKeepCallback<T> callback;
        /// <summary>
        /// 超时秒计数
        /// </summary>
        private readonly long timeoutSeconds;
        /// <summary>
        /// 保持回调超时信息
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="timeoutSeconds"></param>
        internal MethodKeepCallbackTimeout(MethodKeepCallback<T> callback, int timeoutSeconds = 5)
        {
            this.callback = callback;
            this.timeoutSeconds = AutoCSer.Threading.SecondTimer.CurrentSeconds + timeoutSeconds;
        }
        /// <summary>
        ///  获取匹配的保持回调
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
#if NetStandard21
        private MethodKeepCallback<T>? get(CommandServerSocket socket)
#else
        private MethodKeepCallback<T> get(CommandServerSocket socket)
#endif
        {
            var callbackSocket = callback.CommandServerKeepCallback?.Socket;
            if (object.ReferenceEquals(callbackSocket, socket)) return callback;
            if (callbackSocket != null)
            {
                if (timeoutSeconds > AutoCSer.Threading.SecondTimer.CurrentSeconds) return null;
                callback.CancelKeep();
            }
            return callback;
        }

        /// <summary>
        /// 获取匹配的保持回调
        /// </summary>
        /// <param name="callbacks"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
#if NetStandard21
        internal static MethodKeepCallback<T>? GetCallback(ref LeftArray<MethodKeepCallbackTimeout<T>> callbacks, CommandServerSocket socket)
#else
        internal static MethodKeepCallback<T> GetCallback(ref LeftArray<MethodKeepCallbackTimeout<T>> callbacks, CommandServerSocket socket)
#endif
        {
            int count = callbacks.Length;
            if (count != 0)
            {
                MethodKeepCallbackTimeout<T>[] callbackArray = callbacks.Array;
                do
                {
                    var callback = callbackArray[--count].get(socket);
                    if (callback != null)
                    {
                        callbacks.UnsafeRemoveAtToEnd(count);
                        if (callback.CommandServerKeepCallback != null) return callback;
                    }
                }
                while (count != 0);
            }
            return null;
        }
    }
}
