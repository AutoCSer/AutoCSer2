using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 数组链表节点
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class ArrayQueueNode<T> : Link<ArrayQueueNode<T>>
    {
        /// <summary>
        /// 数组
        /// </summary>
        private readonly T[] array;
        /// <summary>
        /// 数组链表
        /// </summary>
        private readonly ArrayQueue<T> link;
        /// <summary>
        /// 当前分配写入位置
        /// </summary>
        internal volatile int Index;
        /// <summary>
        /// 已写入数据数量
        /// </summary>
        private volatile int count;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        public LeftArray<T> Array
        {
            get { return new LeftArray<T>(count, array); }
        }
        /// <summary>
        /// 数组链表节点
        /// </summary>
        /// <param name="link">数组链表</param>
        internal ArrayQueueNode(ArrayQueue<T> link)
        {
            this.link = link;
            array = new T[link.ArraySize];
        }
        /// <summary>
        /// 重置节点数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ArrayQueueNode<T> Reset()
        {
            Index = 0;
            return this;
        }
        /// <summary>
        /// 释放节点并返回下一个节点
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public ArrayQueueNode<T>? Free()
#else
        public ArrayQueueNode<T> Free()
#endif
        {
            if (count != 0)
            {//重复释放检测（非严格检查）
                var nextNode = LinkNext;
                System.Array.Clear(array, 0, count);
                count = 0;
                link.Free(this);
                LinkNext = null;
                return nextNode;
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功</returns>
        internal bool Push(T value)
        {
            int index = Interlocked.Increment(ref Index);
            if ((uint)index <= (uint)array.Length)
            {
                array[index - 1] = value;
                count = index;
                if (index != array.Length)
                {
                    link.OnPush();
                    return true;
                }
                link.OnFullPush();
                return true;
            }
            do
            {
                ThreadYield.YieldOnly();
            }
            while (index >= array.Length && object.ReferenceEquals(this, link.WriteNode));
            return false;
        }
        /// <summary>
        /// 尝试获取链表首节点
        /// </summary>
        /// <returns></returns>
        internal bool TryGet()
        {
            do
            {
                int index = Index;
                if (index == 0 || (uint)index == (uint)array.Length) return false;
                if (Interlocked.CompareExchange(ref Index, array.Length, index) == index)
                {
                    if (link.TryGet())
                    {
                        while (count != index) ;
                        return true;
                    }
                    Interlocked.Exchange(ref Index, index);
                    return false;
                }
            }
            while (true);
        }
    }
}
