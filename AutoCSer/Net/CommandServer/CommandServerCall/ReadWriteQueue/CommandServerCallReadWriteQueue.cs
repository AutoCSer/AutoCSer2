using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// The server synchronizes the read and write queues
    /// 服务端同步读写队列
    /// </summary>
    public abstract class CommandServerCallReadWriteQueue : SecondTimerTaskArrayNode
    {
        /// <summary>
        /// The new execution task queue
        /// 新的执行任务队列
        /// </summary>
        internal LinkStack<ReadWriteQueueNode> Queue;
        /// <summary>
        /// Queue custom context object
        /// 队列自定义上下文对象
        /// </summary>
#if NetStandard21
        public object? ContextObject;
#else
        public object ContextObject;
#endif
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public readonly CommandListener Server;
        /// <summary>
        /// Command service controller
        /// 命令服务控制器
        /// </summary>
#if NetStandard21
        internal readonly CommandServerController? Controller;
#else
        internal readonly CommandServerController Controller;
#endif
        /// <summary>
        /// Thread handle
        /// 线程句柄
        /// </summary>
        protected System.Threading.Thread threadHandle;
        /// <summary>
        /// The new task queue is waiting for events
        /// 新任务队列等待事件
        /// </summary>
        internal System.Threading.AutoResetEvent QueueWaitHandle;
        /// <summary>
        /// Empty queue
        /// </summary>
        protected CommandServerCallReadWriteQueue()
        {
            QueueWaitHandle = AutoCSer.Common.NullAutoResetEvent;
#if NetStandard21
            threadHandle = AutoCSer.Threading.ThreadPool.BackgroundExitThread.Handle;
#endif
        }
        /// <summary>
        /// A synchronous queue on the server side that supports parallel reading (mainly used in scenarios where in-memory database nodes support parallel reading when obtaining persistent data)
        /// 服务端支持并行读的同步队列（主要用于支持内存数据库节点获取持久化数据时支持并行读取的场景）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
#if NetStandard21
        internal CommandServerCallReadWriteQueue(CommandListener server, CommandServerController? controller)
#else
        internal CommandServerCallReadWriteQueue(CommandListener server, CommandServerController controller)
#endif
            : base(SecondTimer.TaskArray, server.Config.QueueTimeoutSeconds, SecondTimerTaskThreadModeEnum.WaitTask, SecondTimerKeepModeEnum.After, server.Config.QueueTimeoutSeconds)
        {
            Server = server;
            Controller = controller;
            QueueWaitHandle = new System.Threading.AutoResetEvent(false);
#if NetStandard21
            threadHandle = AutoCSer.Threading.ThreadPool.BackgroundExitThread.Handle;
#endif
        }
        /// <summary>
        /// Add task nodes
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void push(ReadWriteQueueNode node)
        {
            if (Queue.IsPushHead(node)) QueueWaitHandle.Set();
        }
        /// <summary>
        /// Add a concurrent read operation task node. If read operations are allowed, the tasks will be executed synchronously
        /// 添加并发读操作任务节点，允许读取操作则同步执行任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ConcurrencyRead(ReadWriteQueueNode node)
        {
            node.Type = ReadWriteNodeTypeEnum.ConcurrencyRead;
            push(node);
        }
        /// <summary>
        /// Add the read operation task node
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="node"></param>
        public void AppendRead(ReadWriteQueueNode node)
        {
            if (node.CheckSet(ReadWriteNodeTypeEnum.Read)) push(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// Add the read operation task node
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendReadOnly(ReadWriteQueueNode node)
        {
            node.Type = ReadWriteNodeTypeEnum.Read;
            push(node);
        }
        /// <summary>
        /// Add the read operation task node
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendRead(CommandServerCallReadWriteQueue queue, ReadWriteQueueNode node)
        {
            queue.AppendReadOnly(node);
        }
        /// <summary>
        /// Add the write operation task node
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="node"></param>
        public void AppendWrite(ReadWriteQueueNode node)
        {
            if (node.CheckSet(ReadWriteNodeTypeEnum.Write)) push(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// Add the write operation task node
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendWriteOnly(ReadWriteQueueNode node)
        {
            node.Type = ReadWriteNodeTypeEnum.Write;
            push(node);
        }
        /// <summary>
        /// Add the write operation task node
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendWrite(CommandServerCallReadWriteQueue queue, ReadWriteQueueNode node)
        {
            queue.AppendWriteOnly(node);
        }
        /// <summary>
        /// Add the read operation task node
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum AppendReadIsDeserialize(CommandServerCallReadWriteQueue queue, ReadWriteQueueNode node)
        {
            if (node.IsDeserialize)
            {
                queue.AppendReadOnly(node);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
        /// <summary>
        /// Add the write operation task node
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum AppendWriteIsDeserialize(CommandServerCallReadWriteQueue queue, ReadWriteQueueNode node)
        {
            if (node.IsDeserialize)
            {
                queue.AppendWriteOnly(node);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
    }
}
