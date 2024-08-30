using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 正在处理的消息信息
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MessageArrayItem<T> where T : Message<T>
    {
        /// <summary>
        /// 正在处理的消息
        /// </summary>
        internal T Message;
        /// <summary>
        /// 消息回调
        /// </summary>
        private MethodKeepCallback<T> callback;
        /// <summary>
        /// 下一个空闲位置 或者 上一个正在处理的消息节点位置
        /// </summary>
        internal int NextIndex;
        /// <summary>
        /// 消息超时时间戳
        /// </summary>
        private long timeoutTimestamp;
        /// <summary>
        /// 设置消息数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        /// <param name="timeoutTimestamp"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Set(T message, MethodKeepCallback<T> callback, long timeoutTimestamp)
        {
            int nextIndex = NextIndex;
            this.callback = callback;
            this.Message = message;
            this.timeoutTimestamp = timeoutTimestamp + Stopwatch.GetTimestamp();
            //NextIndex = -1;
            return nextIndex;
        }
        /// <summary>
        /// 获取消息数据
        /// </summary>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T GetMessage(out int nextIndex)
        {
            nextIndex = NextIndex;
            return Message;
        }
        /// <summary>
        /// 释放消息处理状态（消息完成或者失败）
        /// </summary>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal MethodKeepCallback<T> Free(int nextIndex)
        {
            MethodKeepCallback<T> callback = this.callback;
            Message = null;
            NextIndex = nextIndex;
            this.callback = null;
            return callback;
        }
        /// <summary>
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
        /// 超时检查
        /// </summary>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T CheckTimeout(out int nextIndex)
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
