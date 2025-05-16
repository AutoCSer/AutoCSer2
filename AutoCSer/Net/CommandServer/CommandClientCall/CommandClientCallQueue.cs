using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端执行队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandClientCallQueue<T> : SecondTimerTaskArrayNode
        where T : AutoCSer.Threading.Link<T>
    {
        /// <summary>
        /// 命令客户端
        /// </summary>
        protected readonly CommandClient client;
        /// <summary>
        /// 队列
        /// </summary>
        internal LinkStack<T> Queue;
        /// <summary>
        /// 最后一次运行任务时间
        /// </summary>
        protected long runSeconds;
        /// <summary>
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
    }
    /// <summary>
    /// 客户端执行队列
    /// </summary>
    public sealed class CommandClientCallQueue : CommandClientCallQueue<CommandClientCallQueueNode>
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        internal OnceAutoWaitHandle WaitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 客户端执行队列
        /// </summary>
        /// <param name="client"></param>
        internal CommandClientCallQueue(CommandClient client) : base(client)
        {
            WaitHandle.Set(this);
            threadHandle = new System.Threading.Thread(run, ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 关闭执行队列
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close()
        {
            WaitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(CommandClientCallQueueNode node)
        {
            if(Queue.IsPushHead(node)) WaitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(CommandClientCallQueue queue, CommandClientCallQueueNode node)
        {
            queue.Add(node);
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        private void run()
        {
            do
            {
                WaitHandle.Wait();
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
                        client.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (!client.IsDisposed);
        }
        /// <summary>
        /// 创建低优先级任务队列链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandClientCallQueueLowPriorityLink CreateLink()
        {
            return new CommandClientCallQueueLowPriorityLink(this);
        }
        /// <summary>
        /// 超时检查
        /// </summary>
        /// <returns></returns>
        protected internal override Task OnTimerAsync()
        {
            long seconds = SecondTimer.CurrentSeconds - runSeconds;
            if (seconds > KeepSeconds) return client.Config.OnQueueTimeout(this, seconds);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
