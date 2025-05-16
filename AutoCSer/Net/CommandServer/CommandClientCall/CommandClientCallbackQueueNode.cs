using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端队列回调委托
    /// </summary>
    public sealed class CommandClientCallbackQueueNode  : CommandClientCallQueueNode
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue, CommandClientCallQueue> Callback;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// 错误信息
        /// </summary>
#if NetStandard21
        internal string? ErrorMessage;
#else
        internal string ErrorMessage;
#endif
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="returnType"></param>
        internal CommandClientCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue> callback, CommandClientReturnTypeEnum returnType)
        {
            Callback = callback;
            ReturnType = returnType;
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            Callback(ReturnType, queue);
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode(Action<CommandClientReturnValue, CommandClientCallQueue> value) { return new CommandClientCallbackQueueNode(value); }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientCallbackQueueNode Get(Action<CommandClientReturnValue, CommandClientCallQueue> callback)
        {
            return new CommandClientCallbackQueueNode(callback);
        }
        /// <summary>
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action callback;
            /// <summary>
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal SuccessCallback(Action callback)
            {
                this.callback = callback;
            }
            /// <summary>
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
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode(Action value) { return new CommandClientCallbackQueueNode(new SuccessCallback(value).Callback); }
        /// <summary>
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallbackQueue
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action<CommandClientCallQueue> callback;
            /// <summary>
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal SuccessCallbackQueue(Action<CommandClientCallQueue> callback)
            {
                this.callback = callback;
            }
            /// <summary>
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
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode(Action<CommandClientCallQueue> value) { return new CommandClientCallbackQueueNode(new SuccessCallbackQueue(value).Callback); }
        /// <summary>
        /// 返回值类型回调
        /// </summary>
        internal sealed class ReturnTypeCallback
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action<CommandClientReturnValue> callback;
            /// <summary>
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal ReturnTypeCallback(Action<CommandClientReturnValue> callback)
            {
                this.callback = callback;
            }
            /// <summary>
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
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode(Action<CommandClientReturnValue> value) { return new CommandClientCallbackQueueNode(new ReturnTypeCallback(value).Callback); }
    }
    /// <summary>
    /// 客户端队列回调委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandClientCallbackQueueNode<T> : CommandClientCallQueueNode
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        internal readonly Action<CommandClientReturnValue<T>, CommandClientCallQueue> Callback;
        /// <summary>
        /// 返回值
        /// </summary>
        internal CommandClientReturnValue<T> ReturnValue;
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        private CommandClientCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback)
        {
            Callback = callback;
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="returnType"></param>
        /// <param name="errorMessage">错误信息</param>
        internal CommandClientCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback, CommandClientReturnTypeEnum returnType, string errorMessage)
        {
            Callback = callback;
            ReturnValue = new CommandClientReturnValue<T>(returnType, errorMessage);
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="returnValue"></param>
        internal CommandClientCallbackQueueNode(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback, T returnValue)
        {
            Callback = callback;
            ReturnValue = returnValue;
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        /// <param name="callback">成功值回调</param>
        /// <param name="errorCallback">错误类型回调</param>
        public CommandClientCallbackQueueNode(Action<T, CommandClientCallQueue> callback, Action<CommandClientReturnTypeEnum, CommandClientCallQueue> errorCallback)
        {
            Callback = new ErrorCallback(callback, errorCallback).Callback;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            Callback(ReturnValue, queue);
        }
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        internal sealed class ErrorCallback
        {
            /// <summary>
            /// 成功值回调
            /// </summary>
            private readonly Action<T, CommandClientCallQueue> callback;
            /// <summary>
            /// 错误类型回调
            /// </summary>
            private readonly Action<CommandClientReturnTypeEnum, CommandClientCallQueue> errorCallback;
            /// <summary>
            /// 错误回调
            /// </summary>
            /// <param name="callback">成功值回调</param>
            /// <param name="errorCallback">错误类型回调</param>
            internal ErrorCallback(Action<T, CommandClientCallQueue> callback, Action<CommandClientReturnTypeEnum, CommandClientCallQueue> errorCallback)
            {
                this.callback = callback;
                this.errorCallback = errorCallback;
            }
            /// <summary>
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
        /// 隐式转换
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode<T>(Action<CommandClientReturnValue<T>, CommandClientCallQueue> value) { return new CommandClientCallbackQueueNode<T>(value); }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CommandClientCallbackQueueNode<T> Get(Action<CommandClientReturnValue<T>, CommandClientCallQueue> callback)
        {
            return new CommandClientCallbackQueueNode<T>(callback);
        }
        /// <summary>
        /// 成功回调，忽略错误返回
        /// </summary>
        internal sealed class SuccessCallback
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action<T> callback;
            /// <summary>
            /// 成功回调，忽略错误返回
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal SuccessCallback(Action<T> callback)
            {
                this.callback = callback;
            }
            /// <summary>
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
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode<T>(Action<T> value) { return new CommandClientCallbackQueueNode<T>(new SuccessCallback(value).Callback); }
        /// <summary>
        /// 返回值回调
        /// </summary>
        internal sealed class ReturnValueCallback
        {
            /// <summary>
            /// 客户端回调委托
            /// </summary>
            private readonly Action<CommandClientReturnValue<T>> callback;
            /// <summary>
            /// 返回值回调
            /// </summary>
            /// <param name="callback">客户端回调委托</param>
            internal ReturnValueCallback(Action<CommandClientReturnValue<T>> callback)
            {
                this.callback = callback;
            }
            /// <summary>
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
        /// 隐式转换（成功回调，忽略错误返回）
        /// </summary>
        /// <param name="value">客户端回调委托</param>
        /// <returns>客户端回调委托</returns>
        public static implicit operator CommandClientCallbackQueueNode<T>(Action<CommandClientReturnValue<T>> value) { return new CommandClientCallbackQueueNode<T>(new ReturnValueCallback(value).Callback); }
    }
}
