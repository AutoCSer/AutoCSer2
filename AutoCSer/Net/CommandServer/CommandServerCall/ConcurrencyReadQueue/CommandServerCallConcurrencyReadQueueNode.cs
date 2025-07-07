using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Synchronous queue nodes that support parallel reading on the server side
    /// 服务端支持并行读的同步队列节点
    /// </summary>
    public abstract class CommandServerCallConcurrencyReadQueueNode : ReadWriteQueueNode
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
        /// Synchronous queue nodes that support parallel reading on the server side
        /// 服务端支持并行读的同步队列节点
        /// </summary>
        protected CommandServerCallConcurrencyReadQueueNode() : base(ServerMethodTypeEnum.ReadWriteQueue)
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            OfflineCount = OfflineCount.Null;
        }
        /// <summary>
        /// Synchronous queue nodes that support parallel reading on the server side
        /// 服务端支持并行读的同步队列节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="methodType"></param>
        internal CommandServerCallConcurrencyReadQueueNode(CommandServerSocket socket, ServerMethodTypeEnum methodType) : base(methodType)
        {
            this.Socket = socket;
            this.OfflineCount = socket.OfflineCount;
            CallbackIdentity = socket.CallbackIdentity;
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
        internal static void CheckOfflineCount(CommandServerCallConcurrencyReadQueueNode node)
        {
            node.checkOfflineCount();
        }
        /// <summary>
        /// Queue task execution exception
        /// 队列任务执行异常
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="exception"></param>
        internal override void OnException(CommandServerCallConcurrencyReadWriteQueue queue, Exception exception)
        {
            switch (MethodType)
            {
                case ServerMethodTypeEnum.ConcurrencyReadQueue:
                case ServerMethodTypeEnum.CallbackConcurrencyReadQueue:
                    Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    checkOfflineCount();
                    break;
                case ServerMethodTypeEnum.SendOnlyConcurrencyReadQueue:
                    checkOfflineCount();
                    break;
                case ServerMethodTypeEnum.KeepCallbackConcurrencyReadQueue:
                case ServerMethodTypeEnum.KeepCallbackCountConcurrencyReadQueue:
                    Socket.CancelKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    checkOfflineCount();
                    break;
            }
        }
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SocketIsClose(CommandServerCallConcurrencyReadQueueNode node)
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
        internal static void SetIsDeserialize(CommandServerCallConcurrencyReadQueueNode node, bool isDeserialize)
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
        internal static CommandServerSocket GetSocket(CommandServerCallConcurrencyReadQueueNode node)
        {
            return node.Socket;
        }
        /// <summary>
        /// Close the short connection
        /// 关闭短连接
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void CloseShortLink(CommandServerCallConcurrencyReadQueueNode node)
        {
            node.Socket.CloseShortLink();
        }
        /// <summary>
        /// Send the return type successfully
        /// 发送成功返回类型
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send()
        {
            return Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        }
        /// <summary>
        /// Send the return type successfully
        /// 发送成功返回类型
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCallConcurrencyReadQueueNode node)
        {
            return node.send();
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send<T>(ServerInterfaceMethod method, ref T outputParameter)
             where T : struct
        {
            return Socket.Send(CallbackIdentity, method, ref outputParameter);
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="node"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send<T>(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            return node.send(method, ref outputParameter);
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        private bool sendReturnValue<T>(ServerInterfaceMethod method, T outputParameter)
        {
            return Socket.Send(CallbackIdentity, method, new ServerReturnValue<T>(outputParameter));
        }
        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="node"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendReturnValue<T>(CommandServerCallConcurrencyReadQueueNode node, ServerInterfaceMethod method, T outputParameter)
        {
            return node.sendReturnValue(method, outputParameter);
        }
        /// <summary>
        /// Set the status of the verification result of the command service
        /// 设置命令服务验证结果状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="verifyState"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetVerifyState(CommandServerCallConcurrencyReadQueueNode node, CommandServerVerifyStateEnum verifyState)
        {
            node.Socket.SetVerifyState(verifyState);
        }
    }
}
