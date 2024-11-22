using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 原始套接字监听数据缓冲区处理队列线程
    /// </summary>
    internal sealed class RawSocketQueue : TaskQueueBase
    {
        /// <summary>
        /// 数据包处理委托
        /// </summary>
        private readonly Action<RawSocketBuffer> onPacket;
        /// <summary>
        /// 队列头部
        /// </summary>
#if NetStandard21
        private RawSocketBuffer? head;
#else
        private RawSocketBuffer head;
#endif
        /// <summary>
        /// 队列尾部
        /// </summary>
#if NetStandard21
        private RawSocketBuffer? end;
#else
        private RawSocketBuffer end;
#endif
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private SpinLock queueLock;
        /// <summary>
        /// 原始套接字监听数据缓冲区处理队列线程
        /// </summary>
        /// <param name="onPacket">数据包处理委托</param>
        internal RawSocketQueue(Action<RawSocketBuffer> onPacket)
        {
            this.onPacket = onPacket;
        }
        /// <summary>
        /// 添加数据缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void Add(RawSocketBuffer buffer)
        {
            queueLock.EnterYield();
            if (head == null)
            {
                end = buffer;
                head = buffer;
                queueLock.Exit();
                waitHandle.Set();
            }
            else
            {
                end.notNull().LinkNext = buffer;
                end = buffer;
                queueLock.Exit();
            }
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                waitHandle.Wait();
                if (isDisposed) return;
                queueLock.EnterYield();
                var value = head;
                end = null;
                head = null;
                queueLock.Exit();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            onPacket(value);
                            if (isDisposed) return;
                            value = value.LinkNext;
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                    value = value.notNull().LinkNext;
                }
                while (value != null);
            }
            while (!isDisposed);
        }
    }
}
