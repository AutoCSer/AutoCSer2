using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 自定义任务队列节点 await AutoCSer.Net.CommandClientReturnValue
    /// </summary>
    public abstract class InterfaceControllerTaskQueueCustomNode : InterfaceControllerTaskQueueNode
    {
        /// <summary>
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueCustomNode(ClientCallbackTypeEnum callbackType) : base(callbackType) { }
        /// <summary>
        /// 检查是否已经添加到队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckQueue()
        {
            return System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetReturn()
        {
            SetReturnType(CommandClientReturnTypeEnum.Success);
        }
    }
    /// <summary>
    /// 自定义任务队列节点 await AutoCSer.Net.CommandClientReturnValue{T}
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    public abstract class InterfaceControllerTaskQueueCustomNode<T> : InterfaceControllerTaskQueueNode<T>
    {
        /// <summary>
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// 接口任务队列节点
        /// </summary>
        /// <param name="callbackType">客户端 await 等待返回值回调线程模式</param>
        protected InterfaceControllerTaskQueueCustomNode(ClientCallbackTypeEnum callbackType) : base(callbackType) { }
        /// <summary>
        /// 检查是否已经添加到队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckQueue()
        {
            return System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0;
        }
        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="returnValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public new void SetReturn(T returnValue)
        {
            base.SetReturn(returnValue);
        }
    }
}
