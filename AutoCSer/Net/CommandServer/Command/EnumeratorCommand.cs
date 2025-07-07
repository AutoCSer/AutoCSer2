using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Net
{
    /// <summary>
    /// The collection enumeration command does not support multi-threaded concurrent calls to await MoveNext (await EnumeratorCommand{T}, returning null indicates failure to add to the output queue).
    /// 集合枚举命令，不支持多线程并发调用 await MoveNext（await EnumeratorCommand，返回 null 表示添加到输出队列失败）
    /// </summary>
    public class EnumeratorCommand : KeepCommand, IDisposable
    {
        /// <summary>
        /// Whether the next data exists in the collection enumeration command
        /// 集合枚举命令是否存在下一个数据
        /// </summary>
        private readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// Return value queue access lock
        /// 返回值队列访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock queueLock;
#if DEBUG
        private int onReceiveCount;
        private int moveNextCount0;
        private int moveNextCount1;
        private int moveNextCount2;
#endif
        /// <summary>
        /// The collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// Error handling for generating the input data of the request command
        /// 生成请求命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            Close(returnType);
        }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
#if DEBUG
            ++onReceiveCount;
#endif
            CommandClientReturnTypeEnum returnType = (CommandClientReturnTypeEnum)(byte)data.Start;
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                queueLock.EnterYield();
                if (moveNext.Push() == 0)
                {
                    queueLock.Exit();
                    moveNext.SetNextValue(this, true);
                }
                else queueLock.Exit();
            }
            else Close(returnType);
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// Cancel the hold callback (Note that since it is a synchronous call by the IO thread receiving data, if there is a blockage, please open a new thread task to handle it)
        /// 取消保持回调（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                queueLock.EnterYield();
                if (moveNext.TryCancel() == 0)
                {
                    queueLock.Exit();
                    moveNext.SetNextValue(this, false);
                }
                else queueLock.Exit();
            }
            else Close(returnType);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        void IDisposable.Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Close(CommandClientReturnTypeEnum.KeepCallbackDisposed);
                keepCallback.Cancel(false);
            }
        }

        /// <summary>
        /// Wait for the command to add the output queue
        /// 等待命令添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<EnumeratorCommand?> Wait()
#else
        public async Task<EnumeratorCommand> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the collection enumeration command
        /// 获取集合枚举命令
        /// </summary>
        /// <returns>The operation of adding to the output queue failed and returned null
        /// 添加到输出队列操作失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public EnumeratorCommand? GetResult()
#else
        public EnumeratorCommand GetResult()
#endif
        {
            return PushState == CommandPushStateEnum.Success ? this : null;
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public EnumeratorCommandMoveNext MoveNext()
        {
            queueLock.EnterYield();
            switch (moveNext.MoveNext())
            {
                case 0:
                    queueLock.Exit();
#if DEBUG
                    ++moveNextCount1;
#endif
                    return moveNext;
                case 1:
                    queueLock.Exit();
#if DEBUG
                    ++moveNextCount0;
#endif
                    return EnumeratorCommandMoveNext.NextValueTrue;
                default:
                    queueLock.Exit();
#if DEBUG
                    ++moveNextCount2;
#endif
                    return EnumeratorCommandMoveNext.NextValueFalse;
            }
        }
        /// <summary>
        /// Close keep callback
        /// 关闭保持回调
        /// </summary>
        /// <param name="returnType"></param>
        internal void Close(CommandClientReturnTypeEnum returnType)
        {
            ReturnType = returnType;
            queueLock.EnterYield();
            if (moveNext.Close() == 0)
            {
                queueLock.Exit();
                moveNext.SetNextValue(this, false);
            }
            else queueLock.Exit();
        }
    }
    /// <summary>
    /// The collection enumeration command does not support multi-threaded concurrent calls to await MoveNext (await EnumeratorCommand{T}, returning null indicates failure to add to the output queue).
    /// 集合枚举命令，不支持多线程并发调用 await MoveNext（await EnumeratorCommand{T}，返回 null 表示添加到输出队列失败）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumeratorCommand<T> : KeepCommand, IDisposable
