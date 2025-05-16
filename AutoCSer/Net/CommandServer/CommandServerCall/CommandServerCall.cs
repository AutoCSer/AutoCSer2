using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端调用
    /// </summary>
    public abstract class CommandServerCall
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
        /// 空回调
        /// </summary>
        protected CommandServerCall()
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            OfflineCount = OfflineCount.Null; 
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        protected CommandServerCall(CommandServerSocket socket)
        {
            Socket = socket;
            this.OfflineCount = socket.OfflineCount;
            CallbackIdentity = socket.CallbackIdentity;
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="offlineCount"></param>
        protected CommandServerCall(CommandServerSocket socket, OfflineCount offlineCount)
        {
            Socket = socket;
            this.OfflineCount = offlineCount;
            CallbackIdentity = socket.CallbackIdentity;
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="callbackIdentity"></param>
        internal CommandServerCall(CommandServerSocket socket, CallbackIdentity callbackIdentity)
        {
            Socket = socket;
            this.OfflineCount = socket.OfflineCount;
            CallbackIdentity = callbackIdentity;
        }
        /// <summary>
        /// TCP 服务器端异步回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="callbackIdentity"></param>
        /// <param name="offlineCount"></param>
        internal CommandServerCall(CommandServerSocket socket, CallbackIdentity callbackIdentity, OfflineCount offlineCount)
        {
            Socket = socket;
            this.OfflineCount = offlineCount;
            CallbackIdentity = callbackIdentity;
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
        /// 发送成功
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send()
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        }
        /// <summary>
        /// 发送成功
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCall task)
        {
            return task.send();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send<T>(ServerInterfaceMethod method, T outputParameter)
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, method, new ServerReturnValue<T>(outputParameter));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T">输出数据类型</typeparam>
        /// <param name="task"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send<T>(CommandServerCall task, ServerInterfaceMethod method, T outputParameter)
        {
            return task.send(method, outputParameter);
        }
        /// <summary>
        /// 验证函数发送数据
        /// </summary>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send(ServerInterfaceMethod method, CommandServerVerifyStateEnum outputParameter)
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, method, outputParameter);
        }
        /// <summary>
        /// 验证函数发送数据
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method">服务端输出信息</param>
        /// <param name="outputParameter">返回值</param>
        /// <returns>是否成功加入输出队列</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCall task, ServerInterfaceMethod method, CommandServerVerifyStateEnum outputParameter)
        {
            return task.send(method, outputParameter);
        }
        /// <summary>
        /// 移除 TCP 服务器端异步保持调用
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveKeepCallback(Exception exception)
        {
            Socket.RemoveKeepCallback(CallbackIdentity, exception);
        }
        /// <summary>
        /// Task.Run 获取任务异常
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="exception"></param>
        internal void RunTaskException(ServerMethodTypeEnum methodType, Exception exception)
        {
            checkOfflineCount();
            switch (methodType)
            {
                case ServerMethodTypeEnum.SendOnlyTask:
                case ServerMethodTypeEnum.CallbackTask:
                    break;
                case ServerMethodTypeEnum.KeepCallbackTask:
                case ServerMethodTypeEnum.KeepCallbackCountTask:
                case ServerMethodTypeEnum.EnumerableKeepCallbackCountTask:
#if NetStandard21
                case ServerMethodTypeEnum.AsyncEnumerableTask:
#endif
                    RemoveKeepCallback(exception);
                    return;
                default:
                    Socket.SendLog(CallbackIdentity, CommandClientReturnTypeEnum.ServerException, exception);
                    return;
            }
            Socket.Server.Config.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception);
        }

        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        /// <param name="serverCall"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandServerSocket GetSocket(CommandServerCall serverCall)
        {
            return serverCall.Socket;
        }

        /// <summary>
        /// 获取自定义返回值类型
        /// </summary>
        /// <param name="customReturnType">0-0x7f 之间</param>
        /// <returns>自定义返回值类型</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientReturnTypeEnum GetCustom(byte customReturnType)
        {
            return (CommandClientReturnTypeEnum)(byte)(customReturnType | 0x80);
        }
    }
}
