using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端控制器异步调用队列
    /// </summary>
    internal sealed class CommandServerControllerCallTaskQueue : CommandServerCallTaskQueue
    {
        /// <summary>
        /// Command service controller
        /// 命令服务控制器
        /// </summary>
        private readonly CommandServerController controller;
        /// <summary>
        /// 控制器名称
        /// </summary>
        public override string KeyString { get { return controller.ControllerName; } }
        /// <summary>
        /// 默认空服务端异步调用队列
        /// </summary>
        internal CommandServerControllerCallTaskQueue()
        {
            controller = CommandListener.Null.Controller;
        }
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        /// <param name="controller"></param>
        internal CommandServerControllerCallTaskQueue(CommandServerController controller) : base(controller) 
        {
            this.controller = controller;
        }
        /// <summary>
        /// 添加到删除队列
        /// </summary>
        protected override void appendRemove()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 默认空服务端异步调用队列
        /// </summary>
        internal static readonly CommandServerCallTaskQueue Null = new CommandServerControllerCallTaskQueue();
    }
}
