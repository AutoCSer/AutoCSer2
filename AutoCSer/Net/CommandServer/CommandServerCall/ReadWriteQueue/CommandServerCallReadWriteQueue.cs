using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端同步读写队列
    /// </summary>
    public abstract class CommandServerCallReadWriteQueue : SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 新的执行任务队列
        /// </summary>
        internal LinkStack<ReadWriteQueueNode> Queue;
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
#if NetStandard21
        [AllowNull]
#endif
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
        /// 线程句柄
        /// </summary>
        protected System.Threading.Thread threadHandle;
        /// <summary>
        /// 新任务队列等待事件
        /// </summary>
        internal OnceAutoWaitHandle QueueWaitHandle;
        /// <summary>
        /// 空队列
        /// </summary>
        protected CommandServerCallReadWriteQueue()
        {
            QueueWaitHandle.Set(this);
#if NetStandard21
            threadHandle = AutoCSer.Threading.ThreadPool.BackgroundExitThread.Handle;
#endif
        }
        /// <summary>
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
            QueueWaitHandle.Set(new object());
#if NetStandard21
            threadHandle = AutoCSer.Threading.ThreadPool.BackgroundExitThread.Handle;
#endif
        }
        /// <summary>
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void push(ReadWriteQueueNode node)
        {
            if (Queue.IsPushHead(node)) QueueWaitHandle.Set();
        }
        /// <summary>
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
        /// 添加读操作任务节点
        /// </summary>
        /// <param name="node"></param>
        public void AppendRead(ReadWriteQueueNode node)
        {
            if (node.CheckSet(ReadWriteNodeTypeEnum.Read)) push(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
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
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendRead(CommandServerCallReadWriteQueue queue, ReadWriteQueueNode node)
        {
            queue.AppendReadOnly(node);
        }
        /// <summary>
        /// 添加写操作任务节点
        /// </summary>
        /// <param name="node"></param>
        public void AppendWrite(ReadWriteQueueNode node)
        {
            if (node.CheckSet(ReadWriteNodeTypeEnum.Write)) push(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
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
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendWrite(CommandServerCallReadWriteQueue queue, ReadWriteQueueNode node)
        {
            queue.AppendWriteOnly(node);
        }
        /// <summary>
        /// 添加任务
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
        /// 添加任务
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
