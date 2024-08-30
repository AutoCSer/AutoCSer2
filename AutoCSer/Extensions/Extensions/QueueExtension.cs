using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 队列相关扩展
    /// </summary>
    public static class QueueExtension
    {
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="value">弹出数据</param>
        /// <returns>返回 false 表示没有数据可以弹出</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryDequeue<T>(this Queue<T> queue, out T value)
        {
            if (queue.Count != 0)
            {
                value = queue.Dequeue();
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="value">下一个弹出数据</param>
        /// <returns>返回 false 表示没有数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek<T>(this Queue<T> queue, out T value)
        {
            if (queue.Count != 0)
            {
                value = queue.Peek();
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
