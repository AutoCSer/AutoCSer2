using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时任务同步节点链表
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SecondTimerArrayNodeLink
    {
        /// <summary>
        /// 任务尾节点
        /// </summary>
#if NetStandard21
        private SecondTimerArrayNode? end;
#else
        private SecondTimerArrayNode end;
#endif
        /// <summary>
        /// 任务首节点
        /// </summary>
#if NetStandard21
        private SecondTimerArrayNode? head;
#else
        private SecondTimerArrayNode head;
#endif
        /// <summary>
        /// 添加尾节点
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Append(SecondTimerArrayNode next)
        {
            if (end == null) head = end = next;
            else
            {
                next.DoubleLinkPrevious = end;
                end.DoubleLinkNext = next;
                end = next;
            }
        }
        /// <summary>
        /// 将另外一个链表的首节点添加到尾节点并返回下一个节点
        /// </summary>
        /// <param name="otherHead"></param>
        /// <returns></returns>
#if NetStandard21
        internal SecondTimerArrayNode? AppendOtherHead(SecondTimerArrayNode otherHead)
#else
        internal SecondTimerArrayNode AppendOtherHead(SecondTimerArrayNode otherHead)
#endif
        {
            var next = otherHead.DoubleLinkNext;
            if (next == null) Append(otherHead);
            else
            {
                otherHead.DoubleLinkNext = null;
                Append(otherHead);
                next.DoubleLinkPrevious = null;
            }
            return next;
        }
        /// <summary>
        /// 获取首节点并且清除数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal SecondTimerArrayNode? GetClear()
#else
        internal SecondTimerArrayNode GetClear()
#endif
        {
            var value = head;
            head = end = null;
            return value;
        }
    }
}
