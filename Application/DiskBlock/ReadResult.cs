using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReadResult<T>
    {
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Value;
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
        /// 读取数据状态
        /// </summary>
        public ReadBufferStateEnum BufferState;
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return BufferState == ReadBufferStateEnum.Success && ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// 读取数据结果
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal ReadResult(CommandClientReturnTypeEnum returnType, string? errorMessage = null)
#else
        internal ReadResult(CommandClientReturnTypeEnum returnType, string errorMessage = null)
#endif
        {
            Value = default(T);
            ErrorMessage = errorMessage;
            ReturnType = returnType;
            BufferState = ReadBufferStateEnum.Unknown;
        }
        /// <summary>
        /// 读取数据结果
        /// </summary>
        /// <param name="state"></param>
        internal ReadResult(ReadBufferStateEnum state)
        {
            Value = default(T);
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            BufferState = state;
        }
        /// <summary>
        /// 读取数据结果
        /// </summary>
        /// <param name="value"></param>
        internal ReadResult(T value)
        {
            Value = value;
            ErrorMessage = null;
            ReturnType = CommandClientReturnTypeEnum.Success;
            BufferState = ReadBufferStateEnum.Success;
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T value)
        {
            Value = value;
            ReturnType = CommandClientReturnTypeEnum.Success;
            BufferState = ReadBufferStateEnum.Success;
        }
        /// <summary>
        /// 设置读取数据状态
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ReadBufferStateEnum state)
        {
            ReturnType = CommandClientReturnTypeEnum.Success;
            BufferState = state;
        }
    }
}