#if NetStandard21
, IAsyncEnumerator<T>
#else
, IEnumeratorCommand<T>
#endif
    {
#if !AOT
        /// <summary>
        /// The initial return value
        /// 初始返回值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
#endif
        /// <summary>
        /// Whether the next data exists in the collection enumeration command
        /// 集合枚举命令是否存在下一个数据
        /// </summary>
        protected readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// Return value queue
        /// 返回值队列
        /// </summary>
#if NetStandard21
        protected readonly Queue<T?> returnValueQueue = new Queue<T?>();
#else
        protected readonly Queue<T> returnValueQueue = new Queue<T>();
#endif
        /// <summary>
        /// Return value queue access lock
        /// 返回值队列访问锁
        /// </summary>
        protected AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// Current returned data
        /// 当前返回数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Current { get; protected set; }
        /// <summary>
        /// The collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
#if !AOT
        /// <summary>
        /// The collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="returnValue"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, ref T returnValue) : base(controller, methodIndex)
        {
            this.returnValue = returnValue;
        }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            if (data.Length == int.MinValue)
            {
                Close((CommandClientReturnTypeEnum)(byte)data.Start);
                return ClientReceiveErrorTypeEnum.Success;
            }
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            ServerReturnValue<T> outputParameter = new ServerReturnValue<T>(returnValue);
            try
            {
                if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleDeserializeParamter))
                {
                    queueLock.EnterYield();
                    switch (moveNext.PushValue())
                    {
                        case 0:
                            try
                            {
                                returnValueQueue.Enqueue(outputParameter.ReturnValue);
                            }
                            finally { queueLock.Exit(); }
                            returnType = CommandClientReturnTypeEnum.Success;
                            return ClientReceiveErrorTypeEnum.Success;
                        case 1:
                            Current = outputParameter.ReturnValue;
                            queueLock.Exit();
                            moveNext.SetNextValue(this, true);
                            returnType = CommandClientReturnTypeEnum.Success;
                            return ClientReceiveErrorTypeEnum.Success;
                        default:
                            queueLock.Exit();
                            returnType = CommandClientReturnTypeEnum.Success;
                            return ClientReceiveErrorTypeEnum.Success;
                    }
                }
                returnType = CommandClientReturnTypeEnum.ClientDeserializeError;
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) Close(returnType);
            }
        }
#endif
        /// <summary>
        /// Error handling for generating the input data of the request command
        /// 生成请求命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            Close(returnType);
        }
        /// <summary>
        /// Cancel the hold callback (Note that since it is a synchronous call by the IO thread receiving data, if there is a blockage, please open a new thread task to handle it)
        /// 取消保持回调（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal override void CancelKeepCallback(CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                queueLock.EnterYield();
                if (returnValueQueue.Count == 0)
                {
                    if (moveNext.Cancel() == 0)
                    {
                        queueLock.Exit();
                        moveNext.SetNextValue(this, false);
                    }
                    else queueLock.Exit();
                }
                else
                {
                    moveNext.IsCanceled = 2;
                    queueLock.Exit();
                }
            }
            else Close(returnType);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        void IDisposable.Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Close(CommandClientReturnTypeEnum.KeepCallbackDisposed);
                keepCallback.Cancel(false);
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Close(CommandClientReturnTypeEnum.KeepCallbackDisposed);
                AutoCSer.Threading.SecondTimerAppendTaskStateEnum appendTaskState = keepCallback.TryCancel(false);
                if (appendTaskState != Threading.SecondTimerAppendTaskStateEnum.Completed) await keepCallback.AppendTaskArrayAsync(appendTaskState);
            }
        }
        /// <summary>
        /// Wait for the command to add the output queue
        /// 等待命令添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<EnumeratorCommand<T>?> Wait()
#else
        public async Task<EnumeratorCommand<T>> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the collection enumeration command
        /// 获取集合枚举命令
        /// </summary>
        /// <returns>The operation of adding to the output queue failed and returned null
        /// 添加到输出队列操作失败返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public EnumeratorCommand<T>? GetResult()
#else
        public EnumeratorCommand<T> GetResult()
