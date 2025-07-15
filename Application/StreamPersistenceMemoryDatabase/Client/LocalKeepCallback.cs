using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The local service invocation keep callback output
    /// 本地服务调用保持回调输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalKeepCallback<T> : IDisposable, IAsyncEnumerator<LocalResult<T>>, IEnumeratorCommand<LocalResult<T>>
    {
        /// <summary>
        /// The local service invocation callback object
        /// 本地服务调用回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackEnumeratorNodeCallback<T> callback;
        /// <summary>
        /// Whether the next data exists in the collection enumeration command
        /// 集合枚举命令是否存在下一个数据
        /// </summary>
        private readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// Return value queue
        /// 返回值队列
        /// </summary>
        private readonly Queue<KeepCallbackResponseParameter> returnValueQueue = new Queue<KeepCallbackResponseParameter>();
        /// <summary>
        /// Current returned data
        /// 当前返回数据
        /// </summary>
        private KeepCallbackResponseParameter current;
        /// <summary>
        /// Get current data
        /// 获取当前数据
        /// </summary>
        public LocalResult<T> Current
        {
            get
            {
                if (current.State == CallStateEnum.Success)
                {
                    if ((current.flag & MethodFlagsEnum.IsSimpleSerializeParamter) != 0) return ((ResponseParameterSimpleSerializer<T>)current.Serializer).Value.ReturnValue;
                    return ((ResponseParameterBinarySerializer<T>)current.Serializer).Value.ReturnValue; 
                }
                return current.State;
            }
        }
        /// <summary>
        /// The return value type of the callback command
        /// 回调命令返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType { get; private set; }
        /// <summary>
        /// Whether to synchronize the callback of the IO thread
        /// 是否 IO 线程同步回调
        /// </summary>
        private readonly bool isSynchronousCallback;
        /// <summary>
        /// Exception information
        /// 异常信息
        /// </summary>
#if NetStandard21
        public Exception? Exception { get; private set; }
#else
        public Exception Exception { get; private set; }
#endif
        /// <summary>
        /// The local service invocation keep callback output
        /// 本地服务调用保持回调输出
        /// </summary>
        /// <param name="callback">The local service invocation callback object
        /// 本地服务调用回调对象</param>
        /// <param name="isSynchronousCallback">Whether to synchronize the callback of the IO thread
        /// 是否 IO 线程同步回调</param>
        internal LocalKeepCallback(LocalServiceKeepCallbackEnumeratorNodeCallback<T> callback, bool isSynchronousCallback)
        {
            this.callback = callback;
            this.isSynchronousCallback = isSynchronousCallback;
            ReturnType = CommandClientReturnTypeEnum.Success;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            callback.SetCancelKeep();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public ValueTask DisposeAsync()
#else
        public Task DisposeAsync()
#endif
        {
            callback.SetCancelKeep();
            return AutoCSer.Common.AsyncDisposableCompletedTask;
        }
        /// <summary>
        /// Cancel output
        /// 取消输出
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="exception"></param>
#if NetStandard21
        internal void CancelKeep(CommandClientReturnTypeEnum returnType, Exception? exception = null)
#else
        internal void CancelKeep(CommandClientReturnTypeEnum returnType, Exception exception = null)
#endif
        {
            if (returnType == CommandClientReturnTypeEnum.Success)
            {
                Monitor.Enter(returnValueQueue);
                if (returnValueQueue.Count == 0)
                {
                    if (moveNext.Cancel() == 0)
                    {
                        Monitor.Exit(returnValueQueue);
                        moveNext.SetNextValue(isSynchronousCallback, false);
                    }
                    else Monitor.Exit(returnValueQueue);
                }
                else
                {
                    moveNext.IsCanceled = 2;
                    Monitor.Exit(returnValueQueue);
                }
            }
            else
            {
                ReturnType = returnType;
                Exception = exception;

                Monitor.Enter(returnValueQueue);
                if (returnValueQueue.Count != 0) returnValueQueue.Clear();
                if (moveNext.Close() == 0)
                {
                    Monitor.Exit(returnValueQueue);
                    moveNext.SetNextValue(isSynchronousCallback, false);
                }
                else Monitor.Exit(returnValueQueue);
            }
        }
        /// <summary>
        /// Return the data callback operation
        /// 返回数据回调操作
        /// </summary>
        /// <param name="response"></param>
        internal void Callback(KeepCallbackResponseParameter response)
        {
            Monitor.Enter(returnValueQueue);
            switch (moveNext.PushValue())
            {
                case 0:
                    try
                    {
                        returnValueQueue.Enqueue(response);
                    }
                    finally { Monitor.Exit(returnValueQueue); }
                    return;
                case 1:
                    current = response;
                    Monitor.Exit(returnValueQueue);
                    moveNext.SetNextValue(isSynchronousCallback, true);
                    return;
                default: Monitor.Exit(returnValueQueue); return;
            }
        }
        /// <summary>
        /// await bool, the collection enumeration command returns true when the next data exists
        /// await bool，集合枚举命令存在下一个数据返回 true
        /// </summary>
        /// <returns></returns>
        public EnumeratorCommandMoveNext MoveNext()
        {
            Monitor.Enter(returnValueQueue);
            if (returnValueQueue.Count != 0)
            {
                current = returnValueQueue.Dequeue();
                Monitor.Exit(returnValueQueue);
                return EnumeratorCommandMoveNext.NextValueTrue;
            }
            if (moveNext.MoveNextValue() == 0)
            {
                Monitor.Exit(returnValueQueue);
                return moveNext;
            }
            Monitor.Exit(returnValueQueue);
            return EnumeratorCommandMoveNext.NextValueFalse;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        async ValueTask<bool> IAsyncEnumerator<LocalResult<T>>.MoveNextAsync()
#else
        async Task<bool> IAsyncEnumerator<LocalResult<T>>.MoveNextAsync()
#endif
        {
            return await MoveNext();
        }
        ///// <summary>
        ///// Whether the next data exists
        ///// 是否存在下一个数据
        ///// </summary>
        ///// <returns></returns>
        //async Task<bool> IEnumeratorTask.MoveNextAsync()
        //{
        //    return await MoveNext();
        //}
#if NetStandard21
        /// <summary>
        /// Get the IAsyncEnumerable
        /// 获取 IAsyncEnumerable
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<LocalResult<T>> GetAsyncEnumerable()
        {
            bool isError = false;
            do
            {
                Monitor.Enter(returnValueQueue);
                if (returnValueQueue.Count != 0)
                {
                    current = returnValueQueue.Dequeue();
                    Monitor.Exit(returnValueQueue);
                }
                else if (moveNext.MoveNextValue() == 0)
                {
                    Monitor.Exit(returnValueQueue);
                    if (!(await moveNext)) break;
                }
                else
                {
                    Monitor.Exit(returnValueQueue);
                    break;
                }
                LocalResult<T> response = Current;
                yield return response;
                if (response.CallState != CallStateEnum.Success)
                {
                    isError = true;
                    break;
                }
            }
            while (true);

            if (isError) Dispose();
            else if (ReturnType != Net.CommandClientReturnTypeEnum.Success) yield return new LocalResult<T>(CallStateEnum.Unknown, Exception);
        }
        /// <summary>
        /// Convert the data and get the IAsyncEnumerable
        /// 数据转换并获取 IAsyncEnumerable
        /// </summary>
        /// <typeparam name="VT">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="getValue">Delegate for data transformation
        /// 数据转换委托</param>
        /// <returns></returns>
        public async IAsyncEnumerable<LocalResult<VT>> GetAsyncEnumerable<VT>(Func<T?, VT> getValue)
        {
            await foreach (LocalResult<T> value in GetAsyncEnumerable())
            {
                if (value.IsSuccess) yield return getValue(value.Value);
                else yield return value.Cast<VT>();
            }
        }
#endif
    }
}
