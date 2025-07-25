﻿using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP server-side asynchronous callback
    /// TCP 服务器端异步回调
    /// </summary>
    public class CommandServerCallback : CommandServerCall//, IDisposable
    {
        /// <summary>
        /// Empty callback
        /// </summary>
        protected CommandServerCallback() { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        protected CommandServerCallback(CommandServerSocket socket) : base(socket) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        protected CommandServerCallback(CommandServerSocket socket, OfflineCount offlineCount) : base(socket, offlineCount) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerCallback(CommandServerCallQueueNode node) : base(node.Socket, node.CallbackIdentity, node.OfflineCount) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerCallback(CommandServerCallReadWriteQueueNode node) : base(node.Socket, node.CallbackIdentity, node.OfflineCount) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerCallback(CommandServerCallConcurrencyReadQueueNode node) : base(node.Socket, node.CallbackIdentity, node.OfflineCount) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerCallback(CommandServerCallTaskQueueNode node) : base(node.Socket, node.CallbackIdentity, node.OfflineCount) { }
        /// <summary>
        /// Failure callback
        /// 失败回调
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool Callback(CommandClientReturnTypeEnum returnType, Exception? exception)
#else
        public bool Callback(CommandClientReturnTypeEnum returnType, Exception exception)
#endif
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, returnType, exception);
        }
        /// <summary>
        /// Success status callback
        /// 成功状态回调
        /// </summary>
        /// <returns></returns>
        public virtual bool Callback()
        {
            return Callback(CommandClientReturnTypeEnum.Success, null);
        }
        /// <summary>
        /// Successful status queue synchronization callback
        /// 成功状态队列同步回调
        /// </summary>
        /// <returns></returns>
        internal virtual bool SynchronousCallback()
        {
            return Callback();
        }
        /// <summary>
        /// Cancel the keep callback command
        /// 取消保持回调命令
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        public virtual void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception? exception = null)
#else
        public virtual void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception exception = null)
#endif
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Create asynchronous callbacks
        /// 创建异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallback CreateServerCallback(CommandServerSocket socket)
        {
            return new CommandServerCallback(socket);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallback CreateServerCallback(CommandServerCallQueueNode node)
        {
            return new CommandServerCallback(node);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallback CreateServerCallback(CommandServerCallReadWriteQueueNode node)
        {
            return new CommandServerCallback(node);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallback CreateServerCallback(CommandServerCallConcurrencyReadQueueNode node)
        {
            return new CommandServerCallback(node);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerCallback CreateServerCallback(CommandServerCallTaskQueueNode node)
        {
            return new CommandServerCallback(node);
        }
    }
    /// <summary>
    /// TCP server-side asynchronous callback
    /// TCP 服务器端异步回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerCallback<T> : CommandServerCallback
    {
        /// <summary>
        /// The next node of the linked list
        /// 链表的下一个节点
        /// </summary>
#if NetStandard21
        internal CommandServerCallback<T>? LinkNext;
#else
        internal CommandServerCallback<T> LinkNext;
#endif
        /// <summary>
        /// Empty callback
        /// </summary>
        protected CommandServerCallback() { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        internal CommandServerCallback(CommandServerSocket socket) : base(socket)
        {
        }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        internal CommandServerCallback(CommandServerCallQueueNode node) : base(node) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        internal CommandServerCallback(CommandServerCallReadWriteQueueNode node) : base(node) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        internal CommandServerCallback(CommandServerCallConcurrencyReadQueueNode node) : base(node) { }
        /// <summary>
        /// TCP server-side asynchronous callback
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="node"></param>
        internal CommandServerCallback(CommandServerCallTaskQueueNode node) : base(node) { }
        /// <summary>
        /// No output callback is supported
        /// 不支持无输出回调
        /// </summary>
        /// <returns></returns>
        public override bool Callback()
        {
            checkOfflineCount();
            throw new InvalidOperationException();
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        public abstract bool Callback(T returnValue);
        /// <summary>
        /// Queue synchronization callback
        /// 队列同步回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal virtual bool SynchronousCallback(T returnValue)
        {
            return Callback(returnValue);
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool callback(ServerInterfaceMethod method, T returnValue)
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, method, new ServerReturnValue<T>(returnValue));
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="serverCallback"></param>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Callback(CommandServerCallback<T> serverCallback, ServerInterfaceMethod method, T returnValue)
        {
            return serverCallback.callback(method, returnValue);
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        private void synchronousCallback(CommandServerCallQueue queue, ServerInterfaceMethod method, T returnValue)
        {
            ServerReturnValue<T> result = new ServerReturnValue<T>(returnValue);
            ServerOutput output = Socket.GetOutput(CallbackIdentity, method, ref result);
            checkOfflineCount();
            queue.Send(Socket, output);
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="serverCallback"></param>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SynchronousCallback(CommandServerCallQueue queue, CommandServerCallback<T> serverCallback, ServerInterfaceMethod method, T returnValue)
        {
            serverCallback.synchronousCallback(queue, method, returnValue);
            return true;
        }

        /// <summary>
        /// TCP server-side asynchronous callback linked list
        /// TCP 服务器端异步回调链表
        /// </summary>
        public struct Link
        {
            /// <summary>
            /// Head node
            /// </summary>
#if NetStandard21
            private CommandServerCallback<T>? head;
#else
            private CommandServerCallback<T> head;
#endif
            /// <summary>
            /// Add the head node
            /// </summary>
            /// <param name="head"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void PushHead(CommandServerCallback<T> head)
            {
                head.LinkNext = this.head;
                this.head = head;
            }
            /// <summary>
            /// Return value callback, clean up the callback failed object
            /// 返回值回调，清理回调失败对象
            /// </summary>
            /// <param name="value"></param>
            public void Callback(T value)
            {
                while (head != null)
                {
                    head.Callback(value);
                    head = head.LinkNext;
                }
            }
            /// <summary>
            /// Cancel all callbacks
            /// 取消所有回调
            /// </summary>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void CancelKeep()
            {
                while (head != null)
                {
                    head.CancelKeep();
                    head = head.LinkNext;
                }
            }
        }
    }
}
