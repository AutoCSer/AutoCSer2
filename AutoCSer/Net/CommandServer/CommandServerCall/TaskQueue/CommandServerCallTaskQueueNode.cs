using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    public abstract class CommandServerCallTaskQueueNode : AutoCSer.Threading.Link<CommandServerCallTaskQueueNode>
    {
        /// <summary>
        /// Command server socket
        /// 命令服务套接字
        /// </summary>
        internal readonly CommandServerSocket Socket;
        /// <summary>
        /// The server side goes offline to count the object
        /// 服务端下线计数对象
        /// </summary>
        internal readonly OfflineCount OfflineCount;
        /// <summary>
        /// Current session callback identity
        /// 当前会话回调标识
        /// </summary>
        internal readonly CallbackIdentity CallbackIdentity;
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        internal CommandServerCallTaskQueue Queue;
        /// <summary>
        /// 命令服务
        /// </summary>
        internal CommandListener Server
        {
            get
            {
                return Socket?.Server ?? Queue.Server;
            }
        }
        /// <summary>
        /// 运行任务时间
        /// </summary>
        internal long RunSeconds;
        /// <summary>
        /// Server-side method call types
        /// 服务端方法调用类型
        /// </summary>
        private readonly ServerMethodTypeEnum methodType;
        /// <summary>
        /// 是否自动取消回调
        /// </summary>
        protected bool autoCancelKeep;
        ///// <summary>
        ///// 参数是否反序列化成功
        ///// </summary>
        //internal bool IsDeserialize;
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        protected CommandServerCallTaskQueueNode() 
        {
            OfflineCount = OfflineCount.Null;
            Socket = CommandServerSocket.CommandServerSocketContext;
            Queue = CommandServerControllerCallTaskQueue.Null;
        }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="methodType"></param>
        internal CommandServerCallTaskQueueNode(CommandServerSocket socket, ServerMethodTypeEnum methodType)
        {
            Socket = socket;
            CallbackIdentity = socket.CallbackIdentity;
            OfflineCount = socket.OfflineCount;
            this.methodType = methodType;
            Queue = CommandServerControllerCallTaskQueue.Null;
        }
        /// <summary>
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkOfflineCount()
        {
            if (OfflineCount.Get() == 0) Server.DecrementOfflineCount();
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns>是否同步完成任务</returns>
        public abstract bool RunTask();
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        internal bool TryRunTask()
        {
            RunSeconds = SecondTimer.CurrentSeconds;
            try
            {
                return RunTask();
            }
            catch (Exception exception)
            {
                switch (methodType)
                {
                    case ServerMethodTypeEnum.TaskQueue:
                    case ServerMethodTypeEnum.CallbackTaskQueue:
                        Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                        checkOfflineCount();
                        break;
                    case ServerMethodTypeEnum.SendOnlyTaskQueue:
                        checkOfflineCount();
                        break;
                    case ServerMethodTypeEnum.KeepCallbackTaskQueue:
                    case ServerMethodTypeEnum.KeepCallbackCountTaskQueue:
                    case ServerMethodTypeEnum.EnumerableKeepCallbackCountTaskQueue:
#if NetStandard21
                    case ServerMethodTypeEnum.AsyncEnumerableTaskQueue:
#endif
                        Socket.CancelKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                        checkOfflineCount();
                        break;
                }
                Queue.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                return true;
            }
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        /// <returns>是否同步完成任务</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool RunTask(out CommandServerCallTaskQueueNode? next)
#else
        internal bool RunTask(out CommandServerCallTaskQueueNode next)
#endif
        {
            next = LinkNext;
            return LowPriorityRunTask();
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <returns>是否同步完成任务</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool LowPriorityRunTask()
        {
            LinkNext = null;
            if (Socket != null && Socket.IsClose) return true;
            return TryRunTask();
        }
        /// <summary>
        /// 获取运行任务时间
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal long GetRunSeconds(out CommandServerCallTaskQueueNode? next)
#else
        internal long GetRunSeconds(out CommandServerCallTaskQueueNode next)
#endif
        {
            next = LinkNext;
            LinkNext = null;
            return RunSeconds;
        }
        /// <summary>
        /// Server-side queue timeout notification
        /// 服务端队列超时通知
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        internal virtual Task OnTimeout(long seconds)
        {
            return Server.Config.OnQueueTimeout(Socket, Queue, seconds);
        }

        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal CommandServerSocket GetSocket(out CommandServerCallTaskQueue queue)
        {
            queue = Queue;
            return Socket;
        }
        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="task"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(CommandServerCallTaskQueueNode task, out CommandServerCallTaskQueue queue)
        {
            return task.GetSocket(out queue);
        }
        ///// <summary>
        ///// 输出异常信息
        ///// </summary>
        ///// <param name="exception"></param>
        //private void sendException(Exception exception)
        //{
        //    if (Socket != null)
        //    {
        //        Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
        //        Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
        //    }
        //    else AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
        //}
        ///// <summary>
        ///// 输出异常信息
        ///// </summary>
        ///// <param name="exception"></param>
        //private void sendExceptionCallback(Exception exception)
        //{
        //    if (Socket != null)
        //    {
        //        Socket.CancelKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
        //        Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
        //    }
        //    else AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
        //}

        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        protected CommandServerTaskQueueService? getTaskQueue()
#else
        protected CommandServerTaskQueueService getTaskQueue()
#endif
        {
            var service = Queue.TaskQueue;
            if (service != null) service.Socket = Socket;
            return service;
        }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static CommandServerTaskQueueService? GetTaskQueue(CommandServerCallTaskQueueNode task)
#else
        internal static CommandServerTaskQueueService GetTaskQueue(CommandServerCallTaskQueueNode task)
#endif
        {
            return task.getTaskQueue();
        }
        ///// <summary>
        ///// 设置参数是否反序列化成功
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="isDeserialize"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void SetIsDeserialize(CommandServerCallTaskQueueNode node, bool isDeserialize)
        //{
        //    node.IsDeserialize = isDeserialize;
        //}
    }
}
