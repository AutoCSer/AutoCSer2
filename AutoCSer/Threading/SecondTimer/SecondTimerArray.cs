using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维定时任务数组
    /// </summary>
    /// <typeparam name="T">二维秒级定时任务节点链表类型</typeparam>
    public abstract class SecondTimerArray<T> where T : struct
    {
        /// <summary>
        /// 第一维定时任务数组
        /// </summary>
        protected readonly T[] linkArray;
        /// <summary>
        /// 第二维定时任务数组
        /// </summary>
        protected readonly T[] nextLinkArray;
        /// <summary>
        /// 任务数组容器大小
        /// </summary>
        protected readonly int linkArrayCapacity;
        /// <summary>
        /// 容器二进制位长度，最小值为 8，最大值为 12
        /// </summary>
        protected readonly int linkArrayBitSize;
        /// <summary>
        /// 超出二维任务链表
        /// </summary>
        protected T timerLink;
        /// <summary>
        /// 每秒尝试一次的定时任务链表，不能保证每秒触发一次
        /// </summary>
        internal SecondTimerNode.YieldLink NodeLink;
        /// <summary>
        /// 第一维定时任务数组基础秒数计时
        /// </summary>
        protected long linkArrayBaseSeconds;
        /// <summary>
        /// 第一维定时任务数组当前位置
        /// </summary>
        protected int linkArrayIndex;
        /// <summary>
        /// 第二维定时任务数组当前位置
        /// </summary>
        protected int nextLinkArrayIndex;
        /// <summary>
        /// 任务节点访问锁
        /// </summary>
        internal readonly object TimerLinkLock;
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        /// <param name="linkArrayBitSize">容器二进制位长度，最小值为 8，最大值为 12</param>
        internal SecondTimerArray(byte linkArrayBitSize)
        {
            this.linkArrayBitSize = Math.Min(Math.Max((int)linkArrayBitSize, 8), 12);
            linkArrayCapacity = 1 << linkArrayBitSize;
            linkArray = new T[linkArrayCapacity];
            TimerLinkLock = new object();
            nextLinkArray = new T[linkArrayCapacity];
        }
    }
    /// <summary>
    /// 二维定时同步任务数组
    /// </summary>
    internal sealed class SecondTimerArray : SecondTimerArray<SecondTimerArrayNodeLink>
    {
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        /// <param name="linkArrayBitSize">容器二进制位长度，最小值为 8，最大值为 12</param>
        internal SecondTimerArray(byte linkArrayBitSize) : base(linkArrayBitSize) { }
        /// <summary>
        /// 添加定时任务节点
        /// </summary>
        /// <param name="node"></param>
        internal void Append(SecondTimerArrayNode node)
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
                }
                else
                {
                    Monitor.Exit(TimerLinkLock);
                    node.AppendCall();
                }
            }
            else
            {
                index = ((index - linkArrayCapacity) >> linkArrayBitSize) + nextLinkArrayIndex;
                if (index < linkArrayCapacity) nextLinkArray[(int)index].Append(node);
                else timerLink.Append(node);
                Monitor.Exit(TimerLinkLock);
            }
        }
        /// <summary>
        /// 添加定时委托任务
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Append(Action task, int timeoutSeconds)
        {
            Append(new SecondTimerArrayActionNode(task, this, timeoutSeconds, SecondTimerKeepModeEnum.Before, 0));
        }
        /// <summary>
        /// 添加定时委托任务
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="keepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Append(Action task, int timeoutSeconds, SecondTimerKeepModeEnum keepMode, int keepSeconds)
        {
            Append(new SecondTimerArrayActionNode(task, this, timeoutSeconds, keepMode, keepSeconds));
        }
        /// <summary>
        /// 执行定时任务
        /// </summary>
        internal void OnTimer()
        {
            var node = NodeLink.End;
            if (node != null) SecondTimerNode.LinkOnTimer(node);

            var head = default(SecondTimerArrayNode);
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
                                head.Call(ref head);
                            }
                            while (head != null);
                        }
                        catch (Exception exception)
                        {
                            AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                        }
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
    }
}
