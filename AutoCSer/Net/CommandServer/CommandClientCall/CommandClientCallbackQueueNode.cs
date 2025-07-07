using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Client queue callback task node
    /// 客户端队列回调任务节点
    /// </summary>
    public sealed class CommandClientCallbackQueueNode  : CommandClientCallQueueNode
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue, CommandClientCallQueue> Callback;
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        internal CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        internal string? ErrorMessage;
#else
        internal string ErrorMessage;
#endif
        /// <summary>
        /// Client queue callback task node
        /// 客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="returnType"></param>
        internal CommandClientCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue> callback, CommandClientReturnTypeEnum returnType)
        {
            Callback = callback;
            ReturnType = returnType;
        }
        /// <summary>
        /// Client queue callback task node
        /// 客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            Callback(ReturnType, queue);
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue> value) { return new CommandClientCallbackQueueNode(value); }
        /// <summary>
        /// Get the client queue callback task node
        /// 获取客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientCallbackQueueNode Get(Action<CommandClientReturnValue, CommandClientCallQueue> callback)
        {
            return new CommandClientCallbackQueueNode(callback);
        }
        /// <summary>
        /// Successful callback, ignore error return
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action callback;
            /// <summary>
            /// Successful callback, ignore error return
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal SuccessCallback(Action callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            internal void Callback(CommandClientReturnValue returnValue, CommandClientCallQueue queue)
            {
                if (returnValue.IsSuccess) callback();
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode(Action value) { return new CommandClientCallbackQueueNode(new SuccessCallback(value).Callback); }
        /// <summary>
        /// Successful callback, ignore error return
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallbackQueue
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action<CommandClientCallQueue> callback;
            /// <summary>
            /// Successful callback, ignore error return
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal SuccessCallbackQueue(Action<CommandClientCallQueue> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            internal void Callback(CommandClientReturnValue returnValue, CommandClientCallQueue queue)
            {
                if (returnValue.IsSuccess) callback(queue);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode(Action<CommandClientCallQueue> value) { return new CommandClientCallbackQueueNode(new SuccessCallbackQueue(value).Callback); }
        /// <summary>
        /// The callback of the return type of the call
        /// 调用返回类型回调
        /// </summary>
        internal sealed class ReturnTypeCallback
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action<CommandClientReturnValue> callback;
            /// <summary>
            /// Successful callback, ignore error return
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal ReturnTypeCallback(Action<CommandClientReturnValue> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            internal void Callback(CommandClientReturnValue returnValue, CommandClientCallQueue queue)
            {
                callback(returnValue);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode(Action<CommandClientReturnValue> value) { return new CommandClientCallbackQueueNode(new ReturnTypeCallback(value).Callback); }
    }
    /// <summary>
    /// Client queue callback task node
    /// 客户端队列回调任务节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandClientCallbackQueueNode<T> : CommandClientCallQueueNode
    {
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue<T>, CommandClientCallQueue> Callback;
        /// <summary>
        /// Return value
        /// </summary>
        internal CommandClientReturnValue<T> ReturnValue;
        /// <summary>
        /// Client queue callback task node
        /// 客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// Client queue callback task node
        /// 客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">Error message</param>
        internal CommandClientCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback, CommandClientReturnTypeEnum returnType, string errorMessage)
        {
            Callback = callback;
            ReturnValue = new CommandClientReturnValue<T>(returnType, errorMessage);
        }
        /// <summary>
        /// Client queue callback task node
        /// 客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="returnValue"></param>
        internal CommandClientCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback, T returnValue)
        {
            Callback = callback;
            ReturnValue = returnValue;
        }
        /// <summary>
        /// Client queue callback task node
        /// 客户端队列回调任务节点
        /// </summary>
        /// <param name="callback">Success value callback delegate
        /// 成功值回调委托</param>
        /// <param name="errorCallback">Error return value type callback delegate
        /// 错误成功值类型回调委托</param>
        public CommandClientCallbackQueueNode(Action<T, CommandClientCallQueue> callback, Action<CommandClientReturnTypeEnum, CommandClientCallQueue> errorCallback)
        {
            Callback = new ErrorCallback(callback, errorCallback).Callback;
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            Callback(ReturnValue, queue);
        }
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        internal sealed class ErrorCallback
        {
            /// <summary>
            /// Success value callback delegate
            /// 成功值回调委托
            /// </summary>
            private readonly Action<T, CommandClientCallQueue> callback;
            /// <summary>
            /// Error return value type callback delegate
            /// 错误成功值类型回调委托
            /// </summary>
            private readonly Action<CommandClientReturnTypeEnum, CommandClientCallQueue> errorCallback;
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            /// <param name="callback">Success value callback delegate
            /// 成功值回调委托</param>
            /// <param name="errorCallback">Error return value type callback delegate
            /// 错误成功值类型回调委托</param>
            internal ErrorCallback(Action<T, CommandClientCallQueue> callback, Action<CommandClientReturnTypeEnum, CommandClientCallQueue> errorCallback)
            {
                this.callback = callback;
                this.errorCallback = errorCallback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue, CommandClientCallQueue queue)
            {
                if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success) callback(returnValue.Value, queue);
                else errorCallback(returnValue.ReturnType, queue);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode<T>(Action<CommandClientReturnValue<T>, CommandClientCallQueue> value) { return new CommandClientCallbackQueueNode<T>(value); }
        /// <summary>
        /// Get the client queue callback task node
        /// 获取客户端队列回调任务节点
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientCallbackQueueNode<T> Get(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback)
        {
            return new CommandClientCallbackQueueNode<T>(callback);
        }
        /// <summary>
        /// Successful callback, ignore error return
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action<T> callback;
            /// <summary>
            /// Successful callback, ignore error return
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal SuccessCallback(Action<T> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue, CommandClientCallQueue queue)
            {
                if (returnValue.ReturnType == CommandClientReturnTypeEnum.Success) callback(returnValue.Value);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode<T>(Action<T> value) { return new CommandClientCallbackQueueNode<T>(new SuccessCallback(value).Callback); }
        /// <summary>
        /// Return value callback
        /// 返回值回调
        /// </summary>
        internal sealed class ReturnValueCallback
        {
            /// <summary>
            /// The client callback delegate
            /// 客户端回调委托
            /// </summary>
            private readonly Action<CommandClientReturnValue<T>> callback;
            /// <summary>
            /// Return value callback
            /// 返回值回调
            /// </summary>
            /// <param name="callback">The client callback delegate
            /// 客户端回调委托</param>
            internal ReturnValueCallback(Action<CommandClientReturnValue<T>> callback)
            {
                this.callback = callback;
            }
            /// <summary>
            /// Client callback
            /// 客户端回调
            /// </summary>
            /// <param name="returnValue"></param>
            /// <param name="queue"></param>
            internal void Callback(CommandClientReturnValue<T> returnValue, CommandClientCallQueue queue)
            {
                callback(returnValue);
            }
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CommandClientCallbackQueueNode<T>(Action<CommandClientReturnValue<T>> value) { return new CommandClientCallbackQueueNode<T>(new ReturnValueCallback(value).Callback); }
    }
}
