using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Two-way linked list node
    /// 双向链表节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DoubleLink<T>
        where T : DoubleLink<T>
    {
        /// <summary>
        /// The next node
        /// </summary>
#if NetStandard21
        internal T? DoubleLinkNext;
#else
        internal T DoubleLinkNext;
#endif
        /// <summary>
        /// The previous node
        /// 上一个节点
        /// </summary>
#if NetStandard21
        internal T? DoubleLinkPrevious;
#else
        internal T DoubleLinkPrevious;
#endif
        /// <summary>
        /// Reset the linked list
        /// 重置链表状态
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ResetDoubleLink()
        {
            DoubleLinkNext = DoubleLinkPrevious = null;
        }
        /// <summary>
        /// Pop-up node
        /// 弹出节点
        /// </summary>
        /// <param name="linkLock">Linked list access lock
        /// 链表访问锁</param>
        /// <returns>Whether to pop the node or not, false indicates that repeated pop-up operations are not allowed
        /// 是否弹出节点，false 表示不允许重复弹出操作</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool freeNotEnd(ref AutoCSer.Threading.SpinLock linkLock)
        {
            if (DoubleLinkNext != null)
            {
                DoubleLinkNext.DoubleLinkPrevious = DoubleLinkPrevious;
                if (DoubleLinkPrevious != null)
                {
                    DoubleLinkPrevious.DoubleLinkNext = DoubleLinkNext;
                    DoubleLinkPrevious = null;
                }
                DoubleLinkNext = null;
                linkLock.Exit();
                return true;
            }
            //Those that have been released will not be dealt with
            //已经被释放的不做处理
            linkLock.Exit();
            return false;
        }

        /// <summary>
        /// Concurrent linked list
        /// 并发链表
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct YieldLink
        {
            /// <summary>
            /// Tail of the linked list
            /// 链表尾部
            /// </summary>
#if NetStandard21
            internal T? End;
#else
            internal T End;
#endif
            /// <summary>
            /// Linked list access lock
            /// 链表访问锁
            /// </summary>
            private AutoCSer.Threading.SpinLock linkLock;
            /// <summary>
            /// Add a node
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void PushNotNull(T value)
            {
                linkLock.EnterYield();
                if (End == null)
                {
                    End = value;
                    linkLock.Exit();
                }
                else
                {
                    End.DoubleLinkNext = value;
                    value.DoubleLinkPrevious = End;
                    End = value;
                    linkLock.Exit();
                }
            }
            /// <summary>
            /// Pop-up node
            /// 弹出节点
            /// </summary>
            /// <param name="value"></param>
            /// <returns>Whether to pop the node or not, false indicates that repeated pop-up operations are not allowed
            /// 是否弹出节点，false 表示不允许重复弹出操作</returns>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal bool PopNotNull(T value)
            {
                linkLock.EnterYield();
                if (value == End)
                {
                    End = value.DoubleLinkPrevious;
                    if (End != null)
                    {
                        End.DoubleLinkNext = null;
                        value.DoubleLinkPrevious = null;
                    }
                    linkLock.Exit();
                    return true;
                }
                return value.freeNotEnd(ref linkLock);
            }
            ///// <summary>
            ///// 清除数据
            ///// </summary>
            //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            //internal void Clear()
            //{
            //    while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
            //    End = null;
            //    System.Threading.Interlocked.Exchange(ref linkLock, 0);
            //}
        }
    }
}
