using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// TCP server-side asynchronous keep callback
    /// TCP 服务器端异步保持回调
    /// </summary>
    public class CommandServerKeepCallback : CommandServerCallback
    {
        /// <summary>
        /// Has the keep callback been cancelled
        /// 是否已经取消保持回调
        /// </summary>
        internal volatile int IsCancelKeep;
        /// <summary>
        /// Has the keep callback been cancelled
        /// 是否已经取消保持回调
        /// </summary>
        public bool IsCancelKeepCallback { get { return IsCancelKeep != 0; } }
        /// <summary>
        /// Output data count
        /// 输出数据计数
        /// </summary>
        protected int outputCount;
        /// <summary>
        /// Empty callback
        /// </summary>
        protected CommandServerKeepCallback() { IsCancelKeep = 1; }
#if !AOT
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        protected CommandServerKeepCallback(CommandServerSocket socket, OfflineCount offlineCount) : base(socket, offlineCount)
        {
            socket.Add(this);
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="socket"></param>
        protected CommandServerKeepCallback(CommandServerSocket socket) : base(socket)
        {
            socket.Add(this);
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerKeepCallback(CommandServerCallQueueNode node) : base(node)
        {
            Socket.Add(this);
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerKeepCallback(CommandServerCallReadWriteQueueNode node) : base(node)
        {
            Socket.Add(this);
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerKeepCallback(CommandServerCallConcurrencyReadQueueNode node) : base(node)
        {
            Socket.Add(this);
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        protected CommandServerKeepCallback(CommandServerCallTaskQueueNode node) : base(node)
        {
            Socket.Add(this);
        }
        /// <summary>
        /// Success status callback
        /// 成功状态回调
        /// </summary>
        public override bool Callback()
        {
            return IsCancelKeep == 0 && Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        }
        ///// <summary>
        ///// 返回数据集合以后关闭保持回调
        ///// </summary>
        ///// <param name="values"></param>
        ///// <returns></returns>
        //public async Task Callback(IEnumeratorTask values)
        //{
        //    if (IsCancelKeep == 0)
        //    {
        //        if (values != null)
        //        {
        //            try
        //            {
        //                while (IsCancelKeep == 0 && await values.MoveNextAsync())
        //                {
        //                    if (IsCancelKeep == 0 && !Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success))
        //                    {
        //                        SetCancelKeep();
        //                    }
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                SetCancelKeep();
        //                Socket.RemoveKeepCallback(CallbackIdentity, exception);
        //            }
        //            finally { await values.DisposeAsync(); }
        //        }
        //        if (IsCancelKeep == 0)
        //        {
        //            SetCancelKeep();
        //            Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        //        }
        //    }
        //    else if (values != null) await values.DisposeAsync();
        //}
        /// <summary>
        /// Cancel the keep callback command
        /// 取消保持回调命令
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        public override void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception? exception = null)
#else
        public override void CancelKeep(CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success, Exception exception = null)
#endif
        {
            if (IsCancelKeep == 0)
            {
                SetCancelKeep();
                Socket.RemoveKeepCallback(CallbackIdentity, returnType, exception);
            }
        }
        
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback CreateServerKeepCallback(CommandServerSocket socket)
        {
            return new CommandServerKeepCallback(socket, OfflineCount.Null);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback CreateServerKeepCallback(CommandServerCallQueueNode node)
        {
            return new CommandServerKeepCallback(node);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback CreateServerKeepCallback(CommandServerCallReadWriteQueueNode node)
        {
            return new CommandServerKeepCallback(node);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback CreateServerKeepCallback(CommandServerCallConcurrencyReadQueueNode node)
        {
            return new CommandServerKeepCallback(node);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback CreateServerKeepCallback(CommandServerCallTaskQueueNode node)
        {
            return new CommandServerKeepCallback(node);
        }
#endif
        /// <summary>
        /// Cancel the keep callback command
        /// 取消保持回调命令
        /// </summary>
        internal virtual void SetCancelKeep()
        {
            IsCancelKeep = 1;
        }

        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="command"></param>
        private void callback(AutoCSer.Net.CommandClientReturnValue returnValue, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (returnValue.IsSuccess) Callback();
            else
            {
                CancelKeep(returnValue.ReturnType);
                command.Dispose();
            }
        }
        /// <summary>
        /// Implicitly converted to client callback delegate
        /// 隐式转换为客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand>(CommandServerKeepCallback callback) { return callback.callback; }
        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="queue"></param>
        /// <param name="command"></param>
        private void callback(AutoCSer.Net.CommandClientReturnValue returnValue, AutoCSer.Net.CommandClientCallQueue queue, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (returnValue.IsSuccess) Callback();
            else
            {
                CancelKeep(returnValue.ReturnType);
                command.Dispose();
            }
        }
        /// <summary>
        /// Implicitly converted to client callback delegate
        /// 隐式转换为客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>(CommandServerKeepCallback callback) { return callback.callback; }

        /// <summary>
        /// Cancel the keep callback command
        /// 取消保持回调命令
        /// </summary>
        /// <param name="keepCallback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CancelKeep(CommandServerKeepCallback keepCallback)
        {
            keepCallback.CancelKeep();
        }

        /// <summary>
        /// The default empty TCP server-side asynchronous keep callback
        /// 默认空 TCP 服务器端异步保持回调
        /// </summary>
        internal static readonly CommandServerKeepCallback Null = new CommandServerKeepCallback();
    }
    /// <summary>
    /// TCP server-side asynchronous keep callback
    /// TCP 服务器端异步保持回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerKeepCallback<T> : CommandServerKeepCallback
    {
        /// <summary>
        /// The next node of the linked list
        /// 链表下一个节点
        /// </summary>
#if NetStandard21
        internal CommandServerKeepCallback<T>? LinkNext;
#else
        internal CommandServerKeepCallback<T> LinkNext;
#endif
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        internal readonly ServerInterfaceMethod Method;
        /// <summary>
        /// Empty callback
        /// </summary>
        internal CommandServerKeepCallback()
        {
#if NetStandard21
            Method = CommandServerConfig.NullServerInterfaceMethod;
#endif
        }
#if AOT
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        public virtual bool VirtualCallback(T returnValue)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Return a collection of data
        /// 返回数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="isCancel">Whether to close the callback after the callback is completed
        /// 回调完成之后是否关闭回调</param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        public virtual bool Callback(IEnumerable<T> values, bool isCancel = true)
        {
            throw new NotImplementedException();
        }
#else
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        /// <param name="method"></param>
        internal CommandServerKeepCallback(CommandServerSocket socket, OfflineCount offlineCount, ServerInterfaceMethod method) : base(socket, offlineCount)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerKeepCallback(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallback(CommandServerCallQueueNode node, ServerInterfaceMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallback(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallback(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronous keep callback
        /// TCP 服务器端异步保持回调
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallback(CommandServerCallTaskQueueNode node, ServerInterfaceMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Callback(T returnValue)
        {
            return IsCancelKeep == 0 && Socket.Send(CallbackIdentity, Method, new ServerReturnValue<T>(returnValue));
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        public virtual bool VirtualCallback(T returnValue)
        {
            return Callback(returnValue);
        }
        ///// <summary>
        ///// 返回值回调
        ///// </summary>
        ///// <param name="returnValue"></param>
        ///// <returns>是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public bool Callback(ref T returnValue)
        //{
        //    return IsCancelKeep == 0 && Socket.Send(CallbackIdentity, Method, new ServerReturnValue<T>(ref returnValue));
        //}
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="onFree"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Callback(T returnValue, Action onFree)
        {
            return IsCancelKeep == 0 && Socket.Send(CallbackIdentity, Method, new ServerReturnValue<T>(returnValue), onFree);
        }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal virtual bool Callback(CommandServerCallQueue queue, T returnValue)
        {
            if(IsCancelKeep == 0)
            {
                ServerReturnValue<T> result = new ServerReturnValue<T>(returnValue);
                queue.Send(Socket, Socket.GetOutput(CallbackIdentity, Method, ref result));
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Return a collection of data
        /// 返回数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="isCancel">Whether to close the callback after the callback is completed
        /// 回调完成之后是否关闭回调</param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        public virtual bool Callback(IEnumerable<T> values, bool isCancel = true)
        {
            if (IsCancelKeep == 0)
            {
                if (values != null)
                {
                    try
                    {
                        if (!Socket.SendKeepCallback(CallbackIdentity, Method, values)) SetCancelKeep();
                    }
                    catch (Exception exception)
                    {
                        SetCancelKeep();
                        Socket.RemoveKeepCallback(CallbackIdentity, exception);
                    }
                }
                if (isCancel && IsCancelKeep == 0)
                {
                    SetCancelKeep();
                    Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
                }
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 返回数据集合以后关闭保持回调
        ///// </summary>
        ///// <param name="values"></param>
        ///// <returns></returns>
        //public async Task CallbackAsync(IEnumeratorTask<T> values)
        //{
        //    if (IsCancelKeep == 0)
        //    {
        //        if (values != null)
        //        {
        //            try
        //            {
        //                while (IsCancelKeep == 0 && await values.MoveNextAsync())
        //                {
        //                    if (IsCancelKeep == 0 && !Socket.Send(CallbackIdentity, Method, new ServerReturnValue<T>(values.Current)))
        //                    {
        //                        SetCancelKeep();
        //                    }
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                SetCancelKeep();
        //                Socket.RemoveKeepCallback(CallbackIdentity, exception);
        //            }
        //            finally { await values.DisposeAsync(); }
        //        }
        //        if (IsCancelKeep == 0)
        //        {
        //            SetCancelKeep();
        //            Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        //        }
        //    }
        //    else if (values != null) await values.DisposeAsync();
        //}
#if NetStandard21
        /// <summary>
        /// Close the callback when the data collection is returned
        /// 返回数据集合以后关闭回调
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task CallbackAsync(IAsyncEnumerator<T> values)
        {
            if (IsCancelKeep == 0)
            {
                if (values != null)
                {
                    try
                    {
                        while (IsCancelKeep == 0 && await values.MoveNextAsync())
                        {
                            if (IsCancelKeep == 0 && !Socket.Send(CallbackIdentity, Method, new ServerReturnValue<T>(values.Current)))
                            {
                                IsCancelKeep = 1;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        IsCancelKeep = 1;
                        Socket.RemoveKeepCallback(CallbackIdentity, exception);
                    }
                    finally { await values.DisposeAsync(); }
                }
                if (IsCancelKeep == 0)
                {
                    IsCancelKeep = 1;
                    Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
                }
            }
        }
#endif


        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback<T> CreateServerKeepCallback(CommandServerSocket socket, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallback<T>(socket, OfflineCount.Null, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback<T> CreateServerKeepCallback(CommandServerCallQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallback<T>(node, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback<T> CreateServerKeepCallback(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallback<T>(node, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback<T> CreateServerKeepCallback(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallback<T>(node, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallback<T> CreateServerKeepCallback(CommandServerCallTaskQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallback<T>(node, method);
        }

        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="command"></param>
        private void callback(AutoCSer.Net.CommandClientReturnValue<T> returnValue, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (returnValue.IsSuccess) Callback(returnValue.Value);
            else
            {
                CancelKeep(returnValue.ReturnType);
                command.Dispose();
            }
        }
        /// <summary>
        /// Implicitly converted to client callback delegate
        /// 隐式转换为客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<AutoCSer.Net.CommandClientReturnValue<T>, AutoCSer.Net.KeepCallbackCommand>(CommandServerKeepCallback<T> callback) { return callback.callback; }
        /// <summary>
        /// Client callback
        /// 客户端回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="queue"></param>
        /// <param name="command"></param>
        private void callback(AutoCSer.Net.CommandClientReturnValue<T> returnValue, AutoCSer.Net.CommandClientCallQueue queue, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (returnValue.IsSuccess) Callback(returnValue.Value);
            else
            {
                CancelKeep(returnValue.ReturnType);
                command.Dispose();
            }
        }
        /// <summary>
        /// Implicitly converted to client callback delegate
        /// 隐式转换为客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        public static implicit operator Action<AutoCSer.Net.CommandClientReturnValue<T>, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>(CommandServerKeepCallback<T> callback) { return callback.callback; }

        /// <summary>
        /// The TCP server side asynchronously maintains the callback linked list
        /// TCP 服务器端异步保持回调链表
        /// </summary>
        public struct Link
        {
            /// <summary>
            /// Head node
            /// </summary>
#if NetStandard21
            private CommandServerKeepCallback<T>? head;
#else
            private CommandServerKeepCallback<T> head;
#endif
            /// <summary>
            /// Whether a head node exists
            /// 是否存在头节点
            /// </summary>
            public bool IsHead { get { return head != null; } }
            /// <summary>
            /// Add the head node
            /// </summary>
            /// <param name="head"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void PushHead(CommandServerKeepCallback<T> head)
            {
                head.LinkNext = this.head;
                this.head = head;
            }
            /// <summary>
            /// Return value callback, clean up the callback failed object
            /// 返回值回调，清理回调失败对象
            /// </summary>
            /// <param name="value"></param>
            /// <returns>Number of callback outputs (callback success not guaranteed)
            /// 回调输出次数（不保证回调成功）</returns>
            public int Callback(T value)
            {
                int count = 0;
                while (head != null)
                {
                    if (head.Callback(value))
                    {
                        count = 1;
                        break;
                    }
                    head = head.LinkNext;
                }
                if (head != null)
                {
                    var current = head;
                    for (var next = current.LinkNext; next != null;)
                    {
                        if (next.Callback(value))
                        {
                            current = next;
                            ++count;
                            next = next.LinkNext;
                        }
                        else current.LinkNext = next = next.LinkNext;
                    }
                }
                return count;
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
#endif
        /// <summary>
        /// No output callback is supported
        /// 不支持无输出回调
        /// </summary>
        /// <returns></returns>
        public override bool Callback()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// Return the value callback and end the callback
        /// 返回值回调并结束回调
        /// </summary>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallbackCancelKeep(T returnValue)
        {
            if (VirtualCallback(returnValue)) CancelKeep();
        }
        /// <summary>
        /// Return the value callback and end the callback
        /// 返回值回调并结束回调
        /// </summary>
        /// <param name="returnValue"></param>
        public virtual void VirtualCallbackCancelKeep(T returnValue)
        {
            CallbackCancelKeep(returnValue);
        }

    }
}
