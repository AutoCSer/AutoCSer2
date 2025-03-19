using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点回调输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalKeepCallback<T> : IDisposable, IEnumeratorCommand<LocalResult<T>>
#if NetStandard21
, IAsyncEnumerator<LocalResult<T>>
#endif
    {
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackEnumeratorNodeCallback<T> callback;
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        private readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// 返回值队列
        /// </summary>
        private readonly Queue<KeepCallbackResponseParameter> returnValueQueue = new Queue<KeepCallbackResponseParameter>();
        /// <summary>
        /// 当前返回数据
        /// </summary>
        private KeepCallbackResponseParameter current;
        /// <summary>
        /// 当前返回数据
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
        /// 回调命令返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType { get; private set; }
        /// <summary>
        /// 是否 IO 线程同步回调
        /// </summary>
        private readonly bool isSynchronousCallback;
        /// <summary>
        /// 异常信息
        /// </summary>
#if NetStandard21
        public Exception? Exception { get; private set; }
#else
        public Exception Exception { get; private set; }
#endif
        /// <summary>
        /// 本地服务调用节点方法队列节点回调输出
        /// </summary>
        /// <param name="callback">本地服务调用节点方法队列节点回调对象</param>
        /// <param name="isSynchronousCallback">是否 IO 线程同步回调</param>
        internal LocalKeepCallback(LocalServiceKeepCallbackEnumeratorNodeCallback<T> callback, bool isSynchronousCallback)
        {
            this.callback = callback;
            this.isSynchronousCallback = isSynchronousCallback;
            ReturnType = CommandClientReturnTypeEnum.Success;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            callback.SetCancelKeep();
        }
        /// <summary>
        /// 异步释放资源
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            callback.SetCancelKeep();
            return AutoCSer.Common.CompletedValueTask;
        }
        /// <summary>
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
        /// 返回数据回调
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
        /// 判断是否存在下一个数据
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
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        async Task<bool> IEnumeratorTask.MoveNextAsync()
        {
            return await MoveNext();
        }
#if NetStandard21
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        async ValueTask<bool> IAsyncEnumerator<LocalResult<T>>.MoveNextAsync()
        {
            return await MoveNext();
        }
        /// <summary>
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
        /// 数据转换获取 IAsyncEnumerable
        /// </summary>
        /// <typeparam name="VT">目标数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
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
