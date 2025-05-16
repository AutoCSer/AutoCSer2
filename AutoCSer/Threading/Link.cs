using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.TextSerialize;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class Link<T>
        where T : Link<T>
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsIgnoreCurrent = true)]
        [AutoCSer.JsonSerializeMember(IsIgnoreCurrent = true)]
#if NetStandard21
        internal T? LinkNext;
#else
        internal T LinkNext;
#endif
        /// <summary>
        /// 获取并清除下一个节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? GetLinkNextClear()
#else
        internal T GetLinkNextClear()
#endif
        {
            var value = LinkNext;
            LinkNext = null;
            return value;
        }
        /// <summary>
        /// 获取链表最后一个节点
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        internal static T GetEnd(T head)
        {
            do
            {
                var end = head.LinkNext;
                if (end == null) return head;
                head = end;
            }
            while (true);
        }
        /// <summary>
        /// 获取链表最后一个节点
        /// </summary>
        /// <param name="head"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static T GetEnd(T head, out int count)
        {
            count = 1;
            do
            {
                var end = head.LinkNext;
                if (end == null) return head;
                head = end;
                ++count;
            }
            while (true);
        }
        /// <summary>
        /// 逆转链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T Reverse()
        {
            if(LinkNext != null)
            {
                T next = LinkNext;
                LinkNext = null;
                return reverse((T)this, next);
            }
            return (T)this;
        }
        /// <summary>
        /// 逆转链表
        /// </summary>
        /// <param name="head"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private static T reverse(T head, T next)
        {
            do
            {
                var nextNext = next.LinkNext;
                next.LinkNext = head;
                if (nextNext != null)
                {
                    head = next;
                    next = nextNext;
                }
                else return next;
            }
            while (true);
        }
        /// <summary>
        /// 获取中间节点
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        internal static T GetMiddle(T head)
        {
            T node = head;
            for(var next = head.LinkNext?.LinkNext; next != null; next = next.LinkNext?.LinkNext) node = node.LinkNext.notNull();
            return node;
        }
    }
}
