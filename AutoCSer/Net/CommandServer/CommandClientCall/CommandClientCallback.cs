using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端回调委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientCallback
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
#if NetStandard21
        private Action<CommandClientReturnValue>? callback;
#else
        private Action<CommandClientReturnValue> callback;
#endif
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallback(Action<CommandClientReturnValue> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallback(Action<CommandClientReturnValue> value) { return new CommandClientCallback(value); }
        /// <summary>
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
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action callback;
            /// <summary>
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal SuccessCallback(Action callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue returnValue)
            {
                if (returnValue.IsSuccess) callback();
            }
        }
        /// <summary>
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallback(Action value) { return new CommandClientCallback(new SuccessCallback(value).Callback); }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">错误信息</param>
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
        ///// <summary>
        ///// 回调
        ///// </summary>
        ///// <param name="callback"></param>
        ///// <param name="returnType"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void CallCallback(CommandClientCallback callback, CommandClientReturnType returnType)
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
        //internal static void CallCallback(CommandClientCallback callback, CommandClient client, Exception exception)
        //{
        //    callback.Callback(CommandClientReturnType.ClientException);
        //    CatchTask.AddIgnoreException(client.Attribute.Log.ExceptionIgnoreException(exception, null, LogLevel.AutoCSer | LogLevel.Exception));
        //}

        /// <summary>
        /// 客户端回调返回值类型转换
        /// </summary>
        /// <typeparam name="RT">服务接口返回类型</typeparam>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="callback"></param>
        /// <param name="cast"></param>
        /// <returns></returns>
        public static CommandClientCallback<RT, T> Cast<RT, T>(Action<CommandClientReturnValue<T>> callback, Func<RT, T> cast)
        {
            return new CommandClientCallback<RT, T>(callback, cast);
        }
    }
    /// <summary>
    /// 客户端回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientCallback<T>
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
#if NetStandard21
        private Action<CommandClientReturnValue<T>>? callback;
#else
        private Action<CommandClientReturnValue<T>> callback;
#endif
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallback(Action<CommandClientReturnValue<T>> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback">成功值回调</param>
        /// <param name="errorCallback">错误类型回调</param>
        public CommandClientCallback(Action<T> callback, Action<CommandClientReturnTypeEnum> errorCallback)
        {
            this.callback = new ErrorCallback(callback, errorCallback).Callback;
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        internal sealed class ErrorCallback
        {
            /// <summary>
            /// 成功值回调
            /// </summary>
            private readonly Action<T> callback;
            /// <summary>
            /// 错误类型回调
            /// </summary>
            private readonly Action<CommandClientReturnTypeEnum> errorCallback;
            /// <summary>
            /// 错误回调
            /// </summary>
            /// <param name="callback">成功值回调</param>
            /// <param name="errorCallback">错误类型回调</param>
            internal ErrorCallback(Action<T> callback, Action<CommandClientReturnTypeEnum> errorCallback)
            {
                this.callback = callback;
                this.errorCallback = errorCallback;
            }
            /// <summary>
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue)
            {
                if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success) callback(returnValue.Value);
                else errorCallback(returnValue.ReturnType);
            }
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallback<T>(Action<CommandClientReturnValue<T>> value) { return new CommandClientCallback<T>(value); }
        /// <summary>
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
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action<T> callback;
            /// <summary>
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal SuccessCallback(Action<T> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue)
            {
                if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success) callback(returnValue.Value);
            }
        }
        /// <summary>
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallback<T>(Action<T> value) { return new CommandClientCallback<T>(new SuccessCallback(value).Callback); }
        /// <summary>
        /// 成功回调
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
        /// 失败回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">错误信息</param>
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
    /// 客户端回调返回值类型转换
    /// </summary>
    /// <typeparam name="RT">服务接口返回类型</typeparam>
    /// <typeparam name="T">目标类型</typeparam>
    public sealed class CommandClientCallback<RT, T>
    {
        /// <summary>
        /// 客户端回调
        /// </summary>
        private readonly Action<CommandClientReturnValue<T>> callback;
        /// <summary>
        /// 回调数据类型转换
        /// </summary>
        private readonly Func<RT, T> cast;
        /// <summary>
        /// 客户端回调返回值类型转换
        /// </summary>
        /// <param name="callback">客户端回调</param>
        /// <param name="cast">回调数据类型转换</param>
        public CommandClientCallback(Action<CommandClientReturnValue<T>> callback, Func<RT, T> cast)
        {
            this.callback = callback;
            this.cast = cast;
        }
        /// <summary>
        /// 客户端接口回调传参
        /// </summary>
        /// <param name="buffer"></param>
        public void Callback(CommandClientReturnValue<RT> buffer)
        {
            if (callback != null) callback(buffer.Cast(cast));
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Action<CommandClientReturnValue<RT>>(CommandClientCallback<RT, T> value) { return value.Callback; }
    }
}
