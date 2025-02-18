using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务返回结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LocalResult
    {
        /// <summary>
        /// 读取数据状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success; } }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="state"></param>
        internal LocalResult(CallStateEnum state)
        {
            CallState = state;
        }
    }
    /// <summary>
    /// 本地服务返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct LocalResult<T>
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
        /// 读取数据状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// 异常信息
        /// </summary>
#if NetStandard21
        public Exception? Exception;
#else
        public Exception Exception;
#endif
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success; } }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="state"></param>
        internal LocalResult(CallStateEnum state)
        {
            Value = default(T);
            CallState = state;
            Exception = null;
        }
        /// <summary>
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
        /// 返回结果
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
        /// 返回结果
        /// </summary>
        /// <param name="state"></param>
        public static implicit operator LocalResult<T>(CallStateEnum state) { return new LocalResult<T>(state); }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator LocalResult<T>(T? value) { return new LocalResult<T>(value); }
#else
        public static implicit operator LocalResult<T>(T value) { return new LocalResult<T>(value); }
#endif
        /// <summary>
        /// 返回结果类型转换
        /// </summary>
        /// <typeparam name="VT">目标类型</typeparam>
        /// <param name="defaultValue">成功状态默认值</param>
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
    }
}
