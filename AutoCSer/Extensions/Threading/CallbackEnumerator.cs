using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Threading
{
    /// <summary>
    /// 持续回调转 IAsyncEnumerator{T} 包装，MoveNext 操作不支持多任务并发 await 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CallbackEnumerator<T>
#if NetStandard21
: IAsyncEnumerator<T>
#else
: IEnumeratorTask<T>
#endif
    {
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        private readonly CallbackEnumeratorMoveNext moveNext;
        /// <summary>
        /// 返回值队列
        /// </summary>
        private readonly Queue<T> returnValueQueue = new Queue<T>();
        /// <summary>
        /// 返回值队列访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// 当前返回数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Current { get; private set; }
        /// <summary>
        /// 持续回调转 IAsyncEnumerator{T} 包装
        /// </summary>
        /// <param name="isThreadPool">回调处理是否启动线程，回调无阻塞的情况应该设置为 false 直接同步回调</param>
        public CallbackEnumerator(bool isThreadPool = true)
        {
            moveNext = new CallbackEnumeratorMoveNext(isThreadPool);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return AutoCSer.Common.CompletedValueTask;
        }
        /// <summary>
        /// 数据回调
        /// </summary>
        /// <param name="value"></param>
        public void Callback(T value)
        {
            queueLock.EnterYield();
            if (moveNext.PushValue())
            {
                Current = value;
                queueLock.Exit();
                moveNext.SetNextValue();
                return;
            }
            try
            {
                returnValueQueue.Enqueue(value);
            }
            finally { queueLock.Exit(); }
        }
        /// <summary>
        /// 判断是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public CallbackEnumeratorMoveNext MoveNext()
        {
            queueLock.EnterYield();
            if (returnValueQueue.Count != 0)
            {
                Current = returnValueQueue.Dequeue();
                queueLock.Exit();
                return moveNext.MoveNextTrue;
            }
            moveNext.IsCurrentMoveNext = true;
            queueLock.Exit();
            return moveNext;
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
#if NetStandard21
        /// <summary>
        /// 获取 IAsyncEnumerator{T}
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IAsyncEnumerator<T> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            return this;
        }
        /// <summary>
        /// 获取 IAsyncEnumerable
        /// </summary>
        /// <param name="enumeratorCommand"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<T> GetAsyncEnumerable(CallbackEnumerator<T> enumeratorCommand)
        {
            while (await enumeratorCommand.MoveNext()) yield return enumeratorCommand.Current;
        }
#endif
    }
}
