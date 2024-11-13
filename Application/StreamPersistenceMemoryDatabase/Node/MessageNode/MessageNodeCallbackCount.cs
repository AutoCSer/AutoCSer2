using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消费者回调信息
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MessageNodeCallbackCount<T> where T : Message<T>
    {
        /// <summary>
        /// 消费者回调
        /// </summary>
#if NetStandard21
        internal MethodKeepCallback<T?>? Callback;
#else
        internal MethodKeepCallback<T> Callback;
#endif
        /// <summary>
        /// 最大并发数量
        /// </summary>
        internal int MaxCount;
        /// <summary>
        /// 正在处理的回调计数
        /// </summary>
        internal int Count;
        /// <summary>
        /// 空闲并发数
        /// </summary>
        internal int FreeCount
        {
            get { return MaxCount - Count; }
        }
        /// <summary>
        /// 设置消费者回调
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="maxCount"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Set(MethodKeepCallback<T?> callback, int maxCount)
#else
        internal void Set(MethodKeepCallback<T> callback, int maxCount)
#endif
        {
            Callback = callback;
            MaxCount = Math.Max(maxCount, 1);
            Count = 0;
        }
        /// <summary>
        /// 设置消费者回调
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRemove(ref MessageNodeCallbackCount<T> callback)
        {
            Count = callback.Count;
            MaxCount = callback.MaxCount;
            Callback = callback.remove(Callback.notNull().Reserve);
        }
        /// <summary>
        /// 移除并复制消费者回调信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
#if NetStandard21
        private MethodKeepCallback<T?> remove(int index)
#else
        private MethodKeepCallback<T> remove(int index)
#endif
        {
            var callback = Callback.notNull();
            callback.Reserve = index;
            Callback = null;
            return callback;
        }
        /// <summary>
        /// 添加正在处理的回调计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IncrementCount()
        {
            return ++Count == MaxCount;
        }
        /// <summary>
        /// 释放正在处理的回调计数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool DecrementCount()
        {
            return Count-- == MaxCount;
        }
    }
}
