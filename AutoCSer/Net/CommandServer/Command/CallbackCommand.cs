using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Net.CommandServer;

namespace AutoCSer.Net
{
    /// <summary>
    /// 回调命令 await bool 是否自动触发回调
    /// </summary>
    public abstract class CallbackCommand : Command, INotifyCompletion
    {
        /// <summary>
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// 命令添加状态
        /// </summary>
        internal CommandPushStateEnum PushState;
        /// <summary>
        /// 添加输出命令通知
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal CallbackCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
        /// <summary>
        /// 等待添加输出队列
        /// </summary>
        /// <returns>是否成功添加输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<bool> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns>是否成功添加输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool GetResult()
        {
            return PushState == CommandPushStateEnum.Success;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 添加命令到发送队列
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
        /// 检查等待添加队列命令
        /// </summary>
        /// <returns>是否需要继续等待</returns>
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
