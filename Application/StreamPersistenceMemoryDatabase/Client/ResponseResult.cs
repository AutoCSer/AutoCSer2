using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ResponseResult
    {
        /// <summary>
        /// 错误信息
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// 网络客户端返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// 读取数据状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// 是否成功
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
        /// 返回结果
        /// </summary>
        /// <param name="state"></param>
        internal ResponseResult(CallStateEnum state)
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = state;
            ErrorMessage = null;
        }
        /// <summary>
        /// 返回结果
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
        /// 返回结果
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
        /// 返回结果
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
        /// Success
        /// </summary>
        internal static readonly Task<ResponseResult> SuccessTask = Task.FromResult(new ResponseResult(CallStateEnum.Success));
        /// <summary>
        /// 服务已释放资源
        /// </summary>
        internal static readonly Task<ResponseResult> DisposedTask = Task.FromResult(new ResponseResult(CallStateEnum.Disposed));
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
    /// 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ResponseResult<T>
    {
        /// <summary>
        /// 返回值
        /// </summary>
#if NetStandard21
        public T? Value;
#else
        public T Value;
#endif
        /// <summary>
        /// 错误信息
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// 网络客户端返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// 读取数据状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success && ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// 返回结果
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
        /// 返回结果
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
        /// 返回结果
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
        /// 返回结果类型转换
        /// </summary>
        /// <typeparam name="VT">目标类型</typeparam>
        /// <param name="defaultValue">成功状态默认值</param>
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
        /// 返回结果
        /// </summary>
        /// <param name="returnType"></param>
        public static implicit operator ResponseResult<T>(CommandClientReturnTypeEnum returnType) { return new ResponseResult<T>(returnType, null); }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="state"></param>
        public static implicit operator ResponseResult<T>(CallStateEnum state) { return new ResponseResult<T>(state); }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ResponseResult<T>(T? value) { return new ResponseResult<T>(value); }
#else
        public static implicit operator ResponseResult<T>(T value) { return new ResponseResult<T>(value); }
#endif
        /// <summary>
        /// 错误返回结果
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator ResponseResult(ResponseResult<T> result)
        {
            if(result.ReturnType == CommandClientReturnTypeEnum.Success) return new ResponseResult(result.CallState);
            return new ResponseResult(result.ReturnType, result.ErrorMessage);
        }

    }
}
