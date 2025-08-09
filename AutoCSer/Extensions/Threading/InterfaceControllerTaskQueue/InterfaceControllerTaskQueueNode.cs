using AutoCSer.Net.CommandServer;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 接口任务队列节点 await AutoCSer.Net.CommandClientReturnValue
    /// </summary>
    public abstract class InterfaceControllerTaskQueueNode : InterfaceControllerTaskQueueNodeBase
    {
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueNode(ClientCallbackTypeEnum callbackType) : base(callbackType) { }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<CommandClientReturnValue> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue GetResult()
        {
            return new CommandClientReturnValue(returnType, null);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public InterfaceControllerTaskQueueNode GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        ///// <param name="isSynchronousThread"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetReturnType(InterfaceControllerTaskQueueNode node)
        {
            node.SetReturnType(CommandClientReturnTypeEnum.Success, true);
        }
    }
    /// <summary>
    /// 接口任务队列节点 await AutoCSer.Net.CommandClientReturnValue{T}
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public abstract class InterfaceControllerTaskQueueNode<T> : InterfaceControllerTaskQueueNodeBase
    {
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueNode(ClientCallbackTypeEnum callbackType) : base(callbackType) { }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<CommandClientReturnValue<T>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClientReturnValue<T> GetResult()
        {
            if (returnType == CommandClientReturnTypeEnum.Success) return returnValue;
            return new CommandClientReturnValue<T>(returnType, null);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public InterfaceControllerTaskQueueNode<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="isSynchronousThread"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetReturn(T returnValue, bool isSynchronousThread)
        {
            this.returnValue = returnValue;
            SetReturnType(CommandClientReturnTypeEnum.Success, isSynchronousThread);
        }
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetReturn(InterfaceControllerTaskQueueNode<T> node, T returnValue)
        {
            node.SetReturn(returnValue, true);
        }
    }
}
