using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Net.CommandServer;

namespace AutoCSer.Net
{
    /// <summary>
    /// Callback command (await bool, return whether successfully added to the output queue)
    /// 回调命令（await bool，返回是否成功添加到输出队列）
    /// </summary>
    public abstract class CallbackCommand : Command, INotifyCompletion
    {
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// The status of the reqeust command added to the output queue
        /// 请求命令添加到输出队列的状态
        /// </summary>
        internal CommandPushStateEnum PushState;
        /// <summary>
        /// Callback command
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal CallbackCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// Callback command
        /// </summary>
        /// <param name="controller"></param>
        internal CallbackCommand(CommandClientDefaultController controller) : base(controller)
        {
            PushState = CommandPushStateEnum.WaitConnect;
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// Wait for the command to add the output queue
        /// 等待命令添加输出队列
        /// </summary>
        /// <returns>Whether the output queue has been successfully added
        /// 是否成功添加输出队列</returns>
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
            return PushState == CommandPushStateEnum.Success;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// The command is added to the output queue
        /// 命令添加到输出队列
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push()
        {
            //PushState = Controller.Socket.TryPushBatch(this);
            PushState = Controller.Socket.TryPush(this);
            if (PushState != CommandPushStateEnum.WaitCount)
            {
                IsCompleted = true;
                continuation = Common.EmptyAction;
            }
            else AutoCSer.Threading.ThreadYield.YieldOnly();
        }
        /// <summary>
        /// The command waiting for idle output attempts to be added to the output queue again
        /// 等待空闲输出的命令再次尝试添加到输出队列
        /// </summary>
        /// <returns>Is it necessary to keep waiting
        /// 是否需要继续等待</returns>
        internal override bool CheckWaitPush()
        {
            PushState = Controller.Socket.TryPush(this);
            if (PushState != CommandPushStateEnum.WaitCount)
            {
                IsCompleted = true;
                if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
                {
                    Callback(continuation);
                }
                return false;
            }
            return true;
        }
    }
}
