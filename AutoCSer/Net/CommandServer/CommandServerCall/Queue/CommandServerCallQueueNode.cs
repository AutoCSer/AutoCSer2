using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端执行队列任务
    /// </summary>
    public abstract class CommandServerCallQueueNode : QueueTaskNode
    {
        /// <summary>
        /// 命令服务套接字
        /// </summary>
        internal readonly CommandServerSocket Socket;
        /// <summary>
        /// 服务端下线计数对象
        /// </summary>
        internal OfflineCount OfflineCount;
        /// <summary>
        /// 当前处理会话标识
        /// </summary>
        internal readonly CallbackIdentity CallbackIdentity;
        /// <summary>
        /// 服务端方法调用类型
        /// </summary>
        private readonly ServerMethodTypeEnum methodType;
        /// <summary>
        /// 参数是否反序列化成功
        /// </summary>
        internal bool IsDeserialize;
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        protected CommandServerCallQueueNode()
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            OfflineCount = OfflineCount.Null;
        }
        /// <summary>
        /// TCP 服务器端异步回调
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
        /// 下线计数对象检查
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkOfflineCount()
        {
            if (OfflineCount.Get() == 0) Socket.Server.DecrementOfflineCount();
        }
        /// <summary>
        /// 下线计数对象检查
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CheckOfflineCount(CommandServerCallQueueNode node)
        {
            node.checkOfflineCount();
        }
        /// <summary>
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
        /// 设置参数是否反序列化成功
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
        /// 发送成功返回值类型
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
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="queue"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void send<T>(CommandServerCallQueue queue, ServerInterfaceMethod method, ref T outputParameter)
             where T : struct
        {
            queue.Send(Socket, Socket.GetOutput(CallbackIdentity, method, ref outputParameter));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send<T>(CommandServerCallQueueNode node, CommandServerCallQueue queue, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            node.send(queue, method, ref outputParameter);
            return true;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="queue"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        private void sendReturnValue<T>(CommandServerCallQueue queue, ServerInterfaceMethod method, T outputParameter)
        {
            ServerReturnValue<T> returnValue = new ServerReturnValue<T>(outputParameter);
            queue.Send(Socket, Socket.GetOutput(CallbackIdentity, method, ref returnValue));
        }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendReturnValue<T>(CommandServerCallQueueNode node, CommandServerCallQueue queue, ServerInterfaceMethod method, T outputParameter)
        {
            node.sendReturnValue(queue, method, outputParameter);
            return true;
        }
        /// <summary>
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
