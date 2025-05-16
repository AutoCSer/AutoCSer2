using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 套接字上下文绑定服务端实例（每个连接一个实例）
    /// </summary>
    public abstract class CommandServerBindContextController
    {
        /// <summary>
        /// 当前执行任务套接字
        /// </summary>
        public CommandServerSocket Socket { get; private set; }
        /// <summary>
        /// 命令服务控制器
        /// </summary>
        public CommandServerController Controller { get; private set; }
        /// <summary>
        /// 是否已经设置设置当前执行任务套接字
        /// </summary>
        public bool IsContext
        {
            get { return !object.ReferenceEquals(Controller, CommandListener.Null.Controller); }
        }
        /// <summary>
        /// 套接字上下文绑定服务端实例（每个连接一个实例）
        /// </summary>
        public CommandServerBindContextController()
        {
            Socket = CommandServerSocket.CommandServerSocketContext;
            Controller = CommandListener.Null.Controller;
        }
        /// <summary>
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
