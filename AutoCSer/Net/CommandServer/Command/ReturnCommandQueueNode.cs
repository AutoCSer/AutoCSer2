using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 返回值回调队列节点
    /// </summary>
    internal sealed class ReturnCommandQueueNode : CommandClientCallQueueNode
    {
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action callback;
        /// <summary>
        /// 返回值回调队列节点
        /// </summary>
        /// <param name="callback"></param>
        internal ReturnCommandQueueNode(Action callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            callback();
        }
    }
}
