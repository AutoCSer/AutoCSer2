using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The local service returns the result
    /// 本地服务返回结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LocalResult
    {
#if AOT
        /// <summary>
        /// Error message
        /// </summary>
        public string? ErrorMessage;
#endif
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success; } }
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        /// <param name="state"></param>
        internal LocalResult(CallStateEnum state)
        {
            CallState = state;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="state"></param>
        public static implicit operator LocalResult(CallStateEnum state) { return new LocalResult(state); }
#if AOT
        /// <summary>
        /// Set the error call status
        /// 设置错误调用状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="errorMessage"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(CallStateEnum state, string? errorMessage)
        {
            CallState = state;
            ErrorMessage = errorMessage;
        }
#endif
        /// <summary>
        /// Emptu callback
        /// </summary>
        /// <param name="result"></param>
        private static void emptyAction(LocalResult result) { }
        /// <summary>
        /// Emptu callback
        /// </summary>
        internal static readonly Action<LocalResult> EmptyAction = emptyAction;
    }
    /// <summary>
    /// The local service returns the result
    /// 本地服务返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LocalResult<T>
    {
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        public T? Value;
#else
        public T Value;
#endif
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// Exception information
        /// 异常信息
        /// </summary>
#if NetStandard21
        public Exception? Exception;
#else
        public Exception Exception;
#endif
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success; } }
        /// <summary>
        /// Error call state
        /// 错误调用状态
        /// </summary>
        /// <param name="state"></param>
        internal LocalResult(CallStateEnum state)
        {
            Value = default(T);
            CallState = state;
            Exception = null;
        }
        /// <summary>
        /// Return result
        /// 返回结果
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        internal LocalResult(T? value)
#else
        internal LocalResult(T value)
#endif
        {
            Value = value;
            CallState = CallStateEnum.Success;
            Exception = null;
        }
        /// <summary>
        /// Error call state
        /// 错误调用状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal LocalResult(CallStateEnum state, Exception? exception = null)
#else
        internal LocalResult(CallStateEnum state, Exception exception = null)
#endif
        {
            Value = default(T);
            Exception = exception;
            CallState = state;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="state"></param>
        public static implicit operator LocalResult<T>(CallStateEnum state) { return new LocalResult<T>(state); }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator LocalResult<T>(T? value) { return new LocalResult<T>(value); }
#else
        public static implicit operator LocalResult<T>(T value) { return new LocalResult<T>(value); }
#endif
        /// <summary>
        /// Error return result
        /// 错误返回结果
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator LocalResult(LocalResult<T> value) { return new LocalResult(value.CallState); }
        /// <summary>
        /// Error return result
        /// 错误返回结果
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator LocalResult<T>(LocalResult value) { return new LocalResult<T>(value.CallState); }
        /// <summary>
        /// Return the result type conversion
        /// 返回结果类型转换
        /// </summary>
        /// <typeparam name="VT">Target type
        /// 目标类型</typeparam>
        /// <param name="defaultValue">Default value of success status
        /// 成功状态默认值</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public LocalResult<VT> Cast<VT>(VT? defaultValue = default(VT))
#else
        public LocalResult<VT> Cast<VT>(VT defaultValue = default(VT))
#endif
        {
            if (IsSuccess) return defaultValue;
            return CallState;
        }
        /// <summary>
        /// Get the error paging data
        /// 获取错误分页数据
        /// </summary>
        /// <typeparam name="PT"></typeparam>
        /// <returns>Error paging data
        /// 错误分页数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public PageResult<PT> GetPageResult<PT>()
        {
            return new PageResult<PT>(CommandClientReturnTypeEnum.Success, CallState);
        }

        /// <summary>
        /// Emptu callback
        /// </summary>
        /// <param name="result"></param>
        private static void emptyAction(LocalResult<T> result) { }
        /// <summary>
        /// Emptu callback
        /// </summary>
        internal static readonly Action<LocalResult<T>> EmptyAction = emptyAction;
    }
}
