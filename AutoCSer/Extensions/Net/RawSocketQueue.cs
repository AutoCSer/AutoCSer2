using AutoCSer.Threading;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 原始套接字监听数据缓冲区处理队列线程
    /// </summary>
    internal sealed class RawSocketQueue : IDisposable
    {
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 数据包处理委托
        /// </summary>
        private readonly Action<RawSocketBuffer> onPacket;
        /// <summary>
        /// 等待事件
        /// </summary>
        private OnceAutoWaitHandle waitHandle;
        /// <summary>
        /// 队列头部
        /// </summary>
        private RawSocketBuffer head;
        /// <summary>
        /// 队列尾部
        /// </summary>
        private RawSocketBuffer end;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private SpinLock queueLock;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile bool isDisposed;
        /// <summary>
        /// 原始套接字监听数据缓冲区处理队列线程
        /// </summary>
        /// <param name="onPacket">数据包处理委托</param>
        internal RawSocketQueue(Action<RawSocketBuffer> onPacket)
        {
            this.onPacket = onPacket;
            waitHandle.Set(this);
            threadHandle = new System.Threading.Thread(run, ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            waitHandle.Set();
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
                end.LinkNext = buffer;
                end = buffer;
                queueLock.Exit();
            }
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        private void run()
        {
            do
            {
                waitHandle.Wait();
                if (isDisposed) return;
                queueLock.EnterYield();
                RawSocketBuffer value = head;
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
                    value = value.LinkNext;
                }
                while (value != null);
            }
            while (!isDisposed);
        }
    }
}
