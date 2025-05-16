using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端同步线程队列
    /// </summary>
    public sealed class CommandServerCallQueue : SecondTimerTaskArrayNode
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
        /// 任务队列
        /// </summary>
        private LinkStack<QueueTaskNode> queue;
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
        /// 等待事件
        /// </summary>
        internal OnceAutoWaitHandle WaitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 当前执行任务
        /// </summary>
#if NetStandard21
        private QueueTaskNode? currentTask;
#else
        private QueueTaskNode currentTask;
#endif
        /// <summary>
        /// 最后一次运行任务时间
        /// </summary>
        private long runSeconds;
        /// <summary>
        /// 队列编号
        /// </summary>
        internal readonly int Index;
        /// <summary>
        /// 队列线程ID
        /// </summary>
        internal int ThreadId;
        /// <summary>
        /// 当前发送数据套接字
        /// </summary>
        private CommandServerSocket sendSocket;
        /// <summary>
        /// 输出信息头节点
        /// </summary>
        private ServerOutput outputHead;
        /// <summary>
        /// 输出信息尾节点
        /// </summary>
        private ServerOutput outputEnd;
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="controller"></param>
        /// <param name="index"></param>
#if NetStandard21
        internal CommandServerCallQueue(CommandListener server, CommandServerController? controller, int index)
#else
        internal CommandServerCallQueue(CommandListener server, CommandServerController controller, int index)
#endif
             : base(SecondTimer.TaskArray, server.Config.QueueTimeoutSeconds, SecondTimerTaskThreadModeEnum.WaitTask, SecondTimerKeepModeEnum.After, server.Config.QueueTimeoutSeconds)
        {
            Server = server;
            Controller = controller;
            sendSocket = CommandServerSocket.CommandServerSocketContext;
            outputEnd = outputHead = CommandServerConfig.NullServerOutput;
            Index = index;
            runSeconds = long.MaxValue;
            WaitHandle.Set(this);
            threadHandle = new System.Threading.Thread(run, ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
            if (KeepSeconds > 0) AppendTaskArray();
        }
        /// <summary>
        /// 关闭执行队列
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close()
        {
            KeepSeconds = 0;
            WaitHandle.Reserved = 1;
            if (queue.IsEmpty) WaitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void Add(CommandServerCallQueueCustomNode node)
        {
            if (node.CheckQueue()) AddOnly(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(QueueTaskNode node)
        {
            if(queue.IsPushHead(node)) WaitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(CommandServerCallQueue queue, CommandServerCallQueueNode node)
        {
            queue.AddOnly(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum AddIsDeserialize(CommandServerCallQueue queue, CommandServerCallQueueNode node)
        {
            if (node.IsDeserialize)
            {
                queue.AddOnly(node);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        private void run()
        {
            ThreadId = System.Environment.CurrentManagedThreadId;
            do
            {
                WaitHandle.Wait();
                if (Server.IsDisposed || WaitHandle.Reserved != 0) return;
                AutoCSer.Threading.ThreadYield.YieldOnly();
                //bool isThreadAffinity = AutoCSer.Threading.Thread.BeginThreadAffinity();
                var value = queue.GetQueue();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            runSeconds = SecondTimer.CurrentSeconds;
                            currentTask = value;
                            value.RunTask(ref value);
                            runSeconds = long.MaxValue;
                            if (Server.IsDisposed || WaitHandle.Reserved != 0) return;
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        runSeconds = long.MaxValue;
                        currentTask.notNull().OnException(exception);
                        //Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (value != null);

                if (!object.ReferenceEquals(sendSocket, CommandServerSocket.CommandServerSocketContext)) send();

                //if (isThreadAffinity) System.Threading.Thread.EndThreadAffinity();
            }
            while (!Server.IsDisposed && WaitHandle.Reserved == 0);
        }
        /// <summary>
        /// 创建低优先级任务队列链表
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandServerCallLowPriorityQueue CreateLink()
        {
            return new CommandServerCallLowPriorityQueue(this);
        }
        /// <summary>
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
        /// <summary>
        /// 发送数据时间戳
        /// </summary>
        private long sendTimestamp;
        /// <summary>
        /// 发送数据
        /// </summary>
        private void send()
        {
            if (!object.ReferenceEquals(outputHead, outputEnd)) sendSocket.CheckPush(outputHead, outputEnd);
            else sendSocket.TryPush(outputHead);
            sendSocket = CommandServerSocket.CommandServerSocketContext;
            outputHead = outputEnd = CommandServerConfig.NullServerOutput;
            sendTimestamp = Stopwatch.GetTimestamp() + AutoCSer.Date.TimestampByMilliseconds;
        }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="output">服务端输出信息</param>
        /// <returns>是否成功加入输出队列</returns>
        internal void Send(CommandServerSocket socket, ServerOutput output)
        {
            if (object.ReferenceEquals(socket, sendSocket))
            {
                if (!object.ReferenceEquals(outputHead, CommandServerConfig.NullServerOutput)) output.LinkNext = outputHead;
                else outputEnd = output;
                outputHead = output;
                if (Stopwatch.GetTimestamp() > sendTimestamp) send();
            }
            else
            {
                if (!object.ReferenceEquals(sendSocket, CommandServerSocket.CommandServerSocketContext))
                {
                    if (!object.ReferenceEquals(outputHead, outputEnd)) sendSocket.CheckPush(outputHead, outputEnd);
                    else sendSocket.TryPush(outputHead);
                }
                outputHead = outputEnd = output;
                sendSocket = socket;
                sendTimestamp = Stopwatch.GetTimestamp() + AutoCSer.Date.TimestampByMilliseconds;
            }
        }
    }
}
