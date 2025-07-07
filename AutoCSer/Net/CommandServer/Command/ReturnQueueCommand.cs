using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// The return value queue command (await AutoCSer.Net.CommandClientReturnValue)
    /// 返回值队列命令（await AutoCSer.Net.CommandClientReturnValue）
    /// </summary>
    public abstract class ReturnQueueCommand : ReturnCommand
    {
        /// <summary>
        /// The return value queue command
        /// 返回值队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
    }
    /// <summary>
    /// The return value queue command (await AutoCSer.Net.CommandClientReturnValue{T})
    /// 返回值队列命令（await AutoCSer.Net.CommandClientReturnValue{T}）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReturnQueueCommand<T> : ReturnCommand<T>
    {
        /// <summary>
        /// The return value queue command
        /// 返回值队列命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal ReturnQueueCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) { }
    }
}
