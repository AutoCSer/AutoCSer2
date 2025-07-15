using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 持续回调转 IAsyncEnumerator{T} 包装，MoveNext 操作不支持多任务并发 await 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CallbackEnumerator<T> : IAsyncEnumerator<T>
    {
        /// <summary>
        /// Whether the next data exists in the collection enumeration command
        /// 集合枚举命令是否存在下一个数据
        /// </summary>
        private readonly CallbackEnumeratorMoveNext moveNext;
        /// <summary>
        /// Return value queue
        /// 返回值队列
        /// </summary>
        private readonly Queue<T> returnValueQueue = new Queue<T>();
        /// <summary>
        /// Current returned data
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
        /// Release resources
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        ValueTask IAsyncDisposable.DisposeAsync()
#else
        Task IAsyncDisposable.DisposeAsync()
#endif
        {
            return AutoCSer.Common.AsyncDisposableCompletedTask;
        }
        /// <summary>
        /// 数据回调
        /// </summary>
        /// <param name="value"></param>
        public void Callback(T value)
        {
            System.Threading.Monitor.Enter(returnValueQueue);
            if (moveNext.PushValue())
            {
                Current = value;
                System.Threading.Monitor.Exit(returnValueQueue);
                moveNext.SetNextValue();
                return;
            }
            try
            {
                returnValueQueue.Enqueue(value);
            }
            finally { System.Threading.Monitor.Exit(returnValueQueue); }
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        public CallbackEnumeratorMoveNext MoveNext()
        {
            System.Threading.Monitor.Enter(returnValueQueue);
            if (returnValueQueue.Count != 0)
            {
                Current = returnValueQueue.Dequeue();
                System.Threading.Monitor.Exit(returnValueQueue);
                return moveNext.MoveNextTrue;
            }
            moveNext.IsCurrentMoveNext = true;
            System.Threading.Monitor.Exit(returnValueQueue);
            return moveNext;
        }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        async ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
#else
        async Task<bool> IAsyncEnumerator<T>.MoveNextAsync()
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
        /// Get the IAsyncEnumerable
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
