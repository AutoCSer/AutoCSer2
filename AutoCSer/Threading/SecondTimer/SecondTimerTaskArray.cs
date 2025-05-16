using AutoCSer.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维定时任务数组
    /// </summary>
    public sealed class SecondTimerTaskArray : SecondTimerArray<SecondTimerTaskArrayNodeLink>
    {
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        /// <param name="linkArrayBitSize">容器二进制位长度，最小值为 8，最大值为 12</param>
        internal SecondTimerTaskArray(byte linkArrayBitSize) : base(linkArrayBitSize) { }
        /// <summary>
        /// 添加定时任务节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal bool Append(SecondTimerTaskArrayNode node)
        {
            long timeoutSeconds = node.TimeoutSeconds;
            Monitor.Enter(TimerLinkLock);
            long index = timeoutSeconds - linkArrayBaseSeconds;
            if (index < linkArrayCapacity)
            {
                if (index >= linkArrayIndex)
                {
                    linkArray[(int)index].Append(node);
                    Monitor.Exit(TimerLinkLock);
                    return true;
                }
                Monitor.Exit(TimerLinkLock);
                return false;
            }
            index = ((index - linkArrayCapacity) >> linkArrayBitSize) + nextLinkArrayIndex;
            if (index < linkArrayCapacity) nextLinkArray[(int)index].Append(node);
            else timerLink.Append(node);
            Monitor.Exit(TimerLinkLock);
            return true;
        }
        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <returns></returns>
        internal async Task OnTimer()
        {
            var node = NodeLink.End;
            if (node != null) SecondTimerNode.LinkOnTimer(node);

            var head = default(SecondTimerTaskArrayNode);
            SecondTimerAppendTaskStateEnum appendTaskState = SecondTimerAppendTaskStateEnum.Completed;
            do
            {
                Monitor.Enter(TimerLinkLock);
                long seconds = linkArrayBaseSeconds + linkArrayIndex;
                if (seconds < SecondTimer.CurrentSeconds)
                {
                    try
                    {
                        head = linkArray[linkArrayIndex++].GetClear();
                        if (linkArrayIndex == linkArrayCapacity)
                        {
                            linkArrayIndex = 0;
                            linkArrayBaseSeconds += linkArrayCapacity;

                            var nextHead = nextLinkArray[nextLinkArrayIndex++].GetClear();
                            while (nextHead != null) nextHead = linkArray[(int)(nextHead.TimeoutSeconds - linkArrayBaseSeconds)].AppendOtherHead(nextHead);

                            if (nextLinkArrayIndex == linkArrayCapacity)
                            {
                                nextLinkArrayIndex = 0;

                                long baseSeconds = linkArrayBaseSeconds + linkArrayCapacity, maxSeconds = baseSeconds + (linkArrayCapacity << linkArrayBitSize);
                                nextHead = timerLink.GetClear();
                                while (nextHead != null)
                                {
                                    if (nextHead.TimeoutSeconds < maxSeconds)
                                    {
                                        nextHead = nextLinkArray[(int)(nextHead.TimeoutSeconds - baseSeconds) >> linkArrayBitSize].AppendOtherHead(nextHead);
                                    }
                                    else nextHead = timerLink.AppendOtherHead(nextHead);
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                    finally { Monitor.Exit(TimerLinkLock); }

                    while (head != null)
                    {
                        try
                        {
                            do
                            {
                                SecondTimerAppendTaskStateEnum callTaskState = head.TryCall(ref appendTaskState);
                                if (callTaskState != SecondTimerAppendTaskStateEnum.Completed) await head.Call(callTaskState, appendTaskState);
                            }
                            while ((head = head.DoubleLinkNext) != null);
                            break;
                        }
                        catch (Exception exception)
                        {
                            await AutoCSer.LogHelper.Exception(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                        }
                        head = head.notNull().DoubleLinkNext;
                    }
                }
                else
                {
                    Monitor.Exit(TimerLinkLock);
                    return;
                }
            }
            while (true);
        }

        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        public void Append(Action task, int timeoutSeconds, SecondTimerTaskThreadModeEnum threadMode = SecondTimerTaskThreadModeEnum.Synchronous)
        {
            new SecondTimerTaskArrayActionNode(task, this, timeoutSeconds, threadMode).AppendTaskArray();
        }
    }
}
