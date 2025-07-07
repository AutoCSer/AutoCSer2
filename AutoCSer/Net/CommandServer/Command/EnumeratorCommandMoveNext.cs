using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// await bool, the collection enumeration command returns true when the next data exists
    /// await bool，集合枚举命令存在下一个数据返回 true
    /// </summary>
    public sealed class EnumeratorCommandMoveNext : INotifyCompletion
    {
        /// <summary>
        /// The number of return values
        /// 返回值数量
        /// </summary>
        internal int ReturnCount;
        /// <summary>
        /// Has the callback been cancelled, 0/2
        /// 是否已经取消回调 0/2
        /// </summary>
        internal int IsCanceled;
        /// <summary>
        /// Is currently waiting for data
        /// 当前是否等待数据
        /// </summary>
        internal bool IsCurrentMoveNext;

        /// <summary>
        /// Completion status (For reuse requirements, setting IsCompleted = true is not allowed. After the comparison is set, it will be read immediately, resulting in repeated execution.)
        /// 完成状态（重用需求不允许设置 IsCompleted = true 比较设置完以后马上被读取掉导致重复执行）
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        private bool isNextValue;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
#if DEBUG
        private int getResultCount;
        private int onCompletedCount;
        private int onCompletedContinuationCount;
        private int getAwaiterCount;
        private int setNextValueTrueCount;
        private int setNextValueFalseCount;
        private int setNextValueQueueTrueCount;
        private int setNextValueQueueFalseCount;
        private int setNextValueContinuationCount;
        private int setNextValueExchangeContinuationCount;
        private int pushCount;
#endif
        /// <summary>
        /// Whether the next data exists in the collection enumeration command
        /// 集合枚举命令是否存在下一个数据
        /// </summary>
        internal EnumeratorCommandMoveNext() { }
        /// <summary>
        /// Whether the next data exists in the collection enumeration command
        /// 集合枚举命令是否存在下一个数据
        /// </summary>
        /// <param name="isNextValue"></param>
        private EnumeratorCommandMoveNext(bool isNextValue)
        {
            this.isNextValue = isNextValue;
            IsCompleted = true;
        }
        /// <summary>
        /// Wait for the next data
        /// 等待下一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<bool> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Whether the output queue has been successfully added
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool GetResult()
        {
#if DEBUG
            ++getResultCount;
#endif
            return isNextValue;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
#if DEBUG
            ++onCompletedCount;
#endif
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null)
            {
#if DEBUG
                ++onCompletedContinuationCount;
#endif
                this.continuation = null;
                continuation();
            }
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnumeratorCommandMoveNext GetAwaiter()
        {
#if DEBUG
            ++getAwaiterCount;
#endif
            return this;
        }
        /// <summary>
        /// Set whether the next data exists
        /// 设置是否存在下一个数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="isNextValue"></param>
        internal void SetNextValue(Command command, bool isNextValue)
        {
#if DEBUG
            if (isNextValue) ++setNextValueTrueCount;
            else ++setNextValueFalseCount;
#endif
            this.isNextValue = isNextValue;
            var continuation = default(Action);
            if (this.continuation != null)
            {
#if DEBUG
                ++setNextValueContinuationCount;
#endif
                continuation = this.continuation;
                this.continuation = null;
                command.Callback(continuation);
                return;
            }
            continuation = System.Threading.Interlocked.CompareExchange(ref this.continuation, Common.EmptyAction, null);
            if (continuation != null)
            {
#if DEBUG
                ++setNextValueExchangeContinuationCount;
#endif
                this.continuation = null;
                command.Callback(continuation);
                return;
            }
        }
        /// <summary>
        /// Set whether the next data exists
        /// 设置是否存在下一个数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="isNextValue"></param>
        internal void SetNextValueQueue(Command command, bool isNextValue)
        {
#if DEBUG
            if (isNextValue) ++setNextValueQueueTrueCount;
            else ++setNextValueQueueFalseCount;
#endif
            this.isNextValue = isNextValue;
            var continuation = default(Action);
            if (this.continuation != null)
            {
#if DEBUG
                ++setNextValueContinuationCount;
#endif
                continuation = this.continuation;
                this.continuation = null;
                command.AppendQueue(continuation);
                return;
            }
            continuation = System.Threading.Interlocked.CompareExchange(ref this.continuation, Common.EmptyAction, null);
            if (continuation != null)
            {
#if DEBUG
                ++setNextValueExchangeContinuationCount;
#endif
                this.continuation = null;
                command.AppendQueue(continuation);
                return;
            }
        }
        /// <summary>
        /// Set whether the next data exists
        /// 设置是否存在下一个数据
        /// </summary>
        /// <param name="isSynchronousCallback"></param>
        /// <param name="isNextValue"></param>
        internal void SetNextValue(bool isSynchronousCallback, bool isNextValue)
        {
#if DEBUG
            if (isNextValue) ++setNextValueTrueCount;
            else ++setNextValueFalseCount;
#endif
            this.isNextValue = isNextValue;
            var continuation = default(Action);
            if (this.continuation != null)
            {
#if DEBUG
                ++setNextValueContinuationCount;
#endif
                continuation = this.continuation;
                this.continuation = null;
                if (isSynchronousCallback) continuation();
                else Task.Run(continuation);
                return;
            }
            continuation = System.Threading.Interlocked.CompareExchange(ref this.continuation, Common.EmptyAction, null);
            if (continuation != null)
            {
#if DEBUG
                ++setNextValueExchangeContinuationCount;
#endif
                this.continuation = null;
                if (isSynchronousCallback) continuation();
                else Task.Run(continuation);
                return;
            }
        }

        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int MoveNext()
        {
            if (ReturnCount != 0)
            {
                --ReturnCount;
                return 1;
            }
            IsCurrentMoveNext = true;
            return IsCanceled;
        }
        /// <summary>
        /// Add new data
        /// 添加新数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Push()
        {
#if DEBUG
            ++pushCount;
#endif
            if (IsCurrentMoveNext)
            {
                IsCurrentMoveNext = false;
                return 0;
            }
            //if (IsCanceled == 0) ++ReturnCount;
            ReturnCount += IsCanceled.logicalInversion();
            return 1;
        }
        /// <summary>
        /// Close keep callback
        /// 关闭保持回调
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Close()
        {
            IsCanceled = 2;
            ReturnCount = 0;
            if (IsCurrentMoveNext)
            {
                IsCurrentMoveNext = false;
                return 0;
            }
            return 1;
        }
        /// <summary>
        /// Try to cancel the keep callback
        /// 尝试取消保持回调
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int TryCancel()
        {
            if (ReturnCount == 0) return Cancel();
            IsCanceled = 2;
            return 1;
        }
        /// <summary>
        /// Cancel the keep callback
        /// 取消保持回调
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Cancel()
        {
            IsCanceled = 2;
            if (IsCurrentMoveNext)
            {
                IsCurrentMoveNext = false;
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Whether the next data exists
        /// 是否存在下一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int MoveNextValue()
        {
            IsCurrentMoveNext = true;
            return IsCanceled;
            //if (IsCanceled == 0)
            //{
            //    IsCurrentMoveNext = true;
            //    return 0;
            //}
            //return 1;
        }
        /// <summary>
        /// Add new data
        /// 添加新数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int PushValue()
        {
            if (IsCurrentMoveNext)
            {
                IsCurrentMoveNext = false;
                return 1;
            }
            return IsCanceled;
        }

        /// <summary>
        /// The collection enumeration command has the next data
        /// 集合枚举命令存在下一个数据
        /// </summary>
        internal static readonly EnumeratorCommandMoveNext NextValueTrue = new EnumeratorCommandMoveNext(true);
        /// <summary>
        /// The collection enumeration command does not have the next data
        /// 集合枚举命令不存在下一个数据
        /// </summary>
        internal static readonly EnumeratorCommandMoveNext NextValueFalse = new EnumeratorCommandMoveNext(false);
    }
}
