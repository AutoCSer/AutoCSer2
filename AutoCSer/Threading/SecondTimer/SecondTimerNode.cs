using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时同步任务节点
    /// </summary>
    public abstract class SecondTimerNode : DoubleLink<SecondTimerNode>
    {
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        protected internal abstract void OnTimer();

        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <param name="node"></param>
#if NetStandard21
        internal static void LinkOnTimer([MaybeNull] SecondTimerNode node)
#else
        internal static void LinkOnTimer(SecondTimerNode node)
#endif
        {
            do
            {
                try
                {
                    do
                    {
                        node.OnTimer();
                    }
                    while ((node = node.DoubleLinkPrevious) != null);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                }
                if (node == null) break;
                node = node.DoubleLinkPrevious;
            }
            while (node != null);
        }
    }
}
