﻿using AutoCSer.Net;
using System;
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
        public ReadBufferStateEnum BufferState;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get { return BufferState == ReadBufferStateEnum.Success && ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// 读取数据结果
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
        internal ReadResult(CommandClientReturnTypeEnum returnType, string errorMessage = null)
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