using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Return result
    /// 返回结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ResponseResult
    {
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// The return value type of the network client
        /// 网络客户端返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success && ReturnType == CommandClientReturnTypeEnum.Success; } }
        ///// <summary>
        ///// 返回结果
        ///// </summary>
        ///// <param name="returnValue"></param>
        //internal ResponseResult(CommandClientReturnValue<CallStateEnum> returnValue)
        //{
        //    ReturnType = returnValue.ReturnType;
        //    CallState = returnValue.Value;
        //    ErrorMessage = returnValue.ErrorMessage;
        //}
        /// <summary>
        /// Return the calling state
        /// 返回调用状态
        /// </summary>
        /// <param name="state"></param>
        internal ResponseResult(CallStateEnum state)
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = state;
            ErrorMessage = null;
        }
        /// <summary>
        /// Error call return type
        /// 错误调用返回类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal ResponseResult(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal ResponseResult(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            ReturnType = returnType;
            CallState = CallStateEnum.Unknown;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Set the error call status
        /// 设置错误调用状态
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="state"></param>
        /// <param name="errorMessage"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Set(CommandClientReturnTypeEnum returnType, CallStateEnum state, string? errorMessage)
#else
        internal void Set(CommandClientReturnTypeEnum returnType, CallStateEnum state, string errorMessage)
#endif
        {
            ReturnType = returnType;
            CallState = state;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Error call state
        /// 错误调用状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="errorMessage"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Set(CallStateEnum state, string? errorMessage)
#else
        internal void Set(CallStateEnum state, string errorMessage)
#endif
        {
            CallState = state;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="state"></param>
        public static implicit operator ResponseResult(CallStateEnum state) { return new ResponseResult(state); }
        /// <summary>
        /// Get the return value
        /// 获取返回值
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        public void GetValue(bool isIgnoreError = false)
        {
            if (CallState == CallStateEnum.Success)
            {
                if (ReturnType == CommandClientReturnTypeEnum.Success) return;
                new CommandClientReturnValue(ReturnType, ErrorMessage).GetValue(isIgnoreError);
            }
            if (!isIgnoreError) throw new Exception($"调用返回状态错误 {CallState} {ErrorMessage}");
        }

        /// <summary>
        /// Success
        /// </summary>
        internal static readonly Task<ResponseResult> SuccessTask = Task.FromResult(new ResponseResult(CallStateEnum.Success));
        /// <summary>
        /// The service has released resources
        /// 服务已释放资源
        /// </summary>
        internal static readonly Task<ResponseResult> DisposedTask = Task.FromResult(new ResponseResult(CallStateEnum.Disposed));
        /// <summary>
        /// The client initialization loading has not been completed
        /// 客户端初始化加载未完成
        /// </summary>
        internal static readonly Task<ResponseResult> ClientLoadUnfinishedTask = Task.FromResult(new ResponseResult(CallStateEnum.ClientLoadUnfinished));
        /// <summary>
        /// false
        /// </summary>
        internal static readonly Task<ResponseResult<bool>> FalseTask = Task.FromResult((ResponseResult<bool>)false);
        /// <summary>
        /// true
        /// </summary>
        internal static readonly Task<ResponseResult<bool>> TrueTask = Task.FromResult((ResponseResult<bool>)true);
    }
    /// <summary>
    /// Return result
    /// 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ResponseResult<T>
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
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// The return value type of the network client
        /// 网络客户端返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success && ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// Error call return type
        /// 错误调用返回类型
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        public ResponseResult(CommandClientReturnTypeEnum returnType, string? errorMessage = null)
#else
        internal ResponseResult(CommandClientReturnTypeEnum returnType, string errorMessage = null)
#endif
        {
            Value = default(T);
            ErrorMessage = errorMessage;
            ReturnType = returnType;
            CallState = CallStateEnum.Unknown;
        }
        /// <summary>
        /// Error call state
        /// 错误调用状态
        /// </summary>
        /// <param name="state"></param>
        internal ResponseResult(CallStateEnum state)
        {
            Value = default(T);
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = state;
        }
        /// <summary>
        /// Error call state
        /// 错误调用状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal ResponseResult(CallStateEnum state, string? errorMessage = null)
#else
        internal ResponseResult(CallStateEnum state, string errorMessage = null)
#endif
        {
            Value = default(T);
            ErrorMessage = errorMessage;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = state;
        }
        /// <summary>
        /// Return result
        /// 返回结果
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        internal ResponseResult(T? value)
#else
        internal ResponseResult(T value)
#endif
        {
            Value = value;
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = CallStateEnum.Success;
        }
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
        public ResponseResult<VT> Cast<VT>(VT? defaultValue = default(VT))
#else
        public ResponseResult<VT> Cast<VT>(VT defaultValue = default(VT))
#endif
        {
            if (IsSuccess) return defaultValue;
            if (ReturnType == CommandClientReturnTypeEnum.Success) return CallState;
            return new ResponseResult<VT>(ReturnType, ErrorMessage);
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="returnType"></param>
        public static implicit operator ResponseResult<T>(CommandClientReturnTypeEnum returnType) { return new ResponseResult<T>(returnType, null); }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="state"></param>
        public static implicit operator ResponseResult<T>(CallStateEnum state) { return new ResponseResult<T>(state); }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ResponseResult<T>(T? value) { return new ResponseResult<T>(value); }
#else
        public static implicit operator ResponseResult<T>(T value) { return new ResponseResult<T>(value); }
#endif
        /// <summary>
        /// Error return result
        /// 错误返回结果
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator ResponseResult<T>(ResponseResult result)
        {
            if (result.ReturnType == CommandClientReturnTypeEnum.Success) return result.CallState;
            return new ResponseResult<T>(result.ReturnType, result.ErrorMessage);
        }
        /// <summary>
        /// Error return result
        /// 错误返回结果
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator ResponseResult(ResponseResult<T> result)
        {
            if(result.ReturnType == CommandClientReturnTypeEnum.Success) return result.CallState;
            return new ResponseResult(result.ReturnType, result.ErrorMessage);
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
            return new PageResult<PT>(ReturnType, CallState);
        }
        /// <summary>
        /// Get the return value
        /// 获取返回值
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        /// <returns>Return value</returns>
#if NetStandard21
        public T? GetValue(bool isIgnoreError = false)
#else
        public T GetValue(bool isIgnoreError = false)
#endif
        {
            if (CallState == CallStateEnum.Success)
            {
                if (ReturnType == CommandClientReturnTypeEnum.Success) return Value;
                new CommandClientReturnValue(ReturnType, ErrorMessage).GetValue(isIgnoreError);
            }
            else if (!isIgnoreError) throw new Exception($"调用返回状态错误 {CallState} {ErrorMessage}");
            return default(T);
        }
    }
}
