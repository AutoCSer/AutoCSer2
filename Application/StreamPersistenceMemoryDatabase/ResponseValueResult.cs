using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回结果，用于替代 ResponseResult{ValueResult{T}}，IsValue 表示是否存在返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ResponseValueResult<T>
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
        /// 是否存在返回数据，false 表示输入参数非法或者无返回值
        /// </summary>
        public bool IsValue;
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal ResponseValueResult(CommandClientReturnTypeEnum returnType, string? errorMessage = null)
#else
        internal ResponseValueResult(CommandClientReturnTypeEnum returnType, string errorMessage = null)
#endif
        {
            Value = default(T);
            ErrorMessage = errorMessage;
            ReturnType = returnType;
            CallState = CallStateEnum.Unknown;
            IsValue = false;
        }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="state"></param>
        internal ResponseValueResult(CallStateEnum state)
        {
            Value = default(T);
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = state;
            IsValue = false;
        }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        internal ResponseValueResult(T? value)
#else
        internal ResponseValueResult(T value)
#endif
        {
            Value = value;
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = CallStateEnum.Success;
            IsValue = true;
        }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="isValue"></param>
        internal ResponseValueResult(bool isValue)
        {
            Value = default(T);
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = CallStateEnum.Success;
            IsValue = isValue;
        }
    }
}
