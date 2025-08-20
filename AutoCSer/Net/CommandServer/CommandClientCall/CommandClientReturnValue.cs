using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client calls the return value
    /// 命令客户端调用返回值
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientReturnValue
    {
        /// <summary>
        /// The return type of the call
        /// 返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// Error return value
        /// 错误返回值
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        public CommandClientReturnValue(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        public CommandClientReturnValue(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            ReturnType = returnType;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Implicit conversion
        /// 隐式转换
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static implicit operator CommandClientReturnValue(CommandClientReturnTypeEnum returnType)
        {
            return new CommandClientReturnValue(returnType, null);
        }
        /// <summary>
        /// Get the return value
        /// 获取返回值
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        public void GetValue(bool isIgnoreError = false)
        {
            if (ReturnType == CommandClientReturnTypeEnum.Success) return;
            if (!isIgnoreError) throw new Exception($"调用返回类型错误 {ReturnType} {ErrorMessage}");
        }
#if AOT
        /// <summary>
        /// Get exception information
        /// 获取异常信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string GetThrowMessage()
        {
            return string.IsNullOrEmpty(ErrorMessage) ? ReturnType.ToString() : (ReturnType.ToString() + " : " + ErrorMessage);
        }
#else
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool GetIsSuccess(CommandClientReturnValue returnValue)
        {
            return returnValue.IsSuccess;
        }
        /// <summary>
        /// Check the error status and throw an error exception
        /// 检查错误状态并抛出错误异常
        /// </summary>
        /// <param name="returnValue"></param>
        internal static void CheckThrowException(CommandClientReturnValue returnValue)
        {
            if (returnValue.IsSuccess) return;
            if (string.IsNullOrEmpty(returnValue.ErrorMessage)) throw new Exception(returnValue.ReturnType.ToString());
            throw new Exception(returnValue.ReturnType.ToString() + " : " + returnValue.ErrorMessage);
        }
#endif
    }
    /// <summary>
    /// Command client calls the return value
    /// 命令客户端调用返回值
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientReturnValue<T>
    {
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Value;
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        public string? ErrorMessage;
#else
        public string ErrorMessage;
#endif
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// Error return value
        /// 错误返回值
        /// </summary>
        public CommandClientReturnValue ReturnValue { get { return new CommandClientReturnValue(ReturnType, ErrorMessage); } }
        /// <summary>
        /// Command client calls the return value
        /// 命令客户端调用返回值
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        private CommandClientReturnValue(T? value)
#else
        private CommandClientReturnValue(T value)
#endif
        {
            Value = value;
            ReturnType = CommandClientReturnTypeEnum.Success;
            ErrorMessage = null;
        }
        /// <summary>
        /// Error return value
        /// 错误返回值
        /// </summary>
        /// <param name="returnValue"></param>
        internal CommandClientReturnValue(ref CommandClientReturnValue returnValue)
        {
            Value = default(T);
            ReturnType = returnValue.ReturnType;
            ErrorMessage = returnValue.ErrorMessage;
        }
        /// <summary>
        /// Error return value
        /// 错误返回值
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        public CommandClientReturnValue(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        public CommandClientReturnValue(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            Value = default(T);
            ReturnType = returnType;
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Return value type conversion
        /// 返回值类型转换
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <param name="getValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<VT> Cast<VT>(Func<T, VT> getValue)
        {
            if (ReturnType == CommandClientReturnTypeEnum.Success) return getValue(Value);
            return new CommandClientReturnValue<VT>(ReturnType, ErrorMessage);
        }
        /// <summary>
        /// Implicit conversion
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public static implicit operator CommandClientReturnValue<T>(T? value)
#else
        public static implicit operator CommandClientReturnValue<T>(T value)
#endif
        {
            return new CommandClientReturnValue<T>(value);
        }
        /// <summary>
        /// Implicit conversion
        /// 隐式转换
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public static implicit operator CommandClientReturnValue<T>(CommandClientReturnValue returnValue)
        {
            return new CommandClientReturnValue<T>(returnValue.ReturnType, returnValue.ErrorMessage);
        }
        /// <summary>
        /// Get the return value
        /// 获取返回值
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        /// <returns>Return value</returns>
        public T GetValue(bool isIgnoreError = false)
        {
            if (ReturnType == CommandClientReturnTypeEnum.Success) return Value;
            if (!isIgnoreError) throw new Exception($"调用返回类型错误 {ReturnType} {ErrorMessage}");
            return Value;
        }
        /// <summary>
        /// Get return value
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnValue<T> GetReturnValue(T value)
        {
            return new CommandClientReturnValue<T>(value);
        }
        /// <summary>
        /// Get the error return value
        /// 获取错误返回值
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnValue<T> GetReturnValue(CommandClientReturnValue returnValue)
        {
            return new CommandClientReturnValue<T>(ref returnValue);
        }
        ///// <summary>
        ///// 获取错误返回值
        ///// </summary>
        ///// <param name="returnType"></param>
        ///// <param name="errorMessage"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static CommandClientReturnValue<T> GetReturnValue(CommandClientReturnType returnType, string errorMessage)
        //{
        //    return new CommandClientReturnValue<T>(returnType, errorMessage);
        //}
    }
}
