using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 环状队列
    /// </summary>
    /// <typeparam name="T">队列元素数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RingQueue<T>
    {
        /// <summary>
        /// 队列数据集合
        /// </summary>
        private T[] queue;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private int readIndex;
        /// <summary>
        /// 当前写入位置
        /// </summary>
        private int writeIndex;
        /// <summary>
        /// 环状队列
        /// </summary>
        /// <param name="capacity">队列数据数量</param>
        public RingQueue(int capacity)
        {
            queue = new T[Math.Max(capacity, 1) + 1];
            readIndex = writeIndex = 0;
        }
        /// <summary>
        /// 尝试获取当前读取数据
        /// </summary>
        /// <param name="value">当前读取数据</param>
        /// <returns></returns>
#if NetStandard21
        public bool TryGetRead([MaybeNullWhen(false)] out T value)
#else
        public bool TryGetRead(out T value)
#endif
        {
            if (readIndex == writeIndex)
            {
                value = default(T);
                return false;
            }
            value = queue[readIndex];
            return true;
        }
        /// <summary>
        /// 尝试获取当前读取数据，读取成功则移动当前读取数据位置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        public bool TryRead([MaybeNullWhen(false)] out T value)
#else
        public bool TryRead(out T value)
#endif
        {
            if (TryGetRead(out value))
            {
                MoveRead();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移动当前读取数据位置
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void MoveRead()
        {
            queue.setDefault(readIndex);
            if (++readIndex == queue.Length) readIndex = 0;
        }
        /// <summary>
        /// 写入新数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>移除的数据</returns>
#if NetStandard21
        public T? Write(T value)
#else
        public T Write(T value)
#endif
        {
            queue[writeIndex] = value;
            if (++writeIndex == queue.Length)
            {
                writeIndex = 0;
                if (readIndex == 0)
                {
                    value = queue.getSetDefault(0);
                    readIndex = 1;
                    return value;
                }
            }
            else if (readIndex == writeIndex)
            {
                value = queue.getSetDefault(writeIndex);
                if (++readIndex == queue.Length) readIndex = 0;
                return value;
            }
            return default(T);
        }
    }
}
