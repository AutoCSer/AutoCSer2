using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage
{
    /// <summary>
    /// 超时检查定时
    /// </summary>
    /// <typeparam name="T">任务消息数据类型</typeparam>
    internal sealed class CheckTimer<T> : AutoCSer.Threading.SecondTimerArrayNode
    {
        /// <summary>
        /// 超时任务消息节点
        /// </summary>
#if NetStandard21
        private TimeoutMessageNode<T>? messageNode;
#else
        private TimeoutMessageNode<T> messageNode;
#endif
        /// <summary>
        /// 超时检查定时
        /// </summary>
        internal CheckTimer() : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, 1, Threading.SecondTimerKeepModeEnum.After, 1) { }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            messageNode?.CheckTimeout();
        }
        /// <summary>
        /// 设置超时任务消息节点
        /// </summary>
        /// <param name="messageNode"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(TimeoutMessageNode<T> messageNode)
        {
            if (keepSeconds != 0)
            {
                this.messageNode = messageNode;
                AppendTaskArray();
            }
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
