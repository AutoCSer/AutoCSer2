using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 队列返回值命令 await CommandClientReturnValue
    /// </summary>
    public abstract class ReturnQueueCommand : ReturnCommand
    {
        /// <summary>
        /// 队列返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
    }
    /// <summary>
    /// 返回值命令 await CommandClientReturnValue{T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReturnQueueCommand<T> : ReturnCommand<T>
    {
        /// <summary>
        /// 队列返回值命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
    }
}
