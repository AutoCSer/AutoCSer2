using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端同步读写队列节点
    /// </summary>
    public abstract class CommandServerCallReadWriteQueueNode : ReadWriteQueueNode
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
        /// 服务端同步读写队列节点
        /// </summary>
        protected CommandServerCallReadWriteQueueNode() : base(ServerMethodTypeEnum.ReadWriteQueue)
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            OfflineCount = OfflineCount.Null;
        }
        /// <summary>
        /// 服务端同步读写队列节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="methodType"></param>
        internal CommandServerCallReadWriteQueueNode(CommandServerSocket socket, ServerMethodTypeEnum methodType) : base(methodType)
        {
            this.Socket = socket;
            this.OfflineCount = socket.OfflineCount;
            CallbackIdentity = socket.CallbackIdentity;
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
        internal static void CheckOfflineCount(CommandServerCallReadWriteQueueNode node)
        {
            node.checkOfflineCount();
        }
        /// <summary>
        /// 队列任务执行异常
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="exception"></param>
        internal override void OnException(CommandServerCallWriteQueue queue, Exception exception)
        {
            switch (MethodType)
            {
                case ServerMethodTypeEnum.ReadWriteQueue:
                case ServerMethodTypeEnum.CallbackReadWriteQueue:
                    Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    checkOfflineCount();
                    break;
                case ServerMethodTypeEnum.SendOnlyReadWriteQueue:
                    checkOfflineCount();
                    break;
                case ServerMethodTypeEnum.KeepCallbackReadWriteQueue:
                case ServerMethodTypeEnum.KeepCallbackCountReadWriteQueue:
                    Socket.CancelKeepCallback(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    checkOfflineCount();
                    break;
            }
        }
        /// <summary>
        /// 判断套接字是否已经关闭
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SocketIsClose(CommandServerCallReadWriteQueueNode node)
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
        internal static void SetIsDeserialize(CommandServerCallReadWriteQueueNode node, bool isDeserialize)
        {
            node.IsDeserialize = isDeserialize;
        }

        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(CommandServerCallReadWriteQueueNode node)
        {
            return node.Socket;
        }
        /// <summary>
        /// 发送成功返回值类型
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send()
        {
            return Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        }
        /// <summary>
        /// 发送成功返回值类型
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCallReadWriteQueueNode node)
        {
            return node.send();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send<T>(ServerInterfaceMethod method, ref T outputParameter)
             where T : struct
        {
            return Socket.Send(CallbackIdentity, method, ref outputParameter);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="node"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send<T>(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method, ref T outputParameter)
            where T : struct
        {
            return node.send(method, ref outputParameter);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        private bool sendReturnValue<T>(ServerInterfaceMethod method, T outputParameter)
        {
            return Socket.Send(CallbackIdentity, method, new ServerReturnValue<T>(outputParameter));
        }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="node"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool SendReturnValue<T>(CommandServerCallReadWriteQueueNode node, ServerInterfaceMethod method, T outputParameter)
        {
            return node.sendReturnValue(method, outputParameter);
        }
        /// <summary>
        /// 设置命令服务验证结果状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="verifyState"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetVerifyState(CommandServerCallReadWriteQueueNode node, CommandServerVerifyStateEnum verifyState)
        {
            node.Socket.SetVerifyState(verifyState);
        }
    }
}