#endif
        {
            return PushState == CommandPushStateEnum.Success ? this : null;
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorCommand<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public EnumeratorCommandMoveNext MoveNext()
        {
            queueLock.EnterYield();
            if (returnValueQueue.Count != 0)
            {
                Current = returnValueQueue.Dequeue();
                queueLock.Exit();
                return EnumeratorCommandMoveNext.NextValueTrue;
            }
            if (moveNext.MoveNextValue() == 0)
            {
                queueLock.Exit();
                return moveNext;
            }
            queueLock.Exit();
            return EnumeratorCommandMoveNext.NextValueFalse;
        }
        /// <summary>
        /// Close keep callback
        /// 关闭保持回调
        /// </summary>
        /// <param name="returnType"></param>
        internal void Close(CommandClientReturnTypeEnum returnType)
        {
            ReturnType = returnType;
            queueLock.EnterYield();
            if (returnValueQueue.Count != 0) returnValueQueue.Clear();
            if (moveNext.Close() == 0)
            {
                queueLock.Exit();
                moveNext.SetNextValue(this, false);
            }
            else queueLock.Exit();
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        async ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
#else
        async Task<bool> IEnumeratorTask.MoveNextAsync()
#endif
        {
            return await MoveNext();
        }
#if NetStandard21
        /// <summary>
        /// Get the IAsyncEnumerable
        /// </summary>
        /// <param name="enumeratorCommand"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<T> GetAsyncEnumerable(EnumeratorCommand<T> enumeratorCommand)
        {
            var command = await enumeratorCommand;
            if (command != null)
            {
                while (await enumeratorCommand.MoveNext())
                {
                    yield return enumeratorCommand.Current;
                }
            }
            if (enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success) throw new Exception(enumeratorCommand.ReturnType.ToString());
        }
#endif
#if AOT
        /// <summary>
        /// AOT 代码生成模板
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal static CommandClientReturnValue GetAsyncEnumerable(CommandClientReturnValue returnValue)
        {
            return returnValue;
        }
#endif
    }
#if AOT
    /// <summary>
    /// The collection enumeration command does not support multi-threaded concurrent calls to await MoveNext (await EnumeratorCommand{T}, returning null indicates failure to add to the output queue).
    /// 集合枚举命令，不支持多线程并发调用 await MoveNext（await EnumeratorCommand{T}，返回 null 表示添加到输出队列失败）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    public class EnumeratorCommand<T, OT> : EnumeratorCommand<T>
        where OT : struct
    {
        /// <summary>
        /// The initial return value
        /// 初始返回值
        /// </summary>
        private OT outputParameter;
        /// <summary>
        /// The delegate that gets the return value
        /// 获取返回值委托
        /// </summary>
        private readonly Func<OT, T> getReturnValue;
        /// <summary>
        /// The collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue) : base(controller, methodIndex)
        {
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// The collection enumeration command
        /// 集合枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal EnumeratorCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue, OT outputParameter) : base(controller, methodIndex)
        {
            this.getReturnValue = getReturnValue;
            this.outputParameter = outputParameter;
        }
        /// <summary>
        /// Process the response data
        /// 处理响应数据
        /// </summary>
        /// <param name="data">Response data
        /// 响应数据</param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            if (data.Length == int.MinValue)
            {
                Close((CommandClientReturnTypeEnum)(byte)data.Start);
                return ClientReceiveErrorTypeEnum.Success;
            }
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.ClientException;
            try
            {
                if (Controller.Socket.Deserialize(ref data, ref outputParameter, Method.IsSimpleDeserializeParamter))
                {
                    queueLock.EnterYield();
                    switch (moveNext.PushValue())
                    {
                        case 0:
                            try
                            {
                                returnValueQueue.Enqueue(getReturnValue(outputParameter));
                            }
                            finally { queueLock.Exit(); }
                            returnType = CommandClientReturnTypeEnum.Success;
                            return ClientReceiveErrorTypeEnum.Success;
                        case 1:
                            Current = getReturnValue(outputParameter);
                            queueLock.Exit();
                            moveNext.SetNextValue(this, true);
                            returnType = CommandClientReturnTypeEnum.Success;
                            return ClientReceiveErrorTypeEnum.Success;
                        default:
                            queueLock.Exit();
                            returnType = CommandClientReturnTypeEnum.Success;
                            return ClientReceiveErrorTypeEnum.Success;
                    }
                }
                returnType = CommandClientReturnTypeEnum.ClientDeserializeError;
                Method.DeserializeError(Controller);
                return ClientReceiveErrorTypeEnum.Success;
            }
            finally
            {
                if (returnType != CommandClientReturnTypeEnum.Success) Close(returnType);
            }
        }
    }
#endif
}
