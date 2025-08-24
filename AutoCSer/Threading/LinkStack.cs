using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 无锁栈（用于指针）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LinkStack
    {
        /// <summary>
        /// 头节点指针
        /// </summary>
        internal long Head;
        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="value"></param>
        internal void Push(void* value)
        {
#if DEBUG
            if (value == null) throw new Exception("value == null");
#endif
            do
            {
                long head = Head;
                *(long*)value = head;
                if (System.Threading.Interlocked.CompareExchange(ref Head, (long)value, head) == head) return;
            }
            while (true);
        }
        /// <summary>
        /// 弹出一个数据
        /// </summary>
        /// <returns></returns>
        internal void* Pop()
        {
            do
            {
                long head = Head;
                if (head != 0)
                {
                    if (System.Threading.Interlocked.CompareExchange(ref Head, *(long*)head, head) == head)
                    {
                        return (void*)head;
                    }
                }
                else return null;
            }
            while (true);
        }
        /// <summary>
        /// 获取栈链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void* Get()
        {
            return (void*)System.Threading.Interlocked.Exchange(ref Head, 0);
        }
    }
    /// <summary>
    /// 无锁栈
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct LinkStack<T>
        where T : Link<T>
    {
        /// <summary>
        /// 链表头部
        /// </summary>
#if NetStandard21
        private T? head;
#else
        private T head;
#endif
        /// <summary>
        /// 是否空链表
        /// </summary>
        internal bool IsEmpty
        {
            get { return head == null; }
        }
        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushOnly(T value)
        {
            value.LinkNext = head;
            head = value;
        }
        ///// <summary>
        ///// 设置链表头部
        ///// </summary>
        ///// <param name="value"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void SetHead(T value)
        //{
        //    System.Threading.Interlocked.Exchange(ref head, value);
        //}
        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="value"></param>
        internal void Push(T value)
        {
#if DEBUG
                if (value == null) throw new Exception("value == null");
                if (value.LinkNext != null) throw new Exception("value.LinkNext != null");
#endif
            do
            {
                var thisHead = this.head;
                value.LinkNext = thisHead;
                if (object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref this.head, value, thisHead), thisHead)) return;
            }
            while (true);
        }
        /// <summary>
        /// 添加首节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool TryPushHead(T value)
        {
            return head == null && System.Threading.Interlocked.CompareExchange(ref head, value, null) == null;
        }
        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool IsPushHead(T value)
        {
#if DEBUG
                if (value == null) throw new Exception("value == null");
                if (value.LinkNext != null) throw new Exception("value.LinkNext != null");
#endif
            do
            {
                var thisHead = this.head;
                value.LinkNext = thisHead;
                if (object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref this.head, value, thisHead), thisHead)) return thisHead == null;
            }
            while (true);
        }
        /// <summary>
        /// 添加栈链表
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushLink(T head, T end)
        {
            IsPushHeadLink(head, end);
        }
        /// <summary>
        /// 添加栈链表
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal bool IsPushHeadLink(T head, T end)
        {
            do
            {
                var thisHead = this.head;
                end.LinkNext = thisHead;
                if (object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref this.head, head, thisHead), thisHead)) return thisHead == null;
            }
            while (true);
        }
        /// <summary>
        /// 弹出一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal T? Pop()
#else
        internal T Pop()
#endif
        {
            do
            {
                var thisHead = this.head;
                if (thisHead != null)
                {
                    if (object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref this.head, thisHead.LinkNext, thisHead), thisHead))
                    {
                        thisHead.LinkNext = null;
                        return thisHead;
                    }
                }
                else return null;
            }
            while (true);
        }
        /// <summary>
        /// 获取栈链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? Get()
#else
        internal T Get()
#endif
        {
            return System.Threading.Interlocked.Exchange(ref head, null);
        }
        /// <summary>
        /// 获取栈链表
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
#if NetStandard21
        internal T? Get(out T? end)
#else
        internal T Get(out T end)
#endif
        {
            var head = System.Threading.Interlocked.Exchange(ref this.head, null);
            end = head != null ? Link<T>.GetEnd(head) : null;
            return head;
        }
        /// <summary>
        /// 获取栈链表
        /// </summary>
        /// <param name="end"></param>
        internal void GetToEnd(ref T end)
        {
            var head = System.Threading.Interlocked.Exchange(ref this.head, null);
            if (head != null)
            {
                end.LinkNext = head;
                end = Link<T>.GetEnd(head);
            }
        }
        /// <summary>
        /// 获取队列链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? GetQueue()
#else
        internal T GetQueue()
#endif
        {
            return System.Threading.Interlocked.Exchange(ref head, null)?.Reverse();
        }
        /// <summary>
        /// 获取队列链表
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? GetQueue(out T? end)
#else
        internal T GetQueue(out T end)
#endif
        {
            end = System.Threading.Interlocked.Exchange(ref head, null);
            return end?.Reverse();
        }
        /// <summary>
        /// 获取队列链表
        /// </summary>
        /// <param name="end"></param>
        internal void GetQueueToEnd(ref T end)
        {
            var head = System.Threading.Interlocked.Exchange(ref this.head, null);
            if (head != null)
            {
                end.LinkNext = head.Reverse();
                end = head;
            }
        }
        /// <summary>
        /// 获取队列链表
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
#if NetStandard21
        internal void GetQueueToEnd(ref T? head, [MaybeNull] ref T end)
#else
        internal void GetQueueToEnd(ref T head, ref T end)
#endif
        {
            var thisHead = System.Threading.Interlocked.Exchange(ref this.head, null);
            if (thisHead != null)
            {
                head = thisHead.Reverse();
                end.LinkNext = head;
                end = thisHead;
            }
        }
    }
}
