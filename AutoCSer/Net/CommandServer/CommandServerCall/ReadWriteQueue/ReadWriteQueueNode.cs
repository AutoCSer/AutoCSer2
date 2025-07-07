using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The server synchronously reads and writes the queue nodes
    /// 服务端同步读写队列节点
    /// </summary>
    public abstract class ReadWriteQueueNode : AutoCSer.Threading.Link<ReadWriteQueueNode>
    {
        /// <summary>
        /// Synchronous read and write queue node types
        /// 同步读写队列节点类型
        /// </summary>
        public ReadWriteNodeTypeEnum Type { get; internal set; }
        /// <summary>
        /// Server-side method call types
        /// 服务端方法调用类型
        /// </summary>
        internal readonly ServerMethodTypeEnum MethodType;
        /// <summary>
        /// Whether the parameters have been deserialized successfully
        /// 参数是否反序列化成功
        /// </summary>
        internal bool IsDeserialize;
        /// <summary>
        /// Has it been added to the queue
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// 自定义节点
        /// </summary>
        protected ReadWriteQueueNode() { }
        /// <summary>
        /// 服务端同步读写队列节点
        /// </summary>
        internal ReadWriteQueueNode(ServerMethodTypeEnum methodType)
        {
            this.MethodType = methodType;
        }
        /// <summary>
        /// 自定义节点设置服务端同步读写队列
        /// </summary>
        /// <param name="type"></param>
        internal bool CheckSet(ReadWriteNodeTypeEnum type)
        {
            if (System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0)
            {
                this.Type = type;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        public abstract void RunTask();
        /// <summary>
        /// Queue task execution exception
        /// 队列任务执行异常
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="exception"></param>
        internal virtual void OnException(CommandServerCallWriteQueue queue, Exception exception)
        {
            queue.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// Queue task execution exception
        /// 队列任务执行异常
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="exception"></param>
        internal virtual void OnException(CommandServerCallConcurrencyReadWriteQueue queue, Exception exception)
        {
            queue.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
        /// <summary>
        /// Server-side queue timeout notification
        /// 服务端队列超时通知
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        internal virtual Task OnTimeout(AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue, long seconds)
        {
            if (queue.Controller == null) AutoCSer.LogHelper.DebugIgnoreException($"服务并发读队列任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            else AutoCSer.LogHelper.DebugIgnoreException($"控制器并发读队列 {queue.Controller.ControllerName} 任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            return AutoCSer.Common.CompletedTask;
        }
    }
}
