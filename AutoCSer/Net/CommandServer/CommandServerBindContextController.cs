using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Socket context binding service instance (one instance for each connection)
    /// 套接字上下文绑定服务实例（每个连接一个实例）
    /// </summary>
    public abstract class CommandServerBindContextController
    {
        /// <summary>
        /// The socket of the currently executing task
        /// 当前执行任务套接字
        /// </summary>
        public CommandServerSocket Socket { get; private set; }
        /// <summary>
        /// Command service controller
        /// 命令服务控制器
        /// </summary>
        public CommandServerController Controller { get; private set; }
        /// <summary>
        /// Has the socket for the current execution task been set
        /// 是否已经设置当前执行任务套接字
        /// </summary>
        public bool IsContext
        {
            get { return !object.ReferenceEquals(Controller, CommandListener.Null.Controller); }
        }
        /// <summary>
        /// Socket context binding service instance (one instance for each connection)
        /// 套接字上下文绑定服务实例（每个连接一个实例）
        /// </summary>
        public CommandServerBindContextController()
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            Controller = CommandListener.Null.Controller;
        }
        /// <summary>
        /// Set the socket of the currently executing task
        /// 设置当前执行任务套接字
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controller"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(CommandServerSocket socket, CommandServerController controller)
        {
            Socket = socket;
            Controller = controller;
        }
    }
}
