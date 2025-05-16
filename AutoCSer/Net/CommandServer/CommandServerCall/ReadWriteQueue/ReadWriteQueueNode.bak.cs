using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端同步读写队列节点
    /// </summary>
    public abstract class ReadWriteQueueNode : AutoCSer.Threading.Link<ReadWriteQueueNode>
    {
        /// <summary>
        /// 服务端同步读写队列
        /// </summary>
        private CommandServerCallReadQueue queue;
        /// <summary>
        /// 同步读写队列节点类型
        /// </summary>
        internal ReadWriteNodeTypeEnum Type;
        /// <summary>
        /// 服务端方法调用类型
        /// </summary>
        internal readonly ServerMethodTypeEnum MethodType;
        /// <summary>
        /// 参数是否反序列化成功
        /// </summary>
        internal bool IsDeserialize;
        /// <summary>
        /// 服务端同步读写队列节点
        /// </summary>
        internal ReadWriteQueueNode(ServerMethodTypeEnum methodType)
        {
            this.MethodType = methodType;
#if NetStandard21
            queue = CommandListener.Null.CallReadWriteQueue;
#endif
        }
        /// <summary>
        /// 设置服务端同步读写队列
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="type"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(CommandServerCallReadQueue queue, ReadWriteNodeTypeEnum type)
        {
            this.queue = queue;
            this.Type = type;
        }
        /// <summary>
        /// 线程执行任务
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RunThread()
        {
            for (var nextNode = runTask(); nextNode != null; nextNode = nextNode.runTask()) ;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ReadWriteQueueNode? GetNextRunThread()
        {
            var next = LinkNext;
            AutoCSer.Threading.ThreadPool.Tiny.FastStart(RunThread);
            LinkNext = null;
            return next;
        }
        /// <summary>
        /// 同步执行任务并返回下一个任务
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private ReadWriteQueueNode? runTask()
#else
        private CommandServerReadWriteQueueNode runTask()
#endif
        {
            try
            {
                RunTask();
            }
            catch (Exception exception)
            {
                OnException(exception);
            }
            return queue.GetNextNode(Type);
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void RunTask();
        /// <summary>
        /// 队列任务执行异常
        /// </summary>
        /// <param name="exception"></param>
        internal virtual void OnException(Exception exception)
        {
            queue.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }
    }
}
