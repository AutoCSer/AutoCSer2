using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
#if AOT
    /// <summary>
    /// 超时检查定时
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    /// <typeparam name="IT">消息处理节点接口类型</typeparam>
    internal sealed class MessageNodeCheckTimer<T, IT> : AutoCSer.Threading.SecondTimerArrayNode
        where IT : class, IMessageNode<T>
#else
    /// <summary>
    /// 超时检查定时
    /// </summary>
    /// <typeparam name="T">消息数据类型</typeparam>
    internal sealed class MessageNodeCheckTimer<T> : AutoCSer.Threading.SecondTimerArrayNode
#endif
         where T : Message<T>
    {
        /// <summary>
        /// 消息处理节点
        /// </summary>
#if AOT
        private MessageNode<T, IT>? messageNode;
#else
#if NetStandard21
        private MessageNode<T>? messageNode;
#else
        private MessageNode<T> messageNode;
#endif
#endif
        /// <summary>
        /// 超时检查定时
        /// </summary>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        internal MessageNodeCheckTimer(int checkTimeoutSeconds) : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, Threading.SecondTimerKeepModeEnum.After) 
        {
            KeepSeconds = checkTimeoutSeconds;
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            messageNode?.CheckTimeout();
        }
        /// <summary>
        /// 设置消息处理节点
        /// </summary>
        /// <param name="messageNode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        internal void Set(MessageNode<T, IT> messageNode)
#else
        internal void Set(MessageNode<T> messageNode)
#endif
        {
            if (KeepSeconds != 0)
            {
                this.messageNode = messageNode;
                AppendTaskArray(KeepSeconds);
            }
        }
        /// <summary>
        /// 取消定时
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Cancel()
        {
            KeepSeconds = 0;
            messageNode = null;
        }
    }
}
