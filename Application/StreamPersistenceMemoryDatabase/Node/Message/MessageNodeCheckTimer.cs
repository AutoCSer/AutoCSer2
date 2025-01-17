using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 超时检查定时
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    internal sealed class MessageNodeCheckTimer<T> : AutoCSer.Threading.SecondTimerArrayNode
         where T : Message<T>
    {
        /// <summary>
        /// 消息处理节点
        /// </summary>
#if NetStandard21
        private MessageNode<T>? messageNode;
#else
        private MessageNode<T> messageNode;
#endif
        /// <summary>
        /// 超时检查定时
        /// </summary>
        /// <param name="messageNode"></param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        internal MessageNodeCheckTimer(MessageNode<T> messageNode, int checkTimeoutSeconds) : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, checkTimeoutSeconds, Threading.SecondTimerKeepModeEnum.After, checkTimeoutSeconds)
        {
            this.messageNode = messageNode;
            AppendTaskArray();
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            messageNode?.CheckTimeout();
        }
        /// <summary>
        /// 取消定时
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Cancel()
        {
            keepSeconds = 0;
            messageNode = null;
        }
    }
}
