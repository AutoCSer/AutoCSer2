using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Server-side call
    /// 服务端调用
    /// </summary>
    public abstract class CommandServerCall
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
        /// The session callback identifier is currently being processed
        /// 当前处理会话回调标识
        /// </summary>
        internal readonly CallbackIdentity CallbackIdentity;
        /// <summary>
        /// Empty callback
        /// </summary>
        protected CommandServerCall()
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            OfflineCount = OfflineCount.Null; 
        }
        /// <summary>
        /// Server-side call
        /// 服务端调用
        /// </summary>
        /// <param name="socket"></param>
        protected CommandServerCall(CommandServerSocket socket)
        {
            Socket = socket;
            this.OfflineCount = socket.OfflineCount;
            CallbackIdentity = socket.CallbackIdentity;
        }
        /// <summary>
        /// Server-side call
        /// 服务端调用
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
        /// Server-side call
        /// 服务端调用
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
        /// Server-side call
        /// 服务端调用
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
        /// Offline counting processing
        /// 下线计数处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void checkOfflineCount()
        {
            if (OfflineCount.Get() == 0) Socket.Server.DecrementOfflineCount();
        }
        /// <summary>
        /// Send the successful status
        /// 发送成功状态
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send()
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, CommandClientReturnTypeEnum.Success);
        }
        /// <summary>
        /// Send the successful status
        /// 发送成功状态
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCall task)
        {
            return task.send();
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
        private bool send<T>(ServerInterfaceMethod method, T outputParameter)
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, method, new ServerReturnValue<T>(outputParameter));
        }
        /// <summary>
        /// Send data
        /// </summary>
        /// <typeparam name="T">Output data type</typeparam>
        /// <param name="task"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send<T>(CommandServerCall task, ServerInterfaceMethod method, T outputParameter)
        {
            return task.send(method, outputParameter);
        }
        /// <summary>
        /// The verification method sends data
        /// 验证方法发送数据
        /// </summary>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool send(ServerInterfaceMethod method, CommandServerVerifyStateEnum outputParameter)
        {
            checkOfflineCount();
            return Socket.Send(CallbackIdentity, method, outputParameter);
        }
        /// <summary>
        /// The verification method sends data
        /// 验证方法发送数据
        /// </summary>
        /// <param name="task"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool Send(CommandServerCall task, ServerInterfaceMethod method, CommandServerVerifyStateEnum outputParameter)
        {
            return task.send(method, outputParameter);
        }
        /// <summary>
        /// Remove the asynchronous keep callback
        /// 移除异步保持回调
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveKeepCallback(Exception exception)
        {
            Socket.RemoveKeepCallback(CallbackIdentity, exception);
        }
        /// <summary>
        /// Get the exception of the Task.Run
        /// 获取 Task.Run 任务异常
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
        /// Get the command service socket
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
        /// Get the custom return value type
        /// 获取自定义返回值类型
        /// </summary>
        /// <param name="customReturnType">Between 0 and 0x7f
        /// 0-0x7f 之间</param>
        /// <returns>The custom return value type
        /// 自定义返回值类型</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientReturnTypeEnum GetCustom(byte customReturnType)
        {
            return (CommandClientReturnTypeEnum)(byte)(customReturnType | 0x80);
        }
    }
}
