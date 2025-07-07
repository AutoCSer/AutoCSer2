using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端队列保持回调任务节点
    /// </summary>
    public sealed class CommandClientKeepCallbackQueueNode : CommandClientCallQueueNode
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> Callback;
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        private readonly KeepCallbackCommand keepCallbackCommand;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal CommandClientReturnValue ReturnValue;
        /// <summary>
        /// 客户端队列保持回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="keepCallbackCommand"></param>
        /// <param name="returnValue"></param>
        internal CommandClientKeepCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue, KeepCallbackCommand> callback, KeepCallbackCommand keepCallbackCommand, CommandClientReturnValue returnValue)
        {
            Callback = callback;
            this.keepCallbackCommand = keepCallbackCommand;
            ReturnValue = returnValue;
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            Callback(ReturnValue, queue, keepCallbackCommand);
        }
    }
    /// <summary>
    /// 客户端队列保持回调任务节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandClientKeepCallbackQueueNode<T> : CommandClientCallQueueNode
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> Callback;
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        private readonly KeepCallbackCommand keepCallbackCommand;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal CommandClientReturnValue<T> ReturnValue;
        /// <summary>
        /// 客户端队列保持回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="keepCallbackCommand"></param>
        /// <param name="returnValue"></param>
#if NetStandard21
        internal CommandClientKeepCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback, KeepCallbackCommand keepCallbackCommand, T? returnValue)
#else
        internal CommandClientKeepCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback, KeepCallbackCommand keepCallbackCommand, T returnValue)
#endif
        {
            Callback = callback;
            this.keepCallbackCommand = keepCallbackCommand;
            ReturnValue = returnValue;
        }
        /// <summary>
        /// 客户端队列保持回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="keepCallbackCommand"></param>
        /// <param name="returnType"></param>
        /// <param name="errorMessage"></param>
#if NetStandard21
        internal CommandClientKeepCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback, KeepCallbackCommand keepCallbackCommand, CommandClientReturnTypeEnum returnType, string? errorMessage)
#else
        internal CommandClientKeepCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue, KeepCallbackCommand> callback, KeepCallbackCommand keepCallbackCommand, CommandClientReturnTypeEnum returnType, string errorMessage)
#endif
        {
            Callback = callback;
            this.keepCallbackCommand = keepCallbackCommand;
            ReturnValue = new CommandClientReturnValue<T>(returnType, errorMessage);
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            Callback(ReturnValue, queue, keepCallbackCommand);
        }
    }
}
