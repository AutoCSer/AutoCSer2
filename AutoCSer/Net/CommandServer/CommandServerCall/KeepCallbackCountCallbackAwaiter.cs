using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// TCP 服务器端异步保持回调计数回调 Awaiter
    /// </summary>
    internal sealed class KeepCallbackCountCallbackAwaiter : TaskCastAwaiter<bool>
    {
        /// <summary>
        /// TCP 服务器端异步保持回调计数
        /// </summary>
        private readonly CommandServerKeepCallbackCount count;
        /// <summary>
        /// 服务端套接字输出信息
        /// </summary>
        private readonly ServerOutput output;
        /// <summary>
        /// 不支持，直接抛出异常
        /// </summary>
        public override Exception Exception { get { throw new InvalidOperationException(); } }
        /// <summary>
        /// TCP 服务器端异步保持回调计数回调 Awaiter
        /// </summary>
        /// <param name="count">TCP 服务器端异步保持回调计数</param>
        /// <param name="output"></param>
        internal KeepCallbackCountCallbackAwaiter(CommandServerKeepCallbackCount count, ServerOutput output)
        {
            this.count = count;
            this.output = output;
            Task task = count.OutputLock.EnterAsync();
            if (task.IsCompleted)
            {
                setResult(count.Push(output));
            }
            else task.GetAwaiter().UnsafeOnCompleted(onCompleted);
        }
        /// <summary>
        /// 输出数据访问锁申请完成
        /// </summary>
        private new void onCompleted()
        {
            onCompleted(count.Push(output));
        }
    }
}
