using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// A synchronous queue for write operations supported by parallel reads on the server side (mainly used in scenarios where in-memory database nodes support parallel reads when obtaining persistent data)
    /// 服务端支持并行读的写操作同步队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景）
    /// </summary>
    public abstract class CommandServerCallConcurrencyReadWriteQueue : CommandServerCallReadWriteQueue
    {
        /// <summary>
        /// Concurrent read operation thread
        /// 并发读操作线程
        /// </summary>
        internal ConcurrencyReadQueueThread ConcurrencyReadThread;
        /// <summary>
        /// The current task execution node
        /// 当前执行任务节点
        /// </summary>
#if NetStandard21
        private ReadWriteQueueNode? currentTask;
#else
        private ReadWriteQueueNode currentTask;
#endif
        /// <summary>
        /// The time of the last task run
        /// 最后一次运行任务时间
        /// </summary>
        private long runSeconds;
        /// <summary>
        /// The current status of concurrent read operations allows concurrent reads without being restricted by write operations
        /// 当前并发读操作状态，允许并发读取不受写操作限制
        /// </summary>
        protected volatile bool isConcurrencyRead;
        /// <summary>
        /// Has the queue been closed
        /// 是否已经关闭队列
        /// </summary>
        internal volatile bool IsClose;
        /// <summary>
        /// Empty queue
        /// </summary>
        protected CommandServerCallConcurrencyReadWriteQueue()
        {
            ConcurrencyReadThread = new ConcurrencyReadQueueThread(this, true);
        }
        /// <summary>
        /// A synchronous queue for write operations supported by parallel reads on the server side (mainly used in scenarios where in-memory database nodes support parallel reads when obtaining persistent data)
        /// 服务端支持并行读的写操作同步队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
#if NetStandard21
        internal CommandServerCallConcurrencyReadWriteQueue(CommandListener server, CommandServerController? controller) : base(server, controller)
#else
        internal CommandServerCallConcurrencyReadWriteQueue(CommandListener server, CommandServerController controller) : base(server, controller)
#endif
        {
            runSeconds = long.MaxValue;
            bool isThread = false;
            try
            {
                ConcurrencyReadThread = new ConcurrencyReadQueueThread(this);
                threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
                threadHandle.IsBackground = true;
                threadHandle.Start();
                isThread = true;
            }
            finally
            {
                if (!isThread) Close();
            }
            if (KeepSeconds > 0) AppendTaskArray();
        }
        /// <summary>
        /// Close the queue
        /// 关闭队列
        /// </summary>
        internal void Close()
        {
            IsClose = true;
            KeepSeconds = 0;
            QueueWaitHandle.setDispose();
            ConcurrencyReadThread.WaitHandle.setDispose();
        }
        /// <summary>
        /// Task allocation thread
        /// 任务分配线程
        /// </summary>
        private void run()
        {
            var readHead = default(ReadWriteQueueNode);
            var writeHead = default(ReadWriteQueueNode);
            var head = default(ReadWriteQueueNode);
            var writeEnd = default(ReadWriteQueueNode);
            var concurrencyReadHead = default(ReadWriteQueueNode);
            var concurrencyReadEnd = default(ReadWriteQueueNode);
            var nextNode = default(ReadWriteQueueNode);
            var newConcurrencyReadHead = default(ReadWriteQueueNode);
            var newConcurrencyReadEnd = default(ReadWriteQueueNode);
            var newWriteHead = default(ReadWriteQueueNode);
            var newWriteEnd = default(ReadWriteQueueNode);
            bool isWaitConcurrencyRead = false;
            do
            {
                QueueWaitHandle.WaitOne();
                if (!IsClose)
                {
                    newConcurrencyReadHead = newConcurrencyReadEnd = newWriteHead = newWriteEnd = null;
                    AutoCSer.Threading.ThreadYield.YieldOnly();
                    head = Queue.Get().notNull();
                    do
                    {
                        nextNode = head.LinkNext;
                        switch (head.Type)
                        {
                            case ReadWriteNodeTypeEnum.Read:
                                head.LinkNext = readHead;
                                readHead = head;
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
                    do
                    {
                        if (!isWaitConcurrencyRead)
                        {
                            if (writeHead != null)
                            {
                                do
                                {
                                    try
                                    {
                                        do
                                        {
                                            runSeconds = SecondTimer.CurrentSeconds;
                                            currentTask = writeHead;
                                            writeHead.RunTask();
                                            runSeconds = long.MaxValue;
                                        }
                                        while ((writeHead = writeHead.LinkNext) != null);
                                        break;
                                    }
                                    catch (Exception exception)
                                    {
                                        runSeconds = long.MaxValue;
                                        writeHead.notNull().OnException(this, exception);
                                    }
                                }
                                while ((writeHead = writeHead.notNull().LinkNext) != null);
                                writeEnd = null;
                            }
                            if (concurrencyReadHead == null)
                            {
                                if (readHead != null)
                                {
                                    do
                                    {
                                        try
                                        {
                                            do
                                            {
                                                runSeconds = SecondTimer.CurrentSeconds;
                                                currentTask = readHead;
                                                readHead.RunTask();
                                                runSeconds = long.MaxValue;
                                            }
                                            while ((readHead = readHead.LinkNext) != null);
                                            break;
                                        }
                                        catch (Exception exception)
                                        {
                                            runSeconds = long.MaxValue;
                                            readHead.notNull().OnException(this, exception);
                                        }
                                    }
                                    while ((readHead = readHead.notNull().LinkNext) != null);
                                }
                                break;
                            }
                            isConcurrencyRead = true;
                            ConcurrencyReadThread.Set(concurrencyReadHead);
                            concurrencyReadHead = concurrencyReadHead.LinkNext;
                            if (concurrencyReadHead == null) concurrencyReadEnd = null;
                        }
                        else isWaitConcurrencyRead = false;
                        if (readHead != null)
                        {
                            do
                            {
                                try
                                {
                                    do
                                    {
                                        runSeconds = SecondTimer.CurrentSeconds;
                                        currentTask = readHead;
                                        readHead.RunTask();
                                        readHead = readHead.LinkNext;
                                        runSeconds = long.MaxValue;
                                    }
                                    while (readHead != null && isConcurrencyRead);
                                    break;
                                }
                                catch (Exception exception)
                                {
                                    runSeconds = long.MaxValue;
                                    readHead.notNull().OnException(this, exception);
                                }
                            }
                            while ((readHead = readHead.notNull().LinkNext) != null);
                        }
                        if (isConcurrencyRead)
                        {
                            isWaitConcurrencyRead = true;
                            break;
                        }
                    }
                    while (true);
                }
            }
            while (!IsClose);
        }
        /// <summary>
        /// The concurrent read operation task processing has been completed
        /// 并发读操作任务处理结束
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Free(ConcurrencyReadQueueThread thread)
        {
            CloseReadThread();
            if (!IsClose) return true;
            thread.WaitHandle.Set();
            return false;
        }
        /// <summary>
        /// Close the read operation thread
        /// 关闭读取操作线程
        /// </summary>
        /// <returns></returns>
        internal void CloseReadThread()
        {
            isConcurrencyRead = false;
            if (Queue.IsEmpty && Queue.TryPushHead(new NullReadWriteQueueNode())) QueueWaitHandle.Set();
        }
        /// <summary>
        /// Timeout check
        /// 超时检查
        /// </summary>
        /// <returns></returns>
        protected internal override Task OnTimerAsync()
        {
            long seconds = SecondTimer.CurrentSeconds - runSeconds;
            if (seconds > KeepSeconds)
            {
                var currentTask = this.currentTask;
                if (currentTask != null) return currentTask.OnTimeout(this, seconds);
            }
            return AutoCSer.Common.CompletedTask;
        }
    }
}
