using AutoCSer.Extensions;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 套接字异步事件对象池
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SocketAsyncEventArgsPool
    {
        /// <summary>
        /// 套接字异步事件对象池首节点
        /// </summary>
#if NetStandard21
        private SocketAsyncEventArgs? head;
#else
        private SocketAsyncEventArgs head;
#endif
        /// <summary>
        /// 缓存数量
        /// </summary>
        private int count;
        /// <summary>
        /// 最大缓存数量
        /// </summary>
        private int maxCount;
        /// <summary>
        /// 套接字异步事件对象池
        /// </summary>
        /// <param name="maxCount"></param>
        internal SocketAsyncEventArgsPool(int maxCount)
        {
            head = null;
            count = 0;
            this.maxCount = maxCount;
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        internal SocketAsyncEventArgs Get()
        {
            do
            {
                var thisHead = head;
                if (thisHead == null)
                {
                    thisHead = new SocketAsyncEventArgs();
                    thisHead.SocketFlags = System.Net.Sockets.SocketFlags.None;
                    thisHead.DisconnectReuseSocket = false;
                    return thisHead;
                }
                if (object.ReferenceEquals(Interlocked.CompareExchange(ref head, thisHead.UserToken.castClass<SocketAsyncEventArgs>(), thisHead), thisHead))
                {
                    System.Threading.Interlocked.Decrement(ref count);
                    thisHead.UserToken = null;
                    return thisHead;
                }
            }
            while (true);
        }
        /// <summary>
        /// 释放套接字异步事件对象
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            maxCount = 0;
            free();
        }
        /// <summary>
        /// 释放套接字异步事件对象
        /// </summary>
        private void free()
        {
            for (var value = System.Threading.Interlocked.Exchange(ref head, null); value != null; value = value.UserToken.castClass<SocketAsyncEventArgs>())
            {
                System.Threading.Interlocked.Decrement(ref count);
                value.Dispose();
            }
        }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        internal void Push(SocketAsyncEventArgs value)
        {
            if (count >= maxCount)
            {
                value.Dispose();
                return;
            }
            value.SetBuffer(null, 0, 0);
            value.UserToken = null;
            value.SocketError = SocketError.Success;
            System.Threading.Interlocked.Increment(ref count);
            do
            {
                var thisHead = head;
                if (thisHead == null)
                {
                    value.UserToken = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null)
                    {
                        if (maxCount == 0) free();
                        return;
                    }
                }
                else
                {
                    value.UserToken = thisHead;
                    if (object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref head, value, thisHead), thisHead))
                    {
                        if (maxCount == 0) free();
                        return;
                    }
                }
            }
            while (true);
        }
        ///// <summary>
        ///// 添加节点
        ///// </summary>
        ///// <param name="value"></param>
        //internal void PushNotNull(ref SocketAsyncEventArgs value)
        //{
        //    SocketAsyncEventArgs newValue = Interlocked.Exchange(ref value, null);
        //    if (newValue != null)
        //    {
        //        if (count >= maxCount)
        //        {
        //            newValue.Dispose();
        //            return;
        //        }
        //        System.Threading.Interlocked.Increment(ref count);
        //        newValue.SetBuffer(null, 0, 0);
        //        newValue.UserToken = null;
        //        newValue.SocketError = SocketError.Success;
        //        SocketAsyncEventArgs oldHead;
        //        do
        //        {
        //            if ((oldHead = head) == null)
        //            {
        //                newValue.UserToken = null;
        //                if (System.Threading.Interlocked.CompareExchange(ref head, newValue, null) == null)
        //                {
        //                    if (maxCount == 0) free();
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                newValue.UserToken = oldHead;
        //                if (System.Threading.Interlocked.CompareExchange(ref head, newValue, oldHead) == oldHead)
        //                {
        //                    if (maxCount == 0) free();
        //                    return;
        //                }
        //            }
        //            AutoCSer.Threading.ThreadYield.Yield();
        //        }
        //        while (true);
        //    }
        //}
    }
}
