﻿using AutoCSer.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The message information being processed
    /// 正在处理的消息信息
    /// </summary>
    /// <typeparam name="T">Message data type
    /// 消息数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MessageArrayItem<T> where T : Message<T>
    {
        /// <summary>
        /// The message being processed
        /// 正在处理的消息
        /// </summary>
#if NetStandard21
        internal T? Message;
#else
        internal T Message;
#endif
        /// <summary>
        /// Message callback
        /// 消息回调
        /// </summary>
#if NetStandard21
        private MethodKeepCallback<T?>? callback;
#else
        private MethodKeepCallback<T> callback;
#endif
        /// <summary>
        /// The next free location or the location of the previous message node being processed
        /// 下一个空闲位置 或者 上一个正在处理的消息节点位置
        /// </summary>
        internal int NextIndex;
        /// <summary>
        /// Message timeout timestamp
        /// 消息超时时间戳
        /// </summary>
        private long timeoutTimestamp;
        /// <summary>
        /// Set the message data
        /// 设置消息数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        /// <param name="timeoutTimestamp"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal int Set(T message, MethodKeepCallback<T?> callback, long timeoutTimestamp)
#else
        internal int Set(T message, MethodKeepCallback<T> callback, long timeoutTimestamp)
#endif
        {
            int nextIndex = NextIndex;
            this.callback = callback;
            this.Message = message;
            this.timeoutTimestamp = timeoutTimestamp + Stopwatch.GetTimestamp();
            //NextIndex = -1;
            return nextIndex;
        }
        /// <summary>
        /// Get the message data
        /// 获取消息数据
        /// </summary>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? GetMessage(out int nextIndex)
#else
        internal T GetMessage(out int nextIndex)
#endif
        {
            nextIndex = NextIndex;
            return Message;
        }
        /// <summary>
        /// Release the message processing status (message completed or failed)
        /// 释放消息处理状态（消息完成或者失败）
        /// </summary>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal MethodKeepCallback<T?> Free(int nextIndex)
#else
        internal MethodKeepCallback<T> Free(int nextIndex)
#endif
        {
            var callback = this.callback.notNull();
            Message = null;
            NextIndex = nextIndex;
            this.callback = null;
            return callback;
        }
        /// <summary>
        /// Clear the message data
        /// 清除消息数据
        /// </summary>
        /// <param name="nextIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Clear(int nextIndex)
        {
            NextIndex = nextIndex;
            Message = null;
            callback = null;
        }
        /// <summary>
        /// Timeout check
        /// 超时检查
        /// </summary>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? CheckTimeout(out int nextIndex)
#else
        internal T CheckTimeout(out int nextIndex)
#endif
        {
            if (timeoutTimestamp <= Stopwatch.GetTimestamp())
            {
                nextIndex = NextIndex;
                return Message;
            }
            nextIndex = -1;
            return null;
        }
    }
}
