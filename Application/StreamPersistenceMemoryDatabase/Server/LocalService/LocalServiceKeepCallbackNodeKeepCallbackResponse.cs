using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
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
    public sealed class LocalServiceKeepCallbackNodeKeepCallbackResponse<T> : KeepCallbackResponse<T>
#if NetStandard21
, IAsyncEnumerator<ResponseResult<T>>
#else
, IEnumeratorTask<ResponseResult<T>>
#endif
    {
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackNodeCallback<T> callback;
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        private readonly EnumeratorCommandMoveNext moveNext = new EnumeratorCommandMoveNext();
        /// <summary>
        /// 返回值队列
        /// </summary>
        private readonly Queue<KeepCallbackResponseParameter> returnValueQueue = new Queue<KeepCallbackResponseParameter>();
        /// <summary>
        /// 返回值队列访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// 当前返回数据
        /// </summary>
        private KeepCallbackResponseParameter current;
        /// <summary>
        /// 当前返回数据
        /// </summary>
#if NetStandard21
        public ResponseResult<T> Current
#else
        public new ResponseResult<T> Current
#endif
        {
            get
            {
                if (current.State == CallStateEnum.Success)
                {
                    if (current.IsSimpleSerialize) return ((ResponseParameterSimpleSerializer<T>)current.Serializer).Value.ReturnValue;
                    return ((ResponseParameterBinarySerializer<T>)current.Serializer).Value.ReturnValue;
                }
                return current.State;
            }
        }
        /// <summary>
        /// 回调命令返回值类型
        /// </summary>
        public new CommandClientReturnTypeEnum ReturnType { get; private set; }
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
        internal LocalServiceKeepCallbackNodeKeepCallbackResponse(LocalServiceKeepCallbackNodeCallback<T> callback)
        {
            this.callback = callback;
            ReturnType = CommandClientReturnTypeEnum.Success;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            callback.SetCancelKeep();
        }
        /// <summary>
        /// 异步释放资源
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public ValueTask DisposeAsync()
#else
        public new ValueTask DisposeAsync()
#endif
        {
            callback.SetCancelKeep();
#if NET8
            return ValueTask.CompletedTask;
#else
            return AutoCSer.Common.CompletedTask.ToValueTask();
#endif
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
                queueLock.EnterYield();
                if (returnValueQueue.Count == 0)
                {
                    if (moveNext.Cancel() == 0)
                    {
                        queueLock.Exit();
                        moveNext.SetNextValue(false);
                    }
                    else queueLock.Exit();
                }
                else
                {
                    moveNext.IsCanceled = 2;
                    queueLock.Exit();
                }
            }
            else
            {
                ReturnType = returnType;
                Exception = exception;

                queueLock.EnterYield();
                if (returnValueQueue.Count != 0) returnValueQueue.Clear();
                if (moveNext.Close() == 0)
                {
                    queueLock.Exit();
                    moveNext.SetNextValue(false);
                }
                else queueLock.Exit();
            }
        }
        /// <summary>
        /// 返回数据回调
        /// </summary>
        /// <param name="response"></param>
        internal void Callback(KeepCallbackResponseParameter response)
        {
            queueLock.EnterYield();
            switch (moveNext.PushValue())
            {
                case 0:
                    try
                    {
                        returnValueQueue.Enqueue(response);
                    }
                    finally { queueLock.Exit(); }
                    return;
                case 1:
                    current = response;
                    queueLock.Exit();
                    moveNext.SetNextValue(true);
                    return;
                default: queueLock.Exit(); return;
            }
        }
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        internal EnumeratorCommandMoveNext MoveNext()
        {
            queueLock.EnterYield();
            if (returnValueQueue.Count != 0)
            {
                current = returnValueQueue.Dequeue();
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
        async ValueTask<bool> IAsyncEnumerator<ResponseResult<T>>.MoveNextAsync()
#else
        async Task<bool> IEnumeratorTask.MoveNextAsync()
#endif
        {
            return await MoveNext();
        }
#if NetStandard21
        /// <summary>
        /// 获取 IAsyncEnumerable
        /// </summary>
        /// <returns></returns>
        public override async IAsyncEnumerable<ResponseResult<T>> GetAsyncEnumerable()
        {
            bool isError = false;
            do
            {
                queueLock.EnterYield();
                if (returnValueQueue.Count != 0)
                {
                    current = returnValueQueue.Dequeue();
                    queueLock.Exit();
                }
                else if (moveNext.MoveNextValue() == 0)
                {
                    queueLock.Exit();
                    if (!(await moveNext)) break;
                }
                else
                {
                    queueLock.Exit();
                    break;
                }
                ResponseResult<T> response = Current;
                yield return response;
                if (response.CallState != CallStateEnum.Success)
                {
                    isError = true;
                    break;
                }
            }
            while (true);

            if (isError) Dispose();
            else if (ReturnType != Net.CommandClientReturnTypeEnum.Success) yield return ReturnType;
        }
        /// <summary>
        /// 数据转换获取 IAsyncEnumerable
        /// </summary>
        /// <typeparam name="VT">目标数据类型</typeparam>
        /// <param name="getValue">数据转换委托</param>
        /// <returns></returns>
        public override async IAsyncEnumerable<ResponseResult<VT>> GetAsyncEnumerable<VT>(Func<T?, VT> getValue)
        {
            await foreach (ResponseResult<T> value in GetAsyncEnumerable())
            {
                if (value.IsSuccess) yield return getValue(value.Value);
                else yield return value.Cast<VT>();
            }
        }
#endif
    }
}
