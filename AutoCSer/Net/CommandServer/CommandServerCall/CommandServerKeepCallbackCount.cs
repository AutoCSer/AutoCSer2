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
    /// TCP server-side asynchronously keep callback count
    /// TCP 服务器端异步保持回调计数
    /// </summary>
    public class CommandServerKeepCallbackCount : CommandServerKeepCallback
    {
        /// <summary>
        /// Output data access lock
        /// 输出数据访问锁
        /// </summary>
        internal readonly AutoCSer.Threading.SemaphoreSlimLock OutputLock;
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCount(CommandServerSocket socket, OfflineCount offlineCount, int outputCount) : base(socket, offlineCount)
        {
            this.outputCount = outputCount;
            OutputLock = new Threading.SemaphoreSlimLock(0, 1);
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCount(CommandServerSocket socket, int outputCount) : base(socket)
        {
            this.outputCount = outputCount;
            OutputLock = new Threading.SemaphoreSlimLock(0, 1);
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCount(CommandServerCallQueueNode node, int outputCount) : base(node)
        {
            this.outputCount = outputCount;
            OutputLock = new Threading.SemaphoreSlimLock(0, 1);
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCount(CommandServerCallReadWriteQueueNode node, int outputCount) : base(node)
        {
            this.outputCount = outputCount;
            OutputLock = new Threading.SemaphoreSlimLock(0, 1);
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCount(CommandServerCallConcurrencyReadQueueNode node, int outputCount) : base(node)
        {
            this.outputCount = outputCount;
            OutputLock = new Threading.SemaphoreSlimLock(0, 1);
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        protected CommandServerKeepCallbackCount(CommandServerCallTaskQueueNode node, int outputCount) : base(node)
        {
            this.outputCount = outputCount;
            OutputLock = new Threading.SemaphoreSlimLock(0, 1);
        }
        /// <summary>
        /// Release the output data count
        /// 释放输出数据计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeCount()
        {
            if (Interlocked.Increment(ref outputCount) <= 0) OutputLock.Exit();
        }
        /// <summary>
        /// Reset the count when the output data count is valid
        /// 当输出数据计数有效时重新设置计数
        /// </summary>
        /// <param name="outputCount">The validity is greater than 0
        /// 有效为大于 0</param>
        /// <returns>Whether the setting is successful or not, failure indicates that the current output data count is invalid
        /// 是否设置成功，失败表示当前输出数据计数无效</returns>
        public bool TrySetOutputCount(int outputCount)
        {
            if (outputCount > 0)
            {
                do
                {
                    int count = this.outputCount;
                    if (count > 0)
                    {
                        if (Interlocked.CompareExchange(ref this.outputCount, outputCount, count) == count) return true;
                    }
                    else return false;
                }
                while (true);
            }
            return false;

        }
        ///// <summary>
        ///// 返回值回调，保持回调计数等待
        ///// </summary>
        //public virtual async Task<bool> CallbackAsync()
        //{
        //    if (IsCancelKeep == 0)
        //    {
        //        var output = default(ServerOutputReturnTypeKeepCallbackCount);
        //        try
        //        {
        //            output = new ServerOutputReturnTypeKeepCallbackCount(CallbackIdentity, CommandClientReturnTypeEnum.Success, this);
        //            if (Interlocked.Decrement(ref outputCount) >= 0) return Socket.TryPush(output); ;
        //            await OutputLock.EnterAsync();
        //            return IsCancelKeep == 0 && Socket.TryPush(output);
        //        }
        //        finally
        //        {
        //            if (output == null) Socket.DisposeSocket();
        //        }
        //    }
        //    return false;
        //}
        /// <summary>
        /// Return value callback, keep callback count and wait
        /// 返回值回调，保持回调计数等待
        /// </summary>
        public virtual TaskCastAwaiter<bool> CallbackAsync()
        {
            if (IsCancelKeep == 0)
            {
                var output = default(ServerOutputReturnTypeKeepCallbackCount);
                try
                {
                    output = new ServerOutputReturnTypeKeepCallbackCount(CallbackIdentity, CommandClientReturnTypeEnum.Success, this);
                    if (Interlocked.Decrement(ref outputCount) >= 0) return AutoCSer.Common.GetCompletedAwaiter(Socket.TryPush(output));
                    return new KeepCallbackCountCallbackAwaiter(this, output);
                }
                finally
                {
                    if (output == null) Socket.DisposeSocket();
                }
            }
            return CompletedTaskCastAwaiter<bool>.Default;
        }
        /// <summary>
        /// Cancel the keep callback command
        /// 取消保持回调命令
        /// </summary>
        internal override void SetCancelKeep()
        {
            if (Interlocked.CompareExchange(ref IsCancelKeep, 1, 0) == 0) OutputLock.Exit();
        }
        /// <summary>
        /// Try to cancel the keep callback command
        /// 尝试取消保持回调命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected bool tryCancelKeep()
        {
            if (Interlocked.CompareExchange(ref IsCancelKeep, 1, 0) == 0)
            {
                OutputLock.Exit();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount CreateServerKeepCallback(CommandServerSocket socket, int outputCount)
        {
            return new CommandServerKeepCallbackCount(socket, OfflineCount.Null, outputCount);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount CreateServerKeepCallback(CommandServerCallQueueNode node, int outputCount)
        {
            return new CommandServerKeepCallbackCount(node, outputCount);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount CreateServerKeepCallback(CommandServerCallReadWriteQueueNode node, int outputCount)
        {
            return new CommandServerKeepCallbackCount(node, outputCount);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount CreateServerKeepCallback(CommandServerCallConcurrencyReadQueueNode node, int outputCount)
        {
            return new CommandServerKeepCallbackCount(node, outputCount);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outputCount"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount CreateServerKeepCallback(CommandServerCallTaskQueueNode node, int outputCount)
        {
            return new CommandServerKeepCallbackCount(node, outputCount);
        }
        /// <summary>
        /// Add output
        /// 添加输出
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Push(ServerOutput output)
        {
            return IsCancelKeep == 0 && Socket.TryPush(output);
        }
    }
    /// <summary>
    /// TCP server-side asynchronously keep callback count
    /// TCP 服务器端异步保持回调计数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerKeepCallbackCount<T> : CommandServerKeepCallbackCount
    {
        /// <summary>
        /// The next node of the linked list
        /// 链表下一个节点
        /// </summary>
#if NetStandard21
        internal CommandServerKeepCallbackCount<T>? LinkNext;
#else
        internal CommandServerKeepCallbackCount<T> LinkNext;
#endif
        /// <summary>
        /// Server interface method information
        /// 服务端接口方法信息
        /// </summary>
        internal readonly ServerInterfaceMethod Method;
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        /// <param name="method"></param>
        internal CommandServerKeepCallbackCount(CommandServerSocket socket, OfflineCount offlineCount, ServerInterfaceMethod method) : base(socket, offlineCount, method.KeepCallbackOutputCount)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal CommandServerKeepCallbackCount(CommandServerSocket socket, ServerInterfaceMethod method) : base(socket, method.KeepCallbackOutputCount)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallbackCount(CommandServerCallQueueNode node, ServerInterfaceMethod method) : base(node, method.KeepCallbackOutputCount)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallbackCount(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method) : base(node, method.KeepCallbackOutputCount)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallbackCount(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method) : base(node, method.KeepCallbackOutputCount)
        {
            this.Method = method;
        }
        /// <summary>
        /// TCP server-side asynchronously keep callback count
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        private CommandServerKeepCallbackCount(CommandServerCallTaskQueueNode node, ServerInterfaceMethod method) : base(node, method.KeepCallbackOutputCount)
        {
            this.Method = method;
        }
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
        /// No output callback is supported
        /// 不支持无输出回调
        /// </summary>
        /// <returns></returns>
        public override TaskCastAwaiter<bool> CallbackAsync()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// Get the output information
        /// 获取输出信息
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns>Output information</returns>
        private ServerOutputKeepCallbackCount<ServerReturnValue<T>> getOutput(T returnValue)
        {
            if (Method.IsOutputPool)
            {
                var output = LinkPool<ServerOutputKeepCallbackCount<ServerReturnValue<T>>, ServerOutput>.Default.Pop();
                if (output != null)
                {
                    output.Set(CallbackIdentity, Method, new ServerReturnValue<T>(returnValue), this);
                    return output;
                }
            }
            return new ServerOutputKeepCallbackCount<ServerReturnValue<T>>(CallbackIdentity, Method, new ServerReturnValue<T>(returnValue), this);
        }
        /// <summary>
        /// Return value callback, and wait keep callback
        /// 返回值回调，保持回调计数等待
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="isPush"></param>
        /// <returns></returns>
#if NetStandard21
        private ServerOutputKeepCallbackCount<ServerReturnValue<T>>? tryCallback(T returnValue, ref bool isPush)
#else
        private ServerOutputKeepCallbackCount<ServerReturnValue<T>> tryCallback(T returnValue, ref bool isPush)
#endif
        {
            if (IsCancelKeep == 0)
            {
                var output = default(ServerOutputKeepCallbackCount<ServerReturnValue<T>>);
                try
                {
                    output = getOutput(returnValue);
                    if (Interlocked.Decrement(ref outputCount) >= 0)
                    {
                        isPush = Socket.TryPush(output);
                        return null;
                    }
                }
                finally
                {
                    if (output == null) Socket.DisposeSocket();
                }
                return output;
            }
            isPush = false;
            return null;
        }
        ///// <summary>
        ///// 返回值回调，保持回调计数等待
        ///// </summary>
        ///// <param name="output"></param>
        ///// <returns></returns>
        //private async Task<bool> callbackAsync(ServerOutputKeepCallbackCount<ServerReturnValue<T>> output)
        //{
        //    await OutputLock.EnterAsync();
        //    return IsCancelKeep == 0 && Socket.TryPush(output);
        //}
        ///// <summary>
        ///// 返回值回调，保持回调计数等待
        ///// </summary>
        ///// <param name="returnValue"></param>
        ///// <returns>是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        //public async Task<bool> CallbackAsync(T returnValue)
        //{
        //    if (IsCancelKeep == 0)
        //    {
        //        var output = default(ServerOutputKeepCallbackCount<ServerReturnValue<T>>);
        //        try
        //        {
        //            output = getOutput(returnValue);
        //            if (Interlocked.Decrement(ref outputCount) >= 0) return Socket.TryPush(output);
        //            await OutputLock.EnterAsync();
        //            return IsCancelKeep == 0 && Socket.TryPush(output);
        //        }
        //        finally
        //        {
        //            if (output == null) Socket.DisposeSocket();
        //        }
        //    }
        //    return false;
        //}
        /// <summary>
        /// Return value callback, and wait keep callback
        /// 返回值回调，保持回调计数等待
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        public TaskCastAwaiter<bool> CallbackAsync(T returnValue)
        {
            if (IsCancelKeep == 0)
            {
                var output = default(ServerOutputKeepCallbackCount<ServerReturnValue<T>>);
                try
                {
                    output = getOutput(returnValue);
                    if (Interlocked.Decrement(ref outputCount) >= 0) return AutoCSer.Common.GetCompletedAwaiter(Socket.TryPush(output));
                    return new KeepCallbackCountCallbackAwaiter(this, output);
                }
                finally
                {
                    if (output == null) Socket.DisposeSocket();
                }
            }
            return CompletedTaskCastAwaiter<bool>.Default;
        }
        ///// <summary>
        ///// 返回值回调，保持回调计数等待
        ///// </summary>
        ///// <param name="returnValue"></param>
        ///// <param name="onFree"></param>
        ///// <returns>是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        //public async Task<bool> CallbackAsync(T returnValue, Action onFree)
        //{
        //    if (IsCancelKeep == 0)
        //    {
        //        var output = default(ServerOutputKeepCallbackCountFree<ServerReturnValue<T>>);
        //        try
        //        {
        //            output = new ServerOutputKeepCallbackCountFree<ServerReturnValue<T>>(CallbackIdentity, method, new ServerReturnValue<T>(returnValue), this, onFree);
        //            if (Interlocked.Decrement(ref outputCount) >= 0) return Socket.TryPush(output);
        //            await OutputLock.EnterAsync();
        //            return IsCancelKeep == 0 && Socket.TryPush(output);
        //        }
        //        finally
        //        {
        //            if (output == null) Socket.DisposeSocket();
        //        }
        //    }
        //    return false;
        //}
        /// <summary>
        /// Return value callback, and wait keep callback
        /// 返回值回调，保持回调计数等待
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="onFree"></param>
        /// <returns>Whether successfully added to the output queue, a false return indicates that the channel has been closed
        /// 是否成功加入输出队列，返回 false 表示通道已关闭</returns>
        public TaskCastAwaiter<bool> CallbackAsync(T returnValue, Action onFree)
        {
            if (IsCancelKeep == 0)
            {
                var output = default(ServerOutputKeepCallbackCountFree<ServerReturnValue<T>>);
                try
                {
                    output = new ServerOutputKeepCallbackCountFree<ServerReturnValue<T>>(CallbackIdentity, Method, new ServerReturnValue<T>(returnValue), this, onFree);
                    if (Interlocked.Decrement(ref outputCount) >= 0) return AutoCSer.Common.GetCompletedAwaiter(Socket.TryPush(output));
                    return new KeepCallbackCountCallbackAwaiter(this, output);
                }
                finally
                {
                    if (output == null) Socket.DisposeSocket();
                }
            }
            return CompletedTaskCastAwaiter<bool>.Default;
        }
        /// <summary>
        /// Close the keep callback after returning the data collection
        /// 返回数据集合以后关闭保持回调
        /// </summary>
        /// <param name="values"></param>
        public async Task CallbackAsync(IEnumerable<T> values)
        {
            if (IsCancelKeep == 0)
            {
                if (values != null)
                {
                    try
                    {
                        bool isPush = false;
                        foreach (T value in values)
                        {
                            var output = tryCallback(value, ref isPush);
                            if (output != null) isPush = await new KeepCallbackCountCallbackAwaiter(this, output);//callbackAsync(output)
                            if (!isPush)
                            {
                                SetCancelKeep();
                                return;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        SetCancelKeep();
                        Socket.RemoveKeepCallback(CallbackIdentity, exception);
                        return;
                    }
                }
                if (tryCancelKeep()) Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
            }
        }
        /// <summary>
        /// Close the keep callback after returning the data collection
        /// 返回数据集合以后关闭保持回调
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        internal async Task EnumerableCallbackAsync(IEnumerable<T> values)
        {
            try
            {
                bool isPush = false;
                foreach (T value in values)
                {
                    var output = tryCallback(value, ref isPush);
                    if (output != null) isPush = await new KeepCallbackCountCallbackAwaiter(this, output);
                    if (!isPush)
                    {
                        SetCancelKeep();
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                SetCancelKeep();
                Socket.RemoveKeepCallback(CallbackIdentity, exception);
                return;
            }
            if (tryCancelKeep()) Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        }
        ///// <summary>
        ///// 返回数据集合以后关闭保持回调，保持回调计数等待
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
        //                bool isPush = false;
        //                while (IsCancelKeep == 0 && await values.MoveNextAsync())
        //                {
        //                    var output = tryCallback(values.Current, ref isPush);
        //                    if (output != null) isPush = await new KeepCallbackCountCallbackAwaiter(this, output);
        //                    if (!isPush) SetCancelKeep();
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                SetCancelKeep();
        //                Socket.RemoveKeepCallback(CallbackIdentity, exception);
        //            }
        //            finally { await values.DisposeAsync(); }
        //        }
        //        if (tryCancelKeep()) Socket.RemoveKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        //    }
        //    else if (values != null) await values.DisposeAsync();
        //}
#if NetStandard21
        /// <summary>
        /// Close the keep callback after returning the data collection
        /// 返回数据集合以后关闭保持回调
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
                        bool isPush = false;
                        while (IsCancelKeep == 0 && await values.MoveNextAsync())
                        {
                            var output = tryCallback(values.Current, ref isPush);
                            if(output != null) isPush = await new KeepCallbackCountCallbackAwaiter(this, output);
                            if (!isPush) IsCancelKeep = 1;
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
        internal static CommandServerKeepCallbackCount<T> CreateServerKeepCallback(CommandServerSocket socket, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackCount<T>(socket, OfflineCount.Null, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount<T> CreateServerKeepCallback(CommandServerCallQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackCount<T>(node, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount<T> CreateServerKeepCallback(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackCount<T>(node, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount<T> CreateServerKeepCallback(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackCount<T>(node, method);
        }
        /// <summary>
        /// Create an asynchronous callback object
        /// 创建异步回调对象
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerKeepCallbackCount<T> CreateServerKeepCallback(CommandServerCallTaskQueueNode node, ServerInterfaceMethod method)
        {
            return new CommandServerKeepCallbackCount<T>(node, method);
        }

        /// <summary>
        /// The TCP server side asynchronously maintains the callback linked list
        /// TCP 服务器端异步保持回调链表
        /// </summary>
        public sealed class Link
        {
            /// <summary>
            /// Head node
            /// </summary>
#if NetStandard21
            private CommandServerKeepCallbackCount<T>? head;
#else
            private CommandServerKeepCallbackCount<T> head;
#endif
            /// <summary>
            /// Add the head node
            /// </summary>
            /// <param name="head"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void PushHead(CommandServerKeepCallbackCount<T> head)
            {
                head.LinkNext = this.head;
                this.head = head;
            }
            /// <summary>
            /// Return value callback, clean up the callback failed object
            /// 返回值回调，清理回调失败对象
            /// </summary>
            /// <param name="value"></param>
            public async Task Callback(T value)
            {
                while (head != null && !await head.CallbackAsync(value)) head = head.LinkNext;
                if (head != null)
                {
                    var current = head;
                    for (var next = current.LinkNext; next != null;)
                    {
                        if (await next.CallbackAsync(value))
                        {
                            current = next;
                            next = next.LinkNext;
                        }
                        else current.LinkNext = next = next.LinkNext;
                    }
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
