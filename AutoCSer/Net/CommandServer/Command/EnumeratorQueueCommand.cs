using AutoCSer.Extensions;
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
    /// 队列枚举命令，MoveNext 操作不支持多任务并发 await 
    /// </summary>
    public class EnumeratorQueueCommand : KeepCommand, IDisposable
    {
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        private readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// 返回值队列访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// 队列枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal EnumeratorQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            Close(returnType);
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal override ClientReceiveErrorTypeEnum OnReceive(ref SubArray<byte> data)
        {
            CommandClientReturnTypeEnum returnType = (CommandClientReturnTypeEnum)(byte)data.Start;
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                queueLock.EnterYield();
                if (moveNext.Push() == 0)
                {
                    queueLock.Exit();
                    moveNext.SetNextValueQueue(this, true);
                }
                else queueLock.Exit();
            }
            else Close(returnType);
            return ClientReceiveErrorTypeEnum.Success;
        }
        /// <summary>
        /// 取消保持回调
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
                    moveNext.SetNextValueQueue(this, false);
                }
                else queueLock.Exit();
            }
            else Close(returnType);
        }
        /// <summary>
        /// 释放资源
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
        /// 等待添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<EnumeratorQueueCommand?> Wait()
#else
        public async Task<EnumeratorQueueCommand> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public EnumeratorQueueCommand? GetResult()
#else
        public EnumeratorQueueCommand GetResult()
#endif
        {
            return PushState == CommandPushStateEnum.Success ? this : null;
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorQueueCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public EnumeratorCommandMoveNext MoveNext()
        {
            queueLock.EnterYield();
            switch (moveNext.MoveNext())
            {
                case 0:
                    queueLock.Exit();
                    return moveNext;
                case 1:
                    queueLock.Exit();
                    return EnumeratorCommandMoveNext.NextValueTrue;
                default:
                    queueLock.Exit();
                    return EnumeratorCommandMoveNext.NextValueFalse;
            }
        }
        /// <summary>
        /// 关闭回调
        /// </summary>
        internal void Close(CommandClientReturnTypeEnum returnType)
        {
            ReturnType = returnType;
            queueLock.EnterYield();
            if (moveNext.Close() == 0)
            {
                queueLock.Exit();
                moveNext.SetNextValueQueue(this, false);
            }
            else queueLock.Exit();
        }
    }
    /// <summary>
    /// 队列枚举命令，MoveNext 操作不支持多任务并发 await 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumeratorQueueCommand<T> : KeepCommand, IDisposable
#if NetStandard21
, IAsyncEnumerator<T>
#else
, IEnumeratorCommand<T>
#endif
    {
#if !AOT
        /// <summary>
        /// 返回初始值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
#endif
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        protected readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// 返回值队列
        /// </summary>
#if NetStandard21
        protected readonly Queue<T?> returnValueQueue = new Queue<T?>();
#else
        private readonly Queue<T> returnValueQueue = new Queue<T>();
#endif
        /// <summary>
        /// 返回值队列访问锁
        /// </summary>
        protected AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// 当前返回数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Current { get; protected set; }
        /// <summary>
        /// 队列枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal EnumeratorQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
#if !AOT
        /// <summary>
        /// 队列枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="returnValue"></param>
        internal EnumeratorQueueCommand(CommandClientController controller, int methodIndex, ref T returnValue) : base(controller, methodIndex)
        {
            this.returnValue = returnValue;
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
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
                            moveNext.SetNextValueQueue(this, true);
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
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType)
        {
            Close(returnType);
        }
        /// <summary>
        /// 取消保持回调
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
                        moveNext.SetNextValueQueue(this, false);
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
        /// 释放资源
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
        /// 释放资源
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
        /// 等待添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<EnumeratorQueueCommand<T>?> Wait()
#else
        public async Task<EnumeratorQueueCommand<T>> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public EnumeratorQueueCommand<T>? GetResult()
#else
        public EnumeratorQueueCommand<T> GetResult()
#endif
        {
            return PushState == CommandPushStateEnum.Success ? this : null;
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorQueueCommand<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// 判断是否存在下一个数据
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
        /// 判断是否存在下一个数据
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
        /// <summary>
        /// 取消回调
        /// </summary>
        internal void Close(CommandClientReturnTypeEnum returnType)
        {
            ReturnType = returnType;
            queueLock.EnterYield();
            if (returnValueQueue.Count != 0) returnValueQueue.Clear();
            if (moveNext.Close() == 0)
            {
                queueLock.Exit();
                moveNext.SetNextValueQueue(this, false);
            }
            else queueLock.Exit();
        }
    }
#if AOT
    /// <summary>
    /// 队列枚举命令，MoveNext 操作不支持多任务并发 await 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="OT"></typeparam>
    public class EnumeratorQueueCommand<T, OT> : EnumeratorQueueCommand<T>
        where OT : struct
    {
        /// <summary>
        /// 返回初始值
        /// </summary>
        private OT outputParameter;
        /// <summary>
        /// 获取返回值委托
        /// </summary>
        private readonly Func<OT, T> getReturnValue;
        /// <summary>
        /// 队列枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        internal EnumeratorQueueCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue) : base(controller, methodIndex)
        {
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// 队列枚举命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="getReturnValue"></param>
        /// <param name="outputParameter"></param>
        internal EnumeratorQueueCommand(CommandClientController controller, int methodIndex, Func<OT, T> getReturnValue, OT outputParameter) : base(controller, methodIndex)
        {
            this.outputParameter = outputParameter;
            this.getReturnValue = getReturnValue;
        }
        /// <summary>
        /// 委托命令回调
        /// </summary>
        /// <param name="data"></param>
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
                            moveNext.SetNextValueQueue(this, true);
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
