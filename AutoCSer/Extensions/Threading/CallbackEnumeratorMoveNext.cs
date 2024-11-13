using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 持续回调转 IAsyncEnumerator{T} 包装，是否存在下一个数据
    /// </summary>
    public sealed class CallbackEnumeratorMoveNext : INotifyCompletion
    {
        /// <summary>
        /// 回调处理是否启动线程
        /// </summary>
        private readonly bool isThreadPool;
        /// <summary>
        /// 完成状态（重用需求不允许设置 IsCompleted = true 比较设置完以后马上被读取掉导致重复执行）
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 枚举命令是否存在下一个数据 是否当前等待数据
        /// </summary>
        internal bool IsCurrentMoveNext;
        /// <summary>
        /// 是否存在下一个数据
        /// </summary>
        private bool isNextValue;
        /// <summary>
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// 枚举命令存在下一个数据
        /// </summary>
        internal CallbackEnumeratorMoveNext MoveNextTrue
        {
            get { return isThreadPool ? ThreadPoolNextValueTrue : NextValueTrue; }
        }
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        /// <param name="isThreadPool">回调处理是否启动线程，回调无阻塞的情况应该设置为 false 直接同步回调</param>
        internal CallbackEnumeratorMoveNext(bool isThreadPool = true)
        {
            this.isThreadPool = isThreadPool;
        }
        /// <summary>
        /// 枚举命令是否存在下一个数据
        /// </summary>
        /// <param name="isThreadPool">回调处理是否启动线程，回调无阻塞的情况应该设置为 false 直接同步回调</param>
        /// <param name="isNextValue"></param>
        private CallbackEnumeratorMoveNext(bool isNextValue, bool isThreadPool = true) : this(isThreadPool)
        {
            this.isNextValue = isNextValue;
            IsCompleted = true;
        }
        /// <summary>
        /// 等待下一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<bool> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool GetResult()
        {
            return isNextValue;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null)
            {
                this.continuation = null;
                continuation();
            }
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackEnumeratorMoveNext GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// 设置是否存在下一个数据
        /// </summary>
        internal void SetNextValue()
        {
            this.isNextValue = true;
            var continuation = default(Action);
            if (this.continuation != null)
            {
                continuation = this.continuation;
                this.continuation = null;
                if (isThreadPool) AutoCSer.Threading.ThreadPool.TinyBackground.Start(continuation);
                else continuation();
                return;
            }
            continuation = System.Threading.Interlocked.CompareExchange(ref this.continuation, Common.EmptyAction, null);
            if (continuation != null)
            {
                this.continuation = null;
                if (isThreadPool) AutoCSer.Threading.ThreadPool.TinyBackground.Start(continuation);
                else continuation();
                return;
            }
        }
        /// <summary>
        /// 添加新数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool PushValue()
        {
            if (IsCurrentMoveNext)
            {
                IsCurrentMoveNext = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 枚举命令存在下一个数据
        /// </summary>
        internal static readonly CallbackEnumeratorMoveNext ThreadPoolNextValueTrue = new CallbackEnumeratorMoveNext(true, true);
        /// <summary>
        /// 枚举命令存在下一个数据
        /// </summary>
        internal static readonly CallbackEnumeratorMoveNext NextValueTrue = new CallbackEnumeratorMoveNext(true, false);
    }
}
