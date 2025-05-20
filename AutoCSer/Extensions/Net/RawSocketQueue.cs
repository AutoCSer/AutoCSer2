using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

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
        /// 缓冲区队列
        /// </summary>
        private LinkStack<RawSocketBuffer> bufferQueue;
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(RawSocketBuffer buffer)
        {
            if(bufferQueue.IsPushHead(buffer)) WaitHandle.Set();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.WaitOne();
                if (isDisposed) return;
                AutoCSer.Threading.ThreadYield.YieldOnly();
                var value = bufferQueue.GetQueue();
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
