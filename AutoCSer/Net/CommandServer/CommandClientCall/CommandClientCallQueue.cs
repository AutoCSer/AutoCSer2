using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The client executes the queue
    /// 客户端执行队列
    /// </summary>
    /// <typeparam name="T">Execute the type of task node
    /// 执行任务节点类型</typeparam>
    public abstract class CommandClientCallQueue<T> : SecondTimerTaskArrayNode
        where T : AutoCSer.Threading.Link<T>
    {
        /// <summary>
        /// Command client
        /// </summary>
        protected readonly CommandClient client;
        /// <summary>
        /// Execution queue
        /// 执行队列
        /// </summary>
        internal LinkStack<T> Queue;
        /// <summary>
        /// The time of the last task run
        /// 最后一次运行任务时间
        /// </summary>
        protected long runSeconds;
        /// <summary>
        /// The client executes the queue
        /// 客户端执行队列
        /// </summary>
        /// <param name="client"></param>
        protected CommandClientCallQueue(CommandClient client)
            : base(SecondTimer.TaskArray, client.Config.QueueTimeoutSeconds, SecondTimerTaskThreadModeEnum.WaitTask, SecondTimerKeepModeEnum.After, client.Config.QueueTimeoutSeconds)
        {
            this.client = client;
            if (KeepSeconds > 0)
            {
                runSeconds = long.MaxValue;
                AppendTaskArray();
            }
        }
        /// <summary>
        /// The client executes the queue
        /// 客户端执行队列
        /// </summary>
        /// <param name="controller"></param>
        internal CommandClientCallQueue(CommandClientDefaultController controller)
            : base(SecondTimer.TaskArray, 0, SecondTimerTaskThreadModeEnum.WaitTask, SecondTimerKeepModeEnum.After, 0)
        {
            this.client = controller.Client;
        }
    }
    /// <summary>
    /// The client executes the queue
    /// 客户端执行队列
    /// </summary>
    public sealed class CommandClientCallQueue : CommandClientCallQueue<CommandClientCallQueueNode>
    {
        /// <summary>
        /// Queue waiting event
        /// 队列等待事件
        /// </summary>
        internal System.Threading.AutoResetEvent WaitHandle;
        /// <summary>
        /// Thread handle
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// The client executes the queue
        /// 客户端执行队列
        /// </summary>
        /// <param name="client"></param>
        internal CommandClientCallQueue(CommandClient client) : base(client)
        {
            WaitHandle = new System.Threading.AutoResetEvent(false);
            threadHandle = new System.Threading.Thread(run, ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// The client executes the queue
        /// 客户端执行队列
        /// </summary>
        /// <param name="controller"></param>
        internal CommandClientCallQueue(CommandClientDefaultController controller) : base(controller)
        {
            WaitHandle = AutoCSer.Common.NullAutoResetEvent;
            threadHandle = AutoCSer.Threading.ThreadPool.BackgroundExitThread.Handle;
        }
        /// <summary>
        /// Close the execution queue
        /// 关闭执行队列
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close()
        {
            WaitHandle.setDispose();
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(CommandClientCallQueueNode node)
        {
            if(Queue.IsPushHead(node)) WaitHandle.Set();
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(CommandClientCallQueue queue, CommandClientCallQueueNode node)
        {
            queue.Add(node);
        }
        /// <summary>
        /// Task processing thread
        /// 任务处理线程
        /// </summary>
        private void run()
        {
            do
            {
                WaitHandle.WaitOne();
                if (client.IsDisposed) return;
                AutoCSer.Threading.ThreadYield.YieldOnly();
                var value = Queue.GetQueue();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            runSeconds = SecondTimer.CurrentSeconds;
                            value.RunTask(this, ref value);
                            runSeconds = long.MaxValue;
                            if (client.IsDisposed) return;
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        runSeconds = long.MaxValue;
                        client.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (!client.IsDisposed);
        }
        /// <summary>
        /// Create a linked list of low-priority task queues
        /// 创建低优先级任务队列链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandClientCallQueueLowPriorityLink CreateLink()
        {
            return new CommandClientCallQueueLowPriorityLink(this);
        }
        /// <summary>
        /// Timeout check
        /// 超时检查
        /// </summary>
        /// <returns></returns>
        protected internal override Task OnTimerAsync()
        {
            long seconds = SecondTimer.CurrentSeconds - runSeconds;
            if (seconds > KeepSeconds) return client.OnQueueTimeout(this, seconds);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
