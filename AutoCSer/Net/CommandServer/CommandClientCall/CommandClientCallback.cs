using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// The client callback delegate
    /// 客户端回调委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientCallback
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
#if NetStandard21
        private Action<CommandClientReturnValue>? callback;
#else
        private Action<CommandClientReturnValue> callback;
#endif
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallback(Action<CommandClientReturnValue> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallback(Action<CommandClientReturnValue> value) { return new CommandClientCallback(value); }
        /// <summary>
        /// Get the client callback delegate
        /// 获取客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientCallback Get(Action<CommandClientReturnValue> callback)
        {
            return new CommandClientCallback(callback);
        }
        /// <summary>
        /// Successful callback, ignore error return
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action callback;
            /// <summary>
            /// Successful callback, ignore error return
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal SuccessCallback(Action callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue returnValue)
            {
                if (returnValue.IsSuccess) callback();
            }
        }
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal sealed class ErrorCallback
        {
            /// <summary>
            /// Success value callback delegate
            /// 成功值回调委托
            /// </summary>
            private readonly Action callback;
            /// <summary>
            /// Error return value type callback delegate
            /// 错误成功值类型回调委托
            /// </summary>
            private readonly Action<CommandClientReturnValue> errorCallback;
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            /// <param name="callback">A client callback delegate that directly retrieves the return value
            /// 直接获取返回值的客户端回调委托</param>
            public ErrorCallback(ClientReturnValueCallback callback)
            {
                this.callback = callback.Callback;
                this.errorCallback = callback.ErrorCallback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue returnValue)
            {
                if (returnValue.IsSuccess) callback();
                else errorCallback(returnValue);
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="command"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void Callback(CommandClientReturnValue returnValue, AutoCSer.Net.KeepCallbackCommand command)
            {
                Callback(returnValue);
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void Callback(CommandClientReturnValue returnValue, CommandClientCallQueue queue)
            {
                Callback(returnValue);
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            /// <param name="command"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void Callback(CommandClientReturnValue returnValue, CommandClientCallQueue queue, AutoCSer.Net.KeepCallbackCommand command)
            {
                Callback(returnValue);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallback(Action value) { return new CommandClientCallback(new SuccessCallback(value).Callback); }
        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">Error message</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal void Callback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            if (callback != null)
            {
                Action<CommandClientReturnValue> callback = this.callback;
                this.callback = null;
                callback(new CommandClientReturnValue(returnType, errorMessage));
            }
        }

        /// <summary>
        /// Type conversion of the callback return value
        /// 回调返回值类型转换
        /// </summary>
        /// <typeparam name="RT">Service API return type
        /// 服务 API 返回类型</typeparam>
        /// <typeparam name="T">Target return value type
        /// 目标返回值类型</typeparam>
        /// <param name="callback"></param>
        /// <param name="cast"></param>
        /// <returns></returns>
        public static CommandClientCallback<RT, T> Cast<RT, T>(Action<CommandClientReturnValue<T>> callback, Func<RT, T> cast)
        {
            return new CommandClientCallback<RT, T>(callback, cast);
        }
    }
    /// <summary>
    /// The client callback delegate
    /// 客户端回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientCallback<T>
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
#if NetStandard21
        private Action<CommandClientReturnValue<T>>? callback;
#else
        private Action<CommandClientReturnValue<T>> callback;
#endif
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallback(Action<CommandClientReturnValue<T>> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback">Success value callback delegate
        /// 成功值回调委托</param>
        /// <param name="errorCallback">Error return value type callback delegate
        /// 错误成功值类型回调委托</param>
        public CommandClientCallback(Action<T> callback, Action<CommandClientReturnValue> errorCallback)
        {
            this.callback = new ErrorCallback(callback, errorCallback).Callback;
        }
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal sealed class ErrorCallback
        {
            /// <summary>
            /// Success value callback delegate
            /// 成功值回调委托
            /// </summary>
            private readonly Action<T> callback;
            /// <summary>
            /// Error return value type callback delegate
            /// 错误成功值类型回调委托
            /// </summary>
            private readonly Action<CommandClientReturnValue> errorCallback;
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            /// <param name="callback">A client callback delegate that directly retrieves the return value
            /// 直接获取返回值的客户端回调委托</param>
            public ErrorCallback(ClientReturnValueCallback<T> callback)
            {
                this.callback = callback.Callback;
                this.errorCallback = callback.ErrorCallback;
            }
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            /// <param name="callback">Success value callback delegate
            /// 成功值回调委托</param>
            /// <param name="errorCallback">Error return value type callback delegate
            /// 错误成功值类型回调委托</param>
            internal ErrorCallback(Action<T> callback, Action<CommandClientReturnValue> errorCallback)
            {
                this.callback = callback;
                this.errorCallback = errorCallback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue)
            {
                if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success) callback(returnValue.Value);
                else errorCallback(returnValue.ReturnValue);
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="command"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void Callback(CommandClientReturnValue<T> returnValue, AutoCSer.Net.KeepCallbackCommand command)
            {
                Callback(returnValue);
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void Callback(CommandClientReturnValue<T> returnValue, CommandClientCallQueue queue)
            {
                Callback(returnValue);
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            /// <param name="command"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void Callback(CommandClientReturnValue<T> returnValue, CommandClientCallQueue queue, AutoCSer.Net.KeepCallbackCommand command)
            {
                Callback(returnValue);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallback<T>(Action<CommandClientReturnValue<T>> value) { return new CommandClientCallback<T>(value); }
        /// <summary>
        /// Get the client callback delegate
        /// 获取客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientCallback<T> Get(Action<CommandClientReturnValue<T>> callback)
        {
            return new CommandClientCallback<T>(callback);
        }
        /// <summary>
        /// Successful callback, ignore error return
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action<T> callback;
            /// <summary>
            /// Successful callback, ignore error return
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal SuccessCallback(Action<T> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue)
            {
                if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success) callback(returnValue.Value);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallback<T>(Action<T> value) { return new CommandClientCallback<T>(new SuccessCallback(value).Callback); }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(T? returnValue)
#else
        internal void Callback(T returnValue)
#endif
        {
            if (callback != null)
            {
                Action<CommandClientReturnValue<T>> callback = this.callback;
                this.callback = null;
                callback(returnValue);
            }
        }
        /// <summary>
        /// Failure callback
        /// 失败回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">Error message</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal void Callback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            if (callback != null)
            {
                Action<CommandClientReturnValue<T>> callback = this.callback;
                this.callback = null;
                callback(new CommandClientReturnValue<T>(returnType, errorMessage));
            }
        }
        ///// <summary>
        ///// 失败回调
        ///// </summary>
        ///// <param name="callback"></param>
        ///// <param name="returnType"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void CallCallback(ref CommandClientCallback<T> callback, CommandClientReturnType returnType)
        //{
        //    callback.Callback(returnType);
        //}
        ///// <summary>
        ///// 失败回调
        ///// </summary>
        ///// <param name="callback"></param>
        ///// <param name="client"></param>
        ///// <param name="exception"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void CallCallback(ref CommandClientCallback<T> callback, CommandClient client, Exception exception)
        //{
        //    callback.Callback(CommandClientReturnType.ClientException);
        //    CatchTask.AddIgnoreException(client.Attribute.Log.ExceptionIgnoreException(exception, null, LogLevel.AutoCSer | LogLevel.Exception));
        //}
    }
    /// <summary>
    /// Type conversion of the callback return value
    /// 回调返回值类型转换
    /// </summary>
    /// <typeparam name="RT">Service API return type
    /// 服务 API 返回类型</typeparam>
    /// <typeparam name="T">Target return value type
    /// 目标返回值类型</typeparam>
    public sealed class CommandClientCallback<RT, T>
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        private readonly Action<CommandClientReturnValue<T>> callback;
        /// <summary>
        /// Callback data type conversion
        /// 回调数据类型转换
        /// </summary>
        private readonly Func<RT, T> cast;
        /// <summary>
        /// Type conversion of the callback return value
        /// 回调返回值类型转换
        /// </summary>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        /// <param name="cast">Type conversion of the callback return value
        /// 回调数据类型转换</param>
        public CommandClientCallback(Action<CommandClientReturnValue<T>> callback, Func<RT, T> cast)
        {
            this.callback = callback;
            this.cast = cast;
        }
        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="buffer"></param>
        public void Callback(CommandClientReturnValue<RT> buffer)
        {
            if (callback != null) callback(buffer.Cast(cast));
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Action<CommandClientReturnValue<RT>>(CommandClientCallback<RT, T> value) { return value.Callback; }
    }
}
