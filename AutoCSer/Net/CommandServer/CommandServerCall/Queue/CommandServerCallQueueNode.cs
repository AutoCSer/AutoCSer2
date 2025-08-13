using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Server-side queue task nodes
    /// 服务端队列任务节点
    /// </summary>
    public abstract class CommandServerCallQueueNode : QueueTaskNode
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
        internal OfflineCount OfflineCount;
        /// <summary>
        /// Current session callback identity
        /// 当前会话回调标识
        /// </summary>
        internal readonly CallbackIdentity CallbackIdentity;
        /// <summary>
        /// Server-side method call types
        /// 服务端方法调用类型
        /// </summary>
        private readonly ServerMethodTypeEnum methodType;
        /// <summary>
        /// Whether the parameters have been deserialized successfully
        /// 参数是否反序列化成功
        /// </summary>
        internal bool IsDeserialize;
        /// <summary>
        /// Server-side queue task nodes
        /// 服务端队列任务节点
        /// </summary>
        protected CommandServerCallQueueNode()
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            OfflineCount = OfflineCount.Null;
        }
        /// <summary>
        /// Server-side queue task nodes
        /// 服务端队列任务节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="methodType"></param>
        internal CommandServerCallQueueNode(CommandServerSocket socket, ServerMethodTypeEnum methodType)
        {
            this.Socket = socket;
            this.OfflineCount = socket.OfflineCount;
            CallbackIdentity = socket.CallbackIdentity;
            this.methodType = methodType;
            //#if DEBUG
            //            if (offlineCount == null)
            //            {
            //                Console.WriteLine("ERROR");
            //            }
            //#endif
        }
        ///// <summary>
        ///// 执行任务
        ///// </summary>
        //internal override void CheckRunTask()
        //{
        //    if (!Socket.IsClose)
        //    {
        //        RunTask();
        //        checkOfflineCount();
        //    }
        //}
        /// <summary>
        /// Server-side queue timeout notification
        /// 服务端队列超时通知
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        internal override Task OnTimeout(CommandServerCallQueue queue, long seconds)
        {
            if (Socket != null) return Socket.Server.Config.OnQueueTimeout(Socket, queue, seconds);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkOfflineCount()
        {
            if (OfflineCount.Get() == 0) Socket.Server.DecrementOfflineCount();
        }
        /// <summary>
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckOfflineCount(CommandServerCallQueueNode node)
        {
            node.checkOfflineCount();
        }
        /// <summary>
        /// Queue task execution exception
        /// 队列任务执行异常
        /// </summary>
        /// <param name="exception"></param>
        internal override void OnException(Exception exception)
        {
            switch (methodType)
            {
                case ServerMethodTypeEnum.Queue:
                case ServerMethodTypeEnum.CallbackQueue:
                    Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    checkOfflineCount();
                    break;
                case ServerMethodTypeEnum.SendOnlyQueue:
                    checkOfflineCount();
                    break;
                case ServerMethodTypeEnum.KeepCallbackQueue:
                case ServerMethodTypeEnum.KeepCallbackCountQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackQueue:
                case ServerMethodTypeEnum.TwoStage‌CallbackCountQueue:
                    Socket.CancelKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    checkOfflineCount();
                    break;
            }
        }
        ///// <summary>
        ///// 清除下线计数对象
        ///// </summary>
        ///// <param name="node"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static void SetOfflineCountNull(CommandServerCallQueueNode node)
        //{
        //    node.OfflineCount = OfflineCount.Null;
        //}
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SocketIsClose(CommandServerCallQueueNode node)
        {
            return node.Socket.IsClose;
        }
        /// <summary>
        /// Set whether the parameter deserialization is successful
        /// 设置参数反序列化是否成功
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isDeserialize"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetIsDeserialize(CommandServerCallQueueNode node, bool isDeserialize)
        {
            node.IsDeserialize = isDeserialize;
        }

        /// <summary>
        /// Get the command service socket
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(CommandServerCallQueueNode node)
        {
            return node.Socket;
        }
        /// <summary>
        /// Close the short connection
        /// 关闭短连接
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CloseShortLink(CommandServerCallQueueNode node)
        {
            node.Socket.CloseShortLink();
        }
        /// <summary>
        /// Send the return type successfully
        /// 发送成功返回类型
        /// </summary>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCallQueueNode node, CommandServerCallQueue queue)
        {
            queue.Send(node.Socket, new ServerOutputReturnType(node.CallbackIdentity, CommandClientReturnTypeEnum.Success));
            return true;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="queue"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void send<T>(CommandServerCallQueue queue, ServerInterfaceMethod method, ref T outputParameter)
             where T : struct
        {
            queue.Send(Socket, Socket.GetOutput(CallbackIdentity, method, ref outputParameter));
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send<T>(CommandServerCallQueueNode node, CommandServerCallQueue queue, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            node.send(queue, method, ref outputParameter);
            return true;
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="queue"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        private void sendReturnValue<T>(CommandServerCallQueue queue, ServerInterfaceMethod method, T outputParameter)
        {
            ServerReturnValue<T> returnValue = new ServerReturnValue<T>(outputParameter);
            queue.Send(Socket, Socket.GetOutput(CallbackIdentity, method, ref returnValue));
        }
        /// <summary>
        /// The server queue task sends data
        /// 服务端队列任务发送数据
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendReturnValue<T>(CommandServerCallQueueNode node, CommandServerCallQueue queue, ServerInterfaceMethod method, T outputParameter)
        {
            node.sendReturnValue(queue, method, outputParameter);
            return true;
        }
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="verifyState"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetVerifyState(CommandServerCallQueueNode node, CommandServerVerifyStateEnum verifyState)
        {
            node.Socket.SetVerifyState(verifyState);
        }
    }
}
