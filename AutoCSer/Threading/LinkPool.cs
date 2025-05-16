using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 缓存对象链表（用于冲突概率低的场景）
    /// </summary>
    /// <typeparam name="T">缓存对象类型</typeparam>
    internal sealed class LinkPool<T> : LinkPoolBase<T>
        where T : Link<T>
    {
        /// <summary>
        /// 是否需要异步释放资源
        /// </summary>
        private readonly static bool isAsyncDisponse;
        /// <summary>
        /// 是否需要释放资源
        /// </summary>
        private readonly static bool isDisponse;

        /// <summary>
        /// 链表
        /// </summary>
        /// <param name="parameter">默认链表缓存池参数</param>
        private LinkPool(LinkPoolParameter parameter) : base(parameter, typeof(LinkPool<T>), isDisponse, isAsyncDisponse) { }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value">不可为 null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push(T value)
        {
            if (count < maxCount) push(value);
            else if (isDisponse) dispose(value);
        }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value">不可为 null</param>
        /// <returns>是否添加成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int IsPush(T value)
        {
            if (count < maxCount)
            {
                push(value);
                return 1;
            }
            if (isDisponse) dispose(value);
            return 0;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? Pop()
#else
        internal T Pop()
#endif
        {
            return pop();
        }
        ///// <summary>
        ///// 添加链表
        ///// </summary>
        ///// <param name="value">链表头部</param>
        ///// <param name="end">链表尾部</param>
        ///// <param name="count">数据数量</param>
        //internal void PushLink(T value, T end, int count)
        //{
        //    System.Threading.Interlocked.Add(ref this.count, count);
        //    var headValue = default(T);
        //    do
        //    {
        //        if ((headValue = head) == null)
        //        {
        //            end.LinkNext = null;
        //            if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null) return;
        //        }
        //        else
        //        {
        //            end.LinkNext = headValue;
        //            if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
        //        }
        //        AutoCSer.Threading.ThreadYield.Yield();
        //    }
        //    while (true);
        //}

        ///// <summary>
        ///// 释放列表
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private static async Task disposeLinkAsync(T value)
        //{
        //    var node = value;
        //    do
        //    {
        //        try
        //        {
        //            do
        //            {
        //                await ((IAsyncDisposable)node).DisposeAsync();
        //                node = node.LinkNext;
        //            }
        //            while (node != null);
        //            return;
        //        }
        //        catch (Exception exception)
        //        {
        //            await AutoCSer.LogHelper.Exception(exception);
        //        }
        //        node = node.notNull().LinkNext;
        //    }
        //    while (node != null);
        //}
        /// <summary>
        /// 链表节点池
        /// </summary>
        internal readonly static LinkPool<T> Default = new LinkPool<T>(AutoCSer.Common.Config.GetLinkPoolParameter(typeof(T)));

        static LinkPool()
        {
            isAsyncDisponse = typeof(IAsyncDisposable).IsAssignableFrom(typeof(T));
            isDisponse = isAsyncDisponse || typeof(IDisposable).IsAssignableFrom(typeof(T));
        }
    }
    /// <summary>
    /// 缓存对象链表（用于冲突概率低的场景）
    /// </summary>
    /// <typeparam name="T">缓存对象类型</typeparam>
    /// <typeparam name="LT">链表节点泛型类型</typeparam>
    internal sealed class LinkPool<T, LT> : LinkPoolBase<LT>
        where T : LT
        where LT : Link<LT>
    {
        /// <summary>
        /// 是否需要异步释放资源
        /// </summary>
        private readonly static bool isAsyncDisponse;
        /// <summary>
        /// 是否需要释放资源
        /// </summary>
        private readonly static bool isDisponse;

        /// <summary>
        /// 链表
        /// </summary>
        /// <param name="parameter">默认链表缓存池参数</param>
        private LinkPool(LinkPoolParameter parameter) : base(parameter, typeof(LinkPool<T, LT>), isDisponse, isAsyncDisponse) { }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value">不可为 null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push(T value)
        {
            if (count < maxCount) push(value);
            else if (isDisponse) dispose(value);
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T? Pop()
#else
        internal T Pop()
#endif
        {
            return pop().castClass<T>();
        }

        /// <summary>
        /// 链表节点池
        /// </summary>
        internal readonly static LinkPool<T, LT> Default = new LinkPool<T, LT>(AutoCSer.Common.Config.GetLinkPoolParameter(typeof(T)));

        static LinkPool()
        {
            isAsyncDisponse = typeof(IAsyncDisposable).IsAssignableFrom(typeof(T));
            isDisponse = isAsyncDisponse || typeof(IDisposable).IsAssignableFrom(typeof(T));
        }
    }
}
