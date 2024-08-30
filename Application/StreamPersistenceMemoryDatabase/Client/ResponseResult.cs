using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

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
        public string ErrorMessage;
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
        /// <param name="returnValue"></param>
        internal ResponseResult(CommandClientReturnValue<CallStateEnum> returnValue)
        {
            ReturnType = returnValue.ReturnType;
            CallState = returnValue.Value;
            ErrorMessage = returnValue.ErrorMessage;
        }
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
        public T Value;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage;
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
        internal ResponseResult(CommandClientReturnTypeEnum returnType, string errorMessage = null)
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
        /// <param name="value"></param>
        internal ResponseResult(T value)
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
        public ResponseResult<VT> Cast<VT>(VT defaultValue = default(VT))
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
        public static implicit operator ResponseResult<T>(T value) { return new ResponseResult<T>(value); }
    }
}
