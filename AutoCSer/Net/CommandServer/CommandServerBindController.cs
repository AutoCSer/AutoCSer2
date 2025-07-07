using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Bind the command service controller
    /// 绑定命令服务控制器
    /// </summary>
    public abstract class CommandServerBindController : ICommandServerBindController
    {
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
        /// Bind the command service controller
        /// 绑定命令服务控制器
        /// </summary>
        public CommandServerBindController()
        {
            Controller = CommandListener.Null.Controller;
        }
        /// <summary>
        /// Bind the command service controller
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void ICommandServerBindController.Bind(CommandServerController controller)
        {
            Controller = controller;
        }
    }
}
