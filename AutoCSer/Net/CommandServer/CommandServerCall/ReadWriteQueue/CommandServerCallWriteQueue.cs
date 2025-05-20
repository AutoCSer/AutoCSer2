using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端同步读写队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景，也可用于支持多线程并发读取的场景，不适合写操作频率高的需求）
    /// </summary>
    public abstract class CommandServerCallWriteQueue : CommandServerCallReadWriteQueue
    {
        /// <summary>
        /// 并发读操作线程
        /// </summary>
        internal ConcurrencyReadWriteQueueThread ConcurrencyReadThread;
        /// <summary>
        /// 写操作任务首节点
        /// </summary>
#if NetStandard21
        private ReadWriteQueueNode? writeHead;
#else
        private ReadWriteQueueNode writeHead;
#endif
        /// <summary>
        /// 写操作等待事件
        /// </summary>
        private readonly System.Threading.AutoResetEvent writeWaitHandle;
        /// <summary>
        /// 等待执行的读取任务队列
        /// </summary>
        private LinkStack<ReadWriteQueueNode> readQueue;
        /// <summary>
        /// 空闲读操作线程集合
        /// </summary>
        private LinkStack<ReadQueueThread> readThreads;
        /// <summary>
        /// 当前读取操作数量
        /// </summary>
        private int readCount;
        /// <summary>
        /// 当前并发读操作状态，允许并发读取不受写操作限制
        /// </summary>
        protected volatile bool isConcurrencyRead;
        /// <summary>
        /// 是否已经关闭队列
        /// </summary>
        internal volatile bool IsClose;
        /// <summary>
        /// 空队列
        /// </summary>
        protected CommandServerCallWriteQueue()
        {
            writeWaitHandle = AutoCSer.Common.NullAutoResetEvent;
            ConcurrencyReadThread = new ConcurrencyReadWriteQueueThread(this, true);
        }
        /// <summary>
        /// 服务端同步读写队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景，也可用于支持多线程并发读取的场景，不适合写操作频率高的需求）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
        /// <param name="maxConcurrency">最大读取操作并发数量，小于等于 0 表示处理器数量减去设置值（比如处理器数量为 4，并发数量设置为 -1，则读取并发数量为 4 - 1 = 3）</param>
#if NetStandard21
        internal CommandServerCallWriteQueue(CommandListener server, CommandServerController? controller, int maxConcurrency) : base(server, controller)
#else
        internal CommandServerCallWriteQueue(CommandListener server, CommandServerController controller, int maxConcurrency) : base(server, controller)
#endif
        {
            readCount = 1;
            writeWaitHandle = new AutoResetEvent(false);
            if (maxConcurrency <= 0) maxConcurrency = Math.Max(AutoCSer.Common.ProcessorCount - maxConcurrency, 1);
            bool isThread = false;
            try
            {
                ConcurrencyReadThread = new ConcurrencyReadWriteQueueThread(this);
                do
                {
                    readThreads.PushOnly(new ReadQueueThread(this));
                }
                while (--maxConcurrency != 0);
                threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
                threadHandle.IsBackground = true;
                threadHandle.Start();
                isThread = true;
            }
            finally
            {
                if (!isThread) Close();
            }
        }
        /// <summary>
        /// 关闭队列
        /// </summary>
        internal void Close()
        {
            IsClose = true;
            QueueWaitHandle.setDispose();
            writeWaitHandle.setDispose();
            for (var node = readThreads.Get(); node != null; node = node.LinkNext) node.WaitHandle.setDispose();
            ConcurrencyReadThread.WaitHandle.setDispose();
        }
        /// <summary>
        /// 任务分配线程
        /// </summary>
        private void run()
        {
            var head = default(ReadWriteQueueNode);
            var writeEnd = default(ReadWriteQueueNode);
            var concurrencyReadHead = default(ReadWriteQueueNode);
            var concurrencyReadEnd = default(ReadWriteQueueNode);
            var nextNode = default(ReadWriteQueueNode);
            var newReadHead = default(ReadWriteQueueNode);
            var newReadEnd = default(ReadWriteQueueNode);
            var newConcurrencyReadHead = default(ReadWriteQueueNode);
            var newConcurrencyReadEnd = default(ReadWriteQueueNode);
            var newWriteHead = default(ReadWriteQueueNode);
            var newWriteEnd = default(ReadWriteQueueNode);
            var thread = default(ReadQueueThread);
            int step = 0;
            do
            {
            WAITQUEUE:
                QueueWaitHandle.WaitOne();
                if (!IsClose)
                {
                    newConcurrencyReadHead = newConcurrencyReadEnd = newWriteHead = newWriteEnd = newReadHead = newReadEnd = null;
                    AutoCSer.Threading.ThreadYield.YieldOnly();
                    head = Queue.Get().notNull();
                    do
                    {
                        nextNode = head.LinkNext;
                        switch (head.Type)
                        {
                            case ReadWriteNodeTypeEnum.Read:
                                head.LinkNext = newReadHead;
                                if (newReadEnd == null) newReadEnd = head;
                                newReadHead = head;
                                break;
                            case ReadWriteNodeTypeEnum.ConcurrencyRead:
                                head.LinkNext = newConcurrencyReadHead;
                                if (newConcurrencyReadEnd == null) newConcurrencyReadEnd = head;
                                newConcurrencyReadHead = head;
                                break;
                            case ReadWriteNodeTypeEnum.Write:
                                head.LinkNext = newWriteHead;
                                if (newWriteEnd == null) newWriteEnd = head;
                                newWriteHead = head;
                                break;
                        }
                        head = nextNode;
                    }
                    while (head != null);
                    if (newReadHead != null) readQueue.PushLink(newReadHead, newReadEnd.notNull());
                    if (newWriteHead != null)
                    {
                        if (writeEnd == null) writeHead = newWriteHead;
                        else writeEnd.LinkNext = newWriteHead;
                        writeEnd = newWriteEnd;
                    }
                    if (newConcurrencyReadHead != null)
                    {
                        if (concurrencyReadEnd == null) concurrencyReadHead = newConcurrencyReadHead;
                        else concurrencyReadEnd.LinkNext = newConcurrencyReadHead;
                        concurrencyReadEnd = newConcurrencyReadEnd;
                    }
                    switch (step)
                    {
                        case 0:
                        WRITE:
                            if (writeHead != null)
                            {
                                do
                                {
                                    try
                                    {
                                        do
                                        {
                                            writeHead.RunTask();
                                        }
                                        while ((writeHead = writeHead.LinkNext) != null);
                                        break;
                                    }
                                    catch (Exception exception)
                                    {
                                        writeHead.notNull().OnException(this, exception);
                                    }
                                }
                                while ((writeHead = writeHead.notNull().LinkNext) != null);
                                writeEnd = null;
                            }
                            if (concurrencyReadHead != null) goto CONCURRENCY;
                            THREAD:
                            do
                            {
                                if ((thread = readThreads.Pop()) != null)
                                {
                                    if ((head = readQueue.Pop()) != null)
                                    {
                                        System.Threading.Interlocked.Increment(ref readCount);
                                        thread.Set(head);
                                        AutoCSer.Threading.ThreadYield.YieldOnly();
                                        continue;
                                    }
                                    readThreads.Push(thread);
                                }
                                if (!IsClose)
                                {
                                    step = 2;
                                    goto WAITQUEUE;
                                }
                                return;
                            }
                            while (true);
                        CONCURRENCY:
                            isConcurrencyRead = true;
                            System.Threading.Interlocked.Increment(ref readCount);
                            ConcurrencyReadThread.Set(concurrencyReadHead);
                            concurrencyReadHead = concurrencyReadHead.LinkNext;
                            if (concurrencyReadHead == null) concurrencyReadEnd = null;
                            CONCURRENCYTHREAD:
                            do
                            {
                                if ((thread = readThreads.Pop()) != null)
                                {
                                    if ((head = readQueue.Pop()) != null)
                                    {
                                        System.Threading.Interlocked.Increment(ref readCount);
                                        thread.Set(head);
                                        if (isConcurrencyRead)
                                        {
                                            AutoCSer.Threading.ThreadYield.YieldOnly();
                                            continue;
                                        }
                                        if (writeEnd != null) goto WAITWRITE;
                                        if (concurrencyReadHead == null)
                                        {
                                            AutoCSer.Threading.ThreadYield.YieldOnly();
                                            goto THREAD;
                                        }
                                        goto CONCURRENCY;
                                    }
                                    readThreads.Push(thread);
                                }
                                if (!IsClose)
                                {
                                    step = 1;
                                    goto WAITQUEUE;
                                }
                                return;
                            }
                            while (true);
                        case 1:
                            if (isConcurrencyRead) goto CONCURRENCYTHREAD;
                            goto CHECKWRITE;
                        case 2:
                        CHECKWRITE:
                            if (writeEnd == null) goto READ;
                            WAITWRITE:
                            if (System.Threading.Interlocked.Decrement(ref readCount) != 0)
                            {
                                if (!IsClose) writeWaitHandle.WaitOne();
                                else return;
                            }
                            if (!IsClose)
                            {
                                readCount = 1;
                                goto WRITE;
                            }
                            return;
                        READ:
                            if (concurrencyReadHead == null) goto THREAD;
                            goto CONCURRENCY;
                    }
                }
            }
            while (!IsClose);
        }
        /// <summary>
        /// 获取下一个读取任务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ReadWriteQueueNode? GetRead()
#else
        internal ReadWriteQueueNode GetRead()
#endif
        {
            return writeHead == null || isConcurrencyRead ? readQueue.Pop() : null;
        }
        /// <summary>
        /// 读取操作线程添加到空闲集合
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        internal bool Free(ReadQueueThread thread)
        {
            if (System.Threading.Interlocked.Decrement(ref readCount) == 0) writeWaitHandle.Set();
            if (!IsClose)
            {
                readThreads.Push(thread);
                return true;
            }
            thread.WaitHandle.Set();
            return false;
        }
        /// <summary>
        /// 关闭读取操作线程
        /// </summary>
        /// <returns></returns>
        internal void CloseReadThread()
        {
            if (System.Threading.Interlocked.Decrement(ref readCount) == 0) writeWaitHandle.Set();
        }
        /// <summary>
        /// 并发读操作任务处理结束
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Free(ConcurrencyReadWriteQueueThread thread)
        {
            CloseConcurrencyReadThread();
            if (!IsClose) return true;
            thread.WaitHandle.Set();
            return false;
        }
        /// <summary>
        /// 关闭读取操作线程
        /// </summary>
        /// <returns></returns>
        internal void CloseConcurrencyReadThread()
        {
            isConcurrencyRead = false;
            if (System.Threading.Interlocked.Decrement(ref readCount) == 0) writeWaitHandle.Set();
            if (Queue.IsEmpty && Queue.TryPushHead(new NullReadWriteQueueNode())) QueueWaitHandle.Set();
        }
    }
}
