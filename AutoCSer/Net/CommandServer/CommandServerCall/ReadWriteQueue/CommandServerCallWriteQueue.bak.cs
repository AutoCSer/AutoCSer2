using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端同步读写队列
    /// </summary>
    public abstract class CommandServerCallWriteQueue
    {
        /// <summary>
        /// 队列自定义上下文对象
        /// </summary>
#if NetStandard21
        public object? ContextObject;
#else
        public object ContextObject;
#endif
        /// <summary>
        /// 命令服务
        /// </summary>
        public readonly CommandListener Server;
        /// <summary>
        /// 命令服务控制器
        /// </summary>
#if NetStandard21
        internal readonly CommandServerController? Controller;
#else
        internal readonly CommandServerController Controller;
#endif
        /// <summary>
        /// 最大读取操作并发数量
        /// </summary>
        private readonly int maxConcurrency;
        /// <summary>
        /// 当前允许并发读取操作数量
        /// </summary>
        private int canConcurrency;
        /// <summary>
        /// 队列访问锁
        /// </summary>
        private readonly object queueLock;
        /// <summary>
        /// 读操作任务队列首节点
        /// </summary>
#if NetStandard21
        private ReadWriteQueueNode? readHead;
#else
        private ReadWriteQueueNode readHead;
#endif
        /// <summary>
        /// 读操作任务队列尾节点
        /// </summary>
#if NetStandard21
        private ReadWriteQueueNode? readEnd;
#else
        private ReadWriteQueueNode readEnd;
#endif
        /// <summary>
        /// 写操作任务队列首节点
        /// </summary>
#if NetStandard21
        private ReadWriteQueueNode? writeHead;
#else
        private ReadWriteQueueNode writeHead;
#endif
        /// <summary>
        /// 写操作任务队列尾节点
        /// </summary>
#if NetStandard21
        private ReadWriteQueueNode? writeEnd;
#else
        private ReadWriteQueueNode writeEnd;
#endif
        /// <summary>
        /// 当前长读操作数量，允许并发读取不受写操作限制
        /// </summary>
        private int longReadCount;
        /// <summary>
        /// 服务端同步读写队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
        /// <param name="maxConcurrency">最大读取操作并发数量</param>
#if NetStandard21
        internal CommandServerCallWriteQueue(CommandListener server, CommandServerController? controller, int maxConcurrency)
#else
        internal CommandServerCallWriteQueue(CommandListener server, CommandServerController controller, int maxConcurrency)
#endif
        {
            Server = server;
            Controller = controller;
            canConcurrency = this.maxConcurrency = maxConcurrency > 0 ? maxConcurrency : AutoCSer.Common.ProcessorCount;
            queueLock = new object();
        }
        /// <summary>
        /// 获取下一个任务节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        internal ReadWriteQueueNode? GetNextNode(ReadWriteNodeTypeEnum type)
#else
        internal CommandServerReadWriteQueueNode GetNextNode(ReadWriteNodeTypeEnum type)
#endif
        {
            switch (type)
            {
                case ReadWriteNodeTypeEnum.Read:
                    if (readHead == null) AutoCSer.Threading.ThreadYield.YieldOnly();
                    Monitor.Enter(queueLock);
                    if (canConcurrency != 0)
                    {
                        if (canConcurrency != -1)
                        {
                            ++canConcurrency;
                            Monitor.Exit(queueLock);
                            return null;
                        }
                        ReadWriteQueueNode node = writeHead.notNull();
                        if ((writeHead = node.LinkNext) != null)
                        {
                            Monitor.Exit(queueLock);
                            node.LinkNext = null;
                            return node;
                        }
                        writeEnd = null;
                        Monitor.Exit(queueLock);
                        return node;
                    }
                    if (readHead != null)
                    {
                        ReadWriteQueueNode node = readHead;
                        readHead = node.LinkNext;
                        if (node.Type == ReadWriteNodeTypeEnum.LongRead) ++longReadCount;
                        if (readHead != null)
                        {
                            Monitor.Exit(queueLock);
                            node.LinkNext = null;
                            return node;
                        }
                        readEnd = null;
                        Monitor.Exit(queueLock);
                        return node;
                    }
                    canConcurrency = 1;
                    Monitor.Exit(queueLock);
                    return null;
                case ReadWriteNodeTypeEnum.Write:
                    if (readHead == null) AutoCSer.Threading.ThreadYield.YieldOnly();
                    Monitor.Enter(queueLock);
                    if (readHead != null)
                    {
                        ReadWriteQueueNode node = readHead;
                        canConcurrency = maxConcurrency - 1;
                        var end = node.LinkNext;
                        if (node.Type == ReadWriteNodeTypeEnum.LongRead) longReadCount = 1;
                        do
                        {
                            if (end != null)
                            {
                                if (canConcurrency != 0)
                                {
                                    if (end.Type == ReadWriteNodeTypeEnum.LongRead) ++longReadCount;
                                    --canConcurrency;
                                    end = end.LinkNext;
                                }
                                else
                                {
                                    readHead = end;
                                    break;
                                }
                            }
                            else
                            {
                                readHead = readEnd = null;
                                break;
                            }
                        }
                        while (true);
                        if (writeHead != null && longReadCount == 0) canConcurrency -= maxConcurrency;
                        Monitor.Exit(queueLock);
                        for (var next = node.LinkNext; !object.ReferenceEquals(next, end); next = next.notNull().GetNextRunThread()) ;
                        node.LinkNext = null;
                        return node;
                    }
                    if (writeHead != null)
                    {
                        ReadWriteQueueNode node = writeHead;
                        if ((writeHead = node.LinkNext) != null)
                        {
                            Monitor.Exit(queueLock);
                            node.LinkNext = null;
                            return node;
                        }
                        writeEnd = null;
                        Monitor.Exit(queueLock);
                        return node;
                    }
                    canConcurrency = maxConcurrency;
                    Monitor.Exit(queueLock);
                    return null;
                case ReadWriteNodeTypeEnum.LongRead:
                    Monitor.Enter(queueLock);
                    if (writeHead != null && longReadCount == 1)
                    {
                        if (++canConcurrency != maxConcurrency)
                        {
                            longReadCount = 0;
                            canConcurrency -= maxConcurrency;
                            Monitor.Exit(queueLock);
                            return null;
                        }
                        ReadWriteQueueNode node = writeHead;
                        longReadCount = 0;
                        writeHead = node.LinkNext;
                        canConcurrency = -1;
                        if (writeHead != null)
                        {
                            Monitor.Exit(queueLock);
                            node.LinkNext = null;
                            return node;
                        }
                        writeEnd = null;
                        Monitor.Exit(queueLock);
                        return node;
                    }
                    if (canConcurrency != 0 || readHead == null)
                    {
                        --longReadCount;
                        ++canConcurrency;
                        Monitor.Exit(queueLock);
                        return null;
                    }
                    else
                    {
                        ReadWriteQueueNode node = readHead;
                        readHead = node.LinkNext;
                        if (node.Type != ReadWriteNodeTypeEnum.LongRead) --longReadCount;
                        if (readHead != null)
                        {
                            Monitor.Exit(queueLock);
                            node.LinkNext = null;
                            return node;
                        }
                        readEnd = null;
                        Monitor.Exit(queueLock);
                        return node;
                    }
            }
            return null;
        }
        /// <summary>
        /// 添加长读操作任务节点，允许读取操作则同步执行任务
        /// </summary>
        /// <param name="node"></param>
        protected void longRead(ReadWriteQueueNode node)
        {
            Monitor.Enter(queueLock);
            if (canConcurrency > 0)
            {
                ++longReadCount;
                --canConcurrency;
                Monitor.Exit(queueLock);
                node.RunThread();
                return;
            }
            if (readEnd != null) readEnd.LinkNext = node;
            else readHead = node;
            readEnd = node;
            Monitor.Exit(queueLock);
        }
        /// <summary>
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="node"></param>
        protected void appendRead(ReadWriteQueueNode node)
        {
            Monitor.Enter(queueLock);
            if (canConcurrency > 0)
            {
                --canConcurrency;
                Monitor.Exit(queueLock);
                AutoCSer.Threading.ThreadPool.Tiny.FastStart(node.RunThread);
                return;
            }
            if (readEnd != null) readEnd.LinkNext = node;
            else readHead = node;
            readEnd = node;
            Monitor.Exit(queueLock);
        }
        /// <summary>
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="node"></param>
        protected void appendWrite(ReadWriteQueueNode node)
        {
            Monitor.Enter(queueLock);
            if (canConcurrency < 0 || longReadCount != 0 || (canConcurrency -= maxConcurrency) != 0)
            {
                if (writeEnd != null) writeEnd.LinkNext = node;
                else writeHead = node;
                writeEnd = node;
                Monitor.Exit(queueLock);
                return;
            }
            canConcurrency = -1;
            Monitor.Exit(queueLock);
            AutoCSer.Threading.ThreadPool.Tiny.FastStart(node.RunThread);
        }
    }
}
